<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FeedbackFileEcncrypt.aspx.vb" Inherits="FeedbackFileEcncrypt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
  <meta content="width=device-width, initial-scale=1" name="viewport">

  <title>360 Survey</title>
  <meta content="" name="descriptison">
  <meta content="" name="keywords">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
  <!-- Favicons -->
  <link href="assets/img/favicon.png" rel="icon">
  <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">

  <!-- Google Fonts -->
    <link href="assets/css/googlefont.css" rel="stylesheet" />

  <!-- Vendor CSS Files -->
  <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
  <link href="assets/vendor/icofont/icofont.min.css" rel="stylesheet">
  <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
  <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">
  <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
  <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script src="scripts/jquerypromise.min.js"></script>
    <script src="scripts/html2canvas.js"></script>
    <script src="scripts/jsPDF.js"></script>
    <script src="scripts/pdfmake.min.js"></script>
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">
<script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script>
<script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
     <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet">
     <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
  <!-- Include all compiled plugins (below), or include individual files as needed -->
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>


  <!-- Template Main CSS File -->
  <link href="assets/css/style.css" rel="stylesheet">
    <style type="text/css">
        .modal-dialog 
            {
                width: 814px;
    padding-top: 1px;
    padding-bottom: 1px;

            }
    </style>
    <script type="text/javascript">
        function screenshot(date) {
            html2canvas(document.getElementById('pnlContainer'), {
                onrendered: function (canvas) {
                    var data = canvas.toDataURL('image/jpeg');
                    var docDefinition = {
                        pageSize: 'A2',

                        // by default we use portrait, you can change it to landscape if you wish
                        pageOrientation: 'portrait',
                        content: [{
                            image: data,
                            width: 1600,
                            height: 1100,
                        }
                        ]
                    };
                    pdfMake.createPdf(docDefinition).download("Feedback.pdf")
                }
            });
        }
    </script>
    <script type="text/javascript">
        function printDiv(divName) {
            var printContents = document.getElementById(divName).innerHTML;
            var originalContents = document.body.innerHTML;

            document.body.innerHTML = printContents;

            window.print();
        

            document.body.innerHTML = originalContents;
            location.href = 'FeedbackRpt1.aspx'
        }
    </script>
    <script type="text/javascript">
        function msg(title_msg, text_msg, icon_type) {
            swal({
                title: title_msg,
                text: text_msg,
                icon: icon_type
                // type: btn_type
            })
        }

        function Showmodaldiv(i) {
            // debugger;
            $(i).modal('show');


        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
