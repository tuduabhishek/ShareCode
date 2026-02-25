<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RespondentSelectedRpt.aspx.vb" Inherits="RespondentSelectedRpt"   MaintainScrollPositionOnPostback="true"%>

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
    <%--<meta content="width=device-width, initial-scale=1" name="viewport" />--%>
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
    <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
    <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>
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
        .AutoExtender {
            font-family: Verdana;
            font-size: 0.8em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: aliceblue;
            margin-left: 10px;
            width: 230px !important;
            overflow: auto;
            z-index: 2000 !important;
        }

        .AutoExtenderList {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Navy;
            width: 230px !important;
        }

        .AutoExtenderHighlight {
            color: White;
            background-color: Navy;
            cursor: pointer;
            width: 230px !important;
        }

         .panel-heading span
{
    margin-top: -26px;
    /*margin-right: 10px;*/
}

.clickable {
    /*background: rgba(0, 0, 0, 0.15);*/
    display: inline-block;
    padding: 8px 12px;
    border-radius: 10px;
    cursor: pointer;
}

#hero .btn-get-started
	{
	font-size: 15px !important;
	}
	
	
	.badge-primary {
    color: #fff;
    background-color: #e43c5c !important ;
}

      .table-responsive1 {
    display: block;
    width: 100%;
    -webkit-overflow-scrolling: touch;
}
      .divWaiting {
            position: fixed;
            background-color: #FAFAFA;
            z-index: 2147483647 !important;
            filter: alpha(opacity=80);
            opacity: 0.8;
            overflow: hidden;
            text-align: center;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            color: Black;
        }
      .wrap-Text{
          word-wrap: normal; 
          word-break: break-all;
          width:30px;
      }
      #ifMobile1 {
    /*background-image: url(/images/arts/IMG_1447m.png)*/
    width:300px;
    height:200px;
}

@media all and (max-width: 499px) {
    #ifMobile1 {
        /*background-image: url(/images/arts/IMG_1447.png)*/
    }
}
    </style>
    <script type="text/javascript">

       

        function showGenericMessageModal(type, message) {
            swal('', message, type);
        }

       
       
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
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
    </asp:UpdateProgress>
  <!-- ======= Header ======= -->
  <header id="header" class="fixed-top ">
    <div class="container d-flex align-items-center">

      <h1 class="logo mr-auto"><a href="SelectAssesorNew_OPR.aspx">360 DEGREE FEEDBACK SURVEY</a></h1>
      <!-- Uncomment below if you prefer to use an image logo -->
      <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->

      <nav class="nav-menu d-none d-lg-block">
        <ul>
          <li class="active"><a href="SelectAssesorNew_OPR.aspx">Home</a></li>
         
        </ul>
      </nav><!-- .nav-menu -->

    </div>
  </header><!-- End Header -->
        <asp:ScriptManager runat="server" ID="scpmgr">

        </asp:ScriptManager>
  <!-- ======= Hero Section ======= -->
  <section id="hero">
    <div class="hero-container">
      <h3>Welcome  <strong><asp:Label ID="lblname" runat ="server" Text="Admin" ></asp:Label></strong></h3>     
      <!--<h1>We're Creative Agency</h1>-->
    
        <h2 class ="table-responsive1">
            As we aspire to be the most valuable and respected steel company globally in the next 5-10 years, we are developing agile behaviours in our top leadership - accountability, responsiveness, collaboration and people development. We will measure this as an integral part of our Performance Management System for IL3s-IL6s through a 360 degree feedback survey.            
</h2>
      <a href="#about" class="btn-get-started scrollto">SELECT RESPONDENTS</a>
    </div>
  </section><!-- End Hero -->

