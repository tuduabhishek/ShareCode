<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DownloadReport.aspx.vb" Inherits="DownloadReport"  MaintainScrollPositionOnPostback="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <!-- New Library Versions -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-4.0.0-beta.2.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <meta charset="utf-8">
    <meta content="width=device-width, initial-scale=1.0" name="viewport">


    <title>360 Survey</title>
    <meta content="" name="descriptison">
    <meta content="" name="keywords">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <!-- Favicons -->
    <link href="assets/img/favicon.png" rel="icon">
    <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">

    <%-- <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" /> --%>
    <!-- Google Fonts -->
    <link href="assets/css/googlefont.css" rel="stylesheet" />

    <!-- Vendor CSS Files -->
    <!-- <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"> -->
    <link href="assets/vendor/icofont/icofont.min.css" rel="stylesheet">
    <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
    <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">
    <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
    <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <!-- Template Main CSS File -->
    <link href="assets/css/styleIL3.css" rel="stylesheet">
    <style type="text/css">
        @media only screen and (min-width: 992px) {
            .rbl input[type="radio"] {
                margin-left: 210px;
                margin-right: 50px;
                margin-top: 5px;
                transform: scale(2, 2);
                -ms-transform: scale(2, 2);
                -webkit-transform: scale(2, 2);
                align-items: center;
            }

            .option {
                margin-left: 200px;
            }
        }

        @media only screen and (max-width: 600px) {
            .rbl td input[type="radio"] {
                margin-left: 80px;
                margin-right: 20px;
                margin-top: 5px;
                transform: scale(2, 2);
                -ms-transform: scale(2, 2);
                -webkit-transform: scale(2, 2);
                align-items: center;
            }

            .divB {
                display: none;
            }

            #btn_reject {
                width: 300px;
                white-space: normal;
                word-wrap: break-word;
            }

            .option {
                margin-left: 200px;
            }
        }

        @media only screen and (max-width: 768px) {
            .rbl td input[type="radio"] {
                margin-left: 135px;
                margin-right: 20px;
                margin-top: 5px;
                transform: scale(2, 2);
                -ms-transform: scale(2, 2);
                -webkit-transform: scale(2, 2);
                align-items: center;
            }

            #btn_reject {
                width: 300px;
                white-space: normal;
                word-wrap: break-word;
            }

            .option {
                margin-left: 200px;
            }
        }

        @media only screen and (max-width: 800px) {
            .rbl td input[type="radio"] {
                margin-left: 150px;
                margin-right: 20px;
                margin-top: 5px;
                transform: scale(2, 2);
                -ms-transform: scale(2, 2);
                -webkit-transform: scale(2, 2);
                align-items: center;
            }

            #btn_reject {
                width: 300px;
                white-space: normal;
                word-wrap: break-word;
            }

            .option {
                margin-left: 100px;
            }
        }

        @media only screen and (max-width: 373px) {
            .rbl td input[type="radio"] {
                margin-left: 10px;
                margin-right: 10px;
                margin-top: 5px;
                transform: scale(1, 1);
                -ms-transform: scale(1, 1);
                -webkit-transform: scale(1, 1);
                align-items: center;
            }

            table {
                width: 350px;
                font-size: small;
            }

            .option {
                margin-left: 40px;
            }
        }

        @media only screen and (min-width: 374px) and (max-width: 395px) {
            .rbl td input[type="radio"] {
                margin-left: 10px;
                margin-right: 10px;
                margin-top: 5px;
                transform: scale(1, 1);
                -ms-transform: scale(1, 1);
                -webkit-transform: scale(1, 1);
                align-items: center;
            }

            table {
                width: 360px;
                font-size: small;
            }

            .option {
                margin-left: 45px;
            }
        }

        @media only screen and (min-width: 410px) and (max-width:480) {
            .rbl td input[type="radio"] {
                margin-left: 30px;
                margin-right: 10px;
                margin-top: 5px;
                transform: scale(1, 1);
                -ms-transform: scale(1, 1);
                -webkit-transform: scale(1, 1);
                align-items: center;
            }

            table {
                width: 375px;
            }

            .divB {
                display: none;
            }

            #btn_reject {
                width: 300px;
                white-space: normal;
                word-wrap: break-word;
            }

            .option {
                margin-left: 50px;
            }
        }

        ol {
            list-style-type: disc;
        }

        #hero .btn-get-started {
            font-size: 15px !important;
        }


        .badge-primary {
            color: #fff;
            background-color: #e43c5c !important;
        }

        .table-responsive1 {
            display: block;
            width: 100%;
            -webkit-overflow-scrolling: touch;
        }

        #btn_submit:hover {
            background-color: #E43C5C !important;
            color: white !important;
        }

        #btn_reject:hover {
            background-color: #E43C5C !important;
            color: white !important;
        }

        .auto-style1 {
            width: 216px;
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

                <h1 class="logo mr-auto"><a href="SelectAssesor_opr.aspx">360 DEGREE FEEDBACK SURVEY</a></h1>
                <!-- Uncomment below if you prefer to use an image logo -->
                <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->

                <nav class="nav-menu d-none d-lg-block">
                    <ul>
                        <li class="active"><a href="DownloadReport.aspx">Home</a></li>
                    </ul>
                    <ul>
                    </ul>
                </nav>
                <!-- .nav-menu -->

            </div>
        </header>
        <!-- End Header -->
        <asp:ScriptManager runat="server" ID="scpmgr">
        </asp:ScriptManager>
        <!-- ======= Hero Section ======= -->

        <section id="hero">
            <div class="hero-container">
                <h3>Welcome  <strong>
                    <asp:Label ID="lblname" runat="server" Text=""></asp:Label></strong></h3>

                <h2 class="table-responsive1">As we aspire to be the most valuable and respected steel company globally in the next 5-10 years, we are developing agile behaviours across the organization. We will measure this as an integral part of our Performance Management System for all the officers through a 360 degree feedback survey</h2>

                <a href="#about" class="btn-get-started scrollto">View Report</a>
            </div>
        </section>
        <!-- End Hero -->
        <div class="col-lg-12 col-sm-12 col-md-12">
            <main id="main">

                <!-- ======= About Section ======= -->
                <section id="about" class="about">
                    <div class="container">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <label>Year</label>
                                        <asp:DropDownList runat="server" ID="ddlYear" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <label>Cycle</label>
                                        <asp:DropDownList runat="server" ID="ddlCycle" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <div style="margin-top:30px;margin-left:30px;float:left;">
                                            <asp:LinkButton ID="btnsearch" runat="server" class="btn btn-primary" type="submit" CausesValidation="false" OnClick="Button5_Click">
                     Show
                </asp:LinkButton> 
                                        </div>                                        
                                    </div>
                                </div>                                 
                            </div>
                            <div class="offset-3">

                            </div>
                        </div>
                    </div>
                </section>


                <!-- End About Section -->

            </main><!-- End #main -->
        </div>
        <!-- ======= Footer ======= -->
        <footer id="footer">



            <div class="container d-md-flex py-4">
                <div class="mr-md-auto text-center">
                    <span>In case of any queries or issues, please reach out to your BUHR. </span>
                    <span>In case of any system specific queries or issues, please reach out to <b>IT helpdesk (it_helpdesk@tatasteel.com)</b></span>
                </div>
            </div>
        </footer>
        <!-- End Footer -->

        <a href="#" class="back-to-top"><i class="ri-arrow-up-line"></i></a>

        <!-- Vendor JS Files -->
        <%-- <script src="assets/vendor/jquery/jquery.min.js"></script> --%>
        <%-- <script src="assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script> --%>
        <script src="assets/vendor/jquery.easing/jquery.easing.min.js"></script>
        <script src="assets/vendor/php-email-form/validate.js"></script>
        <script src="assets/vendor/isotope-layout/isotope.pkgd.min.js"></script>
        <script src="assets/vendor/venobox/venobox.min.js"></script>
        <script src="assets/vendor/owl.carousel/owl.carousel.min.js"></script>

        <!-- Template Main JS File -->
        <script src="assets/js/main.js"></script>
        <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script> --%>
    </form>
</body>

</html>
