using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using DevExpress.Web.Office;
using DevExpress.Web.Internal;
using DevExpress.XtraRichEdit;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet.Services;
using DevExpress.XtraSpreadsheet.Services.Implementation;

public enum DocumentCommandResult { OK, DocumentAlreadyExists, NotSupportedFormat };

public class DocumentService : ServiceBase {
    const string CurrentDocumentContextKey = "CurrentDocument_3D809529-9F6B-4E61-B87F-F7FAA67B6870";
    const string CurrentDocumentReadOnlyContextKey = "CurrentDocumentReadOnly_87FC142F-4739-4CAF-B9CF-8849FA8E7850";
    static HttpContext Context {
        get { return HttpContext.Current; }
    }

    DataService DataService { get { return DocumentsApp.Data; } }
    FileSystemService FileSystemService { get { return DocumentsApp.FileSystem; } }

    List<DocumentManagerBase> Managers { get; set; }
    SpreadsheetDocumentManager SpreadsheetDocumentManager { get; set; }
    RichEditDocumentManager RichEditDocumentManager { get; set; }

    string RequestFilePath {
        get {
            string filePath = Context.Request.QueryString["path"];
            if(string.IsNullOrWhiteSpace(filePath))
                HttpContext.Current.Response.Redirect("~");
            return filePath;
        }
    }
    string RequestFileType {
        get { return Context.Request.QueryString["type"]; }
    }
    string RequestDataSource {
        get {
            string dataSource = Context.Request.QueryString["src"];
            if(string.IsNullOrWhiteSpace(dataSource))
                return "All";
            return dataSource;
        }
    }
    bool IsCreateNewDocumentRequest {
        get { return !string.IsNullOrEmpty(RequestFileType); }
    }
    
    object documentLocker = new object();
    ConcurrentDictionary<long, User> LockedDocumentRegistry { get; set; }
    
    public Item CurrentDocument {
        get {
            if(!Context.Items.Contains(CurrentDocumentContextKey))
                Context.Items[CurrentDocumentContextKey] = GetCurrentDocument();
            return Context.Items[CurrentDocumentContextKey] as Item;
        }
        set {
            Context.Items[CurrentDocumentContextKey] = value;
        }
    }
    public bool CurrentDocumentReadOnly {
        get {
            if(!Context.Items.Contains(CurrentDocumentReadOnlyContextKey))
                Context.Items[CurrentDocumentReadOnlyContextKey] = true;
            return (bool)Context.Items[CurrentDocumentReadOnlyContextKey];
        }
        set {
            Context.Items[CurrentDocumentReadOnlyContextKey] = value;
        }
    }
    public DevExpress.XtraRichEdit.DocumentFormat CurrentRichEditDocumentFormat {
        get { return GetRichEditDocumentFormat(CurrentDocument); }
    }
    public DevExpress.Spreadsheet.DocumentFormat CurrentSpreadsheetDocumentFormat {
        get { return GetSpreadsheetDocumentFormat(CurrentDocument); }
    }

    public DocumentService(DocumentsApp app) 
        : base(app) {
        RichEditDocumentManager = new RichEditDocumentManager(app);
        SpreadsheetDocumentManager = new SpreadsheetDocumentManager(app);

        Managers = new List<DocumentManagerBase>();
        Managers.Add(RichEditDocumentManager);
        Managers.Add(SpreadsheetDocumentManager);
        Managers.Add(new UnsupportedDocumentManager(app));

        LockedDocumentRegistry = new ConcurrentDictionary<long, User>();
    }

