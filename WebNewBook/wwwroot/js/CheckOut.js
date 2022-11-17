// Document is ready
$(document).ready(function () {
    var regexEmail = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;
   var  regexPhoneNumber = /(84|0[3|5|7|8|9])+([0-9]{8})\b/g;
    // Validate fullname
    $("#fullnamecheck").hide();
    let fullnameError = true;
    $("#fullname").keyup(function () {
        validatefullname();
    });
    function validatefullname() {
        let fullnameValue = $("#fullname").val();
        if (fullnameValue.length == "") {
            $("#fullnamecheck").show();
            $("#fullnamecheck").html("*Tên người nhận là bắt buộc");
            fullnameError = false;
            return false;
        } else if (fullnameValue.length < 3 || fullnameValue.length > 30) {
            $("#fullnamecheck").show();
            $("#fullnamecheck").html("*Tên người nhận không hợp lệ");
            fullnameError = false;
            return false;
        } else {
            fullnameError = true;
            $("#fullnamecheck").hide();
        }
    }
    // Validate Email
    $("#emailcheck").hide();
    let emailError = true;
    $("#email").keyup(function () {
        validateEmail();
    });
    function validateEmail() {
        let emailValue = $("#email").val();
        if (emailValue.length == "") {
            $("#emailcheck").show();
            $("#emailcheck").html("*Email là bắt buộc");
            emailError = false;
            return false;

        } else if (!regexEmail.test(emailValue)) {
            $("#emailcheck").show();
            $("#emailcheck").html("*Email không hợp lệ");
            emailError = false;
            return false;

        } else {
            emailError = true;
            $("#emailcheck").hide();
        }
    }
    // Validate adress
    $("#adresscheck").hide();
    let adressError = true;
    $("#adress").keyup(function () {
        validateAdress();
    });
    function validateAdress() {
        let adressValue = $("#adress").val();
        if (adressValue.length == "") {
            $("#adresscheck").show();
            $("#adresscheck").html("*Địa chỉ là bắt buộc");
            adressError = false;
            return false;

        } else if (adressValue.length < 10 && adressValue.length > 300) {
            $("#adresscheck").show();
            $("#adresscheck").html("*Địa chỉ không hợp lệ");
            adressError = false;
            return false;
        } else {
            adressError = true;
            $("#adresscheck").hide();
        }
    }
    // Validate phonenumber
    $("#phonenumbercheck").hide();
    let phonenumberError = true;
    $("#phonenumber").keyup(function () {
        validatePhonenumber();
    });
    function validatePhonenumber() {
        let phonenumberValue = $("#phonenumber").val();
        if (phonenumberValue.length == "") {
            $("#phonenumbercheck").show();
            $("#phonenumbercheck").html("*Số điện thoại là bắt buộc là bắt buộc");
            phonenumberError = false;
            return false;
        } else if (!regexPhoneNumber.test(phonenumberValue)) {
            $("#phonenumbercheck").show();
            $("#phonenumbercheck").html("*Số điện thoại không hợp lệ");
            emailError = false;
            return false;

        } else {
            phonenumberError = true;
            $("#phonenumbercheck").hide();
        }
    }



    // Submit button
    $("#submitbtn").click(function () {
        validatefullname();
        validateEmail();
        validateAdress();
        validatePhonenumber();
        validateRadio();
        if (
            fullnameError == true && emailError == true && adressError == true && phonenumberError == true 
        ){
            return true;
        } else {
            return false;
        }
    });
});