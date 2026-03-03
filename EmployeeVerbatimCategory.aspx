<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EmployeeVerbatimCategory.aspx.vb"
    Inherits="EmployeeVerbatimCategory" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <meta charset="utf-8" />
    <meta content="width=device-width, initial-scale=1" name="viewport" />

    <title>360 Survey</title>
    <meta content="" name="descriptison" />
    <meta content="" name="keywords" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <!-- Favicons -->
    <link href="assets/img/favicon.png" rel="icon" />
    <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon" />

    <!-- Google Fonts -->
    <link href="assets/css/googlefont.css" rel="stylesheet" />

    <!-- Bootstrap 5.3 CSS (replaces older Bootstrap 3 references) -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Vendor / Icon CSS -->
    <link href="assets/vendor/icofont/icofont.min.css" rel="stylesheet" />
    <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet" />
    <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet" />
    <link href="assets/vendor/venobox/venobox.css" rel="stylesheet" />
    <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet" />
    <%-- <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet" /> --%>

    <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
    <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>

    <!-- Template Main CSS File -->
    <link href="assets/css/styleIL3.css" rel="stylesheet" />

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
                <h1 class="logo me-auto"><a
                        href="https://irisapp-tslin.msappproxy.net/feedback_360/surveyadm_opr.aspx">360 DEGREE
                        FEEDBACK SURVEY</a></h1>

                <nav class="nav-menu d-none d-lg-block">
                    <ul>
                        <li class="active"><a
                                href="https://irisapp-tslin.msappproxy.net/feedback_360/surveyadm_opr.aspx">Home
                                (Admin)</a></li>
                    </ul>
                </nav>
            </div>
        </header>
        <!-- End Header -->

        <asp:ScriptManager runat="server" ID="scpmgr"></asp:ScriptManager>

        <!-- ======= Hero Section ======= -->
        <section id="hero">
            <div class="hero-container">
                <h3>Welcome <strong>
                        <asp:Label ID="lblname" runat="server" Text="Admin"></asp:Label>
                    </strong></h3>

                <h2 class="table-responsive1">As we aspire to be the most valuable and respected steel company
                    globally in the coming years, we are developing agile behaviours across the organization. We
                    will measure this as an integral part of our Performance Management System for all the
                    officers through a 360 degree feedback survey.
                </h2>
                <a href="#about" class="btn-get-started scrollto">VIEW REPORT</a>
            </div>
        </section>
        <!-- End Hero -->

        <main id="main">
            <!-- ======= About Section ======= -->
            <section id="about" class="about">
                <div class="container-fluid">
                    <div class="row mb-3">
                        <div class="col-lg-12">
                            <h1>Employee Verbatim Category</h1>
                        </div>
                    </div>

                    <asp:UpdatePanel ID="upnl1" runat="server">
                        <ContentTemplate>
                            <div class="panel-body">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server"
                                    CssClass="text-danger" HeaderText="Please correct the following:" />

                                <div class="row g-3 align-items-end">
                                    <div class="col-md-1">
                                        <label class="form-label">Year <span class="text-danger">*</span></label>
                                        <asp:TextBox runat="server" ID="txtYear" CssClass="form-control" MaxLength="4" />
                                        <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="txtYear"
                                            ErrorMessage="Year is required" CssClass="text-danger" Display="Dynamic" />
                                    </div>

                                    <div class="col-md-1">
                                        <label class="form-label">Cycle <span class="text-danger">*</span></label>
                                        <asp:TextBox runat="server" ID="txtCycle" CssClass="form-control" MaxLength="1" />
                                        <asp:RequiredFieldValidator ID="rfvCycle" runat="server" ControlToValidate="txtCycle"
                                            ErrorMessage="Cycle is required" CssClass="text-danger" Display="Dynamic" />
                                    </div>

                                    <div class="col-md-2">
                                        <label class="form-label">Officer Per. No.</label>
                                        <asp:TextBox runat="server" ID="txtpnosub" CssClass="form-control" MaxLength="6"
                                            placeholder="Officer Per. No" />
                                    </div>

                                    <div class="col-md-8 text-center">
                                        <div class="d-inline-block">
                                            <asp:LinkButton ID="btn_Show" runat="server" CssClass="btn btn-primary me-2"
                                                Style="width:120px;" CausesValidation="true">
                                                <i class="fa fa-eye" aria-hidden="true"></i> Show
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btn_download" runat="server" CssClass="btn btn-primary me-2"
                                                Style="width:120px;" CausesValidation="false">
                                                <i class="fa fa-download" aria-hidden="true"></i> Download
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btn_download_all" runat="server" CssClass="btn btn-primary"
                                                Style="width:120px;" CausesValidation="false">
                                                <i class="fa fa-download" aria-hidden="true"></i> Download All
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>

                                <div class="row content mt-4">
                                    <div class="col-md-12 col-lg-12">
                                        <div class="table-responsive">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gdvselectAssesor" runat="server" Visible="true"
                                                        AutoGenerateColumns="False"
                                                        CssClass="table table-striped table-hover table-bordered align-middle"
                                                        Font-Names="verdana" EmptyDataText="No Record Found"
                                                        BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                        GridLines="Vertical" RowStyle-CssClass="rows">
                                                        <HeaderStyle BackColor="#e43c5c" Font-Bold="True" ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />

                                                        <Columns>
                                                            <asp:BoundField DataField="Pno" HeaderText="P.No" SortExpression="Pno" />
                                                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                                            <asp:BoundField DataField="Designation" HeaderText="Designation"
                                                                SortExpression="Designation" />
                                                            <asp:BoundField DataField="Initiate" HeaderText="Initiate"
                                                                SortExpression="Initiate" />
                                                            <asp:BoundField DataField="Develop" HeaderText="Develop"
                                                                SortExpression="Develop" />
                                                            <asp:BoundField DataField="Eliminate" HeaderText="Eliminate"
                                                                SortExpression="Eliminate" />
                                                        </Columns>

                                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="White" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <Triggers>
                                <asp:PostBackTrigger ControlID="btn_download" />
                                <asp:PostBackTrigger ControlID="btn_download_all" />
                            </Triggers>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </section>
            <!-- End About Section -->
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
        </footer>
        <!-- End Footer -->

        <a href="#" class="back-to-top"><i class="ri-arrow-up-line"></i></a>

        <!-- jQuery 3.7 and Bootstrap 5.3 JS (bundle includes Popper) -->
        <%-- <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script> --%>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

        <!-- Vendor JS Files -->
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
