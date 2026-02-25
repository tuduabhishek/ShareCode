<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ExtdownloadReport.aspx.vb" Inherits="ExtdownloadReport" MaintainScrollPositionOnPostback="true"  %>

<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="utf-8">
  <meta content="width=device-width, initial-scale=1.0" name="viewport">

  <title>360 Survey</title>
  <meta content="" name="descriptison">
  <meta content="" name="keywords">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
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
  <link href="assets/css/styleIL3.css" rel="stylesheet">
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

.table-responsive1 {
    display: block;
    width: 100%;
    -webkit-overflow-scrolling: touch;
}.button {
  /*padding: 15px 25px;*/
  font-size: 24px;
  text-align: center;
  cursor: pointer;
  outline: none;
  color: #fff;
  background-color: #4CAF50;
  border: none;
  border-radius: 15px;
  /*box-shadow: 0 9px #999;*/
}

.button:hover {background-color: #4CAF50}

.button:active {
  background-color: #4CAF50;
  /*box-shadow: 0 5px #666;*/
  transform: translateY(4px);
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
   
  <!-- ======= Header ======= -->
  <header id="header" class="fixed-top ">
    <div class="container d-flex align-items-center">

      <h1 class="logo mr-auto"><a >360 DEGREE FEEDBACK SURVEY</a></h1>
      <!-- Uncomment below if you prefer to use an image logo -->
      <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->

      <nav class="nav-menu d-none d-lg-block">
        <ul>        

        </ul>
      </nav><!-- .nav-menu -->

    </div>
  </header><!-- End Header -->
        <asp:ScriptManager runat="server" ID="scpmgr">

        </asp:ScriptManager>
  <!-- ======= Hero Section ======= -->
  <section id="hero">
    <div class="hero-container">
     <h3>Welcome  <strong><asp:Label ID="lblname" runat ="server" Text="Guest" ></asp:Label></strong></h3>     
      <!--<h1>We're Creative Agency</h1>-->
      <h2 class ="table-responsive1">
As we aspire to be the most valuable and respected steel company globally in the next 5-10 years, we are developing agile behaviours across the organization. We will measure this as an integral part of our Performance Management System for all the officers through a 360 degree feedback survey</h2>
      <a href="#about" class="btn-get-started scrollto">Download Report With OTP</a>
    </div>
  </section><!-- End Hero -->

  <main id="main">

    <!-- ======= About Section ======= -->
      <section id="about" class="about">
          <asp:UpdatePanel runat="server" ID="uppnl">
              <ContentTemplate>
      <div class="container">
          
          <div class="row">
              <div class="col-lg-2">
                 
              </div>
             <div class="col-lg-2">
                 
              </div>
              <div class="col-lg-4">
                   <asp:TextBox runat="server" ID="txtotp" CssClass="form-control" placeholder="Email id"></asp:TextBox>
                 
              </div>
               
              </div>
              <br />
          
           <div class="row">
              <div class="col-lg-2">
                 
              </div>
             <div class="col-lg-2">
              </div>
              <div class="col-lg-4">
                  <asp:TextBox runat="server" ID="txtemail" CssClass="form-control" placeholder="Enter OTP"  Visible="false"></asp:TextBox>              
                
              </div>
                
              </div>       

          <br />
           <div class="row content">
           
              <div class="col-lg-2">
                  
              </div>
               <div class="col-lg-2">
              </div>
              <div class="col-lg-2">
                 <asp:TextBox runat="server" ID="txtcaptcha" CssClass="form-control" placeholder="Eter captcha code" Visible="false" ></asp:TextBox>
              </div>
              <div class="col-lg-2">
                   <button class="button" id="btncapt" runat="server" disabled="disabled" Visible="false"></button>
                  
                  <asp:LinkButton runat="server" ID="lbcap" CssClass="glyphicon glyphicon-refresh" OnClick="fetchcaptcha" Visible="false"></asp:LinkButton>
              </div>
              
              </div>

          <div class="row content">
           
              <div class="col-lg-2">
                  
              </div>
              <div class="col-lg-2">
                 
              </div>
              <div class="col-lg-4">
                  <asp:Button runat="server" ID="btnopt" Text="Get Otp" CssClass="btn-learn-more" OnClick="btnopt_Click" />
              </div>
              
              </div>
              
      <%--  <div class="section-title">          
               
        </div>--%>
 
      </div>
</ContentTemplate>
          </asp:UpdatePanel>
    </section>
    <!-- End About Section -->     

  </main><!-- End #main -->

  <!-- ======= Footer ======= -->
  <footer id="footer">

  

    <div class="container d-md-flex py-4">

        <div class="mr-md-auto text-center">
              In case of any queries or issues, please reach out to your BUHR. In case of any system specific queries or issues, please reach out to IT helpdesk (it_helpdesk@tatasteel.com)
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