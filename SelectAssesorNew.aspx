<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SelectAssesorNew.aspx.vb" Inherits="SelectAssesorNew"
    MaintainScrollPositionOnPostback="true" %>
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
            <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet">
            <!-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script> -->
            <!-- Include all compiled plugins (below), or include individual files as needed -->
            <!-- <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script> -->


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

                        <h1 class="logo mr-auto"><a href="SelectAssesorNew.aspx">360 DEGREE FEEDBACK SURVEY</a></h1>
                        <!-- Uncomment below if you prefer to use an image logo -->
                        <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->

                        <nav class="nav-menu d-none d-lg-block">
                            <ul>
                                <li class="active"><a href="SelectAssesorNew.aspx">Home</a></li>

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
                            As we aspire to be the most valuable and respected steel company globally in the next 5-10
                            years, we are developing agile behaviours in our top leadership - accountability,
                            responsiveness, collaboration and people development. We will measure this as an integral
                            part of our Performance Management System for IL2s through a 360 degree feedback survey.
                        </h2>
                        <a href="#about" class="btn-get-started scrollto">SELECT RESPONDENTS</a>
                    </div>
                </section><!-- End Hero -->

                <main id="main">

                    <!-- ======= About Section ======= -->
                    <section id="about" class="about">
                        <div class="container">
                            <div class="section-title">
                                <h3 style="font-size:24pt;">Feedback would be requested from four category of
                                    respondents - <span> Manager, Subordinates, Peers and Stakeholders</span></h3>
                                <h4> Accordingly, as a part of your own 360 survey, you are requested to select the
                                    appropriate category and verify the list of your peers ( minimum three required) and
                                    add your Stakeholders. The Manager and Subordinate lists have been pre populated for
                                    your convenience. The survey would be triggered to all the respondents in Manager,
                                    Subordinate and Peers categories and any five ( selected randomly) from the Internal
                                    Stakeholder category</h4>
                            </div>
                            <ul class="faq-list" style="list-style-type:none;">
                                <li>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <div class="row content">
                                                <div class="col-3"></div>
                                                <div class="col-4">
                                                    <asp:TextBox runat="server" ID="txtperno" CssClass="form-control"
                                                        placeholder="Assess pno"></asp:TextBox>
                                                    <%-- <asp:HiddenField runat="server" ID="hdfnfy" />--%>
                                                    <asp:Label runat="server" ID="lblyear" Visible="false"></asp:Label>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server"
                                                        TargetControlID="txtperno"
                                                        ServiceMethod="SearchPrefixesForApprover"
                                                        MinimumPrefixLength="1" CompletionInterval="100"
                                                        DelimiterCharacters="" Enabled="True" ServicePath=""
                                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                        CompletionListCssClass="AutoExtender"
                                                        CompletionListItemCssClass="AutoExtenderList">

                                                    </cc1:AutoCompleteExtender>
                                                </div>
                                                <div class="col-4" style="margin-top:-10px">
                                                    <asp:Button runat="server" ID="btnaddtslsub" Text="Go"
                                                        class="btn-learn-more" OnClick="btnaddtslsub_Click">
                                                    </asp:Button>
                                                    <asp:Button runat="server" ID="Button2" Text="Refresh"
                                                        class="btn-learn-more" OnClick="Button2_Click"></asp:Button>
                                                </div>

                                            </div><br />

                                            <div class="row content">

                                                <div class="col-md-3">


                                                    <asp:Button runat="server" ID="Button1" Text="Add Tata Steel"
                                                        class="btn-learn-more" OnClick="Button1_Click" Visible="false">
                                                    </asp:Button>
                                                </div>
                                                <div class="col-md-3">


                                                    <asp:Button runat="server" ID="btnnontslsub"
                                                        Text="Add Non Tata Steel" class="btn-learn-more"
                                                        OnClick="btnnontslsub_Click" Visible="false"></asp:Button>
                                                </div>
                                                <div class="col-md-3">

                                                </div>

                                            </div>
                                            <br />

                                            <div class="row" runat="server" id="divtsl" visible="false">
                                                <div class="col-md-12 col-sm-12 col-xs-12">
                                                    <div class="panel panel-info" id="close1">
                                                        <div class="panel-heading bg-transparent">
                                                            <h3 class="panel-title">Enter P.no or Name for Add</h3>
                                                            <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                            <span class="float-end clickable"> <button type="button"
                                                                    class="fa fa-times fa-1x" data-bs-target="#close1"
                                                                    aria-label="Close" data-bs-dismiss="alert">
                                                                </button></i></span>
                                                        </div>
                                                        <div class="panel-body">
                                                            <div class="row">
                                                                <div class="col-md-3">
                                                                    <div class="form-group">

                                                                        <div class="col-md-12">
                                                                            <asp:TextBox runat="server" ID="txtpnoI"
                                                                                CssClass="form-control"
                                                                                placeholder="P. No" AutoPostBack="true"
                                                                                OnTextChanged="txtpnoI_TextChanged" />
                                                                            <cc1:AutoCompleteExtender
                                                                                ID="AutoCompleteExtender1"
                                                                                runat="server" TargetControlID="txtpnoI"
                                                                                ServiceMethod="SearchPrefixesForApprover1"
                                                                                MinimumPrefixLength="1"
                                                                                CompletionInterval="100"
                                                                                DelimiterCharacters="" Enabled="True"
                                                                                ServicePath=""
                                                                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                                CompletionListCssClass="AutoExtender"
                                                                                CompletionListItemCssClass="AutoExtenderList">
                                                                            </cc1:AutoCompleteExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <div class="form-group">

                                                                        <div class="col-md-12">
                                                                            <asp:TextBox runat="server" ID="txtdesgI"
                                                                                CssClass="form-control"
                                                                                placeholder="Designation" />
                                                                            <%-- <asp:HiddenField runat="server"
                                                                                ID="hdfnin" /> --%>
                                                                            <asp:Label runat="server" ID="lbllvl"
                                                                                Visible="false"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <div class="form-group">

                                                                        <div class="col-md-12">
                                                                            <asp:TextBox runat="server" ID="txtemailI"
                                                                                CssClass="form-control "
                                                                                placeholder="Email" />
                                                                            <asp:RegularExpressionValidator
                                                                                ID="RegularExpressionValidator2"
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
                                                                            <asp:TextBox runat="server" ID="txtdeptI"
                                                                                CssClass="form-control "
                                                                                placeholder="Org Name" />

                                                                        </div>
                                                                    </div>
                                                                </div>


                                                            </div><br />
                                                            <div class="row">
                                                                <br />
                                                                <div class="col-md-3">
                                                                    <div class="form-group">

                                                                        <div class="col-md-12">
                                                                            <asp:DropDownList ID="DropDownList1"
                                                                                runat="server" CssClass="form-control"
                                                                                data-width="100%"
                                                                                data-live-search="true">
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row content">
                                                                <div class="col-lg-5">
                                                                </div>
                                                                <div class="col-lg-6">
                                                                    <asp:Button runat="server" ID="btnorgadd" Text="Add"
                                                                        class="btn-learn-more"
                                                                        OnClick="btnorgadd_Click"></asp:Button>
                                                                </div>

                                                            </div>

                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" runat="server" id="divntsl" visible="false">
                                                <div class="col-md-12 col-sm-12 col-xs-12">
                                                    <div class="panel panel-info" id="close2">
                                                        <div class="panel-heading bg-transparent">
                                                            <h3 class="panel-title">Enter Details for Add</h3>
                                                            <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                            <span class="float-end clickable"> <button type="button"
                                                                    class="fa fa-times fa-1x" data-bs-target="#close2"
                                                                    aria-label="Close" data-bs-dismiss="alert">
                                                                </button></i></span>
                                                        </div>
                                                        <div class="panel-body">
                                                            <div class="row">
                                                                <div class="col-md-3">
                                                                    <div class="form-group">

                                                                        <div class="col-md-12">
                                                                            <asp:TextBox runat="server" ID="txtnamenI"
                                                                                CssClass="form-control"
                                                                                placeholder="Name" />

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <div class="form-group">

                                                                        <div class="col-md-12">
                                                                            <asp:TextBox runat="server" ID="txtdesgnI"
                                                                                CssClass="form-control"
                                                                                placeholder="Designation" />

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <div class="form-group">

                                                                        <div class="col-md-12">
                                                                            <asp:TextBox runat="server" ID="txtemailnI"
                                                                                CssClass="form-control "
                                                                                placeholder="Email" />
                                                                            <asp:RegularExpressionValidator
                                                                                ID="RegularExpressionValidator1"
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
                                                                            <asp:TextBox runat="server" ID="txtdeptnI"
                                                                                CssClass="form-control "
                                                                                placeholder="Org Name" />

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div><br />
                                                            <div class="row">
                                                                <br />
                                                                <div class="col-md-3">
                                                                    <div class="form-group">

                                                                        <div class="col-md-12">
                                                                            <asp:DropDownList ID="ddlrole"
                                                                                runat="server" CssClass="form-control"
                                                                                data-width="100%"
                                                                                data-live-search="true">
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row content">
                                                                <div class="col-lg-5">
                                                                </div>
                                                                <div class="col-lg-6">
                                                                    <asp:Button runat="server" ID="btnaddnorgI"
                                                                        Text="Add" class="btn-learn-more"
                                                                        OnClick="btnaddnorgI_Click"></asp:Button>
                                                                </div>

                                                            </div>

                                                        </div>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-md-12 col-lg-12">
                                                    <div class="table-responsive">

                                                        <asp:GridView ID="GvRepoties" runat="server"
                                                            AutoGenerateColumns="False"
                                                            CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                            Font-Names="verdana" EmptyDataText="No Record Found"
                                                            BackColor="#ffccff" BorderColor="Black" BorderStyle="None"
                                                            BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                                            RowStyle-CssClass="rows"
                                                            OnRowDataBound="GvRepoties_RowDataBound">
                                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                            <HeaderStyle CssClass="bg-clouds segoe-light"
                                                                BackColor="#FFB6C1" Font-Bold="True"
                                                                ForeColor="Black" />
                                                            <AlternatingRowStyle BackColor="#FFB6C1" />
                                                            <Columns>

                                                                <asp:TemplateField HeaderText="P.no">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblpno" runat="server"
                                                                            Text='<%# Eval("SS_PNO")%>'></asp:Label>

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lnlname" runat="server"
                                                                            Text='<%# Eval("SS_NAME")%>'></asp:Label>

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Level">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllevel" runat="server"
                                                                            Text='<%# Eval("ss_level")%>'></asp:Label>

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Designation">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldesg" runat="server"
                                                                            Text='<%# Eval("SS_DESG")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Department">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldept" runat="server"
                                                                            Text='<%# Eval("SS_DEPT")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Email Id">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblemail" runat="server"
                                                                            Text='<%# Eval("SS_EMAIL")%>'
                                                                            CssClass="wrap-Text"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Category">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcateg" runat="server"
                                                                            Text='<%# Eval("categ")%>'
                                                                            CssClass="wrap-Text"></asp:Label>
                                                                        <%--<asp:HiddenField runat="server"
                                                                            ID="hdfncateg"
                                                                            value='<%# Eval("SS_CATEG")%>' />--%>
                                                                        <asp:Label ID="Label1" runat="server"
                                                                            Text='<%# Eval("SS_CATEG")%>'
                                                                            Visible="false" CssClass="wrap-Text">
                                                                        </asp:Label>

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Add/Remove">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox runat="server" ID="chksub"
                                                                            AutoPostBack="true"
                                                                            OnCheckedChanged="chksub_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <%-- <asp:TemplateField HeaderText="Edit">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="server" ID="lnkedit"
                                                                            Text="Edit"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>--%>


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

                                                        <div class="row content">
                                                            <div class="col-lg-10">

                                                            </div>
                                                            <div class="col-lg-1">
                                                                <asp:Button runat="server" ID="btnsubmit" Text="Submit"
                                                                    class="btn-learn-more" Visible="false"
                                                                    OnClick="btnsubmit_Click"></asp:Button>
                                                            </div>
                                                        </div>
                                        </ContentTemplate>
                                        <Triggers>

                                        </Triggers>
                                    </asp:UpdatePanel>

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



                            </div>
                    </section><!-- End F.A.Q Section -->



                </main><!-- End #main -->

                <!-- ======= Footer ======= -->
                <footer id="footer">
                    <div class="container d-md-flex py-4">

                        <div class="mr-md-auto text-center">
                            In case of any queries or issues please reach out to
                            <strong>Ms. Shruti Choudhury: </strong>shruti.choudhury@tatasteel.com and <strong>Mr. Vikas
                                Kumar : </strong>vikas.kumar1@tatasteel.com
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