// Document is ready
$(document).ready(function () {
    // Validate Username
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
        } else if (fullnameValue.length < 3 || fullnameValue.length > 10) {
            $("#fullnamecheck").show();
            $("#fullnamecheck").html("*Tên người nhận không hợp lệ");
            fullnameError = false;
            return false;
        } else {
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
            $("#eamilcheck").html("*Email là bắt buộc");
            emailError = false;
            return false;
        
        } else {
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
            $("#adresscheck").hide();
        }
    }
    // Validate adress
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
            adressError = false;
            return false;  
        } else {
            $("#phonenumbercheck").hide();
        }
    }

 

    // Submit button
    $("#submitbtn").click(function () {
        validateUsername();
        validateEmail();
        if (
            fullnameError == true 
        ) {
            return true;
        } else {
            return false;
        }
    });
});