    public virtual string GetDocumentIdForEditor(Item document) {
        return document.Id.ToString();
    }
    public bool IsRichEditDocument(Item documentItem) {
        return RichEditDocumentManager.CanProcess(documentItem);
    }
    public bool IsSpreadsheetDocument(Item documentItem) {
        return SpreadsheetDocumentManager.CanProcess(documentItem);
    }
    public bool IsDocumentEditingAllowed(Item documentItem) {
        return IsRichEditDocument(documentItem) || IsSpreadsheetDocument(documentItem);
    }
    public DevExpress.XtraRichEdit.DocumentFormat GetRichEditDocumentFormat(Item document) {
        return RichEditDocumentManager.GetFormat(document);
    }
    public DevExpress.Spreadsheet.DocumentFormat GetSpreadsheetDocumentFormat(Item document) {
        return SpreadsheetDocumentManager.GetFormat(document);
    }
    public string GetDocumentEditorRequestUrl(string itemPath) {
        return GetDocumentEditorRequestUrl(itemPath, RequestDataSource);
    }
    public string GetDocumentEditorRequestUrl(string itemPath, string dataSource) {
        return string.Format("~/Document.aspx?path={0}&src={1}",
            Context.Server.UrlEncode(itemPath), dataSource);
    }
    public void ProcessDocumentRequest(Page page) {
        if(IsCreateNewDocumentRequest)
            CreateNewDocument(RequestFilePath, RequestFileType);
        if(CurrentDocument == null || CurrentDocument.IsFolder)
            throw new Exception("Invalid file item.");
        
        DocumentManagerBase documentManager = GetDocumentManagerForDocument(CurrentDocument);
        if(IsCreateNewDocumentRequest)
            documentManager.ProcessNewDocument(CurrentDocument, page);
        else
            documentManager.ProcessEditDocument(CurrentDocument, page);
    }
    public DevExpress.XtraPrinting.IPrintable CreatePrintableComponent(Item documentItem) {
        DocumentManagerBase documentManager = GetDocumentManagerForDocument(documentItem);
        return documentManager.CreatePrintableComponent(documentItem);
    }
    public string GetIconName(Item documentItem) {
        DocumentManagerBase documentManager = GetDocumentManagerForDocument(documentItem);
        return documentManager.IconName;
    }
    public string GetDefaultNameForCurrentDocument() {
        DocumentManagerBase documentManager = GetDocumentManagerForDocument(CurrentDocument);
        return documentManager.DefaultDocumentName;
    }
    public bool TryLockDocument(Item item, User user) {
        lock(documentLocker) {
            User lockOwner = GetDocumentLockOwner(item);
            if(lockOwner != null) {
                if(user.Id != lockOwner.Id)
                    return false;
            }
            else
                LockedDocumentRegistry.AddOrUpdate(item.Id, user, (itemId, oldUser) => user);
            return true;
        }
    }
    public User GetDocumentLockOwner(Item item) {
        User lockOwner;
        if(LockedDocumentRegistry.TryGetValue(item.Id, out lockOwner))
            return lockOwner;
        return null;
    }
    public void RemoveAllLocksForUser(User user) {
        List<long> idList = LockedDocumentRegistry.
            Where(entry => entry.Value.Id == user.Id).
            Select(entry => entry.Key).
            ToList();

        foreach(long itemId in idList) {
            User lockOwner;
            LockedDocumentRegistry.TryRemove(itemId, out lockOwner);
        }
    }
    public DocumentCommandResult TrySaveAsNewDocument(string documentName, out string newDocumentUrl) {
        newDocumentUrl = null;
        Item newDocument = new Item() { 
            Name = documentName,
            IsFolder = false
        };
        if(!IsDocumentEditingAllowed(newDocument))
            return DocumentCommandResult.NotSupportedFormat;
        
        Item document = GetDocumentInCurrentFolder(documentName);
        if(document != null)
            return DocumentCommandResult.DocumentAlreadyExists;

        newDocument.Owner = DocumentsApp.User.CurrentUser;
        newDocument.ParentItem = CurrentDocument.ParentItem;
        newDocument.CreationTime = DateTime.UtcNow;
        newDocument.LastAccessTime = DateTime.UtcNow;
        newDocument.LastWriteTime = DateTime.UtcNow;

        byte[] newDocumentData;
        string sourceDocIdForEditor = GetDocumentIdForEditor(CurrentDocument);
        IDocumentInfo sourceDocInfo = DocumentManager.FindDocument(sourceDocIdForEditor);
        if(IsRichEditDocument(CurrentDocument)) {
            RichEditDocumentInfo richEditDocInfo = sourceDocInfo as RichEditDocumentInfo;
            DevExpress.XtraRichEdit.DocumentFormat richEditFormat = GetRichEditDocumentFormat(newDocument);
            newDocumentData = richEditDocInfo.SaveCopy(richEditFormat);
        }
        else if(IsSpreadsheetDocument(CurrentDocument)) {
            SpreadsheetDocumentInfo spreadsheetDocInfo = sourceDocInfo as SpreadsheetDocumentInfo;
            DevExpress.Spreadsheet.DocumentFormat spreadsheetFormat = GetSpreadsheetDocumentFormat(newDocument);
            newDocumentData = spreadsheetDocInfo.SaveCopy(spreadsheetFormat);
        }
        else
            throw new Exception("Incorrect document format.");
        newDocument.Content = DataService.CreateBinaryContent(newDocumentData);

        DataService.AddItem(newDocument);
        DataService.SaveChanges();
        string newDocumentPath = GetPathForDocumentInCurrentFolder(documentName);
        newDocumentUrl = GetDocumentEditorRequestUrl(newDocumentPath);
        return DocumentCommandResult.OK;
    }
    public virtual void EnableDocumentAutoSave() {
        DocumentManager.AutoSaving += DocumentManager_AutoSaving;
    }
    public void SaveAllDocuments() {
        List<IDocumentInfo> documents = DocumentManager.GetAllDocuments().ToList();
        foreach(IDocumentInfo documentInfo in documents) {
            try {
                SaveDocument(documentInfo, true);
            }
            catch { }
        }
    }
    public string GetPathForDocumentInCurrentFolder(string documentName) {
        string currentFolderPath = IsCreateNewDocumentRequest
            ? RequestFilePath : Path.GetDirectoryName(RequestFilePath);
        return Path.Combine(currentFolderPath, documentName);
    }
    public void ValidateCurrentDocumentExists() {
        if(CurrentDocument == null)
            throw new DocumentsException("Document not found.");
    }

