
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

    $('.fademe').click(function () {
        $('.fademe').fadeOut('slow');
    });

    $('#loginDiv').click(function (event) {
        event.stopPropagation();
    });

});

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

/*
	Funções da tela de cadastro
*/

function cleanMessages() {
	$('#passVerification').removeClass('messageError');
	$('#passVerification').removeClass('messageWarning');
	$('#passVerification').removeClass('messageSuccess');
	$('#passVerification').html('');
}

function GeneralSubmitCreateUser() {
	if (!$('#passVerification').hasClass('messageError')) {
		// Verifica se todos os campos obrigatórios foram preenchidos
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



