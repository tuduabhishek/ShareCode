<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SurveyCompletionNMBStatusRpt.aspx.vb"
    Inherits="SurveyCompletionNMBStatusRpt" MaintainScrollPositionOnPostback="true" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        <!DOCTYPE html>

        <html xmlns="http://www.w3.org/1999/xhtml">

        <head>
    <!-- New Library Versions -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-4.0.0-beta.2.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

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
            <!-- <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"> -->
            <link href="assets/vendor/icofont/icofont.min.css" rel="stylesheet">
            <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
            <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">
            <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
            <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">
            <!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"> -->
            <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
            <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>
            <!-- <link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css"> -->
            <!-- <script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script> -->
            <!-- <script src="//code.jquery.com/jquery-1.11.1.min.js"></script> -->
            <%-- <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet"> --%>
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

                        <h1 class="logo mr-auto"><a
                                href="https://irisapp-tslin.msappproxy.net/feedback_360/surveyadm_opr.aspx">360 DEGREE
                                FEEDBACK SURVEY</a></h1>
                        <!-- Uncomment below if you prefer to use an image logo -->
                        <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->

                        <nav class="nav-menu d-none d-lg-block">
                            <ul>
                                <li class="active"><a
                                        href="https://irisapp-tslin.msappproxy.net/feedback_360/surveyadm_opr.aspx">Home
                                        (Admin)</a></li>

                            </ul>
                        </nav><!-- .nav-menu -->

                    </div>
                </header><!-- End Header -->
                <asp:ScriptManager runat="server" ID="scpmgr">

                </asp:ScriptManager>
                <!-- ======= Hero Section ======= -->
                <section id="hero">
                    <div class="hero-container">
                        <h3>Welcome <strong>
                                <asp:Label ID="lblname" runat="server" Text="Admin"></asp:Label>
                            </strong></h3>
                        <!--<h1>We're Creative Agency</h1>-->

                        <h2 class="table-responsive1">
                            As we aspire to be the most valuable and respected steel company globally in the coming
                            years, we are developing agile behaviours across the organization. We will measure this as
                            an integral part of our Performance Management System for all the officers through a 360
                            degree feedback survey.
                        </h2>
                        <a href="#about" class="btn-get-started scrollto">VIEW REPORT</a>
                    </div>
                </section><!-- End Hero -->

                <main id="main">

                    <!-- ======= About Section ======= -->
                    <section id="about" class="about">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-lg-12">
                                    <h1>Status Survey Completion</h1>
                                </div>

                            </div>
                            <asp:UpdatePanel ID="upnl1" runat="server">
                                <ContentTemplate>
                                    <div class="panel-body">
                                        <div class="row form-group">
                                            <div class="col-md-1">
                                                <label>Year</label><span style="color:red;">*</span>
                                                <asp:TextBox runat="server" ID="txtYear" CssClass="form-control"
                                                    MaxLength="4" />
                                            </div>
                                            <div class="col-md-1">
                                                <label>Cycle</label><span style="color:red;">*</span>
                                                <asp:TextBox runat="server" ID="txtCycle" CssClass="form-control"
                                                    MaxLength="1" />
                                            </div>
                                            <div class="col-md-3">
                                                <label>Executive Head</label>
                                                <asp:DropDownList runat="server" ID="ddlExecutive"
                                                    CssClass="form-control" AutoPostBack="true" />
                                            </div>
                                            <div class="col-md-3">
                                                <label>Department</label>
                                                <asp:DropDownList runat="server" ID="ddlDept" CssClass="form-control" />
                                            </div>
                                            <div class="col-md-2">
                                                <label>BUHR Per. No.</label>
                                                <asp:TextBox runat="server" ID="txtBuhr" CssClass="form-control"
                                                    MaxLength="6" placeholder="BUHR Per. No" />
                                            </div>
                                            <div class="col-md-2">
                                                <label>Officer Per. No.</label>
                                                <asp:TextBox runat="server" ID="txtpnosub" CssClass="form-control"
                                                    MaxLength="6" placeholder="Officer Per. No" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-10">
                                                <div style="text-align:center;margin:auto;">
                                                    <asp:LinkButton ID="btn_Show" runat="server"
                                                        CssClass="btn btn-primary btn-block" Width="120px" type="submit"
                                                        CausesValidation="false">
                                                        <span aria-hidden="true" class="fa fa-eye"></span> Show
                                                    </asp:LinkButton>&nbsp;
                                                    <asp:LinkButton ID="btn_download" runat="server"
                                                        CssClass="btn btn-primary btn-block" Width="120px" type="submit"
                                                        CausesValidation="false">
                                                        <span aria-hidden="true" class="fa fa-download"></span> Download
                                                    </asp:LinkButton>&nbsp;
                                                    <asp:LinkButton ID="btn_download_all" runat="server"
                                                        CssClass="btn btn-primary btn-block" Width="120px" type="submit"
                                                        CausesValidation="false">
                                                        <span aria-hidden="true" class="fa fa-download"></span> Download
                                                        All
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row content">
                                        <div class="col-md-12 col-lg-12">
                                            <div class="table-responsive">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gdvselectAssesor" runat="server"
                                                            Visible="true" AutoGenerateColumns="False"
                                                            CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                            Font-Names="verdana" EmptyDataText="No Record Found"
                                                            BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                            GridLines="Vertical" RowStyle-CssClass="rows">
                                                            <%-- <FooterStyle BackColor="#CCCCCC"
                                                                ForeColor="Black" />--%>
                                                            <HeaderStyle BackColor="#e43c5c" Font-Bold="True"
                                                                ForeColor="Black" />
                                                            <AlternatingRowStyle BackColor="#FFB6C1" />

                                                            <Columns>

                                                                <asp:BoundField DataField="Pno" HeaderText="P.no"
                                                                    SortExpression="Pno" />
                                                                <asp:BoundField DataField="EMA_ENAME" HeaderText="Name"
                                                                    SortExpression="EMA_ENAME" />
                                                                <asp:BoundField DataField="EMA_EQV_LEVEL"
                                                                    HeaderText="Level" SortExpression="EMA_EQV_LEVEL" />
                                                                <asp:BoundField DataField="Designation"
                                                                    HeaderText="Designation"
                                                                    SortExpression="Designation" />
                                                                <asp:BoundField DataField="Department"
                                                                    HeaderText="Department"
                                                                    SortExpression="Department" />
                                                                <asp:BoundField DataField="Email_id"
                                                                    HeaderText="Email Id" SortExpression="Email_id" />
                                                                <asp:BoundField DataField="Executive_Head"
                                                                    HeaderText="Executive Head"
                                                                    SortExpression="Executive_Head" />
                                                                <asp:BoundField DataField="Superior_Pno"
                                                                    HeaderText="Approver P No."
                                                                    SortExpression="Superior_Pno" />
                                                                <asp:BoundField DataField="Superior_Name"
                                                                    HeaderText="Approver Name"
                                                                    SortExpression="Superior_Name" />
                                                                <asp:BoundField DataField="BUHR_Pno"
                                                                    HeaderText="BUHR P No." SortExpression="BUHR_Pno" />
                                                                <asp:BoundField DataField="BUHR_NAME"
                                                                    HeaderText="BUHR Name" SortExpression="BUHR_NAME" />
                                                                <asp:BoundField DataField="Respondent_Pno"
                                                                    HeaderText="Respondent P.no"
                                                                    SortExpression="Respondent_Pno" />
                                                                <asp:BoundField DataField="Respondent_Name"
                                                                    HeaderText="Respondent Name"
                                                                    SortExpression="Respondent_Name" />
                                                                <asp:BoundField DataField="Respondent_Level"
                                                                    HeaderText="Respondent Level"
                                                                    SortExpression="Respondent_Level" />
                                                                <asp:BoundField DataField="Respondent_Designation"
                                                                    HeaderText="Respondent Designation"
                                                                    SortExpression="Respondent_Designation" />
                                                                <asp:BoundField DataField="Respondent_Department"
                                                                    HeaderText="Respondent Department"
                                                                    SortExpression="Respondent_Department" />
                                                                <asp:BoundField DataField="Respondent_Email_Id"
                                                                    HeaderText="Respondent Email Id"
                                                                    SortExpression="Respondent_Email_Id" />
                                                                <asp:BoundField DataField="Respondent_Category"
                                                                    HeaderText="Respondent Category"
                                                                    SortExpression="Respondent_Category" />
                                                                <asp:BoundField DataField="Status" HeaderText="Status"
                                                                    SortExpression="Status" />
                                                            </Columns>

                                                            <PagerStyle BackColor="#999999" ForeColor="Black"
                                                                HorizontalAlign="Center" />
                                                            <RowStyle BackColor="White" ForeColor="Black" />
                                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True"
                                                                ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                            <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                            <SortedDescendingHeaderStyle BackColor="#000065" />
                                                        </asp:GridView>
                                                        <asp:DataGrid ID="dgMiniCriteria" runat="server" Visible="true"
                                                            AutoGenerateColumns="False"
                                                            CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                            Font-Names="verdana" EmptyDataText="No Record Found"
                                                            BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                            GridLines="Both" RowStyle-CssClass="rows">
                                                            <Columns>
                                                                <asp:BoundColumn DataField="Pno" HeaderText="P.no"
                                                                    SortExpression="Pno" />
                                                                <asp:BoundColumn DataField="EMA_ENAME" HeaderText="Name"
                                                                    SortExpression="EMA_ENAME" />
                                                                <asp:BoundColumn DataField="EMA_EQV_LEVEL"
                                                                    HeaderText="Level" SortExpression="EMA_EQV_LEVEL" />
                                                                <asp:BoundColumn DataField="Designation"
                                                                    HeaderText="Designation"
                                                                    SortExpression="Designation" />
                                                                <asp:BoundColumn DataField="Department"
                                                                    HeaderText="Department"
                                                                    SortExpression="Department" />
                                                                <asp:BoundColumn DataField="Email_id"
                                                                    HeaderText="Email Id" SortExpression="Email_id" />
                                                                <asp:BoundColumn DataField="Executive_Head"
                                                                    HeaderText="Executive Head"
                                                                    SortExpression="Executive_Head" />
                                                                <asp:BoundColumn DataField="Superior_Pno"
                                                                    HeaderText="Approver P No."
                                                                    SortExpression="Superior_Pno" />
                                                                <asp:BoundColumn DataField="Superior_Name"
                                                                    HeaderText="Approver Name"
                                                                    SortExpression="Superior_Name" />
                                                                <asp:BoundColumn DataField="BUHR_Pno"
                                                                    HeaderText="BUHR P No." SortExpression="BUHR_Pno" />
                                                                <asp:BoundColumn DataField="BUHR_NAME"
                                                                    HeaderText="BUHR Name" SortExpression="BUHR_NAME" />
                                                                <asp:BoundColumn DataField="Respondent_Pno"
                                                                    HeaderText="Respondent P.no"
                                                                    SortExpression="Respondent_Pno" />
                                                                <asp:BoundColumn DataField="Respondent_Name"
                                                                    HeaderText="Respondent Name"
                                                                    SortExpression="Respondent_Name" />
                                                                <asp:BoundColumn DataField="Respondent_Level"
                                                                    HeaderText="Respondent Level"
                                                                    SortExpression="Respondent_Level" />
                                                                <asp:BoundColumn DataField="Respondent_Designation"
                                                                    HeaderText="Respondent Designation"
                                                                    SortExpression="Respondent_Designation" />
                                                                <asp:BoundColumn DataField="Respondent_Department"
                                                                    HeaderText="Respondent Department"
                                                                    SortExpression="Respondent_Department" />
                                                                <asp:BoundColumn DataField="Respondent_Email_Id"
                                                                    HeaderText="Respondent Email Id"
                                                                    SortExpression="Respondent_Email_Id" />
                                                                <asp:BoundColumn DataField="Respondent_Category"
                                                                    HeaderText="Respondent Category"
                                                                    SortExpression="Respondent_Category" />
                                                                <asp:BoundColumn DataField="Status" HeaderText="Status"
                                                                    SortExpression="Status" />
                                                            </Columns>

                                                            <PagerStyle BackColor="#999999" ForeColor="Black"
                                                                HorizontalAlign="Center" />
                                                        </asp:DataGrid>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btn_download" />
                                    <asp:PostBackTrigger ControlID="btn_download_all" />
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
                            <span>In case of any queries or issues, please reach out to your HRBP. </span>
                            <span>In case of any system specific queries or IT issues, please reach out to<b> IT
                                    helpdesk (it_helpdesk@tatasteel.com)</b></span>
                        </div>
                    </div>


                </footer><!-- End Footer -->

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
            </form>
        </body>

        </html>