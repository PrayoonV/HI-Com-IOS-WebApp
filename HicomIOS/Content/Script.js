window.SPlanet = (function () {

    // Common function for all control to trap event
    var callbackHelper = (function () {
        $.LoadingOverlay("show", {
            zIndex: 9999
        });
        var callbackControlQueue = [],
            currentCallbackControl = null;

        function doCallback(callbackControl, args, sender, beforeCallback, afterCallback) {
            if (!currentCallbackControl) {
                currentCallbackControl = callbackControl;
                if (typeof (detailsCallbackPanel) !== "undefined" && callbackControl == mainCallbackPanel)
                    detailsCallbackPanel.cpSkipUpdateDetails = true;
                if (!callbackControl.cpHasEndCallbackHandler) {
                    callbackControl.EndCallback.AddHandler(onEndCallback);
                    callbackControl.cpHasEndCallbackHandler = true;
                    callbackControl.cpAfterCallback = afterCallback;
                }
                if (beforeCallback)
                    beforeCallback();
                callbackControl.PerformCallback(args);
            } else
                placeInQueue(callbackControl.name, args, getSenderId(sender));
        };
        function getSenderId(senderObject) {
            if (senderObject.constructor === String)
                return senderObject;
            return senderObject.name || senderObject.id;
        };
        function placeInQueue(callbackControlName, args, sender) {
            var queue = callbackControlQueue;
            for (var i = 0; i < queue.length; i++) {
                if (queue[i].controlName == callbackControlName && queue[i].sender == sender) {
                    queue[i].args = args;
                    return;
                }
            }
            queue.push({ controlName: callbackControlName, args: args, sender: sender });
        };
        function onEndCallback(sender) {
            var queueItem;
            var queuedControl;
            //ASPxClientEdit.ClearEditorsInContainer(sender);
            do {
                queueItem = callbackControlQueue.shift();
                queuedControl = queueItem ? getControlInstance(queueItem.controlName) : null;
            } while (!queuedControl && callbackControlQueue.length > 0);

            if (!queuedControl || queuedControl != sender) {
                sender.EndCallback.RemoveHandler(onEndCallback);
                sender.cpHasEndCallbackHandler = false;
                if (sender.cpAfterCallback)
                    sender.cpAfterCallback();
            }

            currentCallbackControl = null;
            if (queuedControl)
                doCallback(queuedControl, queueItem.args, queueItem.sender);
        }

        function getControlInstance(name) {
            var controls = ASPx.GetControlCollection().GetControlsByPredicate(function (c) { return c.name === name });
            return controls && controls.length > 0 ? controls[0] : null;
        }



        $.LoadingOverlay("hide");
        return {
            DoCallback: doCallback
        };

    })();


    // For Grid Customize Column button
    function gridCustomizationWindow_CloseUp() {
        toolbarMenu.GetItemByName("ColumnsCustomization").SetChecked(false);
    };
    function setToolbarCWItemEnabled(enabled) {
        var item = toolbarMenu.GetItemByName("ColumnsCustomization");
        if (!item)
            return;
        item.SetEnabled(enabled);
        item.SetChecked(false);
    }

    // Event from MainCallBack Control
    function saveEditForm(popup, args, isDetail) {
        $.LoadingOverlay("show", {
            zIndex: 9999
        });
        if (!ASPxClientEdit.ValidateEditorsInContainer(popup.GetMainElement()))
            return;
        popup.Hide();
        //if (checkReadOnlyMode())
        //    return;
        var callbackArgs = ["SaveEditForm", popup.cpEditFormName, args];
        //var panel = isDetail ? detailsCallbackPanel : mainCallbackPanel;
        var panel = mainCallbackPanel;
        callbackHelper.DoCallback(panel, serializeArgs(callbackArgs), popup);
        $.LoadingOverlay("hide");
    };

    // Open Report Viewer & Spreadsheet
    function openReport(reportName, itemID) {
        var url = "DocumentViewer.aspx?ReportArgs=" + serializeArgs([reportName, itemID]);
        openPageViewerPopup(url, reportName);
    };
    function openSpreadsheet(reportName, itemID) {
        var url = "Spreadsheet.aspx?ReportArgs=" + serializeArgs([reportName, itemID]);
        openPageViewerPopup(url, reportName);
    };
    function openPageViewerPopup(contentUrl, reportName) {
        pageViewerPopup.SetHeaderText(pageViewerPopup.cpReportDisplayNames[reportName]);
        pageViewerPopup.Show();
        pageViewerPopup.SetContentUrl(contentUrl);
    };

    //==== Function and Event of Page ===
    var objPage = (function () {
        $.LoadingOverlay("show", {
            zIndex: 9999
        });

        // Toolbar for emplyee
        function toolbarMenu_ItemClick(s, e) {
            var dataID = getSelectedDataID();
            dataID = dataID === null ? -1 : dataID; // Modified by pich 10/10/2017
            //ASPxClientEdit.ClearEditorsInContainer(document.getElementById("EditFormsContainer"));
            if (!dataID)
                return;
            var name = e.item.name;
            switch (name) {
                case "New":
                    addNewData();
                    break;
                case "Delete":
                    deleteData(dataID, s);
                    break;
                case "ColumnsCustomization":
                    if (objGridView.IsCustomizationWindowVisible())
                        objGridView.HideCustomizationWindow();
                    else
                        objGridView.ShowCustomizationWindow(e.htmlElement);
                    break;
            }
        }
        // Grid View
        function controlGrid_Init(s, e) {
            //setToolbarCWItemEnabled(true);
            var a = $('#' + s.name);
            //var height = document.documentElement.clientHeight;
            //a[0].Height(height);
            //s.name.Height(Math.max(0, document.documentElement.clientHeight));

        }
        function controlGrid_ContextMenuItemClick(s, e) {
            switch (e.item.name) {
                case "NewRow":
                    addNewData();
                    e.handled = true;
                    break;
                case "EditRow":
                    editData(s.GetRowKey(e.elementIndex), s);
                    e.handled = true;
                    break;
                case "DeleteRow":
                    deleteData(s.GetRowKey(e.elementIndex), s);
                    e.handled = true;
                    break;
                case "Refresh":
                    refreshData(s);
                    e.handled = true;
                    break;
            }
        }
        // Add/Edit/Delete Actions for emplyee
        function addNewData() {

            console.log(HicomPageName);
            objEditPopup.cpPrimaryKeyValue = -1;
            objEditPopup.SetHeaderText("New " + HicomPageName);
            showClearedPopup(objEditPopup);
            firstFocusTextBox.Focus();
        }
        function editData(id, sender) { // TODO
            $.LoadingOverlay("show", {
                zIndex: 9999
            });
            objEditPopup.SetHeaderText("Edit " + HicomPageName);
            showClearedPopup(objEditPopup);
            callbackHelper.DoCallback(objEditPopup, id, sender);
            $.LoadingOverlay("hide");
        }
        function deleteData(id, sender) {

            //if (checkReadOnlyMode())
            //    return;
            if (confirm("Remove " + HicomPageName + " Data [ID : " + id + "] ?"))
                callbackHelper.DoCallback(mainCallbackPanel, serializeArgs(["DeleteEntry", id]), sender);
        }
        function refreshData(sender) {
            callbackHelper.DoCallback(mainCallbackPanel, serializeArgs(["RefreshEntry"]), sender);
        }

        // General Function
        function enableToolbarMenu() {
            toolbarMenu.SetEnabled(true);
        }
        function disableToolbarMenu() {
            toolbarMenu.SetEnabled(false);
        }
        function getSelectedDataID() {
            var getIndex, getKey;
            try {
                getIndex = objGridView.GetFocusedRowIndex.aspxBind(objGridView);
                getKey = objGridView.GetRowKey.aspxBind(objGridView);
                if (getIndex() >= 0)
                    return getKey(getIndex());
            } catch (e) {
                throw e;
            }
            return null;
        }
        function getSelectedItemID() {
            return getSelectedDataID();
        }
        $.LoadingOverlay("hide");
        // Return Events
        return {
            ToolbarMenu_ItemClick: toolbarMenu_ItemClick,
            ControlGrid_Init: controlGrid_Init,
            ControlGrid_ContextMenuItemClick: controlGrid_ContextMenuItemClick,
            GetSelectedItemID: getSelectedItemID,
            EditData: editData,
            DeleteData: deleteData
        };
    })();



    //==== Main Event =====
    //==== These function must always  place bettom script ====
    function getCurrentPage() {
        var pageName = HicomPageName;
        /*
        switch (pageName) {
            case "Employee":
                return objPage;
        }
        */
        return objPage;

    };

    var page = getCurrentPage();

    // Filter Control Popup
    function filterControl_Applied(s, e) {
        changeFilter(e.filterExpression, s);
    }
    function changeFilter(expression, sender) {
        callbackHelper.DoCallback(mainCallbackPanel, serializeArgs(["FilterChanged", expression]), sender);
    }
    function serializeArgs(args) {
        var result = [];
        for (var i = 0; i < args.length; i++) {
            var value = args[i] ? args[i].toString() : "";
            result.push(value);
            result.push("|");
        }
        return result.join("");
    }
    function saveCustomFilterCheckBox_CheckedChanged(s, e) {
        customFilterTextBox.SetEnabled(s.GetChecked());
        customFilterTextBox.SetIsValid(true);
    }
    function customFilterTextBox_Validation(s, e) {
        e.isValid = !!e.value || !saveCustomFilterCheckBox.GetChecked();
    }
    function saveFilterButton_Click(s, e) {
        if (saveCustomFilterCheckBox.GetChecked()) {
            var validated = ASPxClientEdit.ValidateEditorsInContainer(filterPopup.GetMainElement());
            if (validated)
                filterPopup.Hide();
            return;
        }
        e.processOnServer = false;
        filterPopup.Hide();
        filterControl.Apply();
    }
    function cancelFilterButton_Click(s, e) {
        filterPopup.Hide();
    }

    // Event relate with mainCallbackPanel_EndCallback
    function mainCallbackPanel_EndCallback(s, e) {
        //TODO: If want to do after end of saveform
    }

    // PageViewer for popup ReportViewer and Spreadsheet Viewer
    function pageViewerPopup_Shown(s, e) {
        preparePopupWithIframe(s);
    }
    function pageViewerPopup_CloseUp(s, e) {
        s.SetContentUrl("");
    }
    function preparePopupWithIframe(popup) {
        var iframe = popup.GetContentIFrame();
        setAttribute(iframe, "scrolling", "no");
        iframe.style.overflow = "hidden";
    };

    // Show Popup UserControl
    function showClearedPopup(popup) {

        popup.Show();
        //setTimeout(function () {
        ASPxClientEdit.ClearEditorsInContainer(document.getElementById("EditFormsContainer"));
        //}, 500);
        //ASPxClientEdit.ClearEditorsInContainer('EditFormsContainer');

    };

    function toolbarMenu_ItemClick(s, e) {
        var name = e.item.name;
        var selectedItemID = page.GetSelectedItemID && page.GetSelectedItemID();
        if (name === "Print" || e.item.parent && e.item.parent.name === "Print")
            openReport(s.cpReportNames[name], selectedItemID);
        if (name === "ExportToSpreadsheet")
            openSpreadsheet(s.cpReportNames[name], selectedItemID);
        if (name === "Filter")
            filterPopup.Show();
        page.ToolbarMenu_ItemClick(s, e);

    }

    // Event buttons from Edit Form 
    function saveButton_Click(s, e) {
        var commandName = objEditPopup.cpPrimaryKeyValue > 0 ? "Edit" : "New";
        console.log(commandName);
        saveEditForm(objEditPopup, serializeArgs([commandName, objEditPopup.cpPrimaryKeyValue]));
        objEditPopup.cpPrimaryKeyValue = -1;
    };
    function cancelButton_Click(s, e) {

        console.log(objEditPopup.cpPrimaryKeyValue);
        objEditPopup.Hide();
        //callbackControl.PerformCallback(document.location.pathname);
        location.reload();
        //callbackHelper.DoCallback(s, serializeArgs(callbackArgs));
        //showClearedPopup(objEditPopup.name)
    };

    return {
        Page: page,
        ToolbarMenu_ItemClick: toolbarMenu_ItemClick,
        FilterControl_Applied: filterControl_Applied,
        SaveCustomFilterCheckBox_CheckedChanged: saveCustomFilterCheckBox_CheckedChanged,
        CustomFilterTextBox_Validation: customFilterTextBox_Validation,
        SaveFilterButton_Click: saveFilterButton_Click,
        CancelFilterButton_Click: cancelFilterButton_Click,
        MainCallbackPanel_EndCallback: mainCallbackPanel_EndCallback,
        PageViewerPopup_Shown: pageViewerPopup_Shown,
        PageViewerPopup_CloseUp: pageViewerPopup_CloseUp,
        SaveButton_Click: saveButton_Click,
        CancelButton_Click: cancelButton_Click
    };
    //==== End of Main Event =====

})();
