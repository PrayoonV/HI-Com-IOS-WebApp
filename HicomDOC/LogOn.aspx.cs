using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LogOn : System.Web.UI.Page {
    UserService UserService {
        get { return DocumentsApp.Instance.User; }
    }

    string UserAccountName { 
        get { return AccountNameTextBox.Text; }
        set { AccountNameTextBox.Text = value; }
    }
    string UserPasswordText { 
        get { return UserPasswordTextBox.Text; }
        set { UserPasswordTextBox.Text = value; }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if(IsPostBack) {
            if(!UserService.SignIn(UserAccountName, null)) {
                LogonContainer.Attributes["class"] += " LogonError";
                FormLayout.FindItemOrGroupByName("Error").Visible = true;
                ErrorLabel.Text = string.Format(
                    "Login failed for '{0}'. Make sure your account name is correct and retype the password in the correct case.", UserAccountName);
            }
        }
        else
            UserAccountName = UserService.DefaultUserAccountName;
    }
}
