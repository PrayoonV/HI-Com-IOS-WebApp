using System;
using System.Web;
using System.Web.UI;

public partial class Document : Page {
    protected void Page_PreInit(object sender, EventArgs e) {
        DocumentsApp.Instance.Document.ProcessDocumentRequest(this);
    }
}
