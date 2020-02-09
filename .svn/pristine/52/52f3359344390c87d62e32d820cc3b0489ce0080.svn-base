using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Office;

public partial class RichTextEdit : System.Web.UI.Page {
    DocumentsApp DocumentsApp { get { return DocumentsApp.Instance; } }
    DocumentService DocumentService { get { return DocumentsApp.Document; } }
    Item CurrentDocument { get { return DocumentService.CurrentDocument; } }

    protected void Page_Load(object sender, EventArgs e) {
        if(!IsPostBack) {
            RichEdit.ReadOnly = DocumentService.CurrentDocumentReadOnly;
            string currentDocumentId = DocumentService.GetDocumentIdForEditor(CurrentDocument);
            IDocumentInfo openedDocument = DocumentManager.FindDocument(currentDocumentId);
            if(openedDocument == null) {
                if(CurrentDocument.Content == null) {
                    RichEdit.New();
                    RichEdit.DocumentId = currentDocumentId;
                    CurrentDocument.Content = DocumentsApp.Data.CreateBinaryContent(RichEdit.SaveCopy(DocumentService.GetRichEditDocumentFormat(CurrentDocument)));
                    DocumentsApp.Data.SaveChanges();
                }
                else {
                    RichEdit.Open(
                        currentDocumentId,
                        DocumentService.CurrentRichEditDocumentFormat,
                        () => CurrentDocument.Content.Data);
                }
            }
            else
                RichEdit.Open((RichEditDocumentInfo)openedDocument);
        }
    }
    protected void RichEdit_PreRender(object sender, EventArgs e) {
        RichEdit.Focus();
    }
}
