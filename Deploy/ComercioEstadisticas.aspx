<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComercioEstadisticas.aspx.cs" Inherits="ContraDescuento.GUI.ComercioEstadistincas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Lib/Jquery/jquery-3.4.1.js"></script>
    <script src="Lib/CanvasJs/js/canvasjs.min.js"></script>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="chartContainer" style="height: 370px; max-width: 920px; margin: 0px auto;"></div>
    </div>
    </form>
</body>
</html>

<script type="text/javascript">

    $(document).ready(function () { ShowCurrentTime(); });

    function ShowCurrentTime() {
        debugger;
        $.ajax({
            type: "POST",
            url: 'ComercioEstadisticas.aspx/DescuentosAcreditadosPorProducto',
            data: '{name: "' + $("#nombre").val() + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: CrearChart,
            failure: function (response) {
                alert(response.d);
            }
        });
    }

    function CrearChart(producto) {
        debugger;
       
            var chart = new CanvasJS.Chart("chartContainer", {
                animationEnabled: true,

                title: {
                    text: "Fortune 500 Companies by Country"
                },
                axisX: {
                    interval: 1
                },
                axisY2: {
                    interlacedColor: "rgba(1,77,101,.2)",
                    gridColor: "rgba(1,77,101,.1)",
                    title: "Number of Companies"
                },
                data: [{
                    type: "bar",
                    name: "companies",
                    axisYType: "secondary",
                    color: "#014D65",
                    dataPoints: [
                        { y: 3, label: producto.d },
                        { y: 7, label: "Taiwan" },
                        { y: 5, label: "Russia" },
                        { y: 9, label: "Spain" },
                        { y: 7, label: "Brazil" },
                        { y: 7, label: "India" },
                        { y: 9, label: "Italy" },
                        { y: 8, label: "Australia" },
                        { y: 11, label: "Canada" },
                        { y: 15, label: "South Korea" },
                        { y: 12, label: "Netherlands" },
                        { y: 15, label: "Switzerland" },
                        { y: 25, label: "Britain" },
                        { y: 28, label: "Germany" },
                        { y: 29, label: "France" },
                        { y: 52, label: "Japan" },
                        { y: 103, label: "China" },
                        { y: 134, label: "US" }
                    ]
                }]
            });
            chart.render();

        }
</script>
