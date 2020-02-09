<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RichTextEdit.aspx.cs" Inherits="RichTextEdit" MasterPageFile="~/DocumentEditor.master" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="HeaderHolder">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentHolder">
    <dx:ASPxRibbon ID="DocumentRibbon" runat="server" ShowFileTab="false" Theme="MetropolisBlue" CssClass="DocumentRibbon">
        <Tabs>
            <dx:RERHomeTab>
                <Groups>
                    <dx:RERFileCommonGroup>
                        <Items>
                            <dx:RibbonButtonItem Name="NewButton" Size="Small" Text="New Document">
                                <SmallImage IconID="actions_new_16x16gray" />
                            </dx:RibbonButtonItem>
                            <dx:RibbonButtonItem Name="SaveAsButton" Size="Small" Text="Save As Document">
                                <SmallImage IconID="save_saveas_16x16gray" />
                            </dx:RibbonButtonItem>
                            <dx:RERPrintCommand Size="Small" Text="Print Document">
                            </dx:RERPrintCommand>
                        </Items>
                    </dx:RERFileCommonGroup>
                    <dx:RERUndoGroup>
                        <Items>
                            <dx:RERUndoCommand Size="Large">
                            </dx:RERUndoCommand>
                            <dx:RERRedoCommand Size="Large">
                            </dx:RERRedoCommand>
                        </Items>
                    </dx:RERUndoGroup>
                    <dx:RERClipboardGroup>
                        <Items>
                            <dx:RERPasteCommand Size="Large">
                            </dx:RERPasteCommand>
                            <dx:RERCutCommand>
                            </dx:RERCutCommand>
                            <dx:RERCopyCommand>
                            </dx:RERCopyCommand>
                        </Items>
                    </dx:RERClipboardGroup>
                    <dx:RERFontGroup>
                        <Items>
                            <dx:RERFontNameCommand>
                                <PropertiesComboBox Width="150px">
                                </PropertiesComboBox>
                            </dx:RERFontNameCommand>
                            <dx:RERFontSizeCommand>
                                <PropertiesComboBox Width="60px">
                                </PropertiesComboBox>
                            </dx:RERFontSizeCommand>
                            <dx:RERIncreaseFontSizeCommand>
                            </dx:RERIncreaseFontSizeCommand>
                            <dx:RERDecreaseFontSizeCommand>
                            </dx:RERDecreaseFontSizeCommand>
                            <dx:RERChangeCaseCommand DropDownMode="False">
                            </dx:RERChangeCaseCommand>
                            <dx:RERFontBoldCommand>
                            </dx:RERFontBoldCommand>
                            <dx:RERFontItalicCommand>
                            </dx:RERFontItalicCommand>
                            <dx:RERFontUnderlineCommand>
                            </dx:RERFontUnderlineCommand>
                            <dx:RERFontStrikeoutCommand>
                            </dx:RERFontStrikeoutCommand>
                            <dx:RERFontSuperscriptCommand>
                            </dx:RERFontSuperscriptCommand>
                            <dx:RERFontSubscriptCommand>
                            </dx:RERFontSubscriptCommand>
                            <dx:RERFontColorCommand AutomaticColorItemCaption="Automatic" AutomaticColorItemValue="0" Color="Black" EnableAutomaticColorItem="True" EnableCustomColors="True">
                            </dx:RERFontColorCommand>
                            <dx:RERFontBackColorCommand AutomaticColor="" AutomaticColorItemCaption="No Color" AutomaticColorItemValue="16777215" EnableAutomaticColorItem="True" EnableCustomColors="True">
                            </dx:RERFontBackColorCommand>
                            <dx:RERClearFormattingCommand>
                            </dx:RERClearFormattingCommand>
                        </Items>
                    </dx:RERFontGroup>
                    <dx:RERParagraphGroup>
                        <Items>
                            <dx:RERBulletedListCommand>
                            </dx:RERBulletedListCommand>
                            <dx:RERNumberingListCommand>
                            </dx:RERNumberingListCommand>
                            <dx:RERMultilevelListCommand>
                            </dx:RERMultilevelListCommand>
                            <dx:RERDecreaseIndentCommand>
                            </dx:RERDecreaseIndentCommand>
                            <dx:RERIncreaseIndentCommand>
                            </dx:RERIncreaseIndentCommand>
                            <dx:RERShowWhitespaceCommand>
                            </dx:RERShowWhitespaceCommand>
                            <dx:RERAlignLeftCommand>
                            </dx:RERAlignLeftCommand>
                            <dx:RERAlignCenterCommand>
                            </dx:RERAlignCenterCommand>
                            <dx:RERAlignRightCommand>
                            </dx:RERAlignRightCommand>
                            <dx:RERAlignJustifyCommand>
                            </dx:RERAlignJustifyCommand>
                            <dx:RERParagraphLineSpacingCommand DropDownMode="False">
                            </dx:RERParagraphLineSpacingCommand>
                            <dx:RERParagraphBackColorCommand AutomaticColor="" AutomaticColorItemCaption="No Color" AutomaticColorItemValue="16777215" EnableAutomaticColorItem="True" EnableCustomColors="True">
                            </dx:RERParagraphBackColorCommand>
                        </Items>
                    </dx:RERParagraphGroup>
                    <dx:RERStylesGroup>
                        <Items>
                            <dx:RERChangeStyleCommand ImageWidth="60px" MaxColumnCount="10" MaxTextWidth="65px" MinColumnCount="2">
                                <PropertiesDropDownGallery RowCount="3" />
                            </dx:RERChangeStyleCommand>
                        </Items>
                    </dx:RERStylesGroup>
                    <dx:REREditingGroup>
                        <Items>
                            <dx:RERFindCommand>
                            </dx:RERFindCommand>
                            <dx:RERReplaceCommand>
                            </dx:RERReplaceCommand>
                            <dx:RERSelectAllCommand>
                            </dx:RERSelectAllCommand>
                        </Items>
                    </dx:REREditingGroup>
                </Groups>
            </dx:RERHomeTab>
            <dx:RERInsertTab>
                <Groups>
                    <dx:RERPagesGroup>
                        <Items>
                            <dx:RERInsertPageBreakCommand Size="Large">
                            </dx:RERInsertPageBreakCommand>
                        </Items>
                    </dx:RERPagesGroup>
                    <dx:RERTablesGroup>
                        <Items>
                            <dx:RERInsertTableCommand Size="Large">
                            </dx:RERInsertTableCommand>
                        </Items>
                    </dx:RERTablesGroup>
                    <dx:RERIllustrationsGroup>
                        <Items>
                            <dx:RERInsertPictureCommand Size="Large">
                            </dx:RERInsertPictureCommand>
                        </Items>
                    </dx:RERIllustrationsGroup>
                    <dx:RERLinksGroup>
                        <Items>
                            <dx:RERShowBookmarksFormCommand Size="Large">
                            </dx:RERShowBookmarksFormCommand>
                            <dx:RERShowHyperlinkFormCommand Size="Large">
                            </dx:RERShowHyperlinkFormCommand>
                        </Items>
                    </dx:RERLinksGroup>
                    <dx:RERHeaderAndFooterGroup Text="Header &amp; Footer">
                        <Items>
                            <dx:REREditPageHeaderCommand Size="Large">
                            </dx:REREditPageHeaderCommand>
                            <dx:REREditPageFooterCommand Size="Large">
                            </dx:REREditPageFooterCommand>
                            <dx:RERInsertPageNumberFieldCommand Size="Large">
                            </dx:RERInsertPageNumberFieldCommand>
                            <dx:RERInsertPageCountFieldCommand Size="Large">
                            </dx:RERInsertPageCountFieldCommand>
                        </Items>
                    </dx:RERHeaderAndFooterGroup>
                    <dx:RERSymbolsGroup>
                        <Items>
                            <dx:RERShowSymbolFormCommand Size="Large">
                            </dx:RERShowSymbolFormCommand>
                        </Items>
                    </dx:RERSymbolsGroup>
                </Groups>
            </dx:RERInsertTab>
            <dx:RERPageLayoutTab>
                <Groups>
                    <dx:RERPageSetupGroup>
                        <Items>
                            <dx:RERPageMarginsCommand DropDownMode="False" Size="Large">
                            </dx:RERPageMarginsCommand>
                            <dx:RERChangeSectionPageOrientationCommand DropDownMode="False" Size="Large">
                            </dx:RERChangeSectionPageOrientationCommand>
                            <dx:RERChangeSectionPaperKindCommand DropDownMode="False" Size="Large">
                            </dx:RERChangeSectionPaperKindCommand>
                            <dx:RERSetSectionColumnsCommand DropDownMode="False" Size="Large">
                            </dx:RERSetSectionColumnsCommand>
                            <dx:RERInsertBreakCommand DropDownMode="False" Size="Large">
                            </dx:RERInsertBreakCommand>
                        </Items>
                    </dx:RERPageSetupGroup>
                    <dx:RERBackgroundGroup>
                        <Items>
                            <dx:RERChangePageColorCommand AutomaticColor="Transparent" AutomaticColorItemCaption="No Color" AutomaticColorItemValue="16777215" Color="Transparent" DropDownMode="False" EnableAutomaticColorItem="True" EnableCustomColors="True" Size="Large">
                            </dx:RERChangePageColorCommand>
                        </Items>
                    </dx:RERBackgroundGroup>
                </Groups>
            </dx:RERPageLayoutTab>
            <dx:RERMailMergeTab Text="Mail Merge">
                <Groups>
                    <dx:RERInsertFieldsGroup>
                        <Items>
                            <dx:RERCreateFieldCommand Size="Large">
                            </dx:RERCreateFieldCommand>
                            <dx:RERInsertMergeFieldCommand Size="Large">
                            </dx:RERInsertMergeFieldCommand>
                        </Items>
                    </dx:RERInsertFieldsGroup>
                    <dx:RERMailMergeViewGroup>
                        <Items>
                            <dx:RERToggleViewMergedDataCommand Size="Large">
                            </dx:RERToggleViewMergedDataCommand>
                            <dx:RERToggleShowAllFieldCodesCommand Size="Large">
                            </dx:RERToggleShowAllFieldCodesCommand>
                            <dx:RERToggleShowAllFieldResultsCommand Size="Large">
                            </dx:RERToggleShowAllFieldResultsCommand>
                            <dx:RERUpdateAllFieldsCommand Size="Large">
                            </dx:RERUpdateAllFieldsCommand>
                        </Items>
                    </dx:RERMailMergeViewGroup>
                    <dx:RERCurrentRecordGroup>
                        <Items>
                            <dx:RERFirstDataRecordCommand Size="Large">
                            </dx:RERFirstDataRecordCommand>
                            <dx:RERPreviousDataRecordCommand Size="Large">
                            </dx:RERPreviousDataRecordCommand>
                            <dx:RERNextDataRecordCommand Size="Large">
                            </dx:RERNextDataRecordCommand>
                            <dx:RERLastDataRecordCommand Size="Large">
                            </dx:RERLastDataRecordCommand>
                        </Items>
                    </dx:RERCurrentRecordGroup>
                    <dx:RERFinishMailMergeGroup>
                        <Items>
                            <dx:RERFinishAndMergeCommand Size="Large">
                            </dx:RERFinishAndMergeCommand>
                        </Items>
                    </dx:RERFinishMailMergeGroup>
                </Groups>
            </dx:RERMailMergeTab>
            <dx:RERViewTab>
                <Groups>
                    <dx:RERShowGroup>
                        <Items>
                            <dx:RERToggleShowHorizontalRulerCommand Checked="True" Size="Large">
                            </dx:RERToggleShowHorizontalRulerCommand>
                        </Items>
                    </dx:RERShowGroup>
                    <dx:RERViewGroup>
                        <Items>
                            <dx:RERToggleFullScreenCommand Size="Large">
                            </dx:RERToggleFullScreenCommand>
                        </Items>
                    </dx:RERViewGroup>
                </Groups>
            </dx:RERViewTab>
        </Tabs>
        <ContextTabCategories>
            <dx:RERTableToolsContextTabCategory Color="211, 19, 19">
                <Tabs>
                    <dx:RERTableDesignTab>
                        <Groups>
                            <dx:RERTableStyleOptionsGroup>
                                <Items>
                                    <dx:RERToggleFirstRowCommand>
                                    </dx:RERToggleFirstRowCommand>
                                    <dx:RERToggleLastRowCommand>
                                    </dx:RERToggleLastRowCommand>
                                    <dx:RERToggleBandedRowsCommand>
                                    </dx:RERToggleBandedRowsCommand>
                                    <dx:RERToggleFirstColumnCommand>
                                    </dx:RERToggleFirstColumnCommand>
                                    <dx:RERToggleLastColumnCommand>
                                    </dx:RERToggleLastColumnCommand>
                                    <dx:RERToggleBandedColumnCommand>
                                    </dx:RERToggleBandedColumnCommand>
                                </Items>
                            </dx:RERTableStyleOptionsGroup>
                            <dx:RERTableStylesGroup>
                                <Items>
                                    <dx:RERChangeTableStyleCommand ImageHeight="36px" ImageWidth="60px" MaxColumnCount="10" MaxTextWidth="65px" MinColumnCount="2">
                                        <PropertiesDropDownGallery RowCount="3" />
                                    </dx:RERChangeTableStyleCommand>
                                </Items>
                            </dx:RERTableStylesGroup>
                            <dx:RERBordersAndShadingsGroup Text="Borders &amp; Shadings">
                                <Items>
                                    <dx:RERChangeCurrentBorderRepositoryItemLineStyleCommand>
                                        <PropertiesComboBox Width="100px">
                                        </PropertiesComboBox>
                                    </dx:RERChangeCurrentBorderRepositoryItemLineStyleCommand>
                                    <dx:RERChangeCurrentBorderRepositoryItemLineThicknessCommand>
                                        <PropertiesComboBox Width="100px">
                                        </PropertiesComboBox>
                                    </dx:RERChangeCurrentBorderRepositoryItemLineThicknessCommand>
                                    <dx:RERChangeCurrentBorderRepositoryItemColorCommand AutomaticColorItemCaption="Automatic" AutomaticColorItemValue="0" Color="Black" EnableAutomaticColorItem="True" EnableCustomColors="True" Size="Large">
                                    </dx:RERChangeCurrentBorderRepositoryItemColorCommand>
                                    <dx:RERChangeTableBordersCommand DropDownMode="False" Size="Large">
                                    </dx:RERChangeTableBordersCommand>
                                    <dx:RERChangeTableCellShadingCommand AutomaticColor="" AutomaticColorItemCaption="No Color" AutomaticColorItemValue="16777215" EnableAutomaticColorItem="True" EnableCustomColors="True" Size="Large">
                                    </dx:RERChangeTableCellShadingCommand>
                                </Items>
                            </dx:RERBordersAndShadingsGroup>
                        </Groups>
                    </dx:RERTableDesignTab>
                    <dx:RERTableLayoutTab>
                        <Groups>
                            <dx:RERTableGroup>
                                <Items>
                                    <dx:RERSelectTableElementsCommand DropDownMode="False" Size="Large">
                                    </dx:RERSelectTableElementsCommand>
                                    <dx:RERToggleShowTableGridLinesCommand Size="Large">
                                    </dx:RERToggleShowTableGridLinesCommand>
                                    <dx:RERShowTablePropertiesFormCommand Size="Large">
                                    </dx:RERShowTablePropertiesFormCommand>
                                </Items>
                            </dx:RERTableGroup>
                            <dx:RERRowAndColumnsGroup ShowDialogBoxLauncher="True">
                                <Items>
                                    <dx:RERDeleteTableElementsCommand DropDownMode="False" Size="Large">
                                    </dx:RERDeleteTableElementsCommand>
                                    <dx:RERInsertTableRowAboveCommand Size="Large">
                                    </dx:RERInsertTableRowAboveCommand>
                                    <dx:RERInsertTableRowBelowCommand Size="Large">
                                    </dx:RERInsertTableRowBelowCommand>
                                    <dx:RERInsertTableColumnToTheLeftCommand Size="Large">
                                    </dx:RERInsertTableColumnToTheLeftCommand>
                                    <dx:RERInsertTableColumnToTheRightCommand Size="Large">
                                    </dx:RERInsertTableColumnToTheRightCommand>
                                </Items>
                            </dx:RERRowAndColumnsGroup>
                            <dx:RERMergeGroup>
                                <Items>
                                    <dx:RERMergeTableCellsCommand Size="Large">
                                    </dx:RERMergeTableCellsCommand>
                                    <dx:RERSplitTableCellsCommand Size="Large">
                                    </dx:RERSplitTableCellsCommand>
                                </Items>
                            </dx:RERMergeGroup>
                            <dx:RERAlignmentGroup>
                                <Items>
                                    <dx:RERToggleTableCellsTopLeftAlignmentCommand>
                                    </dx:RERToggleTableCellsTopLeftAlignmentCommand>
                                    <dx:RERToggleTableCellsTopCenterAlignmentCommand>
                                    </dx:RERToggleTableCellsTopCenterAlignmentCommand>
                                    <dx:RERToggleTableCellsTopRightAlignmentCommand>
                                    </dx:RERToggleTableCellsTopRightAlignmentCommand>
                                    <dx:RERToggleTableCellsMiddleLeftAlignmentCommand>
                                    </dx:RERToggleTableCellsMiddleLeftAlignmentCommand>
                                    <dx:RERToggleTableCellsMiddleCenterAlignmentCommand>
                                    </dx:RERToggleTableCellsMiddleCenterAlignmentCommand>
                                    <dx:RERToggleTableCellsMiddleRightAlignmentCommand>
                                    </dx:RERToggleTableCellsMiddleRightAlignmentCommand>
                                    <dx:RERToggleTableCellsBottomLeftAlignmentCommand>
                                    </dx:RERToggleTableCellsBottomLeftAlignmentCommand>
                                    <dx:RERToggleTableCellsBottomCenterAlignmentCommand>
                                    </dx:RERToggleTableCellsBottomCenterAlignmentCommand>
                                    <dx:RERToggleTableCellsBottomRightAlignmentCommand>
                                    </dx:RERToggleTableCellsBottomRightAlignmentCommand>
                                    <dx:RERShowCellOptionsFormCommand Size="Large">
                                    </dx:RERShowCellOptionsFormCommand>
                                </Items>
                            </dx:RERAlignmentGroup>
                        </Groups>
                    </dx:RERTableLayoutTab>
                </Tabs>
            </dx:RERTableToolsContextTabCategory>
            <dx:RERHeaderAndFooterToolsContextTabCategory Color="23, 163, 0">
                <Tabs>
                    <dx:RERHeaderAndFooterTab Text="Header &amp; Footer">
                        <Groups>
                            <dx:RERHeaderFooterNavigationGroup>
                                <Items>
                                    <dx:RERGoToPageHeaderCommand Size="Large">
                                    </dx:RERGoToPageHeaderCommand>
                                    <dx:RERGoToPageFooterCommand Size="Large">
                                    </dx:RERGoToPageFooterCommand>
                                    <dx:RERGoToPreviousPageHeaderFooterCommand Size="Large">
                                    </dx:RERGoToPreviousPageHeaderFooterCommand>
                                    <dx:RERGoToNextPageHeaderFooterCommand Size="Large">
                                    </dx:RERGoToNextPageHeaderFooterCommand>
                                    <dx:RERToggleHeaderFooterLinkToPreviousCommand Size="Large">
                                    </dx:RERToggleHeaderFooterLinkToPreviousCommand>
                                </Items>
                            </dx:RERHeaderFooterNavigationGroup>
                            <dx:RERHeaderFooterOptionsGroup>
                                <Items>
                                    <dx:RERToggleDifferentFirstPageCommand>
                                    </dx:RERToggleDifferentFirstPageCommand>
                                    <dx:RERToggleDifferentOddAndEvenPagesCommand>
                                    </dx:RERToggleDifferentOddAndEvenPagesCommand>
                                </Items>
                            </dx:RERHeaderFooterOptionsGroup>
                            <dx:RERHeaderFooterCloseGroup>
                                <Items>
                                    <dx:RERClosePageHeaderFooterCommand Size="Large">
                                    </dx:RERClosePageHeaderFooterCommand>
                                </Items>
                            </dx:RERHeaderFooterCloseGroup>
                        </Groups>
                    </dx:RERHeaderAndFooterTab>
                </Tabs>
            </dx:RERHeaderAndFooterToolsContextTabCategory>
        </ContextTabCategories>
        <ClientSideEvents CommandExecuted="function(s, e) {
             App.onDocumentRibbonCommandExecuted(e.item.name, 'richtext');
         }" />
    </dx:ASPxRibbon>
    <dx:ASPxRichEdit ID="RichEdit" runat="server" Width="100%" Height="700px" ActiveTabIndex="0" 
        ShowConfirmOnLosingChanges="false" RibbonMode="ExternalRibbon" AssociatedRibbonID="DocumentRibbon"
        AutoSaveMode="On" AutoSaveTimeout="00:01:00" OnPreRender="RichEdit_PreRender">
        <Settings>
            <Behavior FullScreen="Hidden" />
            <HorizontalRuler Visibility="Hidden" />
        </Settings>
        <ClientSideEvents Init="App.onDocumentEditorInit"
            DocumentChanged="App.onDocumentEditorDocumentChanged"
            BeginSynchronization="App.onDocumentEditorBeginSynchronization" 
            EndSynchronization="App.onDocumentEditorEndSynchronization" />
    </dx:ASPxRichEdit>
</asp:Content>
