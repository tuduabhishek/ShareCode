<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SelectAssesor_OPR.aspx.vb" Inherits="SelectAssesor_OPR"
    MaintainScrollPositionOnPostback="true" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        <!DOCTYPE html>
        <html lang="en">
        <%--<summary>
            Manoj KUmar 30-05-2021
            WI368 Add css class
            </summary>
            --%>

            <head>
                <meta charset="utf-8">
                <meta content="width=device-width, initial-scale=1" name="viewport">


                <title>360 Survey</title>
                <meta content="" name="descriptison">
                <meta content="" name="keywords">
                <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
                <%--<meta content="width=device-width, initial-scale=1" name="viewport" />--%>
                <!-- Favicons -->
                <%--<link href="assets/img/favicon.png" rel="icon">--%>
                    <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">

                    <!-- Google Fonts -->
                    <link href="assets/css/googlefont.css" rel="stylesheet" />

                    <!-- Vendor CSS Files -->
                    <!-- New Library Versions -->
                    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
                        rel="stylesheet">
                    <link href="https://code.jquery.com/ui/1.14.2/themes/ui-lightness/jquery-ui.css" rel="stylesheet">

                    <script src="https://code.jquery.com/jquery-4.0.0-beta.2.min.js"></script>
                    <script src="https://code.jquery.com/ui/1.14.2/jquery-ui.min.js"></script>
                    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
                    <%-- <link href="https://netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
                        rel="stylesheet"> --%>

                        <!-- Vendor CSS Files Commented Out -->
                        <!-- <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"> -->
                        <%--<link href="assets/vendor/icofont/icofont.min.css" rel="stylesheet">--%>
                            <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
                            <%-- Start WI368 by Manoj Kumar on 30-05-2021--%>
                                <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">

                                <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
                                <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">
                                <!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"> -->
                                <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
                                <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>
                                <!-- <link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css"> -->
                                <%--<!-- <script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js">
                                    </script> -->--%>
                                    <!-- <script src="//code.jquery.com/jquery-1.11.1.min.js"></script> -->
                                    <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css"
                                        rel="stylesheet">
                                    <!-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script> -->
                                    <!-- Include all compiled plugins (below), or include individual files as needed -->
                                    <!-- <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script> -->


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

                                        .panel-heading span {
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

                                        .wrap-Text {
                                            word-wrap: normal;
                                            word-break: break-all;
                                            width: 30px;
                                        }

                                        #ifMobile1 {
                                            /*background-image: url(/images/arts/IMG_1447m.png)*/
                                            width: 300px;
                                            height: 200px;
                                        }


                                        @media only screen and (min-width:410px) and (max-width: 429px) {
                                            #headings {
                                                width: 300px;
                                            }
                                        }

                                        .lbl-records {
                                            font-weight: bold;
                                            color: #0000ff;
                                            /* blue label text "Total Records:" */
                                            font-family: Verdana, sans-serif;
                                        }

                                        .btn-total-records {
                                            display: inline-block;
                                            background-color: #f8f8f8;
                                            /* light gray like your button */
                                            color: #d71a28;
                                            /* Tata Steel red text color */
                                            border: 2px solid #d71a28;
                                            border-radius: 30px;
                                            padding: 3px 16px;
                                            font-weight: 600;
                                            font-size: 13px;
                                            text-align: center;
                                            min-width: 70px;
                                            transition: all 0.3s ease;
                                        }

                                        .btn-total-records:hover {
                                            background-color: #d71a28;
                                            color: white;
                                        }
                                    </style>
                                    <script type="text/javascript">


                                        function showmodalAddSabashAwardee() {
                                            var myModal = new bootstrap.Modal(document.getElementById('modalAddSabashAwardee'));
                                            myModal.show();
                                        }

                                        function closemodalAddSabashAwardee() {
                                            var myModalEl = document.getElementById('modalAddSabashAwardee');
                                            var modal = bootstrap.Modal.getInstance(myModalEl);
                                            if (modal) modal.hide();
                                        }

                                        function showmodalAddSabashAwardee1() {
                                            var myModal = new bootstrap.Modal(document.getElementById('modalAddSabashAwardee1'));
                                            myModal.show();
                                        }

                                        function closemodalAddSabashAwardee1() {
                                            var myModalEl = document.getElementById('modalAddSabashAwardee1');
                                            var modal = bootstrap.Modal.getInstance(myModalEl);
                                            if (modal) modal.hide();
                                        }

                                        $(function () {
                                            $('.clickable').on('click', function () {
                                                var effect = $(this).data('effect');
                                                $(this).closest('.panel')[effect]();
                                            })

                                                <%--$("#btntatasteelopr").click(
                                                    function () {
                                                        btnaddpeertsl
                                                        $("#<%=txtmgrnontslpno.ClientID %>").fadeIn(2000);
                                                    })--%>
            //$("#btntatasteelopr").click(function () {                
            //    $("html, body").animate({ scrollTop: 820 }, "slow");
            //    Div18.Visible = true;
            //    divnontsl.Visible = false;
            //    return false;
            //});
            //$("#btnnonopr").click(function () {
            //    $("html, body").animate({ scrollTop: 820 }, "slow");
            //    Div18.Visible = false;
            //    divnontsl.Visible = true;
            //    return false;
            //});
            //$("#btnaddpeertsl").click(function () {
            //    if (Div18.Visible == true)
            //    {
            //        $("html, body").animate({ scrollTop: 1200 }, "slow");
            //    }
            //    else {
            //        $("html, body").animate({ scrollTop: 900 }, "slow");
            //    }

            //    divtsl.Visible = true;
            //    divntsl.Visible = false;
            //    return false;
            //});
        })

                                        function showGenericMessageModal(type, message) {
                                            swal('', message, type);
                                        }

                                        function openURL() {
                                            var isIE = false || !!document.documentMode;

                                            if (isIE == true) {
                                                //var shell = new ActiveXObject("WScript.Shell");

                                                //shell.run("Chrome http://webappsdev01.corp.tatasteel.com/Feedback360/SelectAssesor_OPR.aspx", true);

                                            }
                                            else {
                                                //var shell = new ActiveXObject("WScript.Shell");

                                                //shell.run("Chrome http://webappsdev01.corp.tatasteel.com/Feedback360/SelectAssesor_OPR.aspx", true);
                                            }
                                        }

                                    </script>

            </head>

            <body onload="openURL();" onkeydown="return (event.keyCode != 116)">


                <form id="form1" runat="server">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                        <ProgressTemplate>
                            <div class="divWaiting">
                                <center>
                                    <div>
                                        <h3>
                                            <asp:Label ID="lblWait" runat="server" Text="Please wait" />
                                        </h3>
                                        <img src="Images/loader.gif" alt="Loading..." id="ifMobile1" />
                                    </div>
                                </center>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <!-- ======= Header ======= -->
                    <header id="header" class="fixed-top ">
                        <div class="container d-flex align-items-center">

                            <h1 class="logo mr-auto" id="headings"><a>360 DEGREE FEEDBACK SURVEY</a></h1>
                            <!-- Uncomment below if you prefer to use an image logo -->
                            <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->

                            <nav class="nav-menu d-none d-lg-block">
                                <ul>
                                    <li class="active"><a href="SelectAssesor_OPR.aspx">Home</a></li>
                                    <!--<li><a href="#about">About</a></li>
          <li><a href="#services">Services</a></li>
          <li><a href="#portfolio">Portfolio</a></li>
          <li><a href="#team">Team</a></li>
          <li><a href="blog.html">Blog</a></li>
          <li class="drop-down"><a href="">Drop Down</a>
            <ul>
              <li><a href="#">Drop Down 1</a></li>
              <li class="drop-down"><a href="#">Deep Drop Down</a>
                <ul>
                  <li><a href="#">Deep Drop Down 1</a></li>
                  <li><a href="#">Deep Drop Down 2</a></li>
                  <li><a href="#">Deep Drop Down 3</a></li>
                  <li><a href="#">Deep Drop Down 4</a></li>
                  <li><a href="#">Deep Drop Down 5</a></li>
                </ul>
              </li>
              <li><a href="#">Drop Down 2</a></li>
              <li><a href="#">Drop Down 3</a></li>
              <li><a href="#">Drop Down 4</a></li>
            </ul>
          </li>
          <li><a href="#contact">Contact</a></li>-->
                                    <li><a href="Images/Step -1_Selection of Respondents.pdf" target="_blank">Help</a>
                                    </li>

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
                            <h3>Welcome <strong>
                                    <asp:Label ID="lblname" runat="server" Text=""></asp:Label>
                                </strong></h3>
                            <!--<h1>We're Creative Agency</h1>-->

                            <h2 class="table-responsive1">As we aspire to be the most valuable and respected steel
                                company globally in the coming years, we are developing agile behaviours across the
                                organization. We will measure this as an integral part of our Performance Management
                                System for all the officers through a 360 degree feedback survey.

                            </h2>
                            <a href="#about" class="btn-get-started scrollto">SELECT RESPONDENTS</a>
                        </div>
                    </section>
                    <!-- End Hero -->

                    <main id="main">

                        <!-- ======= About Section ======= -->
                        <section id="about" class="about">
                            <div class="container">
                                <div class="section-title">
                                    <h3 style="font-size: 24pt;">Feedback would be requested from <span id="nocat"
                                            runat="server"></span>category of respondents - <span id="namecat"
                                            runat="server"></span></h3>
                                    <h4 style="text-align: justify">Accordingly, as a part of your own 360 survey, you
                                        are requested to select the appropriate number of respondents for every
                                        category.<span id="catdetails" runat="server" visible="false"></span><span
                                            id="mgcat" runat="server" visible="false"></span></h4>
                                </div>
                                <ul class="faq-listmg" style="list-style-type: none;">

                                    <li>
                                        <a data-bs-toggle="collapse" href="#catg4" class="collapsed">
                                            <h3 style="font-size: 20pt;">MANAGER/ SUPERIOR
                                                <asp:Label runat="server" ID="lblmmms" Font-Size="Large"></asp:Label>
                                            </h3>
                                        </a>
                                        <div id="catg4" class="collapse" data-parent=".faq-listmg">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel9">
                                                <ContentTemplate>
                                                    <div class="container">
                                                        <div class="row">
                                                            <h4>All reporting managers (i.e., functional, skip level
                                                                manager and administrative) of the officer
                                                            </h4>
                                                        </div>
                                                        <%--'''''''''''''''''''''''''''add
                                                            here''''''''''''''''''''''''''''''''''--%>

                                                            <div class="row content">
                                                                <div class="col-md-3">


                                                                    <asp:Button runat="server" ID="btntatasteelopr"
                                                                        Text="Add Tata Steel" class="btn-learn-more"
                                                                        OnClick="btntatasteelopr_Click"></asp:Button>
                                                                </div>

                                                                <div class="col-md-3">
                                                                    <asp:Button runat="server" ID="btnnonopr"
                                                                        Text="Add Non Tata Steel" class="btn-learn-more"
                                                                        OnClick="btnnonopr_Click"></asp:Button>
                                                                </div>
                                                            </div>

                                                    </div>
                                                    <br />
                                                    <div class="row" runat="server" id="Div18" visible="false">
                                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                                            <div class="panel panel-info" id="close9">
                                                                <div class="panel-heading bg-transparent">
                                                                    <h3 class="panel-title">Enter P.no or Name and click
                                                                        on <b>Add</b> button</h3>
                                                                    <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                    <span class="float-end clickable">
                                                                        <button type="button" class="fa fa-times fa-1x"
                                                                            data-bs-target="#close9" aria-label="Close"
                                                                            data-bs-dismiss="alert">
                                                                        </button>
                                                                    </span>
                                                                </div>
                                                                <div class="panel-body">
                                                                    <div class="row">
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtpnoopr"
                                                                                        CssClass="form-control"
                                                                                        placeholder="P. No"
                                                                                        AutoPostBack="true"
                                                                                        OnTextChanged="txtPnoNoOpr_TextChanged" />
                                                                                    <cc1:AutoCompleteExtender
                                                                                        ID="AutoCompleteExtender9"
                                                                                        runat="server"
                                                                                        TargetControlID="txtpnoopr"
                                                                                        ServiceMethod="SearchPrefixesForApprover1"
                                                                                        MinimumPrefixLength="1"
                                                                                        CompletionInterval="100"
                                                                                        DelimiterCharacters=""
                                                                                        Enabled="True" ServicePath=""
                                                                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                                        CompletionListCssClass="AutoExtender"
                                                                                        CompletionListItemCssClass="AutoExtenderList">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <cc1:FilteredTextBoxExtender
                                                                                        runat="server" ID="ftbppnoopr"
                                                                                        TargetControlID="txtpnoopr"
                                                                                        FilterMode="InvalidChars"
                                                                                        InvalidChars="':;()--#">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtdesgopr"
                                                                                        CssClass="form-control"
                                                                                        placeholder="Designation"
                                                                                        Enabled="false" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtemailopr"
                                                                                        CssClass="form-control "
                                                                                        placeholder="Email"
                                                                                        Enabled="false" />
                                                                                    <asp:RegularExpressionValidator
                                                                                        ID="RegularExpressionValidator9"
                                                                                        runat="server"
                                                                                        ErrorMessage="Please Enter a valid Email ID"
                                                                                        ControlToValidate="txtemailopr"
                                                                                        CssClass="label label-danger fontWhite"
                                                                                        Display="Dynamic"
                                                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                    </asp:RegularExpressionValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtorgopr"
                                                                                        CssClass="form-control "
                                                                                        placeholder="Org Name"
                                                                                        Enabled="false" />
                                                                                    <%-- <asp:HiddenField runat="server"
                                                                                        ID="hdfnpeer" />--%>
                                                                                    <asp:Label runat="server"
                                                                                        ID="lblpeerlevelopr"
                                                                                        Visible="false"></asp:Label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row content">
                                                                        <div class="col-lg-5">
                                                                        </div>
                                                                        <div class="col-lg-6">
                                                                            <asp:Button runat="server" ID="btnAddopr"
                                                                                Text="Add" class="btn-learn-more"
                                                                                OnClick="btnaddmanager_Click">
                                                                            </asp:Button>
                                                                        </div>

                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row" runat="server" id="divnontsl" visible="false">
                                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                                            <div class="panel panel-info" id="closemgrnontsl">
                                                                <div class="panel-heading bg-transparent">
                                                                    <h3 class="panel-title">Enter details and click on
                                                                        <b>Add</b> button
                                                                    </h3>
                                                                    <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                    <span class="float-end clickable">
                                                                        <button type="button" class="fa fa-times fa-1x"
                                                                            data-bs-target="#closemgrnontsl"
                                                                            aria-label="Close" data-bs-dismiss="alert">
                                                                        </button>
                                                                        </i></span>
                                                                </div>
                                                                <div class="panel-body">
                                                                    <div class="row">
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtmgrnontslpno"
                                                                                        CssClass="form-control"
                                                                                        placeholder="Name"
                                                                                        MaxLength="50" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtdesgnontslpno"
                                                                                        CssClass="form-control"
                                                                                        placeholder="Designation"
                                                                                        MaxLength="50" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtmailnontsl"
                                                                                        CssClass="form-control "
                                                                                        placeholder="Email"
                                                                                        MaxLength="50" />
                                                                                    <asp:RegularExpressionValidator
                                                                                        ID="RegularExpressionValidator7"
                                                                                        runat="server"
                                                                                        ErrorMessage="Please Enter a valid Email ID"
                                                                                        ControlToValidate="txtmailnontsl"
                                                                                        CssClass="label label-danger fontWhite"
                                                                                        Display="Dynamic"
                                                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                    </asp:RegularExpressionValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtorgnamenon"
                                                                                        CssClass="form-control "
                                                                                        placeholder="Org Name"
                                                                                        MaxLength="50" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <br />
                                                                    <div class="row content">
                                                                        <div class="col-lg-3">
                                                                        </div>
                                                                        <div class="col-lg-7">
                                                                            <asp:Label runat="server" ID="lblcaptmsg"
                                                                                Text="Enter Captcha before clicking on Add button"
                                                                                class="label label-primary"
                                                                                Visible="false"></asp:Label>
                                                                        </div>


                                                                    </div>
                                                                    <div class="row content">
                                                                        <div class="col-lg-3">
                                                                        </div>
                                                                        <div class="col-lg-3">
                                                                            <div class="form-group">
                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtotpman"
                                                                                        CssClass="form-control "
                                                                                        placeholder="Captcha"
                                                                                        MaxLength="6"
                                                                                        oncopy="return false"
                                                                                        onpaste="return false"
                                                                                        oncut="return false"
                                                                                        Visible="false" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-lg-3">
                                                                            <div class="form-group">
                                                                                <div class="col-md-12">
                                                                                    <asp:Label runat="server"
                                                                                        ID="lblcaptman"
                                                                                        class="label label-info"
                                                                                        Style="font-size: 14px; user-select: none"
                                                                                        Visible="false" />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>

                                                                    <div class="row content">
                                                                        <div class="col-lg-3">
                                                                        </div>
                                                                        <div
                                                                            class="col-lg-3 d-flex justify-content-center">
                                                                            <asp:Button runat="server"
                                                                                ID="btnaddmanagernon" Text="Add"
                                                                                class="btn-learn-more"
                                                                                OnClick="btnaddmanagernon_Click">
                                                                            </asp:Button>
                                                                        </div>


                                                                    </div>

                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>





                                                    <div class="row">
                                                        <div class="col-md-12 col-lg-12">

                                                            <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                                <ContentTemplate>
                                                                    <div class="table-responsive">
                                                                        <div style="margin-top: 10px;">
                                                                            <span class="btn-total-records">Total
                                                                                Records:
                                                                                <asp:Label ID="lblManagerCount"
                                                                                    runat="server" Text="0"></asp:Label>
                                                                            </span>
                                                                        </div>
                                                                        <br />
                                                                        <asp:GridView ID="GvManager" runat="server"
                                                                            AutoGenerateColumns="False"
                                                                            CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                                            Font-Names="verdana" Width="95%"
                                                                            Style="overflow: scroll"
                                                                            EmptyDataText="No Record Found"
                                                                            OnRowDataBound="GvManager_RowDataBound"
                                                                            BackColor="#ffccff" BorderColor="Black"
                                                                            BorderStyle="None" BorderWidth="1px"
                                                                            CellPadding="3" GridLines="Vertical"
                                                                            RowStyle-CssClass="rows">
                                                                            <FooterStyle BackColor="#CCCCCC"
                                                                                ForeColor="Black" />
                                                                            <HeaderStyle
                                                                                CssClass="bg-clouds segoe-light"
                                                                                BackColor="#FFB6C1" Font-Bold="True"
                                                                                ForeColor="Black" />
                                                                            <AlternatingRowStyle BackColor="#FFB6C1" />
                                                                            <Columns>

                                                                                <asp:TemplateField HeaderText="P.no">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblpno"
                                                                                            runat="server"
                                                                                            Text='<%# Eval("ema_perno")%>'>
                                                                                        </asp:Label>
                                                                                        <%--<asp:HiddenField
                                                                                            runat="server" ID="hdfnid"
                                                                                            Value='<%# Eval("SS_ID")%>' />--%>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Name">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lnlname"
                                                                                            runat="server"
                                                                                            Text='<%# Eval("ema_ename")%>'>
                                                                                        </asp:Label>

                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Level">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lbllevel"
                                                                                            runat="server"
                                                                                            Text='<%# Eval("EMA_EMPL_PGRADE")%>'>
                                                                                        </asp:Label>

                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField
                                                                                    HeaderText="Designation">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lbldesg"
                                                                                            runat="server"
                                                                                            Text='<%# Eval("EMA_DESGN_DESC")%>'>
                                                                                        </asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField
                                                                                    HeaderText="Department">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lbldept"
                                                                                            runat="server"
                                                                                            Text='<%# Eval("EMA_DEPT_DESC")%>'>
                                                                                        </asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField
                                                                                    HeaderText="Email Id">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblemail"
                                                                                            runat="server"
                                                                                            Text='<%# Eval("EMA_EMAIL_ID")%>'
                                                                                            CssClass="wrap-Text">
                                                                                        </asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Email Id"
                                                                                    Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lbltype"
                                                                                            runat="server"
                                                                                            Text='<%# Eval("SSTYPE")%>'
                                                                                            CssClass="wrap-Text">
                                                                                        </asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField
                                                                                    HeaderText="Uncheck to remove"
                                                                                    ItemStyle-Width="5%"
                                                                                    HeaderStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox runat="server"
                                                                                            ID="chkseldmanagr"
                                                                                            CssClass="checkbox"
                                                                                            AutoPostBack="true"
                                                                                            OnCheckedChanged="chkseldmanagr_CheckedChanged" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle
                                                                                        HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center"
                                                                                        Width="5%" />
                                                                                </asp:TemplateField>

                                                                            </Columns>
                                                                            <PagerStyle BackColor="#999999"
                                                                                ForeColor="Black"
                                                                                HorizontalAlign="Center" />
                                                                            <RowStyle BackColor="White"
                                                                                ForeColor="Black" />
                                                                            <SelectedRowStyle BackColor="#008A8C"
                                                                                Font-Bold="True" ForeColor="White" />
                                                                            <SortedAscendingCellStyle
                                                                                BackColor="#F1F1F1" />
                                                                            <SortedAscendingHeaderStyle
                                                                                BackColor="#0000A9" />
                                                                            <SortedDescendingCellStyle
                                                                                BackColor="#CAC9C9" />
                                                                            <SortedDescendingHeaderStyle
                                                                                BackColor="#000065" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                </ContentTemplate>

                                                            </asp:UpdatePanel>

                                                        </div>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>

                                    </li>
                                </ul>
                                <ul class="faq-list" style="list-style-type: none;">
                                    <li>
                                        <a data-bs-toggle="collapse" href="#faq10" class="collapsed" id="lnk_sub"
                                            runat="server" visible="False">
                                            <h3 style="font-size: 20pt;">SUBORDINATES
                                                <asp:Label runat="server" ID="lblsub" Font-Size="Large"></asp:Label>
                                            </h3>
                                        </a>
                                        <div id="faq10" class="collapse" data-parent=".faq-list">
                                            <div class="container">
                                                <div class="row">
                                                    <p>
                                                        Subordinate is the individual for whom you are tagged as the
                                                        functional superior.
                                                    </p>
                                                </div>
                                                <asp:UpdatePanel runat="server" ID="uppnlmgr">
                                                    <ContentTemplate>


                                                        <div class="row content">

                                                            <div class="col-md-3">


                                                                <asp:Button runat="server" ID="btnaddtslsub"
                                                                    Text="Add Tata Steel" class="btn-learn-more"
                                                                    OnClick="btnaddtslsub_Click"></asp:Button>
                                                            </div>
                                                            <div class="col-md-3">


                                                                <asp:Button runat="server" ID="btnnontslsub"
                                                                    Text="Add Non Tata Steel" class="btn-learn-more"
                                                                    OnClick="btnnontslsub_Click"></asp:Button>
                                                            </div>
                                                            <div class="col-md-3">
                                                            </div>

                                                        </div>
                                                        <br />
                                                        <div class="row" runat="server" id="div2" visible="false">
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <div class="panel panel-info" id="close5">
                                                                    <div class="panel-heading bg-transparent">
                                                                        <h3 class="panel-title">Enter details and click
                                                                            on <b>Add</b> button</h3>
                                                                        <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                        <span class="float-end clickable">
                                                                            <button type="button"
                                                                                class="fa fa-times fa-1x"
                                                                                data-bs-target="#close5"
                                                                                aria-label="Close"
                                                                                data-bs-dismiss="alert">
                                                                            </button>
                                                                            </i></span>
                                                                    </div>
                                                                    <div class="panel-body">
                                                                        <div class="row">
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtnamemgr"
                                                                                            CssClass="form-control"
                                                                                            placeholder="Name"
                                                                                            MaxLength="50" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtdesgmgr"
                                                                                            CssClass="form-control"
                                                                                            placeholder="Designation"
                                                                                            MaxLength="50" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtemailmgr"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Email"
                                                                                            MaxLength="50" />
                                                                                        <asp:RegularExpressionValidator
                                                                                            ID="RegularExpressionValidator5"
                                                                                            runat="server"
                                                                                            ErrorMessage="Please Enter a valid Email ID"
                                                                                            ControlToValidate="txtemailmgr"
                                                                                            CssClass="label label-danger fontWhite"
                                                                                            Display="Dynamic"
                                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                        </asp:RegularExpressionValidator>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtdeptmgr"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Org Name"
                                                                                            MaxLength="50" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <br />
                                                                        <div class="row content">
                                                                            <div class="col-lg-5">
                                                                            </div>
                                                                            <div class="col-lg-3">
                                                                                <asp:Button runat="server"
                                                                                    ID="btnaddmgr" Text="Add"
                                                                                    class="btn-learn-more"
                                                                                    OnClick="btnaddmgr_Click">
                                                                                </asp:Button>
                                                                            </div>


                                                                        </div>

                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row" runat="server" id="Div3" visible="false">
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <div class="panel panel-info" id="close10">
                                                                    <div class="panel-heading bg-transparent">
                                                                        <h3 class="panel-title">Enter P.no or name and
                                                                            click on <b>Add</b> button</h3>
                                                                        <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                        <span class="float-end clickable">
                                                                            <button type="button"
                                                                                class="fa fa-times fa-1x"
                                                                                data-bs-target="#close10"
                                                                                aria-label="Close"
                                                                                data-bs-dismiss="alert">
                                                                            </button>
                                                                            </i></span>
                                                                    </div>
                                                                    <div class="panel-body">
                                                                        <div class="row">
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtpnosub"
                                                                                            CssClass="form-control"
                                                                                            placeholder="P. No"
                                                                                            AutoPostBack="true"
                                                                                            OnTextChanged="txtpnosub_TextChanged"
                                                                                            MaxLength="50" />
                                                                                        <cc1:AutoCompleteExtender
                                                                                            ID="AutoCompleteExtender3"
                                                                                            runat="server"
                                                                                            TargetControlID="txtpnosub"
                                                                                            ServiceMethod="SearchPrefixesForApprover"
                                                                                            MinimumPrefixLength="1"
                                                                                            CompletionInterval="100"
                                                                                            DelimiterCharacters=""
                                                                                            Enabled="True"
                                                                                            ServicePath=""
                                                                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                                            CompletionListCssClass="AutoExtender"
                                                                                            CompletionListItemCssClass="AutoExtenderList">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <cc1:FilteredTextBoxExtender
                                                                                            runat="server"
                                                                                            ID="FilteredTextBoxExtender2"
                                                                                            TargetControlID="txtpnosub"
                                                                                            FilterMode="InvalidChars"
                                                                                            InvalidChars="':;()--#">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtdesgsub"
                                                                                            CssClass="form-control"
                                                                                            placeholder="Designation"
                                                                                            MaxLength="50" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtmailsub"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Email"
                                                                                            MaxLength="50" />
                                                                                        <asp:RegularExpressionValidator
                                                                                            ID="RegularExpressionValidator6"
                                                                                            runat="server"
                                                                                            ErrorMessage="Please Enter a valid Email ID"
                                                                                            ControlToValidate="txtmailsub"
                                                                                            CssClass="label label-danger fontWhite"
                                                                                            Display="Dynamic"
                                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                        </asp:RegularExpressionValidator>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtdeptsub"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Org Name"
                                                                                            MaxLength="50" />
                                                                                        <%--<asp:HiddenField
                                                                                            runat="server"
                                                                                            ID="hdfnsub" />--%>
                                                                                        <asp:Label runat="server"
                                                                                            ID="lblsublvl"
                                                                                            Visible="false"></asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="row content">
                                                                            <div class="col-lg-5">
                                                                            </div>
                                                                            <div class="col-lg-6">
                                                                                <asp:Button runat="server"
                                                                                    ID="txtaddsub" Text="Add"
                                                                                    class="btn-learn-more"
                                                                                    OnClick="txtaddsub_Click">
                                                                                </asp:Button>
                                                                            </div>

                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>



                                                        <div class="row">
                                                            <div class="col-md-12 col-lg-12">
                                                                <div class="table-responsive">
                                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                                                        <ContentTemplate>
                                                                            <asp:GridView ID="GvRepoties" runat="server"
                                                                                AutoGenerateColumns="False"
                                                                                CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                                                Font-Names="verdana"
                                                                                EmptyDataText="No Record Found"
                                                                                OnRowDataBound="GvRepoties_RowDataBound"
                                                                                BackColor="#ffccff" BorderColor="Black"
                                                                                BorderStyle="None" BorderWidth="1px"
                                                                                CellPadding="3" GridLines="Vertical"
                                                                                RowStyle-CssClass="rows">
                                                                                <FooterStyle BackColor="#CCCCCC"
                                                                                    ForeColor="Black" />
                                                                                <HeaderStyle
                                                                                    CssClass="bg-clouds segoe-light"
                                                                                    BackColor="#FFB6C1" Font-Bold="True"
                                                                                    ForeColor="Black" />
                                                                                <AlternatingRowStyle
                                                                                    BackColor="#FFB6C1" />
                                                                                <Columns>

                                                                                    <asp:TemplateField
                                                                                        HeaderText="P.no">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblpno"
                                                                                                runat="server"
                                                                                                Text='<%# Eval("ema_perno")%>'>
                                                                                            </asp:Label>

                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField
                                                                                        HeaderText="Name">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lnlname"
                                                                                                runat="server"
                                                                                                Text='<%# Eval("ema_ename")%>'>
                                                                                            </asp:Label>

                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField
                                                                                        HeaderText="Level">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbllevel"
                                                                                                runat="server"
                                                                                                Text='<%# Eval("EMA_EMPL_PGRADE")%>'>
                                                                                            </asp:Label>

                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField
                                                                                        HeaderText="Designation">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbldesg"
                                                                                                runat="server"
                                                                                                Text='<%# Eval("EMA_DESGN_DESC")%>'>
                                                                                            </asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField
                                                                                        HeaderText="Department">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbldept"
                                                                                                runat="server"
                                                                                                Text='<%# Eval("EMA_DEPT_DESC")%>'>
                                                                                            </asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField
                                                                                        HeaderText="Email Id">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblemail"
                                                                                                runat="server"
                                                                                                Text='<%# Eval("EMA_EMAIL_ID")%>'
                                                                                                CssClass="wrap-Text">
                                                                                            </asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField
                                                                                        HeaderText="Uncheck to remove">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox runat="server"
                                                                                                ID="chksub"
                                                                                                OnCheckedChanged="chksub_CheckedChanged"
                                                                                                AutoPostBack="true" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>


                                                                                </Columns>
                                                                                <PagerStyle BackColor="#999999"
                                                                                    ForeColor="Black"
                                                                                    HorizontalAlign="Center" />
                                                                                <RowStyle BackColor="White"
                                                                                    ForeColor="Black" />
                                                                                <SelectedRowStyle BackColor="#008A8C"
                                                                                    Font-Bold="True"
                                                                                    ForeColor="White" />
                                                                                <SortedAscendingCellStyle
                                                                                    BackColor="#F1F1F1" />
                                                                                <SortedAscendingHeaderStyle
                                                                                    BackColor="#0000A9" />
                                                                                <SortedDescendingCellStyle
                                                                                    BackColor="#CAC9C9" />
                                                                                <SortedDescendingHeaderStyle
                                                                                    BackColor="#000065" />
                                                                            </asp:GridView>

                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </li>

                                    <li>
                                        <a data-bs-toggle="collapse" class="collapsed" href="#faq6" runat="server"
                                            id="lnk_peers">
                                            <h3 style="font-size: 20pt;">
                                                <asp:Label ID="lblHeaderPeer" runat="server"
                                                    Text="PEERS AND SUBORDINATES" />
                                                <asp:Label runat="server" ID="lblpeer" Font-Size="Large"></asp:Label>
                                            </h3>
                                        </a>
                                        <div id="faq6" class="collapse" data-parent=".faq-list">
                                            <asp:UpdatePanel runat="server" ID="updiv">
                                                <ContentTemplate>
                                                    <div class="container">
                                                        <div class="row">
                                                            <p>
                                                                Peer is defined as officers reporting to the same
                                                                superior(in this case your functional superior).
                                                            </p>
                                                        </div>
                                                        <div class="row content">
                                                            <div class="col-md-3">


                                                                <asp:Button runat="server" ID="btntatasteel"
                                                                    Text="Add Tata Steel" class="btn-learn-more"
                                                                    OnClick="btntatasteel_Click"></asp:Button>
                                                            </div>

                                                            <div class="col-md-3">
                                                                <asp:Button runat="server" ID="btnnontslp"
                                                                    Text="Add Non Tata Steel" class="btn-learn-more"
                                                                    OnClick="btnnontslp_Click"></asp:Button>
                                                            </div>

                                                        </div>
                                                        <br />
                                                        <div class="row" runat="server" id="rowpeer" visible="false">
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <div class="panel panel-info" id="close1">
                                                                    <div class="panel-heading bg-transparent">
                                                                        <h3 class="panel-title">Enter P.no or Name and
                                                                            click on <b>Add</b> button</h3>
                                                                        <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                        <span class="float-end clickable">
                                                                            <button type="button"
                                                                                class="fa fa-times fa-1x"
                                                                                data-bs-target="#close1"
                                                                                aria-label="Close"
                                                                                data-bs-dismiss="alert">
                                                                            </button>
                                                                            </i></span>
                                                                    </div>
                                                                    <div class="panel-body">
                                                                        <div class="row">
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtpnoP"
                                                                                            CssClass="form-control"
                                                                                            placeholder="P. No"
                                                                                            AutoPostBack="true"
                                                                                            OnTextChanged="txtCouponNo_TextChanged"
                                                                                            MaxLength="50" />
                                                                                        <cc1:AutoCompleteExtender
                                                                                            ID="AutoCompleteExtender2"
                                                                                            runat="server"
                                                                                            TargetControlID="txtpnoP"
                                                                                            ServiceMethod="SearchPrefixesForApprover"
                                                                                            MinimumPrefixLength="1"
                                                                                            CompletionInterval="100"
                                                                                            DelimiterCharacters=""
                                                                                            Enabled="True"
                                                                                            ServicePath=""
                                                                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                                            CompletionListCssClass="AutoExtender"
                                                                                            CompletionListItemCssClass="AutoExtenderList">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <cc1:FilteredTextBoxExtender
                                                                                            runat="server" ID="ftbppno"
                                                                                            TargetControlID="txtpnoP"
                                                                                            FilterMode="InvalidChars"
                                                                                            InvalidChars="':;()--#">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtdesgP"
                                                                                            CssClass="form-control"
                                                                                            placeholder="Designation"
                                                                                            Enabled="false" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtemailP"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Email"
                                                                                            Enabled="false" />
                                                                                        <asp:RegularExpressionValidator
                                                                                            ID="RegularExpressionValidator2"
                                                                                            runat="server"
                                                                                            ErrorMessage="Please Enter a valid Email ID"
                                                                                            ControlToValidate="txtemailP"
                                                                                            CssClass="label label-danger fontWhite"
                                                                                            Display="Dynamic"
                                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                        </asp:RegularExpressionValidator>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtorgP"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Org Name"
                                                                                            Enabled="false" />
                                                                                        <%-- <asp:HiddenField
                                                                                            runat="server"
                                                                                            ID="hdfnpeer" />--%>
                                                                                        <asp:Label runat="server"
                                                                                            ID="lblpeerlevel"
                                                                                            Visible="false"></asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="row content">
                                                                            <div class="col-lg-5">
                                                                            </div>
                                                                            <div class="col-lg-6">
                                                                                <asp:Button runat="server" ID="btnAddP"
                                                                                    Text="Add" class="btn-learn-more"
                                                                                    OnClick="btnAddP_Click">
                                                                                </asp:Button>
                                                                            </div>

                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>

                                                        <br />

                                                        <div class="row" runat="server" id="div1" visible="false">
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <div class="panel panel-info" id="close4">
                                                                    <div class="panel-heading bg-transparent">
                                                                        <h3 class="panel-title">Enter details and click
                                                                            on <b>Add</b> button</h3>
                                                                        <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                        <span class="float-end clickable">
                                                                            <button type="button"
                                                                                class="fa fa-times fa-1x"
                                                                                data-bs-target="#close4"
                                                                                aria-label="Close"
                                                                                data-bs-dismiss="alert">
                                                                            </button>
                                                                            </i></span>
                                                                    </div>
                                                                    <div class="panel-body">
                                                                        <div class="row">
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtnmpeer"
                                                                                            CssClass="form-control"
                                                                                            placeholder="Name"
                                                                                            MaxLength="50" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtdesgpeer"
                                                                                            CssClass="form-control"
                                                                                            placeholder="Designation"
                                                                                            MaxLength="50" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtmailpeer"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Email"
                                                                                            MaxLength="50" />
                                                                                        <asp:RegularExpressionValidator
                                                                                            ID="RegularExpressionValidator4"
                                                                                            runat="server"
                                                                                            ErrorMessage="Please Enter a valid Email ID"
                                                                                            ControlToValidate="txtmailpeer"
                                                                                            CssClass="label label-danger fontWhite"
                                                                                            Display="Dynamic"
                                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                        </asp:RegularExpressionValidator>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtdeptpeer"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Org Name"
                                                                                            MaxLength="50" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <br />
                                                                        <div class="row content">
                                                                            <div class="col-lg-3">
                                                                            </div>
                                                                            <div class="col-lg-7 wrap-Text">
                                                                                <asp:Label runat="server" ID="Label2"
                                                                                    Text="Enter Captcha before clicking on Add button"
                                                                                    class="label label-primary"
                                                                                    Visible="false"></asp:Label>
                                                                            </div>


                                                                        </div>
                                                                        <div class="row content">
                                                                            <div class="col-lg-3"></div>
                                                                            <div class="col-lg-3">
                                                                                <div class="form-group">
                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtcaptpeer"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Captcha"
                                                                                            MaxLength="6"
                                                                                            oncopy="return false"
                                                                                            onpaste="return false"
                                                                                            oncut="return false"
                                                                                            Visible="false" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-3">
                                                                                <div class="form-group">
                                                                                    <div class="col-md-12">
                                                                                        <asp:Label runat="server"
                                                                                            ID="lblcaptpeer"
                                                                                            class="label label-info"
                                                                                            Style="font-size: 14px; user-select: none"
                                                                                            Visible="false" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                        </div>
                                                                        <div class="row content">
                                                                            <div class="col-lg-3">
                                                                            </div>
                                                                            <div
                                                                                class="col-lg-3 d-flex justify-content-center">
                                                                                <asp:Button runat="server"
                                                                                    ID="btnaddpeer" Text="Add"
                                                                                    class="btn-learn-more"
                                                                                    OnClick="btnaddpeer_Click">
                                                                                </asp:Button>
                                                                            </div>

                                                                        </div>

                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>




                                                        <div class="row">
                                                            <div class="col-md-12 col-lg-12">
                                                                <div class="table-responsive">
                                                                    <div style="margin-top: 10px;">
                                                                        <span class="btn-total-records">Total Records:
                                                                            <asp:Label ID="lblPeerCount" runat="server"
                                                                                Text="0"></asp:Label>
                                                                        </span>
                                                                    </div>
                                                                    <br />
                                                                    <asp:GridView ID="GvPeer" runat="server"
                                                                        AutoGenerateColumns="False"
                                                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                                        Font-Names="verdana"
                                                                        EmptyDataText="No Record Found"
                                                                        OnRowDataBound="GvPeer_RowDataBound"
                                                                        BackColor="#ffccff" BorderColor="Black"
                                                                        BorderStyle="None" BorderWidth="1px"
                                                                        CellPadding="3" GridLines="Vertical"
                                                                        RowStyle-CssClass="rows">
                                                                        <FooterStyle BackColor="#CCCCCC"
                                                                            ForeColor="Black" />
                                                                        <HeaderStyle CssClass="bg-clouds segoe-light"
                                                                            BackColor="#FFB6C1" Font-Bold="True"
                                                                            ForeColor="Black" />
                                                                        <AlternatingRowStyle BackColor="#FFB6C1" />
                                                                        <Columns>

                                                                            <asp:TemplateField HeaderText="P.no">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblpno"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("ema_perno")%>'>
                                                                                    </asp:Label>
                                                                                    <%-- <asp:HiddenField runat="server"
                                                                                        ID="hdfnid"
                                                                                        Value='<%# Eval("SS_ID")%>' />--%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Name">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lnlname"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("ema_ename")%>'>
                                                                                    </asp:Label>

                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Level">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbllevel"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("EMA_EMPL_PGRADE")%>'>
                                                                                    </asp:Label>

                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Designation">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbldesg"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("EMA_DESGN_DESC")%>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Department">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbldept"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("EMA_DEPT_DESC")%>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Email Id">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblemail"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("EMA_EMAIL_ID")%>'
                                                                                        CssClass="wrap-Text">
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField
                                                                                HeaderText="Uncheck to remove"
                                                                                ItemStyle-Width="5%"
                                                                                HeaderStyle-HorizontalAlign="Center"
                                                                                ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox runat="server"
                                                                                        ID="chkseldsel"
                                                                                        CssClass="checkbox"
                                                                                        AutoPostBack="true"
                                                                                        OnCheckedChanged="chkseldsel_CheckedChanged1" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center"
                                                                                    Width="5%" />
                                                                            </asp:TemplateField>

                                                                        </Columns>
                                                                        <PagerStyle BackColor="#999999"
                                                                            ForeColor="Black"
                                                                            HorizontalAlign="Center" />
                                                                        <RowStyle BackColor="White" ForeColor="Black" />
                                                                        <SelectedRowStyle BackColor="#008A8C"
                                                                            Font-Bold="True" ForeColor="White" />
                                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                        <SortedAscendingHeaderStyle
                                                                            BackColor="#0000A9" />
                                                                        <SortedDescendingCellStyle
                                                                            BackColor="#CAC9C9" />
                                                                        <SortedDescendingHeaderStyle
                                                                            BackColor="#000065" />
                                                                    </asp:GridView>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <%-- <asp:PostBackTrigger ControlID="btntatasteel" />--%>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>

                                    </li>

                                    <li>
                                        <a data-bs-toggle="collapse" class="collapsed" href="#faq7" runat="server"
                                            id="lnk_subordinates" visible="false">
                                            <h3 style="font-size: 20pt;">
                                                <asp:Label ID="Label1" runat="server" Text="SUBORDINATES" />
                                                <asp:Label runat="server" ID="lblSubordinatesCriteria"
                                                    Font-Size="Large"></asp:Label>
                                            </h3>
                                        </a>
                                        <div id="faq7" class="collapse" data-parent=".faq-list">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                <ContentTemplate>
                                                    <div class="container">
                                                        <div class="row">
                                                            <p>
                                                                Subordinate is the individual for whom you are tagged as
                                                                the functional superior.
                                                            </p>
                                                        </div>
                                                        <div class="row content">
                                                            <div class="col-md-3">


                                                                <asp:Button runat="server" ID="btnAddSub"
                                                                    Text="Add Tata Steel" class="btn-learn-more"
                                                                    OnClick="btnAddSub_Click"></asp:Button>
                                                            </div>

                                                            <div class="col-md-3">
                                                                <asp:Button runat="server" ID="btnAddNSSub"
                                                                    Text="Add Non Tata Steel" class="btn-learn-more"
                                                                    OnClick="btnAddNSSub_Click"></asp:Button>
                                                            </div>

                                                        </div>
                                                        <br />
                                                        <div class="row" runat="server" id="rowSubordinates"
                                                            visible="false">
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <div class="panel panel-info" id="close6">
                                                                    <div class="panel-heading bg-transparent">
                                                                        <h3 class="panel-title">Enter P.no or Name and
                                                                            click on <b>Add</b> button</h3>
                                                                        <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                        <span class="float-end clickable">
                                                                            <button type="button"
                                                                                class="fa fa-times fa-1x"
                                                                                data-bs-target="#close6"
                                                                                aria-label="Close"
                                                                                data-bs-dismiss="alert">
                                                                            </button>
                                                                            </i></span>
                                                                    </div>
                                                                    <div class="panel-body">
                                                                        <div class="row">
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtAddSubPno"
                                                                                            CssClass="form-control"
                                                                                            placeholder="P. No"
                                                                                            AutoPostBack="true"
                                                                                            OnTextChanged="txtAddSubPno_TextChanged"
                                                                                            MaxLength="50" />
                                                                                        <cc1:AutoCompleteExtender
                                                                                            ID="AutoCompleteExtender4"
                                                                                            runat="server"
                                                                                            TargetControlID="txtAddSubPno"
                                                                                            ServiceMethod="SearchPrefixesForApprover"
                                                                                            MinimumPrefixLength="1"
                                                                                            CompletionInterval="100"
                                                                                            DelimiterCharacters=""
                                                                                            Enabled="True"
                                                                                            ServicePath=""
                                                                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                                            CompletionListCssClass="AutoExtender"
                                                                                            CompletionListItemCssClass="AutoExtenderList">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <cc1:FilteredTextBoxExtender
                                                                                            runat="server"
                                                                                            ID="FilteredTextBoxExtender3"
                                                                                            TargetControlID="txtAddSubPno"
                                                                                            FilterMode="InvalidChars"
                                                                                            InvalidChars="':;()--#">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtSubDesignation"
                                                                                            CssClass="form-control"
                                                                                            placeholder="Designation"
                                                                                            Enabled="false" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtSubEmail"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Email"
                                                                                            Enabled="false" />
                                                                                        <asp:RegularExpressionValidator
                                                                                            ID="RegularExpressionValidator8"
                                                                                            runat="server"
                                                                                            ErrorMessage="Please Enter a valid Email ID"
                                                                                            ControlToValidate="txtSubEmail"
                                                                                            CssClass="label label-danger fontWhite"
                                                                                            Display="Dynamic"
                                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                        </asp:RegularExpressionValidator>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtSubOrgName"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Org Name"
                                                                                            Enabled="false" />
                                                                                        <%-- <asp:HiddenField
                                                                                            runat="server"
                                                                                            ID="hdfnpeer" />--%>
                                                                                        <asp:Label runat="server"
                                                                                            ID="Label5" Visible="false">
                                                                                        </asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="row content">
                                                                            <div class="col-lg-5">
                                                                            </div>
                                                                            <div class="col-lg-6">
                                                                                <asp:Button runat="server"
                                                                                    ID="btnAddSubTSL" Text="Add"
                                                                                    class="btn-learn-more"
                                                                                    OnClick="btnAddSubTSL_Click">
                                                                                </asp:Button>
                                                                            </div>

                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>

                                                        <br />

                                                        <div class="row" runat="server" id="rowNTSLSubordiinates"
                                                            visible="false">
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <div class="panel panel-info" id="close7">
                                                                    <div class="panel-heading bg-transparent">
                                                                        <h3 class="panel-title">Enter details and click
                                                                            on <b>Add</b> button</h3>
                                                                        <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                        <span class="float-end clickable">
                                                                            <button type="button"
                                                                                class="fa fa-times fa-1x"
                                                                                data-bs-target="#close7"
                                                                                aria-label="Close"
                                                                                data-bs-dismiss="alert">
                                                                            </button>
                                                                            </i></span>
                                                                    </div>
                                                                    <div class="panel-body">
                                                                        <div class="row">
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtAddSubName"
                                                                                            CssClass="form-control"
                                                                                            placeholder="Name"
                                                                                            MaxLength="50" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtSubNDes"
                                                                                            CssClass="form-control"
                                                                                            placeholder="Designation"
                                                                                            MaxLength="50" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtSubNEmail"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Email"
                                                                                            MaxLength="50" />
                                                                                        <asp:RegularExpressionValidator
                                                                                            ID="RegularExpressionValidator10"
                                                                                            runat="server"
                                                                                            ErrorMessage="Please Enter a valid Email ID"
                                                                                            ControlToValidate="txtSubNEmail"
                                                                                            CssClass="label label-danger fontWhite"
                                                                                            Display="Dynamic"
                                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                        </asp:RegularExpressionValidator>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtSubNOrg"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Org Name"
                                                                                            MaxLength="50" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <br />
                                                                        <div class="row content">
                                                                            <div class="col-lg-3">
                                                                            </div>
                                                                            <div class="col-lg-7 wrap-Text">
                                                                                <asp:Label runat="server" ID="Label6"
                                                                                    Text="Enter Captcha before clicking on Add button"
                                                                                    class="label label-primary"
                                                                                    Visible="false"></asp:Label>
                                                                            </div>


                                                                        </div>
                                                                        <div class="row content">
                                                                            <div class="col-lg-3"></div>
                                                                            <div class="col-lg-3">
                                                                                <div class="form-group">
                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="TextBox9"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Captcha"
                                                                                            MaxLength="6"
                                                                                            oncopy="return false"
                                                                                            onpaste="return false"
                                                                                            oncut="return false"
                                                                                            Visible="false" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-3">
                                                                                <div class="form-group">
                                                                                    <div class="col-md-12">
                                                                                        <asp:Label runat="server"
                                                                                            ID="Label7"
                                                                                            class="label label-info"
                                                                                            Style="font-size: 14px; user-select: none"
                                                                                            Visible="false" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                        </div>
                                                                        <div class="row content">
                                                                            <div class="col-lg-3">
                                                                            </div>
                                                                            <div
                                                                                class="col-lg-3 d-flex justify-content-center">
                                                                                <asp:Button runat="server"
                                                                                    ID="btnAddSubNTSL" Text="Add"
                                                                                    class="btn-learn-more"
                                                                                    OnClick="btnAddSubNTSL_Click">
                                                                                </asp:Button>
                                                                            </div>

                                                                        </div>

                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>




                                                        <div class="row">
                                                            <div class="col-md-12 col-lg-12">
                                                                <div class="table-responsive">
                                                                    <div style="margin-top: 10px;">
                                                                        <span class="btn-total-records">Total Records:
                                                                            <asp:Label ID="lblSubOrdCount"
                                                                                runat="server" Text="0"></asp:Label>
                                                                        </span>
                                                                    </div>
                                                                    <br />
                                                                    <asp:GridView ID="gvSubordinates" runat="server"
                                                                        AutoGenerateColumns="False"
                                                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                                        Font-Names="verdana"
                                                                        EmptyDataText="No Record Found"
                                                                        OnRowDataBound="gvSubordinates_RowDataBound"
                                                                        BackColor="#ffccff" BorderColor="Black"
                                                                        BorderStyle="None" BorderWidth="1px"
                                                                        CellPadding="3" GridLines="Vertical"
                                                                        RowStyle-CssClass="rows">
                                                                        <FooterStyle BackColor="#CCCCCC"
                                                                            ForeColor="Black" />
                                                                        <HeaderStyle CssClass="bg-clouds segoe-light"
                                                                            BackColor="#FFB6C1" Font-Bold="True"
                                                                            ForeColor="Black" />
                                                                        <AlternatingRowStyle BackColor="#FFB6C1" />
                                                                        <Columns>

                                                                            <asp:TemplateField HeaderText="P.no">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSubordinatespno"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("ema_perno")%>'>
                                                                                    </asp:Label>
                                                                                    <%-- <asp:HiddenField runat="server"
                                                                                        ID="hdfnid"
                                                                                        Value='<%# Eval("SS_ID")%>' />--%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Name">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lnlSubordinatesname"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("ema_ename")%>'>
                                                                                    </asp:Label>

                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Level">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSubordinateslevel"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("EMA_EMPL_PGRADE")%>'>
                                                                                    </asp:Label>

                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Designation">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSubordinatesdesg"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("EMA_DESGN_DESC")%>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Department">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSubordinatesdept"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("EMA_DEPT_DESC")%>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Email Id">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSubordinatesemail"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("EMA_EMAIL_ID")%>'
                                                                                        CssClass="wrap-Text">
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField
                                                                                HeaderText="Uncheck to remove"
                                                                                ItemStyle-Width="5%"
                                                                                HeaderStyle-HorizontalAlign="Center"
                                                                                ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox runat="server"
                                                                                        ID="chkseldselSubordinates"
                                                                                        CssClass="checkbox"
                                                                                        AutoPostBack="true"
                                                                                        OnCheckedChanged="chkseldselSubordinates_CheckedChanged" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center"
                                                                                    Width="5%" />
                                                                            </asp:TemplateField>

                                                                        </Columns>
                                                                        <PagerStyle BackColor="#999999"
                                                                            ForeColor="Black"
                                                                            HorizontalAlign="Center" />
                                                                        <RowStyle BackColor="White" ForeColor="Black" />
                                                                        <SelectedRowStyle BackColor="#008A8C"
                                                                            Font-Bold="True" ForeColor="White" />
                                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                        <SortedAscendingHeaderStyle
                                                                            BackColor="#0000A9" />
                                                                        <SortedDescendingCellStyle
                                                                            BackColor="#CAC9C9" />
                                                                        <SortedDescendingHeaderStyle
                                                                            BackColor="#000065" />
                                                                    </asp:GridView>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <%-- <asp:PostBackTrigger ControlID="btntatasteel" />--%>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>

                                    </li>

                                    <li>
                                        <a data-bs-toggle="collapse" href="#catg3" class="collapsed" runat="server"
                                            id="lnk_intsh" visible="false">
                                            <h3 style="font-size: 20pt;">
                                                <asp:Label ID="lbl_intsh" runat="server"></asp:Label>&nbsp;<asp:Label
                                                    runat="server" ID="lblinst" Font-Size="Large"></asp:Label>
                                            </h3>
                                        </a>
                                        <div id="catg3" class="collapse" data-parent=".faq-list">
                                            <div class="container">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <h4>An officer (within the Tata Steel ecosystem) with whom
                                                                you have a working/ business relationship. This category
                                                                of respondents contains the name of officers whom you
                                                                would have chosen as collaborator during UpNext
                                                                performance contracting stage.
                                                            </h4>
                                                        </div>
                                                        <div class="row content">
                                                            <div class="col-md-3">


                                                                <asp:Button runat="server" ID="btnaddpeertsl"
                                                                    Text="Add Tata Steel" class="btn-learn-more"
                                                                    OnClick="btnaddpeertatasteel_Click"></asp:Button>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:Button runat="server" ID="btnaddnontsl"
                                                                    Text="Add Non Tata Steel" class="btn-learn-more"
                                                                    OnClick="btnaddnontsl_Click"></asp:Button>
                                                            </div>

                                                        </div>
                                                        <br />
                                                        <div class="row" runat="server" id="divtsl" visible="false">
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <div class="panel panel-info" id="close2">
                                                                    <div class="panel-heading bg-transparent">
                                                                        <h3 class="panel-title">Enter P.no or Name and
                                                                            click on <b>Add</b> button</h3>
                                                                        <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                        <span class="float-end clickable">
                                                                            <button type="button"
                                                                                class="fa fa-times fa-1x"
                                                                                data-bs-target="#close2"
                                                                                aria-label="Close"
                                                                                data-bs-dismiss="alert">
                                                                            </button>
                                                                            </i></span>
                                                                    </div>
                                                                    <div class="panel-body">
                                                                        <div class="row">
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtpnoI"
                                                                                            CssClass="form-control"
                                                                                            placeholder="P. No"
                                                                                            AutoPostBack="true"
                                                                                            OnTextChanged="txtpnoI_TextChanged" />
                                                                                        <cc1:AutoCompleteExtender
                                                                                            ID="AutoCompleteExtender1"
                                                                                            runat="server"
                                                                                            TargetControlID="txtpnoI"
                                                                                            ServiceMethod="SearchPrefixesForApprover"
                                                                                            MinimumPrefixLength="1"
                                                                                            CompletionInterval="100"
                                                                                            DelimiterCharacters=""
                                                                                            Enabled="True"
                                                                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                                            CompletionListCssClass="AutoExtender"
                                                                                            CompletionListItemCssClass="AutoExtenderList">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <cc1:FilteredTextBoxExtender
                                                                                            runat="server"
                                                                                            ID="FilteredTextBoxExtender1"
                                                                                            TargetControlID="txtpnoI"
                                                                                            FilterMode="InvalidChars"
                                                                                            InvalidChars="':;()--#">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtdesgI"
                                                                                            CssClass="form-control"
                                                                                            placeholder="Designation"
                                                                                            Enabled="false" />
                                                                                        <%--<asp:HiddenField
                                                                                            runat="server"
                                                                                            ID="hdfnin" />--%>
                                                                                        <asp:Label runat="server"
                                                                                            ID="lblinst1"
                                                                                            Visible="false"></asp:Label>
                                                                                        <asp:Label runat="server"
                                                                                            ID="lblinst2"
                                                                                            Visible="false"></asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtemailI"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Email"
                                                                                            Enabled="false" />
                                                                                        <asp:RegularExpressionValidator
                                                                                            ID="RegularExpressionValidator1"
                                                                                            runat="server"
                                                                                            ErrorMessage="Please Enter a valid Email ID"
                                                                                            ControlToValidate="txtemailI"
                                                                                            CssClass="label label-danger fontWhite"
                                                                                            Display="Dynamic"
                                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                        </asp:RegularExpressionValidator>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtdeptI"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Org Name"
                                                                                            Enabled="false" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <br />
                                                                        <div class="row content">
                                                                            <div class="col-lg-5">
                                                                            </div>
                                                                            <div class="col-lg-6">
                                                                                <asp:Button runat="server"
                                                                                    ID="btnorgadd" Text="Add"
                                                                                    class="btn-learn-more"
                                                                                    OnClick="btnorgadd_Click">
                                                                                </asp:Button>
                                                                            </div>

                                                                        </div>

                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" runat="server" id="divntsl" visible="false">
                                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                                <div class="panel panel-info" id="close3">
                                                                    <div class="panel-heading bg-transparent">
                                                                        <h3 class="panel-title">Enter details and click
                                                                            on <b>Add</b> button</h3>
                                                                        <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                        <span class="float-end clickable">
                                                                            <button type="button"
                                                                                class="fa fa-times fa-1x"
                                                                                data-bs-target="#close3"
                                                                                aria-label="Close"
                                                                                data-bs-dismiss="alert">
                                                                            </button>
                                                                            </i></span>
                                                                    </div>
                                                                    <div class="panel-body">
                                                                        <div class="row">
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtnamenI"
                                                                                            CssClass="form-control"
                                                                                            placeholder="Name"
                                                                                            MaxLength="50" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtdesgnI"
                                                                                            CssClass="form-control"
                                                                                            placeholder="Designation"
                                                                                            MaxLength="50" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtemailnI"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Email"
                                                                                            MaxLength="50" />
                                                                                        <asp:RegularExpressionValidator
                                                                                            ID="RegularExpressionValidator3"
                                                                                            runat="server"
                                                                                            ErrorMessage="Please Enter a valid Email ID"
                                                                                            ControlToValidate="txtemailnI"
                                                                                            CssClass="label label-danger fontWhite"
                                                                                            Display="Dynamic"
                                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                        </asp:RegularExpressionValidator>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-md-3">
                                                                                <div class="form-group">

                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtdeptnI"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Org Name"
                                                                                            MaxLength="50" />

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <br />
                                                                        <div class="row content">
                                                                            <div class="col-lg-3">
                                                                            </div>
                                                                            <div class="col-lg-7">
                                                                                <asp:Label runat="server" ID="Label3"
                                                                                    Text="Enter Captcha before clicking on Add button"
                                                                                    class="label label-primary"
                                                                                    Visible="false"></asp:Label>
                                                                            </div>


                                                                        </div>
                                                                        <div class="row content">
                                                                            <div class="col-lg-3"></div>
                                                                            <div class="col-lg-3">
                                                                                <div class="form-group">
                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox runat="server"
                                                                                            ID="txtcaptintsh"
                                                                                            CssClass="form-control "
                                                                                            placeholder="Captcha"
                                                                                            MaxLength="6"
                                                                                            oncopy="return false"
                                                                                            onpaste="return false"
                                                                                            oncut="return false"
                                                                                            Visible="false" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-3">
                                                                                <div class="form-group">
                                                                                    <div class="col-md-12">
                                                                                        <asp:Label runat="server"
                                                                                            ID="lblcaptintsh"
                                                                                            class="label label-info"
                                                                                            Style="font-size: 14px; user-select: none"
                                                                                            Visible="false" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <br />

                                                                        <div class="row content">
                                                                            <div class="col-lg-3">
                                                                            </div>
                                                                            <div
                                                                                class="col-lg-3 d-flex justify-content-center">
                                                                                <asp:Button runat="server"
                                                                                    ID="btnaddnorgI" Text="Add"
                                                                                    class="btn-learn-more"
                                                                                    OnClick="btnaddnorgI_Click">
                                                                                </asp:Button>
                                                                            </div>

                                                                        </div>

                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-12 col-lg-12">
                                                                <div class="table-responsive">
                                                                    <div style="margin-top: 10px;">
                                                                        <span class="btn-total-records">Total Records:
                                                                            <asp:Label ID="lblTotalCount" runat="server"
                                                                                Text="0"></asp:Label>
                                                                        </span>
                                                                    </div>
                                                                    <br />

                                                                    <asp:GridView ID="Gvintstholder" runat="server"
                                                                        AutoGenerateColumns="False"
                                                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                                        Font-Names="verdana"
                                                                        EmptyDataText="No Record Found"
                                                                        OnRowDataBound="Gvintstholder_RowDataBound"
                                                                        BackColor="#ffccff" BorderColor="Black"
                                                                        BorderStyle="None" BorderWidth="1px"
                                                                        CellPadding="3" GridLines="Vertical"
                                                                        RowStyle-CssClass="rows">
                                                                        <FooterStyle BackColor="#CCCCCC"
                                                                            ForeColor="Black" />
                                                                        <HeaderStyle CssClass="bg-clouds segoe-light"
                                                                            BackColor="#FFB6C1" Font-Bold="True"
                                                                            ForeColor="Black" />
                                                                        <AlternatingRowStyle BackColor="#FFB6C1" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="P.no">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblpno"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("ss_pno")%>'>
                                                                                    </asp:Label>
                                                                                    <%-- <asp:HiddenField runat="server"
                                                                                        ID="hdfnid"
                                                                                        Value='<%# Eval("SS_ID")%>' />--%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Name">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lnlname"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("ss_name")%>'>
                                                                                    </asp:Label>

                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Level">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbllevel"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("SS_LEVEL")%>'>
                                                                                    </asp:Label>

                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Designation">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbldesg"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("SS_DESG")%>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Department">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbldept"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("SS_DEPT")%>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Email Id">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblemail"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("SS_EMAIL")%>'
                                                                                        CssClass="wrap-Text">
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField
                                                                                HeaderText="Uncheck to remove"
                                                                                ItemStyle-Width="5%"
                                                                                HeaderStyle-HorizontalAlign="Center"
                                                                                ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox runat="server"
                                                                                        ID="chkseldsel"
                                                                                        CssClass="checkbox"
                                                                                        AutoPostBack="true"
                                                                                        OnCheckedChanged="chkseldsel_CheckedChanged" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center"
                                                                                    Width="5%" />
                                                                            </asp:TemplateField>

                                                                        </Columns>
                                                                        <PagerStyle BackColor="#999999"
                                                                            ForeColor="Black"
                                                                            HorizontalAlign="Center" />
                                                                        <RowStyle BackColor="White" ForeColor="Black" />
                                                                        <SelectedRowStyle BackColor="#008A8C"
                                                                            Font-Bold="True" ForeColor="White" />
                                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                        <SortedAscendingHeaderStyle
                                                                            BackColor="#0000A9" />
                                                                        <SortedDescendingCellStyle
                                                                            BackColor="#CAC9C9" />
                                                                        <SortedDescendingHeaderStyle
                                                                            BackColor="#000065" />
                                                                    </asp:GridView>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <%--<asp:PostBackTrigger ControlID="btnaddpeertsl" />--%>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>

                                    </li>
                                </ul>
                                <ul class="faq-listfinal" style="list-style-type: none;">

                                    <li>
                                        <a data-bs-toggle="collapse" href="#catg6" class="collapsed">
                                            <h3 style="font-size: 20pt;">FINAL RESPONDENTS SELECTED LIST&nbsp;<asp:Label
                                                    runat="server" ID="lblSubmitStatus"
                                                    Text="(Click to Review/Save/Submit)" Font-Size="Large"></asp:Label>
                                            </h3>
                                        </a>
                                        <div id="catg6" class="collapse" data-parent=".faq-listfinal">
                                            <div class="container">
                                                <div class="row">
                                                    <div class="col-md-12 col-lg-12">
                                                        <div class="table-responsive">
                                                            <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="gvfinal" runat="server"
                                                                        AutoGenerateColumns="False"
                                                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                                        Font-Names="verdana"
                                                                        EmptyDataText="No Record Found"
                                                                        BackColor="#ffccff" BorderColor="Black"
                                                                        BorderStyle="None" BorderWidth="1px"
                                                                        CellPadding="3" GridLines="Vertical"
                                                                        RowStyle-CssClass="rows">
                                                                        <FooterStyle BackColor="#CCCCCC"
                                                                            ForeColor="Black" />
                                                                        <HeaderStyle CssClass="bg-clouds segoe-light"
                                                                            BackColor="#FFB6C1" Font-Bold="True"
                                                                            ForeColor="Black" />
                                                                        <AlternatingRowStyle BackColor="#FFB6C1" />
                                                                        <Columns>

                                                                            <asp:TemplateField HeaderText="P.no">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblpno"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("SS_PNO")%>'>
                                                                                    </asp:Label>

                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Name">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lnlname"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("SS_NAME")%>'>
                                                                                    </asp:Label>

                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Level">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbllevel"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("EMA_EMPL_PGRADE")%>'>
                                                                                    </asp:Label>

                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Designation">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbldesg"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("SS_DESG")%>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Department">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbldept"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("SS_DEPT")%>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Email Id">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblemail"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("SS_EMAIL")%>'
                                                                                        CssClass="wrap-Text">
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Category">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblcategory"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("IRC_DESC")%>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField
                                                                                HeaderText="Feedback Status"
                                                                                Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblwflStatus"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("SS_WFL_STATUS")%>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <%--<asp:BoundField
                                                                                HeaderText="Feedback Status"
                                                                                DataField="SS_WFL_STATUS"
                                                                                SortExpression="SS_WFL_STATUS"
                                                                                Visible="false" />--%>
                                                                        </Columns>
                                                                        <PagerStyle BackColor="#999999"
                                                                            ForeColor="Black"
                                                                            HorizontalAlign="Center" />
                                                                        <RowStyle BackColor="White" ForeColor="Black" />
                                                                        <SelectedRowStyle BackColor="#008A8C"
                                                                            Font-Bold="True" ForeColor="White" />
                                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                        <SortedAscendingHeaderStyle
                                                                            BackColor="#0000A9" />
                                                                        <SortedDescendingCellStyle
                                                                            BackColor="#CAC9C9" />
                                                                        <SortedDescendingHeaderStyle
                                                                            BackColor="#000065" />
                                                                    </asp:GridView>

                                                        </div>

                                                        </ContentTemplate>

                                                        <Triggers>
                                                        </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>



                                            <asp:UpdatePanel runat="server" ID="upappbtn">
                                                <ContentTemplate>
                                                    <div>
                                                        <div class="row content col-lg-10">
                                                            <div class="col-lg-4">
                                                            </div>
                                                            <div class="col-lg-5">
                                                                <asp:Button runat="server" ID="btnSaveAsDraft"
                                                                    Text="Save As Draft"
                                                                    class="btn-learn-more stickyDraft"
                                                                    OnClick="btnSaveAsDraft_Click"></asp:Button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <div class="row content">

                                                        <div class="col-lg-10">
                                                            <div class="col-lg-3">
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <asp:Label ID="lblcaptfinalmsg" runat="server"
                                                                    Text="Enter captcha and click on submit button"
                                                                    Class="label label-primary"></asp:Label>
                                                            </div>


                                                        </div>
                                                    </div>
                                                    <div class="row content">

                                                        <div class="col-lg-10">
                                                            <div class="col-lg-3">
                                                            </div>
                                                            <div class="col-lg-5">
                                                                <asp:TextBox runat="server" ID="txtfinalcap"
                                                                    CssClass="form-control " placeholder="Captcha"
                                                                    MaxLength="6" oncopy="return false"
                                                                    onpaste="return false" oncut="return false" />
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:Label runat="server" ID="lblfinalcap"
                                                                    class="label label-info"
                                                                    Style="font-size: 20px; user-select: none; padding: 10px 10px 10px 10px; background-color: #e5dede; border: 1px solid red; color: red;" />
                                                            </div>

                                                        </div>
                                                    </div>
                                                    <div class="row content col-lg-10">
                                                        <div class="col-lg-4">
                                                        </div>

                                                        <div class="col-lg-5">
                                                            <asp:Button runat="server" ID="lbOrg"
                                                                Text="Submit For Approval" class="btn-learn-more"
                                                                data-bs-toggle="modal" data-bs-target="#staticBackdrop">
                                                            </asp:Button>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                            </div>
                            </li>

                            </ul>
                            </div>
                        </section>
                        <!-- End About Section -->

                        <!-- ======= F.A.Q Section ======= -->
                        <section id="faq" class="faq">
                            <div class="container">

                                <div class="section-title">
                                    <%-- <h2>F.A.Q</h2>--%>
                                        <%-- <h3 style="font-size:24pt;">Frequently Asked <span>Questions</span></h3>
                                            --%>
                                </div>

                                <%-- <ul class="faq-list">

                                    <li>
                                        <a data-bs-toggle="collapse" class="collapsed" href="#faq1"
                                            style="font-size:24pt;">Who is a Manager/Superior? <i
                                                class="icofont-simple-up"></i></a>
                                        <div id="faq1" class="collapse" data-parent=".faq-list">
                                            <p>
                                                All reporting managers both solid and dotted reporting and respective
                                                Executive Head.
                                            </p>
                                        </div>
                                    </li>



                                    <li>
                                        <a data-bs-toggle="collapse" href="#faq5" class="collapsed"
                                            style="font-size:24pt;"> Who is a Subordinate?<i
                                                class="icofont-simple-up"></i></a>
                                        <div id="faq5" class="collapse" data-parent=".faq-list">
                                            <p>
                                                Subordinate is the individual for whom you are tagged as the functional
                                                superior.
                                            </p>
                                        </div>
                                    </li>

                                    <li>
                                        <a data-bs-toggle="collapse" href="#faq3" class="collapsed"
                                            style="font-size:24pt;"> Who is a Peer?<i class="icofont-simple-up"></i></a>
                                        <div id="faq3" class="collapse" data-parent=".faq-list">
                                            <p>
                                                Peer is defined as officers reporting to the same superior(in this case
                                                your functional superior).
                                            </p>
                                        </div>
                                    </li>

                                    <li>
                                        <a data-bs-toggle="collapse" href="#faq4" class="collapsed"
                                            style="font-size:24pt;"> Who is an Internal Stakeholder?<i
                                                class="icofont-simple-up"></i></a>
                                        <div id="faq4" class="collapse" data-parent=".faq-list">
                                            <h4>
                                                An Internal Stakeholder may be a Tata Steel/Non Tata Steel professional
                                                with whom you have direct/indirect business relations.
                                            </h4>
                                        </div>
                                    </li>
                                    </ul>--%>
                            </div>
                            <div class="modal fade" id="staticBackdrop" data-bs-backdrop="static"
                                data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel"
                                aria-hidden="true">
                                <div class="modal-dialog">
                                    <div class="modal-content">

                                        <div class="modal-body">
                                            Are you sure you want to submit the form?
                                        </div>
                                        <div class="modal-footer">

                                            <asp:Button runat="server" ID="btnyesclick" OnClick="Submit" Text="Yes"
                                                class="btn btn-primary" />
                                            <button type="button" class="btn btn-secondary"
                                                data-bs-dismiss="modal">No</button>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </section>
                        <!-- End F.A.Q Section -->



                    </main>
                    <!-- End #main -->

                    <!-- ======= Footer ======= -->
                    <footer id="footer">
                        <div class="container d-md-flex py-4">

                            <div class="mr-md-auto text-center">
                                <span>In case of any queries or issues, please reach out to your HRBP. </span>
                                <span>In case of any system specific queries or IT issues, please reach out to<b> IT
                                        helpdesk (it_helpdesk@tatasteel.com)</b></span>
                            </div>
                        </div>

                        <%-- <div class="container d-md-flex py-4">
                            <div class="mr-md-auto text-center text-md-left">
                                <div class="copyright">
                                    &copy; Copyright <strong><span>Tata Steel</span></strong>.
                                </div>
                            </div>
                            </div>--%>
                    </footer>
                    <!-- End Footer -->

                    <a href="#" class="back-to-top"><i class="ri-arrow-up-line"></i></a>

                    <!-- Vendor JS Files Commented Out -->
                    <!-- <script src="assets/vendor/jquery/jquery.min.js"></script> -->
                    <!-- <script src="assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script> -->
                    <script src="assets/vendor/jquery.easing/jquery.easing.min.js"></script>
                    <script src="assets/vendor/php-email-form/validate.js"></script>
                    <script src="assets/vendor/isotope-layout/isotope.pkgd.min.js"></script>
                    <script src="assets/vendor/venobox/venobox.min.js"></script>
                    <script src="assets/vendor/owl.carousel/owl.carousel.min.js"></script>

                    <!-- Template Main JS File -->
                    <script src="assets/js/main.js"></script>
                    <%-- <script type="text/javascript"
                        src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script> --%>
                        <!-- <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.0/jquery.min.js"></script> -->
                        <script type="text/javascript">
                            if (window.history.replaceState) {
                                window.history.replaceState(null, null, window.location.href);
                            }
                            $(function () {
                                $(document).keydown(function (e) {
                                    return (e.which || e.keyCode) != 116;
                                });
                            });

                            var timeout = '<%=ConfigurationManager.AppSettings("sessionTimeOut").ToString() %>';
                            var timeOutWarning = '<%=ConfigurationManager.AppSettings("timeOutWarning").ToString() %>';
                            // section for autotimeout-- START
                            var sessionTimeoutWarning = timeOutWarning;
                            var sessionTimeout = timeout;
                            var timeOnPageLoad = new Date();
                            var sessionWarningTimer = null;
                            var RedirectToSamePageTimer = null;
                            //For warning
                            var sessionWarningTimer = setTimeout('SessionWarning()',
                                parseInt(sessionTimeoutWarning) * 60 * 1000);
                            //To redirect to the welcome page
                            var RedirectToSamePageTimer = setTimeout('RedirectToSamePage()',
                                parseInt(sessionTimeout) * 60 * 1000);

                            //Session Warning
                            function SessionWarning() {
                                //minutes left for expiry
                                var minutesForExpiry = (parseInt(sessionTimeout) - parseInt(sessionTimeoutWarning));
                                var message = "Your session will expire in another " + minutesForExpiry + " mins. Please save your work.";
                                //if yes, extend the session.
                                swal({
                                    title: 'Warning',
                                    text: message,
                                    icon: 'warning'
                                }, function () {
                                    $.ajax({
                                        type: "POST",
                                        url: "SelectAssesor_OPR.aspx/ResetSession",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function (r) {
                                            newTimeout = r.d;
                                            escape(new Date());
                                            clearTimeout(sessionWarningTimer);
                                            if (RedirectToSamePageTimer != null) {
                                                clearTimeout(RedirectToSamePageTimer);
                                            }
                                            //reset the time on page load
                                            timeOnPageLoad = new Date();
                                            sessionTimeoutWarning = timeOutWarning;
                                            sessionTimeout = timeout;
                                            sessionWarningTimer = setTimeout('SessionWarning()',
                                                parseInt(sessionTimeoutWarning) * 60 * 1000);
                                            //To redirect to the welcome page
                                            RedirectToSamePageTimer = setTimeout
                                                ('RedirectToSamePage()', parseInt(sessionTimeout) * 60 * 1000);
                                        },
                                        error: function (r) {
                                            alert(r.d);
                                        }
                                    });
                                })
                                //*************************
                                //Even after clicking ok(extending session) or cancel button,
                                //if the session time is over. Then exit the session.
                                var currentTime = new Date();
                                //time for expiry
                                var timeForExpiry = timeOnPageLoad.setMinutes(timeOnPageLoad.getMinutes() +
                                    parseInt(sessionTimeout));

                                //Current time is greater than the expiry time
                                if (Date.parse(currentTime) > timeForExpiry) {
                                    swal({
                                        title: 'Session expired.',
                                        text: 'Page will auto refreshed!!',
                                        icon: 'warning',
                                        timer: 5000,
                                        buttons: false,
                                        allowOutsideClick: false,
                                        allowEscapeKey: false
                                    }, function () {
                                        window.location.reload();
                                    })
                                }
                                //**************************
                            }
                            //Session timeout
                            function RedirectToSamePage() {
                                //swal("Warning","Session expired. You will be redirected to Logout Page!!","warning");
                                //window.location.reload();
                                swal({
                                    title: 'Session expired.',
                                    text: 'Page will auto refreshed!!',
                                    icon: 'warning',
                                    timer: 5000,
                                    buttons: false,
                                    allowOutsideClick: false,
                                    allowEscapeKey: false
                                }, function () {
                                    window.location.reload();
                                })
                            }
                            function draftButtonPosition() {
                                var nav = $('#about');
                                var btn = $('#btnSaveAsDraft');
                                if (nav.length && btn.length) {
                                    var sOffset = $("#about").offset().top;
                                    var btnPosition = $("#btnSaveAsDraft").offset().top;
                                    scrollFunction(sOffset, btnPosition);
                                    $(window).scroll(function () {
                                        scrollFunction(sOffset, btnPosition);
                                    });
                                }
                            };
                            function scrollFunction(sOffset, btnPosition) {
                                var scrollYpos = $(document).scrollTop();
                                if (isScrolledIntoView("#lbOrg")) {
                                    $(".stickyDraft").css({
                                        'top': 'auto',
                                        'position': 'relative',
                                        'right': '0%'

                                    });
                                }
                                else {
                                    if (scrollYpos + 300 > sOffset) {
                                        $(".stickyDraft").css({
                                            'position': 'fixed',
                                            'right': '5%',
                                            'bottom': '2%',
                                            'z-index': '1000',
                                            'transform': 'rotate(360deg)',
                                            'webkit-transform': 'rotate(360deg)',
                                            '-moz-transform': 'rotate(360deg)',
                                            '-o-transform': 'rotate(360deg)',
                                            'filter': 'progid: dximagetransform.microsoft.basicimage(rotation=3)',
                                            'text-align': 'center',
                                            'text-decoration': 'none',
                                        });
                                    } else {
                                        $(".stickyDraft").css({
                                            'top': 'auto',
                                            'position': 'relative',
                                            'right': '0%'

                                        });
                                    }
                                }
                            }
                            function isScrolledIntoView(elem) {
                                var docViewTop = $(window).scrollTop();
                                var docViewBottom = docViewTop + $(window).height();

                                var elemTop = $(elem).offset().top;
                                var elemBottom = elemTop + $(elem).height();

                                return ((elemBottom <= docViewBottom) && (elemTop >= docViewTop));
                            };

                            function pageLoad() {
                                draftButtonPosition();
                                escape(new Date());
                                clearTimeout(sessionWarningTimer);
                                if (RedirectToSamePageTimer != null) {
                                    clearTimeout(RedirectToSamePageTimer);
                                }
                                //reset the time on page load
                                timeOnPageLoad = new Date();
                                sessionTimeoutWarning = timeOutWarning;
                                sessionTimeout = timeout;
                                sessionWarningTimer = setTimeout('SessionWarning()',
                                    parseInt(sessionTimeoutWarning) * 60 * 1000);
                                //To redirect to the welcome page
                                RedirectToSamePageTimer = setTimeout
                                    ('RedirectToSamePage()', parseInt(sessionTimeout) * 60 * 1000);
                            }
                        </script>
                </form>
            </body>

        </html>