<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AssesorRpt.aspx.vb" Inherits="AssesorRpt" MaintainScrollPositionOnPostback="true"  %>

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

.square_btn{
    display: inline-block;
    padding: 7px 20px;
	border-radius: 25px;
    text-decoration: none;
    color: #FFF;
    background-image: -webkit-linear-gradient(45deg, #FFC107 0%, #ff8b5f 100%);
    background-image: linear-gradient(45deg, #FFC107 0%, #ff8b5f 100%);
    transition: .4s;
}

.square_btn:hover {
    background-image: -webkit-linear-gradient(45deg, #FFC107 0%, #f76a35 100%);
    background-image: linear-gradient(45deg, #FFC107 0%, #f76a35 100%);
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
        $(function () {
            debugger;
            $('.clickable').on('click', function () {
                var effect = $(this).data('effect');
                $(this).closest('.panel')[effect]();
            })
        });

        $(".close-button").on("click", function () {
            $(this).closest('.collapse-group').find('.collapse').collapse('hide');
        });

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

      <h1 class="logo mr-auto"><a>360 DEGREE FEEDBACK SURVEY</a></h1>
      <!-- Uncomment below if you prefer to use an image logo -->
      <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->

      <nav class="nav-menu d-none d-lg-block">
        <ul>
           <li class="active"><a href="AssesorRpt.aspx">Home</a></li>
            <li class=""><a >Help</a></li>
            
        </ul>
      </nav><!-- .nav-menu -->

    </div>
  </header><!-- End Header -->
        <asp:ScriptManager runat="server" ID="scpmgr">

        </asp:ScriptManager>
  <!-- ======= Hero Section ======= -->
  <section id="hero">
    <div class="hero-container">
     <h3>Welcome  <strong><asp:Label ID="lblname" runat ="server" Text="" ></asp:Label></strong></h3>     
      <!--<h1>We're Creative Agency</h1>-->
      <h2 class ="table-responsive1">As we aspire to be the most valuable and respected steel company globally in the next 5-10 years, we are developing agile behaviours in our top leadership - accountability, responsiveness, collaboration and people development. We will measure this as an integral part of our Performance Management System for IL2s through a 360 degree feedback survey.</h2>
      <a href="#about" class="btn-get-started scrollto">View Report</a>
    </div>
  </section><!-- End Hero -->

  <main id="main">

    <!-- ======= About Section ======= -->
      <section id="about" class="about">
      <div class="container">
          <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                <ContentTemplate>
                    <h3>Overall report for <asp:Label runat ="server" ID="lblreciptnm" Text="Zubin Palia"></asp:Label></h3><br /><br />
                    <p>
                         <asp:Label runat="server" ID="lblnor"></asp:Label> 
                    </p>
                    <div class ="row">
                        <div class="col-md-12">

                               <div class="card-deck">
  <div class="card">
   
    <div class="card-body">
      <h5 class="card-title"></h5>
  <div class="row">
      <div class="col-md-2">  </div>
      <div class="col-md-2">Accountability</div>
      <div class="col-md-2">Collaboration</div>
      <div class="col-md-2">Responsiveness</div>
      <div class="col-md-4">People Development
</div>
  </div><br />
        <div class="row">
      <div class="col-md-2"> Self </div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
            
  </div>
        <br />
         <div class="row">
      <div class="col-md-2"> Manager </div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
      
  </div>
        <br />
         <div class="row">
      <div class="col-md-2"> Subordinates </div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
      
  </div>
        <br />
         <div class="row">
      <div class="col-md-2"> Peers </div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
           
  </div>
        <br />
         <div class="row">
      <div class="col-md-2"> Internal stakeholders </div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
            
  </div>
        <br />
         <div class="row">
      <div class="col-md-2"> Overall1
 </div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
      <div class="col-md-2"></div>
            
  </div>
    </div>
    
  </div>
  
 
</div>
                        </div>
                       
                    </div>
                 
        

                    </ContentTemplate>
                <Triggers>
                   
                </Triggers>
            </asp:UpdatePanel>
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
     <%-- <div class="mr-md-auto text-center text-md-left">
        <div class="copyright">
          &copy; Copyright <strong><span>Tata Steel</span></strong>. All Rights Reserved
        </div>
      </div>  --%> 
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