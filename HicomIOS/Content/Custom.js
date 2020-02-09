﻿$(document).ready(function () {
    var hasValid = false;
    $("#btnSaveDraft").click(function () {
        $("#div-content .chkValidate").each(function () {
            if ($(this).val() == "") {
                hasValid = true;
                $(this).css("border", "1px solid #de2a2a");
            }
        });

        if (hasValid) {
            return false;
        }
    });

    $("#div-content .chkValidate").keyup(function () {
        if ($(this).val() != "") {
            $(this).css("border", "0");
        }
    });

    /*$(".chkValidate").each(function () {
        var id = $(this).attr('id');

        //check null
        if ($("#" + id).val() === "") {
            var elm = $("#" + id);
            var error_elm = '<span id="' + (elm.attr('id')) + '-error" style="color: #e40703;" class="help-block">' + elm.attr("data-msg-required") + '</span>';
            elm.parent().addclass('has-error');

            if ($('#' + elm.attr('id') + '-error').hasClass('help-block') === false) {
                elm.after(error_elm);
            }
            return false;
        }

        //key up
        $("#" + id).keyup(function () {
            if ($("#" + id).val != "") {
                $("#" + id).parent().removeClass("has-error");
                $("#" + id).next().remove('.help-block');
            }
        });
    });*/
});

function disableInput() {
    $('input, textarea, select').prop('disabled', true);
    $('button').prop('disabled', true);
    $('button').addClass('disabled');
    $('#btnBack').prop('disabled', false);
    $('#btnBack').removeClass('disabled');

    $('#btnConfirm').prop('disabled', false);
    $('#btnConfirm').removeClass('disabled');

    $('.dxeEditAreaSys').prop('disabled', true);
    $('.dxeEditAreaSys').addClass('readonly');
    $('.noDisabled').prop('disabled', false);
    $('.noDisabled').removeClass('disabled');
    $("a").addClass('disabled');
    $('.dxeIRadioButton_Office2010Blue').prop('disabled', true);
    $('.dxeIRadioButton_Office2010Blue').addClass('readonly');
}

function readOnyInput() {
    $('input, textarea, select').prop('readonly', true);
    $('button').prop('readonly', true);
    $('button').addClass('readonly');
    $('#btnBack').prop('readonly', false);
    $('#btnBack').removeClass('readonly');

    $('.dxeEditAreaSys').prop('disabled', true);
    $('.dxeEditAreaSys').addClass('disabled');
}


function setCheckboxIsEnableSwitch() {
    $("[name='is_enable']").bootstrapSwitch();
}

function validateCombobox(cboId, cboVal) {

    isValid = false;
    if (cboVal == null) {
        $("#" + cboId).parent().parent().addClass("has-error");
        isValid = true;
    } else {
        $("#" + cboId).parent().parent().removeClass("has-error");
    }

    console.log(cboId + " : " + cboVal);

    return isValid;
}

function clearHasError() {
    $("#modal_form input[type=text]").keyup(function () {
        if ($(this).val() != "") {
            $(this).parent().parent().removeClass("has-error");
        }
    });
}

function getSum(total, num) {
    return total + num;
}