using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Internal;

public partial class DocumentEditor : System.Web.UI.MasterPage {
    DocumentsApp DocumentsApp { get { return DocumentsApp.Instance; } }
    
    public string StatusMarkText {
        get { return StatusMarkLabel.Text; }
        set { StatusMarkLabel.Text = value; }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if(!IsPostBack) {
            string documentName = DocumentsApp.Document.CurrentDocument.Name;
            Page.Title = documentName;
            TitleLabel.Text = documentName;
            DevExpress.Web.MenuItem userItem = UserMenu.Items.FindByName("User");
            userItem.Text = DocumentsApp.User.CurrentUser.Name;
            userItem.Image.Url = DocumentsApp.GetCurrentUserAvatarVirtPath();
        }
    }
    protected void UserMenu_ItemClick(object source, MenuItemEventArgs e) {
        DocumentsApp.User.SignOut();
    }
    protected void DialogCallbackPanel_Callback(object sender, CallbackEventArgsBase e) {
        DocumentsApp.Document.ValidateCurrentDocumentExists();

        UserControls_SaveAsDocumentDialog dialog = (UserControls_SaveAsDocumentDialog)LoadControl("~/UserControls/SaveAsDocumentDialog.ascx");
        dialog.ID = "SaveAsDocumentDialog";
        dialog.DefaultDocumentName = DocumentsApp.Document.GetDefaultNameForCurrentDocument();
        DialogCallbackPanel.Controls.Add(dialog);
    }
    protected void DocumentEditorCallback_Callback(object source, CallbackEventArgs e) {
        DocumentsApp.Document.ValidateCurrentDocumentExists();

        string newDocumentName = e.Parameter;
        string newDocumentUrl;
        DocumentCommandResult commandStatus = DocumentsApp.Document.TrySaveAsNewDocument(newDocumentName, out newDocumentUrl);
        if(commandStatus == DocumentCommandResult.OK)
            ASPxWebControl.RedirectOnCallback(newDocumentUrl);
        else
            e.Result = commandStatus == DocumentCommandResult.DocumentAlreadyExists ? "exists" : "format";
    }
}