    DocumentManagerBase GetDocumentManagerForDocument(Item documentItem) {
        return Managers.First(manager => manager.CanProcess(documentItem));
    }
    Item GetCurrentDocument() {
        return FileSystemService.GetFileFromSource(RequestFilePath, RequestDataSource);
    }
    Item GetDocumentInCurrentFolder(string documentName) {
        string newDocumentPath = GetPathForDocumentInCurrentFolder(documentName);
        return FileSystemService.GetFileFromSource(newDocumentPath, RequestDataSource);
    }

    void CreateNewDocument(string folderPath, string docType) {
        if(string.IsNullOrEmpty(folderPath))
            folderPath = "Files";

        string documentName;
        if(docType == "richtext")
            documentName = "Document.rtf";
        else if(docType == "worksheet")
            documentName = "Worksheet.xlsx";
        else
            throw new Exception("Incorrect document type.");

        Item newDocument = FileSystemService.CreateNewFileItem(folderPath, documentName);
        CurrentDocument = newDocument;
    }

    static void DocumentManager_AutoSaving(IDocumentInfo documentInfo, DocumentSavingEventArgs e) {
        DocumentsApp.Instance.Document.OnDocumentAutoSaving(documentInfo, e);
    }
    void OnDocumentAutoSaving(IDocumentInfo documentInfo, DocumentSavingEventArgs e) {
        try {
            SaveDocument(documentInfo, false);
        }
        finally {
            e.Handled = true;
            DataService.CloseUnitOfWork();
        }
    }
    void SaveDocument(IDocumentInfo documentInfo, bool checkModificationTime) {
        long itemId = ParseDocumentIdFromEditor(documentInfo.DocumentId);
        Item docItem = DataService.FindItemById(itemId);
        if(docItem == null || checkModificationTime && documentInfo.LastModifyTime < docItem.LastWriteTime)
            return;

        byte[] content = null;
        if(IsRichEditDocument(docItem)) {
            RichEditDocumentInfo richEditDocument = (RichEditDocumentInfo)documentInfo;
            content = richEditDocument.SaveCopy(RichEditDocumentManager.GetFormat(docItem));
        }
        else if(IsSpreadsheetDocument(docItem)) {
            SpreadsheetDocumentInfo spreadsheetDocument = (SpreadsheetDocumentInfo)documentInfo;
            content = spreadsheetDocument.SaveCopy(SpreadsheetDocumentManager.GetFormat(docItem));
        }
        else
            throw new Exception("Incorrect document format.");

        docItem.UpdateContent(content);
        DataService.SaveChanges();
    }
    protected virtual long ParseDocumentIdFromEditor(string editorDocId) {
        return long.Parse(editorDocId);
    }
}

public abstract class DocumentManagerBase {
    protected DocumentsApp DocumentsApp { get; private set; }

    public DocumentManagerBase(DocumentsApp app) {
        DocumentsApp = app;
    }

    public string IconName { get; protected set; }
    public string DefaultDocumentName { get; protected set; }

    public virtual bool CanProcess(Item fileItem) {
        throw new NotImplementedException();
    }
    public virtual void ProcessNewDocument(Item fileItem, Page page) {
        throw new NotImplementedException();
    }
    public virtual void ProcessEditDocument(Item fileItem, Page page) {
        throw new NotImplementedException();
    }
    public virtual DevExpress.XtraPrinting.IPrintable CreatePrintableComponent(Item fileItem) {
        return null;
    }
}

