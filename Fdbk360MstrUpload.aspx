<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Fdbk360MstrUpload.aspx.vb" Inherits="Fdbk360MstrUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="shortcut icon" type="image/x-ico" href="images/tata-logo.ico" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <title>Feedback360</title>
    <link href="styles/bootstrap.min.css" rel="stylesheet" />
  <%--  <link href="css/bootstrap.min.css" rel="stylesheet" />--%>
 <%--   <script src="js/jquery.js" type="text/javascript"></script>--%>
    <script src="scripts/jquery.min.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
 <%--   <script src="js/bootstrap.min.js" type="text/javascript"></script>--%>
    <style type="text/css">
        .messagealert {
            width: 100%;
            position: fixed;
             top:110px;
             right:12px;
             width:30%;
            z-index: 100000;
            padding: 0;
            font-size: 15px;
        }
        .bg-custom-2 {
            background-image: linear-gradient(15deg, #13547a 0%, #80d0c7 100%);
        }
        /* Chrome only: */
        @media all and (-webkit-min-device-pixel-ratio:0) and (min-resolution: .001dpcm) {
            body {
                zoom: 90%;
                -moz-transform: scale(1.2);
                -moz-transform-origin: left top;
            }
        }

        option:disabled {
            background: #efeaea;
        }

        @media (min-width:388px) {
            .show-on-mobile {
                display: none;
            }
        }

        @media (min-width:768px) {
            .modal-sm {
                width: 400px;
            }
        }

        @media (max-width:387px) {
            .hide-on-mobile {
                display: none;
            }
        }
         body {
            padding: 30px 0;
            margin: 0 auto;
           /* background: url('img/GPTW.jpg') no-repeat;*/
            background-size: cover;
            min-height: 100vh;
           /* display: -webkit-box;
            display: -ms-flexbox;
            display: flex;*/
            -webkit-box-align: center;
            -ms-flex-align: center;
            align-items: center;
        }
    </style>
 <script type="text/javascript">
     function ShowMessage(message, messagetype) {
         var cssclass;
         switch (messagetype) {
             case 'Success':
                 cssclass = 'alert-success'
                 break;
             case 'Errors':
                 cssclass = 'alert-danger'
                 break;
             case 'Warning':
                 cssclass = 'alert-warning'
                 break;
             default:
                 cssclass = 'alert-info'
         }
$('#alert_container').append('<div id="alert_div" style="margin: 0 0.5%; -webkit-box-shadow: 3px 4px 6px #999;" class="alert fade in ' + cssclass + '"><a href="#" class="btn-close" data-bs-dismiss="alert" aria-label="close">&times;</a><strong>' + messagetype + '!</strong> <span>' + message + '</span></div>');
$('#alert_container').delay(4200).fadeOut(300);
     }
 </script>
</head>
<body class="home-page" oncontextmenu="return false" style="padding-top: 0px; padding-bottom: 0px; background:#FAF9F6;">
    <nav class="navbar fixed-top" style="height: 100px; border: 0; border-radius: 0px; background-color: #696969">
        <div class="row col-lg-12">
            <div class="col-lg-2 " style="padding:0px;">
        <a class="navbar-brand" href="#" style="height: 50px">
            <img src="img/tslleft.png" style="width:200px;padding-top: 20px;" class="d-inline-block align-top" alt="" /></a></div>
            <div class="col-lg-9">
        <div class=" text-center  hidden-sm hidden-xs">
            <h1 style="font-size: 30px; color: white;padding-top: 20px;">Feedback 360 Master Upload</h1>
        </div></div>
            <div class="col-lg-1" style="padding:0px;">
        <a class="navbar-brand float-end" href="#">
            <img src="img/tslright.png" style="width:100px; margin-top:-20px;" class="d-inline-block align-top" alt="" /></a>
            </div>
          </div>
    </nav>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="updtPnl" runat="server">
            <ContentTemplate>
                <div class="messagealert" id="alert_container">
            </div>
                        <div class="container-fluid" style="padding-top: 70px;">
            <div class="wrapper addMarginTop15">
                <div class="main main-raised" style="background-color: white;">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class=" col-lg-offset-3 col-lg-2 ">
                                <%--<asp:Label ID="lblExcelUpload" runat="server" Text="Uplload Excel File"></asp:Label>   --%>   
                                <br /><asp:FileUpload ID="fluExcel" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-lg-2">
                                &nbsp;<br />
                                <asp:LinkButton ID="lbtnUpload" runat="server" CssClass="btn btn-primary form-control" OnClick="lbtnUpload_Click"><i class="fa fa-upload"></i>&nbsp;Upload</asp:LinkButton>
                            </div>
                            <div class="col-lg-2">
                                <br />
                                <asp:LinkButton ID="lbtnTemplateDownload" runat="server" OnClick="lbtnTemplateDownload_Click" CssClass="btn btn-info form-control"><i class="fa fa-download"></i>&nbsp;Template Download</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="lbtnTemplateDownload" />
                <asp:PostBackTrigger ControlID="lbtnUpload" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>
