<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FeedbackRpt1.aspx.vb" Inherits="FeedbackRpt1"  EnableEventValidation = "false"  MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html lang="en">
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
    <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div class="divWaiting">
                <center>
                    <div>
                        <h3>
                            <asp:Label ID="lblWait" runat="server" Text="Please wait"  /></h3>
                       <img src="Images/loader.gif" alt="Loading..."  id="ifMobile1"/>
                    </div>
                </center>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
        <asp:ScriptManager runat="server" ID="scpmgr">
        </asp:ScriptManager>
       

           
        <div style="margin:20px 15px; float:right;">
            <asp:LinkButton ID="btnExport" runat="server" Text="Export" class="btn btn-warning"  Width="180px" OnClientClick="printDiv('pnlContainer')" CausesValidation="false">Download as PDF &nbsp;
            <span aria-hidden="true" class="glyphicon glyphicon-export"></span></asp:LinkButton>
            <asp:Label ID="lblname" runat ="server" Text="" Visible="false"></asp:Label>
        </div>
        <div id="pnlSerach" runat="server" visible="false" class="row">
            <div class="col-md-4">

            </div>
            <div class="col-md-4">
                <div style="margin: 20px 85px;">
                <div style="float:left;margin-right:5px;margin-top: 5px;"><label>Per. No.</label></div>
                 <div style="float:left;margin-right:5px;"><asp:TextBox ID="txt_Pno" runat="server" TabIndex="1" autocomplete="off" class="form-control" MaxLength="6" Width="120px"></asp:TextBox></div>
             <div style="float:left;margin-right:5px;">
                 <asp:LinkButton ID="lbtnView" runat="server" Text="Export" Font-Size="Larger" class="btn btn-primary"  Width="180px"  CausesValidation="false">View &nbsp;
            <span aria-hidden="true" class="glyphicon glyphicon-eye-open"></span></asp:LinkButton></div>
            </div>
                </div>
            <div class="col-md-4">
                <div style="margin-top:20px;font-size:20px;color:red;">
                <asp:Label ID ="lblMessage" runat="server" Text=""></asp:Label>
                </div>
            </div>
           </div>
         <%-- <asp:UpdatePanel ID="upnl" runat="server">
            <ContentTemplate>--%>
        <div id="pnlContainer" style="background-color:white;">
         <!-- ======= Header ======= -->
  <header id="header" style="height:150px;">
    <div class="container d-flex align-item-left">

      <%--<h1 class="logo mr-auto"><a>360 DEGREE FEEDBACK SURVEY</a></h1>--%>
      <!-- Uncomment below if you prefer to use an image logo -->
       <asp:Image ID="imgLogo" ImageUrl="Images/Logo.JPG" runat="server" alt="" height="100" width="230" class="img-fluid"/>

      

    </div>
  </header><!-- End Header -->

        <%--======================Image section=====================--%>
        <section>
            <div class="container d-flex align-items-center">
                <%--<img src="Images/Feedback360.JPG" alt="Feedback360" height="800" width="1250" />--%>
                <asp:Image ID="bgImg" runat="server" ImageUrl="Images/Feedback360.JPG"  alt="Feedback360" height="1200" width="1250" />
            </div>
            <div class="row" style="margin: 0 142px; text-align: center; font-size: 28px; font-weight: 200;">
                <div class="col-12">
                    <asp:Label ID="lblReceiptNm" runat="server" Text=""></asp:Label>,
                    <asp:Label ID="lblDesignation" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </section>
        <!-- End Image section -->
        

        <%--======================Grade section=====================--%>
        <%--<section>--%>
            <div class="container" style="height:1500px;">
                <h2>Overall report for <asp:Label ID="lblReceiptNm1" runat="server" Text=""></asp:Label></h2><br /><br />
                    <p style="font-size: 17px;">
                         <asp:Label runat="server" ID="lblnor"></asp:Label> 
                    </p>
                <br />
                <asp:GridView ID="gvScore" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer" Font-Names="verdana"
                                                        EmptyDataText="No Record Found" BorderStyle="None" BorderWidth="1px" Font-Size="20px" CellPadding="3" GridLines="Vertical" RowStyle-CssClass="rows">
                                                       <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                        <HeaderStyle  BackColor="#e43c5c" Font-Bold="True" ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />
                        
                          <Columns>
                                                           <asp:BoundField DataField="Category" HeaderText="" />
                                                           <asp:BoundField DataField="Q1" HeaderText="Accountability" />
                                                           <asp:BoundField DataField="Q2" HeaderText="Collaboration" />
                                                           <asp:BoundField DataField="Q3" HeaderText="Responsiveness" />
                                                           <asp:BoundField DataField="Q4" HeaderText="People Development" />
                                                        </Columns>                                

                                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="White" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                        <SortedDescendingHeaderStyle BackColor="#000065" />
                                                    </asp:GridView>
            </div>
        <%--</section>--%>
        <!-- End Grade section -->

         <%--======================Strength section=====================--%>
        <%--<section>--%>
            <div class="container">
                <h2>Qualitative comments for <asp:Label ID="lblReceiptNm3" runat="server" Text=""></asp:Label></h2><br /><br />
                <div class="form-group">
                    <h4><b>Strengths</b></h4>
                </div>
                <table>
                    <tr>
                        <td style="vertical-align:top"><h4>Self</h4></td>
                        <td style="padding: 6px;font-size: 20px;"><asp:Label ID="lblQASelf" runat="server" Text=""/></td>
                    </tr>
                    </table>
                <table>
                    <tr>
                        <td style="vertical-align:top"><h4>Other respondents</h4></td>
                        <td style="padding: 6px;font-size: 20px;"><asp:Label ID="lblQAOtrRespond" runat="server" Text=""/></td>
                    </tr>
                </table>
               <%-- <div class="row">
                      <div class="col-lg-2">
                          <h4>Self</h4>
                      </div>
                      <div class="col-lg-10">
                          <asp:Label ID="lblQASelf" runat="server" Text=""/>
                      </div>
                  </div>
                  <div class="row">
                      <div class="col-lg-2">
                          <h4>Other respondents</h4>
                      </div>
                      <div class="col-lg-10" style="font-size: 18px; text-align: justify;">
                          <asp:Label ID="lblQAOtrRespond" runat="server" Text=""/>
                      </div>
                  </div>--%>
            </div>
        <%--</section>--%>
        <!-- End Grade section -->

          <%--======================Opportunities for improvement section=====================--%>
        <%--<section>--%>
            <div class="container">
                <h2>Qualitative comments for <asp:Label ID="lblReceiptNm4" runat="server" Text=""></asp:Label></h2><br /><br />
                <div class="form-group">
                    <h4><b>Opportunities for improvement</b></h4>
                </div>
                <table>
                    <tr>
                        <td style="vertical-align:top"><h4>Self</h4></td>
                        <td style="padding: 6px;font-size: 20px;"><asp:Label ID="lblQBSelf" runat="server" Text=""/></td>
                    </tr>
                    <tr>
                        <td  style="vertical-align:top"><h4>Other respondents</h4></td>
                        <td style="padding: 6px;font-size: 20px;"><asp:Label ID="lblQBOtrRespond" runat="server" Text=""/></td>
                    </tr>
                </table>
                <%--<div class="row">
                      <div class="col-lg-2">
                          <h4>Self</h4>
                      </div>
                      <div class="col-lg-10" style="font-size: 18px; text-align: justify;">
                          <asp:Label ID="lblQBSelf" runat="server" Text=""/>
                      </div>
                  </div>
                  <div class="row">
                      <div class="col-lg-2">
                          <h4>Other respondents</h4>
                      </div>
                      <div class="col-lg-10" style="font-size: 18px; text-align: justify;">
                          <asp:Label ID="lblQBOtrRespond" runat="server" Text=""/>
                      </div>
                  </div>--%>
            </div>
        <%--</section>--%>
        <!-- End Opportunities for improvement -->

    
        <!-- ======= Footer ======= -->
  <footer id="footer">

  

    <div class="container d-md-flex py-4">

        <div class="mr-md-auto text-center">
              In case of any queries or issues please reach out to 
 <strong>Ms. Shruti Choudhury: </strong>shruti.choudhury@tatasteel.com and <strong>Mr. Vikas Kumar : </strong>vikas.kumar1@tatasteel.com
        </div>
     <%-- <div class="mr-md-auto text-center text-md-left">
        <div class="copyright">
          &copy; Copyright <strong><span>Tata Steel</span></strong>. All Rights Reserved
        </div>
      </div>  --%> 
    </div>
  </footer><!-- End Footer -->
            </div>
        <div class="modal fade" data-backdrop="static" id="MSGBOX">
                <div class="modal-dialog small">
                    <div class="modal-content">
                        <div class="modal-header bg-green sm">
                            <div class="bootstrap-dialog-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true" style="color: White;">&times;</span></button>
                                <div class="bootstrap-dialog-title">
                                    <label id="Label5" style="font-size: large">
                                        <i class="fa fa-info-circle"></i>&nbsp; Message Box
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-body">
                            <div class="bootstrap-dialog-body">
                                <div class="bootstrap-dialog-message" id="Div5">
                                    <b>Records Successfully Inserted!</b>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer bg-green sm">
                            <div class="bootstrap-dialog-footer">
                                <div class="bootstrap-dialog-footer-buttons" align="center">
                                    <asp:LinkButton ID="btn_med" runat="server"
                                        class="btn btn-default" OnClientClick="closeModal();">OK</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
                <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>

  <a href="#" class="back-to-top"><i class="ri-arrow-up-line"></i></a>

  <!-- Vendor JS Files -->
  <script src="assets/vendor/jquery/jquery.min.js"></script>
  <script src="assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
  <script src="assets/vendor/jquery.easing/jquery.easing.min.js"></script>
  <script src="assets/vendor/php-email-form/validate.js"></script>
  <script src="assets/vendor/isotope-layout/isotope.pkgd.min.js"></script>
  <script src="assets/vendor/venobox/venobox.min.js"></script>
  <script src="assets/vendor/owl.carousel/owl.carousel.min.js"></script>

  <!-- Template Main JS File -->
  <script src="assets/js/main.js"></script>
    </form>
</body>
</html>
