<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SaveAsDocumentDialog.ascx.cs" Inherits="UserControls_SaveAsDocumentDialog" %>
<dx:ASPxPopupControl runat="server" ID="SaveAsDocumentPopupControl" ClientInstanceName="SaveAsDocumentPopupControl" 
    CssClass="DialogPopupControl" ShowOnPageLoad="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
    Modal="true" CloseAction="CloseButton" ShowFooter="true" AllowDragging="true" HeaderText="Save As Document" CloseOnEscape="true">
    <ContentStyle CssClass="DialogPopupContent" />
    <FooterStyle CssClass="DialogPopupFooter" />
    <ContentCollection>
        <dx:PopupControlContentControl>
            <dx:ASPxFormLayout runat="server" ID="DialogFormLayout" RequiredMarkDisplayMode="None"  
                CssClass="DialogFormLayout">
                <Styles>
                    <LayoutItem CaptionCell-CssClass="DialogCaptionCell" />
                </Styles>
                <Items>
                    <dx:LayoutItem Caption="Document name" Name="Title">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox runat="server" ID="DocumentNameTextBox" ClientInstanceName="DocumentNameTextBox" Width="100%" 
                                    Text="" CssClass="DocumentNameTextBox">
                                    <ValidationSettings ErrorDisplayMode="Text" Display="Dynamic" ErrorTextPosition="Bottom" 
                                        SetFocusOnError="true" ErrorFrameStyle-CssClass="AccountNameError" ValidateOnLeave="false">
                                        <RequiredField IsRequired="True" ErrorText="The value is required" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:ASPxFormLayout>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <FooterTemplate>
        <dx:ASPxButton runat="server" ID="SaveButton" Text="Save" CssClass="DialogButton"
            ClientSideEvents-Click="App.onSaveAsDialogSaveButtonClick" AutoPostBack="false"></dx:ASPxButton>
        <dx:ASPxButton runat="server" ID="CancelButton" Text="Cancel"
            ClientSideEvents-Click="App.onSaveAsDialogCancelButtonClick" AutoPostBack="false"></dx:ASPxButton>
    </FooterTemplate>
    <ClientSideEvents Init="App.onSaveAsDialogInit" />
</dx:ASPxPopupControl>