<main id="main">

    <!-- ======= About Section ======= -->
      <section id="about" class="about">
      <div class="container">
          <%--<div class="row">
        <div class="col-lg-10">
            <h1>Status Survey Completion</h1>
        </div>
        <div class="col-lg-2">
            <div style="margin-top:26px;margin-left:30px;">
                <asp:LinkButton ID="btn_download" runat="server" class="btn btn-primary" Width="120px" type="submit" Visible="false" CausesValidation="false">
                    <span aria-hidden="true" class="glyphicon glyphicon-download"></span> Download
                </asp:LinkButton> 
            </div>
        </div>
    </div>--%>
 
         <div class="row content">
      <div class="col-md-12 col-lg-12">
        <div class="table-responsive">
            <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                <ContentTemplate>
                    <asp:GridView ID="gdvselectAssesor" runat="server" Visible="true" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer" Font-Names="verdana"
                                                        EmptyDataText="No Record Found" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" RowStyle-CssClass="rows">
                                                       <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                        <HeaderStyle  BackColor="#e43c5c" Font-Bold="True" ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />
                        
                          <Columns>
                                                           
                                            <asp:BoundField DataField="Pno" HeaderText="P.no" SortExpression="Pno"/>  
                                            <asp:BoundField DataField="EMA_ENAME" HeaderText="Name" SortExpression="EMA_ENAME"/>  
                                            <asp:BoundField DataField="EMA_EQV_LEVEL" HeaderText="Level" SortExpression="EMA_EQV_LEVEL"/>  
                                            <asp:BoundField DataField="Designation" HeaderText="Designation" SortExpression="Designation"/>  
                                            <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department"/>  
                                            <asp:BoundField DataField="Email_id" HeaderText="Email Id" SortExpression="Email_id"/>  
                                            <asp:BoundField DataField="Executive_Head" HeaderText="Executive Head" SortExpression="Executive_Head"/>  
                                            <asp:BoundField DataField="Superior_Pno" HeaderText="Superior P No." SortExpression="Superior_Pno"/>  
                                            <asp:BoundField DataField="Superior_Name" HeaderText="Superior Name" SortExpression="Superior_Name"/>  
                                            <asp:BoundField DataField="BUHR_Pno" HeaderText="BUHR P No." SortExpression="BUHR_Pno"/>  
                                            <asp:BoundField DataField="BUHR_NAME" HeaderText="BUHR Name" SortExpression="BUHR_NAME"/> 
                                            <asp:BoundField DataField="Respondent_Pno" HeaderText="Respondent P.no" SortExpression="Respondent_Pno"/> 
                                            <asp:BoundField DataField="Respondent_Name" HeaderText="Respondent Name" SortExpression="Respondent_Name"/> 
                                            <asp:BoundField DataField="Respondent_Level" HeaderText="Respondent Level" SortExpression="Respondent_Level"/> 
                                            <asp:BoundField DataField="Respondent_Designation" HeaderText="Respondent Designation" SortExpression="Respondent_Designation"/> 
                                            <asp:BoundField DataField="Respondent_Department" HeaderText="Respondent Department" SortExpression="Respondent_Department"/> 
                                            <asp:BoundField DataField="Respondent_Email_Id" HeaderText="Respondent Email Id" SortExpression="Respondent_Email_Id"/> 
                                            <asp:BoundField DataField="Respondent_Category" HeaderText="Respondent Category" SortExpression="Respondent_Category"/> 
                                                        </Columns>                                

                                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="White" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                        <SortedDescendingHeaderStyle BackColor="#000065" />
                                                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
          </div>
        </div>
      </div>

    </section>
    <!-- End About Section -->     

  </main><!-- End #main -->
 <!-- ======= Footer ======= -->
  <footer id="footer">
    <div class="container d-md-flex py-4">

         <div class="mr-md-auto text-center">
              In case of any queries or issues please reach out to 
 <strong>Ms. Shruti Choudhury: </strong>shruti.choudhury@tatasteel.com and <strong>Mr. Vikas Kumar : </strong>vikas.kumar1@tatasteel.com
        </div>
    </div>

  
  </footer><!-- End Footer -->

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
