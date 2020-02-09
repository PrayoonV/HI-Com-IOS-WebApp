using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;

public partial class UserControls_SaveAsDocumentDialog : System.Web.UI.UserControl {
    public string DefaultDocumentName { get; set; }

    protected void Page_Load(object sender, EventArgs e) {
        DocumentNameTextBox.Text = DefaultDocumentName;
    }
}