public abstract class StandardDocumentManagerBase<TFormat> : DocumentManagerBase {
    protected Dictionary<string, TFormat> FormatRegistry { get; private set; }
    protected string EditorUrl { get; set; }

    protected StandardDocumentManagerBase(DocumentsApp app)
        : base(app) {
        FormatRegistry = new Dictionary<string, TFormat>();
    }

    public override bool CanProcess(Item fileItem) {
        string extension = Path.GetExtension(fileItem.Name);
        return FormatRegistry.ContainsKey(extension);
    }
    public override void ProcessNewDocument(Item fileItem, Page page) {
        string newDocumentPath = DocumentsApp.Document.GetPathForDocumentInCurrentFolder(fileItem.Name);
        string newDocumentUrl = DocumentsApp.Document.GetDocumentEditorRequestUrl(newDocumentPath);
        HttpContext.Current.Response.Redirect(newDocumentUrl);
    }
    public override void ProcessEditDocument(Item fileItem, Page page) {
        DocumentsApp.Document.CurrentDocumentReadOnly = !DocumentsApp.TryLockDocument(fileItem);
        HttpContext.Current.Server.Transfer(EditorUrl);
    }
    public TFormat GetFormat(Item fileItem) {
        string extension = Path.GetExtension(fileItem.Name);
        TFormat format;
        if(FormatRegistry.TryGetValue(extension, out format))
            return format;
        throw new Exception("Extension is not supported.");
    }
}

public class RichEditDocumentManager : StandardDocumentManagerBase<DevExpress.XtraRichEdit.DocumentFormat> {
    public RichEditDocumentManager(DocumentsApp app)
        : base(app) {
        FormatRegistry.Add(".rtf", DevExpress.XtraRichEdit.DocumentFormat.Rtf);
        FormatRegistry.Add(".doc", DevExpress.XtraRichEdit.DocumentFormat.Doc);
        FormatRegistry.Add(".docx", DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
        FormatRegistry.Add(".txt", DevExpress.XtraRichEdit.DocumentFormat.PlainText);

        EditorUrl = "RichTextEdit.aspx";
        IconName = "RTF.png";
        DefaultDocumentName = "Document1.rtf";
    }
    public override DevExpress.XtraPrinting.IPrintable CreatePrintableComponent(Item fileItem) {
        RichEditDocumentServer docServer = new RichEditDocumentServer();
        Stream contentStream = DocumentsApp.Data.ReadFileContent(fileItem);
        docServer.LoadDocument(contentStream, GetFormat(fileItem));
        return docServer;
    }
}

public class SpreadsheetDocumentManager : StandardDocumentManagerBase<DevExpress.Spreadsheet.DocumentFormat> {
    public SpreadsheetDocumentManager(DocumentsApp app)
        : base(app) {
        FormatRegistry.Add(".xlsx", DevExpress.Spreadsheet.DocumentFormat.Xlsx);

        EditorUrl = "SpreadsheetEdit.aspx";
        IconName = "Spreadsheet.png";
        DefaultDocumentName = "Worksheet1.xlsx";
    }
    public override DevExpress.XtraPrinting.IPrintable CreatePrintableComponent(Item fileItem) {
        IWorkbook workbook = new Workbook();
        workbook.AddService(typeof(IChartControllerFactoryService), new ChartControllerFactoryService());
        workbook.AddService(typeof(IChartImageService), new ChartImageService());
        Stream contentStream = DocumentsApp.Data.ReadFileContent(fileItem);
        workbook.LoadDocument(contentStream, GetFormat(fileItem));
        return workbook;
    }
}

public class UnsupportedDocumentManager : DocumentManagerBase {
    public UnsupportedDocumentManager(DocumentsApp app) : base(app) { }
    public override bool CanProcess(Item fileItem) {
        return true;
    }
    public override void ProcessEditDocument(Item fileItem, Page page) {
        Stream contentStream = DocumentsApp.Data.ReadFileContent(fileItem);
        string fileName = Path.GetFileNameWithoutExtension(fileItem.Name);
        string fileExt = Path.GetExtension(fileItem.Name);
        WriteStreamToResponse(page, contentStream, fileName, fileExt);
    }
    void WriteStreamToResponse(Page page, Stream stream, string fileName, string fileExt) {
        HttpUtils.WriteFileToResponse(page, stream, fileName, true, fileExt, HttpUtils.GetContentType(fileExt), true);
    }
}

public class DocumentsException : Exception {
    public DocumentsException(string message)
        : base(message) {
    }
}
