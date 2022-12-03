//var _Url = "https://localhost:7047/"

//$(document).ready(function () {

//    GetAllSell();
//});
//function GetAllSell() {
//    var html = '';
//    $.ajax(
//        {
//            type: 'GET',
//            dataType: 'json',
//            contentType: 'application/json',
//            url: _Url +'Home/GetOderSell',
//            success: function (result) {
//                $.each(result, function (key, item) {
                   
//                    html += '<div class="product>';
//                    html += '<div class="product-img">';
//                    html += '<img src="./img/product01.png" alt="">';
//                    html += '</div>';
//                    html += '</div>';
//                    html += '<div class="product-body">';
//                    html += '<p class="product-category">Category</p>';
//                    html += '<h3 class="product-name"><a href="#">product name goes here</a></h3>';
//                    html += '<h4 class="product-price"> 980.00 <del class="product-old-price"> 990.00</del></h4>';
//                    html += '</div>'
//                    html += '<div class="add-to-cart">'
//                    html += '<button class="add-to-cart-btn"><i class="fa fa-shopping-cart"></i> Thêm vào giỏ</button>'
//                    html += '</div>'
//                    html += '</div>';
//                });
//                $('#data_sell').html(html);
               
//            },

//            error: function () {
//                alert("Error loading data! Please try again.");
//            }
//        });
//}