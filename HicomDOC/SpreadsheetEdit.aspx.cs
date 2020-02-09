using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.ASPxSpreadsheet;
using DevExpress.Web.Office;

public partial class SpreadsheetEdit : System.Web.UI.Page {
    DocumentsApp DocumentsApp { get { return DocumentsApp.Instance; } }
    DocumentService DocumentService { get { return DocumentsApp.Document; } }
    Item CurrentDocument { get { return DocumentService.CurrentDocument; } }

    protected void Spreadsheet_Init(object sender, EventArgs e) {
        if(!IsPostBack) {
            Spreadsheet.ReadOnly = DocumentService.CurrentDocumentReadOnly;
            string currentDocumentId = DocumentService.GetDocumentIdForEditor(CurrentDocument);
            IDocumentInfo openedDocument = DocumentManager.FindDocument(currentDocumentId);
            if(openedDocument == null) {
                if(CurrentDocument.Content == null) {
                    Spreadsheet.New();
                    Spreadsheet.DocumentId = currentDocumentId;
                    CurrentDocument.Content = DocumentsApp.Data.CreateBinaryContent(Spreadsheet.SaveCopy(DocumentService.GetSpreadsheetDocumentFormat(CurrentDocument)));
                    DocumentsApp.Data.SaveChanges();
                }
                else {
                    Spreadsheet.Open(
                        currentDocumentId,
                        DocumentService.CurrentSpreadsheetDocumentFormat,
                        () => CurrentDocument.Content.Data);
                }
            }
            else
                Spreadsheet.Open((SpreadsheetDocumentInfo)openedDocument);
        }
    }
}
