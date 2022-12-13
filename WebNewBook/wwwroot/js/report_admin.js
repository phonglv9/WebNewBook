var _URL = "https://localhost:7047/"; 
$(document).ready(function () {

    GetReport();
    $('#value_Type').change(function () {
        GetReport();
    });

});
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
                    html += '   <td> ' + val.TotalMoney + '</td>'
                    html += '   <td> ' + val.DonHang + '</td>'
                    html += '   <td> ' + val.SLSanPham + ' </td>'                 
                    html += '  </tr >'
                      
                            
                   
                });
                $(".datatable_report").html(html);
                $("#total_money").text(total_money);
                $("#total_oder").text(total_oder);
                $("#total_product").text(total_product);
            
               
                BarChartReport(result);
                





            }, error: function () {
                alert("Error loading data! Please try again.");
            }

        });



}

function BarChartReport(data) {
    var labels_p = [];
    var dataTotal = [];
    $.each(data, function (key, val) {
        dataTotal.push(val.TotalMoney);
        labels_p.push(val.DateValue);
    });
   
    var dataSet = [{
        label: 'Doanh thu',
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
                x: {
                    stacked: true,

                },
                y: {
                    stacked: true,


                }
            }, responsive: true,
            maintainAspectRatio: false
        }
    });
    
}