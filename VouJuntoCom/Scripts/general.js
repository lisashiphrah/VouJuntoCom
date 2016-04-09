
/*
* Define funções gerais presentes no sistema VouJunto.com
*/

$(document).ready(function () {
    //Valida todas as datas na perda de foco
    $('.date').focusout(function () {
        ValidateDate(this);
    });
    //Valida todos os emails na perda de foco
    $('.email').focusout(function () {
        ValidateEmail(this);
    });

    $('.time').focusout(function () {
        ValidateTime(this);
    });

    $('.fademe').click(function () {
        $('.fademe').hide()
        $('#popupWindow').hide();
    });

    $('#loginDiv').click(function (event) {
        event.stopPropagation();
    });

    $(".numbers").keydown(function (event) {
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
            (event.keyCode == 65 && event.ctrlKey === true) ||
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            return;
        }
        else {
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });

});

function OpenPopup(height, width, content) {
    $('#popupWindow').css('width', '' + width + 'px');
    $('#popupWindow').css('height', '' + height + 'px');
    $('#popupWindow').css('margin-top', '-' + height / 2 + 'px');
    $('#popupWindow').css('margin-left', '-' + width / 2 + 'px');
    $('#popupWindow').html(content);
    $('#darkBackground').show();
    $('#popupWindow').show();
}

function ResizePopup(height, width) {
    $('#popupWindow').css('width', '' + width + 'px');
    $('#popupWindow').css('height', '' + height + 'px');
    $('#popupWindow').css('margin-top', '-' + height / 2 + 'px');
    $('#popupWindow').css('margin-left', '-' + width / 2 + 'px');
}

function ShowErrorDialog(message) {
    $('#messageError').html(message);
    $('#messageError').show();
}

function ValidateDate(element) {
    var currVal = $(element).val();
    if (currVal == '') {
        return false;
    }

    //Declare Regex  
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null) {
        return false;
    }

    //Checks for dd/mm/yyyy format.
    dtDay = dtArray[1];
    dtMonth = dtArray[3];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12) {
        $(element).val('');
        $(element).addClass('errorField');
        return false;
    }
    else if (dtDay < 1 || dtDay > 31) {
        $(element).val('');
        $(element).addClass('errorField');
        return false;
    }
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31) {
        $(element).val('');
        $(element).addClass('errorField');
        return false;
    }
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap)) {
            $(element).val('');
            $(element).addClass('errorField');
            return false;
        }
    }
    $(element).removeClass('errorField');
    return true;
}

function ValidateEmail(element) {
    var hasError = false;
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    var emailaddressVal = $(element).val();
    if (emailaddressVal == '') {
        $(element).val('');
        $(element).addClass('errorField');
        hasError = true;
    }
    else if (!emailReg.test(emailaddressVal)) {
        $(element).val('');
        $(element).addClass('errorField');
        hasError = true;
    }
    if (hasError == true) {
        return false;
    }
    else {
        $(element).removeClass('errorField');
        return true;
    }
}

function ValidateTime(element) {
    var timeStr = $(element).val();
    var valid = (timeStr.substr(0, 2) >= 0 && timeStr.substr(0, 2) <= 24) &&
            (timeStr.substr(3, 2) >= 0 && timeStr.substr(3, 2) <= 59);
    if (valid == false) {
        $(element).addClass('errorField');
    }
    else {
        $(element).removeClass('errorField');
    }
    return valid;
}

/*
	Funções da tela de cadastro
*/

function cleanMessages() {
    $('#passVerification').removeClass('messageError');
    $('#passVerification').removeClass('messageWarning');
    $('#passVerification').removeClass('messageSuccess');
    $('#passVerification').html('');
}

function ValidateFields() {
    var validate = true;
    $('.required').each(function () {
        if ($.trim($(this).val()).length == 0) {
            validate = false;
            $(this).addClass('errorField');
        }
        else {
            $(this).removeClass("errorField");
        }
    });

    if (validate) {
        return true;
    }
}

