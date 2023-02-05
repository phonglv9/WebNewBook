// Document is ready
var _URL = "https://localhost:7047/";
const VND = new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND',
});
$(document).ready(function () {

   
    $("#phiship").hide();
    //Chọn tỉnh thành
    $('#provin').change(function () {
        loadTotal();
        var id_provin = this.value;
        $('#district option').remove();
        $('#district').append(new Option("-- Chọn quận/huyện --",0));

        $('#ward option').remove();
        $('#ward').append(new Option("-- Chọn phường/xã --", 0));

        if (this.value != 0 ) {

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
        loadTotal();
        var id_ward = this.value;
        $('#ward option').remove();
        $('#ward').append(new Option("-- Chọn phường/xã --", 0));

        if (this.value != 0 ) {

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
   
    function formatVND(val) {


        return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(val);


    }
    function loadTotal() {
        $("#phiship").hide();
        $("#totalship").text('');
        $.ajax({
            url: _URL + 'Payment/GetTotalOder',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            success: function (result) {
               
                $("#totaloder").text(formatVND(parseInt(result)));
            }

        });
    }
    //Tính phí ship
    //Get xã
    $('#ward').change(function () {
        var id_ward = this.value;
        var totalOder = 0;      
        sessionStorage.removeItem('shiptotal');
        $.ajax({
            url: _URL + 'Payment/GetTotalOder',
            type: 'POST',
            dataType: 'json',         
            contentType: 'application/json',
            success: function (result) {
                totalOder = parseInt(result);
                $("#totaloder").text(totalOder);
            }

        });

        $("#phiship").hide();
        $("#totalship").text('');
       
        if (this.value != 0) {

            var obj = {
                service_id: 100039,
                insurance_value: 100000,
                service_type_id: 3,
                coupon: null,
                from_district_id: 3440,
                to_ward_code: id_ward,
                to_district_id: parseInt($('#district').val()),
                weight: 1000,
                length: 15,
                height: 15,
                width: 15,  

            }
            $.ajax({
                url: _URL + 'Payment/GetTotalShipping',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(obj),
                contentType: 'application/json',
                success: function (result) {
                    $("#phiship").show();
                    $("#totalship").text(formatVND(result.data.total));

                    var temp = result.data.total + totalOder;
                    $("#totaloder").text(formatVND(temp));

                    sessionStorage.setItem("shiptotal", result.data.total);
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
    var selectError = true;
    $("#adress2check").hide();
    function validateSelect() {

        var temp = parseInt($("#ward").val());
        if (temp < 1) {
            $("#adress2check").show();
            $("#adress2check").html("*Vui lòng chọn địa chỉ đầy đủ");
            selectError = false;
            return false;
        } else {
            $("#adress2check").hide();
            selectError = true;
        }
    }

    // Submit button
    $("#submitbtn").click(function () {
        validatefullname();
        validateEmail();
        validatePhonenumber();
        validateAdress();
        validateSelect();
        
        if (
            fullnameError == true && emailError == true && phonenumberError == true && adressError == true && selectError == true
        ){
            return true;
        } else {
            $('#exampleModal').modal('hide');
            return false;
        }
    });
});


