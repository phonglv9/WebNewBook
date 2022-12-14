var _URL = "https://localhost:7047/"; 
$(document).ready(function () {

    GetReport();
    $('#value_Type').change(function () {
        GetReport();
    });
    $("#btnExport").click(function () {
        let table = document.getElementsByTagName("table");
        TableToExcel.convert(table[0], { // html code may contain multiple tables so here we are refering to 1st table tag
            name: `BaoCaoDoanhThu.xlsx`, // fileName you could use any name
            sheet: {
                name: 'Sheet 1' // sheetName
            }
        });
    });
});

function GetFillterReprort() {

   
    var startDate = $('#date_start').val();
    var endDate = $('#date_end').val();
    console.log(startDate)
    console.log(endDate)
    //var f = new Date(startDate);
    //var t = new Date(endDate);
  
    var html = '';
    $.ajax(
        {
            type: 'GET',
            dataType: 'json',
            contentType: 'application/json',
            url: _URL + 'Report/GetFillterReport',
            data: {
                startDate: startDate,
                endDate: endDate
            },
            success: function (result) {

                var total_money = 0;
                var total_oder = 0;
                var total_product = 0;

                $.each(result, function (key, val) {

                    total_money += val.TotalMoney;
                    total_oder += val.DonHang;
                    total_product += val.SLSanPham;
                    html += '  <tr>';
                    html += '   <td> ' + val.DateValue + ' </td>'
                    html += '   <td> ' + formatVND(val.TotalMoney) + '</td>'
                    html += '   <td> ' + val.DonHang + '</td>'
                    html += '   <td> ' + val.SLSanPham + ' </td>'
                    html += '  </tr >'



                });

                $(".datatable_report").html(html);
                $("#total_money").text(formatVND(total_money));
                $("#total_oder").text(total_oder);
                $("#total_product").text(total_product);


                BarChartReport(result);


            }, error: function () {
                alert("Error loading data! Please try again.");
            }

        });
}
function GetReport() {
    var value_type = $("#value_Type").val();
    var html = '';
    $.ajax(
        {
            type: 'GET',
            dataType: 'json',
            contentType: 'application/json',
            url: _URL + 'Report/GetReportNewBook',
            data: {
                type: value_type,       
            },
            success: function (result) {
        
                var total_money = 0;
                var total_oder = 0;
                var total_product = 0;
              
                $.each(result, function (key, val) {

                    total_money += val.TotalMoney;
                    total_oder += val.DonHang;
                    total_product += val.SLSanPham;
                    html += '  <tr>';
                    html += '   <td> ' + val.DateValue + ' </td>'
                    html += '   <td> ' + formatVND(val.TotalMoney) + '</td>'
                    html += '   <td> ' + val.DonHang + '</td>'
                    html += '   <td> ' + val.SLSanPham + ' </td>'                 
                    html += '  </tr >'
                      
                            
                   
                });
               
                $(".datatable_report").html(html);
                $("#total_money").text(formatVND(total_money));
                $("#total_oder").text(total_oder);
                $("#total_product").text(total_product);
            
               
                BarChartReport(result);
          

            }, error: function () {
                alert("Error loading data! Please try again.");
            }

        });



}
function formatVND(money) {

    var temp = new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(money);
    return temp
}


function BarChartReport(data) {
    var labels_p = [];
    var dataTotal = [];
    $.each(data, function (key, val) {
        dataTotal.push(val.TotalMoney);
        labels_p.push(val.DateValue);
    });
   
    var dataSet = [{
        label: 'VND',
        data: dataTotal,
        backgroundColor: '#FFA000'
    }
    ]
    const ctx = document.getElementById('myBarChart').getContext('2d');
    if (window.bar != undefined)
        window.bar.destroy();
    window.bar = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels_p,
            datasets: dataSet
        },
        options: {
            scales: {
                xAxes: {
                   
                    ticks: {
                        callback: function (dataTotal) {
                            return formatVND(dataTotal)
                        }
                    }
                    
                },
                yAxes: {
                   


                }
            }, responsive: true,
            maintainAspectRatio: false
        }
    });      
}