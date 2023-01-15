// Document is ready
var _URL = "https://localhost:7047/";
$(document).ready(function () {
    
    //Chọn tỉnh thành
    $('#provin').change(function () {
        var id_provin = this.value;
        $('#district option').remove();
        $('#district').append(new Option("-- Chọn quận/huyện --", 0));

        $('#ward option').remove();
        $('#ward').append(new Option("-- Chọn phường/xã --", 0));

        if (this.value > 1) {

            $.ajax({
                url: _URL + 'Payment/GetListDistrict',
                type: 'GET',
                dataType: 'json',
                data: {
                    idProvin: id_provin

                 },
                contentType: 'application/json',
                success: function (result) {

                    $.each(result.data, function (key, val) {
                        $("#district").append(new Option(val.DistrictName,val.DistrictID));
                    });
                  
                }

            });


        }

    });
    //Chọn quận huyện
    $('#district').change(function () {
        var id_ward = this.value;
        $('#ward option').remove();
        $('#ward').append(new Option("-- Chọn phường/xã --", 0));

        if (this.value > 1) {

            $.ajax({
                url: _URL + 'Payment/GetListWard',
                type: 'GET',
                dataType: 'json',
                data: {
                    idWard: id_ward

                },
                contentType: 'application/json',
                success: function (result) {

                    $.each(result.data, function (key, val) {
                        $("#ward").append(new Option(val.WardName, val.WardCode));
                    });
                   
                }

            });


        }

    });
    //Tính phí ship
    $('#ward').change(function () {
        var id_ward = this.value;
      
        if (this.value > 1) {

            var obj = {
                service_id: 53321,
                insurance_value: 20000,
                service_type_id: 2,
                coupon: null,
                from_district_id: 3440,
                to_ward_code: id_ward,
                to_district_id: parseInt($('#district').val()),
                weight: 1000,
                length: 15,
                height: 15,
                width: 15,  

            }
            console.log(obj);

            $.ajax({
                url: _URL + 'Payment/GetTotalShipping',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(obj),
                contentType: 'application/json',
                success: function (result) {

                    
                }

            });


        }

    });







   








    var regexEmail = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;
    var regexPhoneNumber = /(84|0[3|5|7|8|9])+([0-9]{8})\b/g;
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
    var phonenumberError = true;
    //$("#phonenumber").keyup(function () {
    //    validatePhonenumber();
    //});
    function validatePhonenumber() {
        var phonenumberValue = $("#phonenumber").val();
        if (phonenumberValue.length == "") {
            $("#phonenumbercheck").show();
            $("#phonenumbercheck").html("*Số điện thoại là bắt buộc là bắt buộc");
            phonenumberError = false;
            return false;
        } else if (!regexPhoneNumber.test(phonenumberValue)) {
            $("#phonenumbercheck").show();
            $("#phonenumbercheck").html("*Số điện thoại không hợp lệ");
            phonenumberError = false;
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
        validatePhonenumber();
        validateAdress();
       
        
        if (
            fullnameError == true && emailError == true && phonenumberError == true && adressError == true
        ){
            return true;
        } else {
           
            return false;
        }
    });
});


