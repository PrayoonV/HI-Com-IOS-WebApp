using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;

public partial class _Default : System.Web.UI.Page {
    DocumentsApp DocumentsApp { get { return DocumentsApp.Instance; } }

    string SelectedFilter { get { return FilterMenu.SelectedItem.Name; } }
    string SelectedFilterText {
        get {
            switch(SelectedFilter) {
                case "My":
                    return "My documents";
                case "Recent":
                    return "Recent documents";
                case "RTFDocs":
                    return "Rich text documents";
                case "Sheets":
                    return "Worksheet documents";
            }
            return FilterMenu.SelectedItem.Text;
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        PrepareUserMenu();
        UserControls_DocumentBrowser docBrowser = (UserControls_DocumentBrowser)LoadControl("~/UserControls/DocumentBrowser.ascx");
        docBrowser.ID = "DocumentBrowserAll";
        docBrowser.FileSystemProvider = CreateFileSystemProvider();
        docBrowser.IsFilterApplied = SelectedFilter != "All";
        docBrowser.PanelName = SelectedFilterText;
        docBrowser.SourceName = SelectedFilter;
        CallbackPanel.Controls.Add(docBrowser);
    }
    protected FileSystemProviderBase CreateFileSystemProvider() {
        if(SelectedFilter != "All") {
            FilteredFileSystemProvider queryProvider = new FilteredFileSystemProvider(DocumentsApp, "");
            queryProvider.FileQuery = DocumentsApp.FileSystem.GetFilesFromSource(SelectedFilter);
            return queryProvider;
        }
        return new GeneralFileSystemProvider(DocumentsApp, "Files");
    }
    protected void PrepareUserMenu() {
        if(!IsPostBack) {
            DevExpress.Web.MenuItem userItem = UserMenu.Items.FindByName("User");
            userItem.Text = DocumentsApp.User.CurrentUser.Name;
            userItem.Image.Url = DocumentsApp.GetCurrentUserAvatarVirtPath();
        }
    }
    protected void UserMenu_ItemClick(object source, MenuItemEventArgs e) {
        DocumentsApp.User.SignOut();
    }
}