function ValidateFields(formID) {
    var validate = true;
    $('#' + formID + ' .required').each(function () {
        if ($.trim($(this).val()).length == 0) {
            validate = false;
            $(this).addClass('errorField');
        }
        else {
            $(this).removeClass("errorField");
        }
    });

    if (validate) {
        return true;
    }
}

function GeneralSubmitCreateUser() {
    if (!$('#passVerification').hasClass('messageError')) {
        // Verifica se todos os campos obrigatórios foram preenchidos
        var validate = ValidateFields();
        if (validate) {
            //Verifica se senha confere com confirmação
            var pass = $('#Password').val();
            var conf = $('#passwordConfirmation').val();
            if (pass == conf) {
                $('#Password').removeClass('errorField');
                $('#passwordConfirmation').removeClass('errorField');
                $('#messageError').hide();
                $('form').submit();
            }
            else {
                $('#Password').addClass('errorField');
                $('#passwordConfirmation').addClass('errorField');
                ShowErrorDialog('Senha e confirmação de senha diferentes.');
            }
        }
        else {
            ShowErrorDialog('Há campos obrigatórios não preenchidos.');
        }
    }
}

/*

    Funções específicas para Views da CarsController

*/

function DisableChanges(tr_ID) {

    /// <summary>Desabilita os campos da tabela para não permitir sua modificação.</summary>
    /// <param name="tr_ID">ID da tabela / carro</param>

    $('input').each(function () {
        $(this).attr('disabled', true);
        if ($(this).hasClass('inputEnabled')) {
            $(this).addClass('inputDisabled');
            $(this).removeClass('inputEnabled');
        }
    });

    $('#' + tr_ID + ' .imgEdit').show();
    $('#' + tr_ID + ' .imgSave').hide();
    $('#' + tr_ID + ' .imgRemove').attr('title', 'Remover Veículo');
    $('#' + tr_ID + ' .imgRemove').attr('alt', 'Remover Veículo');
    $('#' + tr_ID + ' .imgRemove').attr('onClick', 'Remove(\'' + tr_ID + '\')');
}

function EnableChanges(tr_ID) {

    /// <summary>Habilita os campos da tabela para permitir sua modificação.</summary>
    /// <param name="tr_ID">ID da tabela / carro</param>

    $('#' + tr_ID + ' input').each(function () {
        $(this).attr('disabled', false);
        if ($(this).hasClass('inputDisabled')) {
            $(this).addClass('inputEnabled');
            $(this).removeClass('inputDisabled');
        }
    });

    $('#' + tr_ID + ' .imgEdit').hide();
    $('#' + tr_ID + ' .imgSave').show();
    $('#' + tr_ID + ' .imgRemove').attr('title', 'Cancelar Alterações');
    $('#' + tr_ID + ' .imgRemove').attr('alt', 'Cancelar Alterações');
    $('#' + tr_ID + ' .imgRemove').attr('onClick', 'DisableChanges(\'' + tr_ID + '\')');
}

/*

    Funções para HOME

*/

function OpenOferecidas() {
    if ($('#liAceitas').hasClass('active')) {
        $('#tableAceitas').hide();
        $('#tableOferecidas').show();
        $('#liOferecidas').addClass('active');
        $('#liAceitas').removeClass('active');
    }
}

function OpenAceitas() {
    if ($('#liOferecidas').hasClass('active')) {
        $('#tableAceitas').show();
        $('#tableOferecidas').hide();
        $('#liAceitas').addClass('active');
        $('#liOferecidas').removeClass('active');
    }
}

function OpenRecebidas() {
    if ($('#liEnviadas').hasClass('active')) {
        $('#message_sended').hide();
        $('#message_received').show();
        $('#liRecebidas').addClass('active');
        $('#liEnviadas').removeClass('active');
    }
}

function OpenEnviadas() {
    if ($('#liRecebidas').hasClass('active')) {
        $('#message_received').hide();
        $('#message_sended').show();
        $('#liEnviadas').addClass('active');
        $('#liRecebidas').removeClass('active');
    }
}

