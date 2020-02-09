<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogOn.aspx.cs" Inherits="LogOn" MasterPageFile="~/Site.master" %>
<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <div runat="server" ID="LogonContainer" class="LogonContainer">
        <form id="form1" runat="server">
            <div class="LogonContent">
                <dx:ASPxImage runat="server" ID="LogoImage" ImageUrl="~/Content/Images/Icons/DXLogo_login.png" ShowLoadingImage="true" Visible="true"></dx:ASPxImage>
                <dx:ASPxFormLayout runat="server" ID="FormLayout" RequiredMarkDisplayMode="None" ShowItemCaptionColon="false" 
                    CssClass="LogonFormLayout">
                    <Styles>
                        <LayoutItem Caption-CssClass="LogonCaption" />
                        <LayoutGroupBox CssClass="LogonGroupBox" Caption-CssClass="LogonLogoCaption" />
                    </Styles>
                    <Items>
                        <dx:LayoutGroup GroupBoxDecoration="HeadingLine" ShowCaption="false">
                            <Items>
                                <dx:LayoutItem Caption="Access Your Documents - From Anywhere, Anytime" Name="Title">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:EmptyLayoutItem CssClass="LogonEmptyItem" />
                                <dx:LayoutItem Name="AccountName" ShowCaption="False" CssClass="LogonAccountNameItem">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox runat="server" ID="AccountNameTextBox" Width="490px"
                                                CssClass="AccountNameTextBox">
                                                <FocusedStyle CssClass="AccountNameFocused" />
                                                <BackgroundImage ImageUrl="Content/Images/Logon/Login.png" Repeat="NoRepeat" 
                                                        HorizontalPosition="left" VerticalPosition="center" />
                                                <ValidationSettings ErrorDisplayMode="Text" Display="Dynamic" ErrorTextPosition="Bottom" 
                                                    SetFocusOnError="true" ErrorFrameStyle-CssClass="AccountNameError" ValidateOnLeave="false">
                                                    <RegularExpression ErrorText="Invalid user name" ValidationExpression="\w+" />
                                                    <RequiredField IsRequired="True" ErrorText="The value is required" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Name="UserPassword" ShowCaption="False" CssClass="LogonUserPasswordItem">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="UserPasswordTextBox" runat="server" Password="true" Width="490px" 
                                                CssClass="UserPasswordTextBox">
                                                <FocusedStyle CssClass="PasswordFocused" />
                                                <BackgroundImage ImageUrl="Content/Images/Logon/Password.png" Repeat="NoRepeat" 
                                                        HorizontalPosition="left" VerticalPosition="center" />
                                                <ClientSideEvents Init="App.onUserPasswordInit" />
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:EmptyLayoutItem CssClass="LogonEmptyItem" />
                                <dx:LayoutItem Name="SignIn" ShowCaption="False">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxButton ID="SignInButton" runat="server" Text="Sign In" CssClass="SignInButton">
                                            </dx:ASPxButton>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Name="Error" Visible="False" ShowCaption="False" CssClass="LogonErrorItem">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxLabel runat="server" ID="ErrorLabel" Text="" CssClass="ErrorLabel" />
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                            </Items>
                        </dx:LayoutGroup>
                    </Items>
                </dx:ASPxFormLayout>
            </div>
        </form>
    </div>
</asp:Content>
