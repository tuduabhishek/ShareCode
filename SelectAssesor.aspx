<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SelectAssesor.aspx.vb" Inherits="SelectAssesor"
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
            <!-- New Library Versions -->
            <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
            <link href="https://code.jquery.com/ui/1.14.2/themes/ui-lightness/jquery-ui.css" rel="stylesheet">

            <script src="https://code.jquery.com/jquery-4.0.0-beta.2.min.js"></script>
            <script src="https://code.jquery.com/ui/1.14.2/jquery-ui.min.js"></script>
            <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
            <%-- <link href="https://netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
                rel="stylesheet"> --%>

                <!-- Vendor CSS Files Commented Out -->
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
                    })

                    function showGenericMessageModal(type, message) {
                        swal('', message, type);
                    }

                    function openURL() {
                        var isIE = false || !!document.documentMode;

                        if (isIE == true) {
                            //var shell = new ActiveXObject("WScript.Shell");

                            //shell.run("Chrome http://webappsdev01.corp.tatasteel.com/Feedback360/SelectAssesor.aspx", true);

                        }
                        else {
                            //var shell = new ActiveXObject("WScript.Shell");

                            //shell.run("Chrome http://webappsdev01.corp.tatasteel.com/Feedback360/SelectAssesor.aspx", true);
                        }
                    }

                </script>

        </head>

        <body onload="openURL();">


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

                        <h1 class="logo mr-auto"><a href="SelectAssesor.aspx">360 DEGREE FEEDBACK SURVEY</a></h1>
                        <!-- Uncomment below if you prefer to use an image logo -->
                        <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->

                        <nav class="nav-menu d-none d-lg-block">
                            <ul>
                                <li class="active"><a href="SelectAssesor.aspx">Home</a></li>
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
                                <li><a href="Images/User_manual_360 DEGREE_Select Assessor.pdf" target="_blank">Help</a>
                                </li>

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
                                <asp:Label ID="lblname" runat="server" Text=""></asp:Label>
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
                                    <a data-bs-toggle="collapse" href="#catg4" class="">
                                        <h3 style="font-size:24pt;">MANAGER/SUPERIOR<asp:Label runat="server"
                                                ID="lblmmms" Font-Size="Large"></asp:Label>
                                        </h3>
                                    </a>
                                    <div id="catg4" class="collapse show" data-parent=".faq-list">
                                        <div class="container">
                                            <div class="row">
                                                <h4>
                                                    All reporting managers both solid and dotted reporting and
                                                    respective Executive Head.
                                                </h4>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12 col-lg-12">
                                                    <div class="table-responsive">
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="GvManager" runat="server"
                                                                    AutoGenerateColumns="False"
                                                                    CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                                    Font-Names="verdana" EmptyDataText="No Record Found"
                                                                    OnRowDataBound="GvCateg_RowDataBound"
                                                                    BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                                    GridLines="Vertical" RowStyle-CssClass="rows">
                                                                    <%-- <FooterStyle BackColor="#CCCCCC"
                                                                        ForeColor="Black" />--%>
                                                                    <HeaderStyle BackColor="#e43c5c" Font-Bold="True"
                                                                        ForeColor="Black" />
                                                                    <AlternatingRowStyle BackColor="#FFB6C1" />
                                                                    <Columns>

                                                                        <asp:TemplateField HeaderText="P.no">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblpno" runat="server"
                                                                                    Text='<%# Eval("ema_perno")%>'>
                                                                                </asp:Label>
                                                                                <%--<asp:HiddenField runat="server"
                                                                                    ID="hdfnid"
                                                                                    Value='<%# Eval("SS_ID")%>' />--%>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lnlname" runat="server"
                                                                                    Text='<%# Eval("ema_ename")%>'>
                                                                                </asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Level">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbllevel" runat="server"
                                                                                    Text='<%# Eval("EMA_EMPL_PGRADE")%>'>
                                                                                </asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Designation">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbldesg" runat="server"
                                                                                    Text='<%# Eval("EMA_DESGN_DESC")%>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Department">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbldept" runat="server"
                                                                                    Text='<%# Eval("EMA_DEPT_DESC")%>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Email Id">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblemail" runat="server"
                                                                                    Text='<%# Eval("EMA_EMAIL_ID")%>'
                                                                                    CssClass="wrap-Text"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                    </Columns>
                                                                    <PagerStyle BackColor="#999999" ForeColor="Black"
                                                                        HorizontalAlign="Center" />
                                                                    <RowStyle BackColor="White" ForeColor="Black" />
                                                                    <SelectedRowStyle BackColor="#008A8C"
                                                                        Font-Bold="True" ForeColor="White" />
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
                                    </div>
                                </li>



                                <li>
                                    <a data-bs-toggle="collapse" href="#faq10" class="collapsed">
                                        <h3 style="font-size:24pt;"> SUBORDINATES <asp:Label runat="server" ID="lblsub"
                                                Font-Size="Large"></asp:Label>
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
                                                                    <h3 class="panel-title">Enter Details for Add</h3>
                                                                    <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                    <span class="float-end clickable"> <button
                                                                            type="button" class="fa fa-times fa-1x"
                                                                            data-bs-target="#close5" aria-label="Close"
                                                                            data-bs-dismiss="alert">
                                                                        </button></i></span>
                                                                </div>
                                                                <div class="panel-body">
                                                                    <div class="row">
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtnamemgr"
                                                                                        CssClass="form-control"
                                                                                        placeholder="Name" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtdesgmgr"
                                                                                        CssClass="form-control"
                                                                                        placeholder="Designation" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtemailmgr"
                                                                                        CssClass="form-control "
                                                                                        placeholder="Email" />
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
                                                                                        placeholder="Org Name" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div><br />
                                                                    <div class="row content">
                                                                        <div class="col-lg-5">
                                                                        </div>
                                                                        <div class="col-lg-6">
                                                                            <asp:Button runat="server" ID="btnaddmgr"
                                                                                Text="Add" class="btn-learn-more"
                                                                                OnClick="btnaddmgr_Click"></asp:Button>
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
                                                                    <h3 class="panel-title">Enter P.no or Name for Add
                                                                    </h3>
                                                                    <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                    <span class="float-end clickable"> <button
                                                                            type="button" class="fa fa-times fa-1x"
                                                                            data-bs-target="#close10" aria-label="Close"
                                                                            data-bs-dismiss="alert">
                                                                        </button></i></span>
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
                                                                                        OnTextChanged="txtpnosub_TextChanged" />
                                                                                    <cc1:AutoCompleteExtender
                                                                                        ID="AutoCompleteExtender3"
                                                                                        runat="server"
                                                                                        TargetControlID="txtpnosub"
                                                                                        ServiceMethod="SearchPrefixesForApprover"
                                                                                        MinimumPrefixLength="1"
                                                                                        CompletionInterval="100"
                                                                                        DelimiterCharacters=""
                                                                                        Enabled="True" ServicePath=""
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
                                                                                        placeholder="Designation" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtmailsub"
                                                                                        CssClass="form-control "
                                                                                        placeholder="Email" />
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
                                                                                        placeholder="Org Name" />
                                                                                    <%--<asp:HiddenField runat="server"
                                                                                        ID="hdfnsub" />--%>
                                                                                    <asp:Label runat="server"
                                                                                        ID="lblsublvl" Visible="false">
                                                                                    </asp:Label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row content">
                                                                        <div class="col-lg-5">
                                                                        </div>
                                                                        <div class="col-lg-6">
                                                                            <asp:Button runat="server" ID="txtaddsub"
                                                                                Text="Add" class="btn-learn-more"
                                                                                OnClick="txtaddsub_Click"></asp:Button>
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
                                                                            <AlternatingRowStyle BackColor="#FFB6C1" />
                                                                            <Columns>

                                                                                <asp:TemplateField HeaderText="P.no">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblpno"
                                                                                            runat="server"
                                                                                            Text='<%# Eval("ema_perno")%>'>
                                                                                        </asp:Label>

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

                                                                                <asp:TemplateField
                                                                                    HeaderText="Add/Remove">
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
                                    <a data-bs-toggle="collapse" class="collapsed" href="#faq6">
                                        <h3 style="font-size:24pt;"> PEERS <asp:Label runat="server" ID="lblpeer"
                                                Font-Size="Large"></asp:Label>
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

                                                    </div><br />
                                                    <div class="row" runat="server" id="rowpeer" visible="false">
                                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                                            <div class="panel panel-info" id="close1">
                                                                <div class="panel-heading bg-transparent">
                                                                    <h3 class="panel-title">Enter P.no or Name for Add
                                                                    </h3>
                                                                    <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                    <span class="float-end clickable"> <button
                                                                            type="button" class="fa fa-times fa-1x"
                                                                            data-bs-target="#close1" aria-label="Close"
                                                                            data-bs-dismiss="alert">
                                                                        </button></i></span>
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
                                                                                        OnTextChanged="txtCouponNo_TextChanged" />
                                                                                    <cc1:AutoCompleteExtender
                                                                                        ID="AutoCompleteExtender2"
                                                                                        runat="server"
                                                                                        TargetControlID="txtpnoP"
                                                                                        ServiceMethod="SearchPrefixesForApprover"
                                                                                        MinimumPrefixLength="1"
                                                                                        CompletionInterval="100"
                                                                                        DelimiterCharacters=""
                                                                                        Enabled="True" ServicePath=""
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
                                                                                        placeholder="Designation" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtemailP"
                                                                                        CssClass="form-control "
                                                                                        placeholder="Email" />
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
                                                                                        placeholder="Org Name" />
                                                                                    <%-- <asp:HiddenField runat="server"
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
                                                                                OnClick="btnAddP_Click"></asp:Button>
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
                                                                    <h3 class="panel-title">Enter Details for Add</h3>
                                                                    <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                    <span class="float-end clickable"> <button
                                                                            type="button" class="fa fa-times fa-1x"
                                                                            data-bs-target="#close4" aria-label="Close"
                                                                            data-bs-dismiss="alert">
                                                                        </button></i></span>
                                                                </div>
                                                                <div class="panel-body">
                                                                    <div class="row">
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtnmpeer"
                                                                                        CssClass="form-control"
                                                                                        placeholder="Name" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtdesgpeer"
                                                                                        CssClass="form-control"
                                                                                        placeholder="Designation" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtmailpeer"
                                                                                        CssClass="form-control "
                                                                                        placeholder="Email" />
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
                                                                                        placeholder="Org Name" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div><br />
                                                                    <div class="row content">
                                                                        <div class="col-lg-5">
                                                                        </div>
                                                                        <div class="col-lg-6">
                                                                            <asp:Button runat="server" ID="btnaddpeer"
                                                                                Text="Add" class="btn-learn-more"
                                                                                OnClick="btnaddpeer_Click"></asp:Button>
                                                                        </div>

                                                                    </div>

                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>




                                                    <div class="row">
                                                        <div class="col-md-12 col-lg-12">
                                                            <div class="table-responsive">

                                                                <asp:GridView ID="GvPeer" runat="server"
                                                                    AutoGenerateColumns="False"
                                                                    CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                                    Font-Names="verdana" EmptyDataText="No Record Found"
                                                                    OnRowDataBound="GvPeer_RowDataBound"
                                                                    BackColor="#ffccff" BorderColor="Black"
                                                                    BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                                    GridLines="Vertical" RowStyle-CssClass="rows">
                                                                    <FooterStyle BackColor="#CCCCCC"
                                                                        ForeColor="Black" />
                                                                    <HeaderStyle CssClass="bg-clouds segoe-light"
                                                                        BackColor="#FFB6C1" Font-Bold="True"
                                                                        ForeColor="Black" />
                                                                    <AlternatingRowStyle BackColor="#FFB6C1" />
                                                                    <Columns>

                                                                        <asp:TemplateField HeaderText="P.no">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblpno" runat="server"
                                                                                    Text='<%# Eval("ema_perno")%>'>
                                                                                </asp:Label>
                                                                                <%-- <asp:HiddenField runat="server"
                                                                                    ID="hdfnid"
                                                                                    Value='<%# Eval("SS_ID")%>' />--%>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lnlname" runat="server"
                                                                                    Text='<%# Eval("ema_ename")%>'>
                                                                                </asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Level">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbllevel" runat="server"
                                                                                    Text='<%# Eval("EMA_EMPL_PGRADE")%>'>
                                                                                </asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Designation">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbldesg" runat="server"
                                                                                    Text='<%# Eval("EMA_DESGN_DESC")%>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Department">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbldept" runat="server"
                                                                                    Text='<%# Eval("EMA_DEPT_DESC")%>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Email Id">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblemail" runat="server"
                                                                                    Text='<%# Eval("EMA_EMAIL_ID")%>'
                                                                                    CssClass="wrap-Text"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Add/Remove"
                                                                            ItemStyle-Width="5%"
                                                                            HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox runat="server"
                                                                                    ID="chkseldsel" CssClass="checkbox"
                                                                                    AutoPostBack="true"
                                                                                    OnCheckedChanged="chkseldsel_CheckedChanged1" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center"
                                                                                Width="5%" />
                                                                        </asp:TemplateField>

                                                                    </Columns>
                                                                    <PagerStyle BackColor="#999999" ForeColor="Black"
                                                                        HorizontalAlign="Center" />
                                                                    <RowStyle BackColor="White" ForeColor="Black" />
                                                                    <SelectedRowStyle BackColor="#008A8C"
                                                                        Font-Bold="True" ForeColor="White" />
                                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                    <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                                    <SortedDescendingHeaderStyle BackColor="#000065" />
                                                                </asp:GridView>

                                            </ContentTemplate>
                                            <Triggers>
                                                <%-- <asp:PostBackTrigger ControlID="btntatasteel" />--%>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                </li>

                                <li>
                                    <a data-bs-toggle="collapse" href="#catg3" class="collapsed">
                                        <h3 style="font-size:24pt;">INTERNAL STAKEHOLDERS<asp:Label runat="server"
                                                ID="lblinst" Font-Size="Large"></asp:Label>
                                        </h3>
                                    </a>
                                    <div id="catg3" class="collapse" data-parent=".faq-list">
                                        <div class="container">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <p>
                                                            An Internal Stakeholder may be a Tata Steel/Non Tata Steel
                                                            professional with whom you have direct/indirect business
                                                            relations.
                                                        </p>
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

                                                    </div><br />
                                                    <div class="row" runat="server" id="divtsl" visible="false">
                                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                                            <div class="panel panel-info" id="close2">
                                                                <div class="panel-heading bg-transparent">
                                                                    <h3 class="panel-title">Enter P.no or Name for Add
                                                                    </h3>
                                                                    <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                    <span class="float-end clickable"> <button
                                                                            type="button" class="fa fa-times fa-1x"
                                                                            data-bs-target="#close2" aria-label="Close"
                                                                            data-bs-dismiss="alert">
                                                                        </button></i></span>
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
                                                                                        placeholder="Designation" />
                                                                                    <%--<asp:HiddenField runat="server"
                                                                                        ID="hdfnin" />--%>
                                                                                    <asp:Label runat="server"
                                                                                        ID="lblinst1" Visible="false">
                                                                                    </asp:Label>
                                                                                    <asp:Label runat="server"
                                                                                        ID="lblinst2" Visible="false">
                                                                                    </asp:Label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtemailI"
                                                                                        CssClass="form-control "
                                                                                        placeholder="Email" />
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
                                                                                        placeholder="Org Name" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div><br />
                                                                    <div class="row content">
                                                                        <div class="col-lg-5">
                                                                        </div>
                                                                        <div class="col-lg-6">
                                                                            <asp:Button runat="server" ID="btnorgadd"
                                                                                Text="Add" class="btn-learn-more"
                                                                                OnClick="btnorgadd_Click"></asp:Button>
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
                                                                    <h3 class="panel-title">Enter Details for Add</h3>
                                                                    <!-- Watch Out: Here We must use the effect name in the data tag-->
                                                                    <span class="float-end clickable"> <button
                                                                            type="button" class="fa fa-times fa-1x"
                                                                            data-bs-target="#close3" aria-label="Close"
                                                                            data-bs-dismiss="alert">
                                                                        </button></i></span>
                                                                </div>
                                                                <div class="panel-body">
                                                                    <div class="row">
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtnamenI"
                                                                                        CssClass="form-control"
                                                                                        placeholder="Name" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtdesgnI"
                                                                                        CssClass="form-control"
                                                                                        placeholder="Designation" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">

                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox runat="server"
                                                                                        ID="txtemailnI"
                                                                                        CssClass="form-control "
                                                                                        placeholder="Email" />
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
                                                                                        placeholder="Org Name" />

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div><br />
                                                                    <div class="row content">
                                                                        <div class="col-lg-5">
                                                                        </div>
                                                                        <div class="col-lg-6">
                                                                            <asp:Button runat="server" ID="btnaddnorgI"
                                                                                Text="Add" class="btn-learn-more"
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

                                                                <asp:GridView ID="Gvintstholder" runat="server"
                                                                    AutoGenerateColumns="False"
                                                                    CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                                    Font-Names="verdana" EmptyDataText="No Record Found"
                                                                    OnRowDataBound="Gvintstholder_RowDataBound"
                                                                    BackColor="#ffccff" BorderColor="Black"
                                                                    BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                                    GridLines="Vertical" RowStyle-CssClass="rows">
                                                                    <FooterStyle BackColor="#CCCCCC"
                                                                        ForeColor="Black" />
                                                                    <HeaderStyle CssClass="bg-clouds segoe-light"
                                                                        BackColor="#FFB6C1" Font-Bold="True"
                                                                        ForeColor="Black" />
                                                                    <AlternatingRowStyle BackColor="#FFB6C1" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="P.no">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblpno" runat="server"
                                                                                    Text='<%# Eval("ss_pno")%>'>
                                                                                </asp:Label>
                                                                                <%-- <asp:HiddenField runat="server"
                                                                                    ID="hdfnid"
                                                                                    Value='<%# Eval("SS_ID")%>' />--%>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lnlname" runat="server"
                                                                                    Text='<%# Eval("ss_name")%>'>
                                                                                </asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Level">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbllevel" runat="server"
                                                                                    Text='<%# Eval("SS_LEVEL")%>'>
                                                                                </asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Designation">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbldesg" runat="server"
                                                                                    Text='<%# Eval("SS_DESG")%>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Department">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbldept" runat="server"
                                                                                    Text='<%# Eval("SS_DEPT")%>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Email Id">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblemail" runat="server"
                                                                                    Text='<%# Eval("SS_EMAIL")%>'
                                                                                    CssClass="wrap-Text"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Add/Remove"
                                                                            ItemStyle-Width="5%"
                                                                            HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox runat="server"
                                                                                    ID="chkseldsel" CssClass="checkbox"
                                                                                    AutoPostBack="true"
                                                                                    OnCheckedChanged="chkseldsel_CheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center"
                                                                                Width="5%" />
                                                                        </asp:TemplateField>

                                                                    </Columns>
                                                                    <PagerStyle BackColor="#999999" ForeColor="Black"
                                                                        HorizontalAlign="Center" />
                                                                    <RowStyle BackColor="White" ForeColor="Black" />
                                                                    <SelectedRowStyle BackColor="#008A8C"
                                                                        Font-Bold="True" ForeColor="White" />
                                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                    <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                                    <SortedDescendingHeaderStyle BackColor="#000065" />
                                                                </asp:GridView>

                                                </ContentTemplate>
                                                <Triggers>
                                                    <%--<asp:PostBackTrigger ControlID="btnaddpeertsl" />--%>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                </li>


                                <li>
                                    <a data-bs-toggle="collapse" href="#catg6" class="collapsed">
                                        <h3 style="font-size:24pt;"> FINAL RESPONDENTS SELECTED LIST </h3>
                                    </a>
                                    <div id="catg6" class="collapse" data-parent=".faq-list">
                                        <div class="container">
                                            <div class="row">
                                                <div class="col-md-12 col-lg-12">
                                                    <div class="table-responsive">
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="gvfinal" runat="server"
                                                                    AutoGenerateColumns="False"
                                                                    CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                                    Font-Names="verdana" EmptyDataText="No Record Found"
                                                                    BackColor="#ffccff" BorderColor="Black"
                                                                    BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                                    GridLines="Vertical" RowStyle-CssClass="rows">
                                                                    <FooterStyle BackColor="#CCCCCC"
                                                                        ForeColor="Black" />
                                                                    <HeaderStyle CssClass="bg-clouds segoe-light"
                                                                        BackColor="#FFB6C1" Font-Bold="True"
                                                                        ForeColor="Black" />
                                                                    <AlternatingRowStyle BackColor="#FFB6C1" />
                                                                    <Columns>

                                                                        <asp:TemplateField HeaderText="P.no">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblpno" runat="server"
                                                                                    Text='<%# Eval("SS_PNO")%>'>
                                                                                </asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lnlname" runat="server"
                                                                                    Text='<%# Eval("SS_NAME")%>'>
                                                                                </asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Level">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbllevel" runat="server"
                                                                                    Text='<%# Eval("EMA_EMPL_PGRADE")%>'>
                                                                                </asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Designation">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbldesg" runat="server"
                                                                                    Text='<%# Eval("SS_DESG")%>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Department">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbldept" runat="server"
                                                                                    Text='<%# Eval("SS_DEPT")%>'>
                                                                                </asp:Label>
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
                                                                                <asp:Label ID="lblcategory"
                                                                                    runat="server"
                                                                                    Text='<%# Eval("IRC_DESC")%>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>


                                                                    </Columns>
                                                                    <PagerStyle BackColor="#999999" ForeColor="Black"
                                                                        HorizontalAlign="Center" />
                                                                    <RowStyle BackColor="White" ForeColor="Black" />
                                                                    <SelectedRowStyle BackColor="#008A8C"
                                                                        Font-Bold="True" ForeColor="White" />
                                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                    <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                                    <SortedDescendingHeaderStyle BackColor="#000065" />
                                                                </asp:GridView>

                                                            </ContentTemplate>
                                                            <Triggers>

                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row content">
                                                <div class="col-lg-9">

                                                </div>
                                                <div class="col-lg-1">
                                                    <asp:Button runat="server" ID="lbOrg" Text="Submit For Approval"
                                                        class="btn-learn-more"></asp:Button>
                                                </div>
                                            </div>

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
                                    <%-- <h3 style="font-size:24pt;">Frequently Asked <span>Questions</span></h3>--%>
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
                                    <a data-bs-toggle="collapse" href="#faq5" class="collapsed" style="font-size:24pt;">
                                        Who is a Subordinate?<i class="icofont-simple-up"></i></a>
                                    <div id="faq5" class="collapse" data-parent=".faq-list">
                                        <p>
                                            Subordinate is the individual for whom you are tagged as the functional
                                            superior.
                                        </p>
                                    </div>
                                </li>

                                <li>
                                    <a data-bs-toggle="collapse" href="#faq3" class="collapsed" style="font-size:24pt;">
                                        Who is a Peer?<i class="icofont-simple-up"></i></a>
                                    <div id="faq3" class="collapse" data-parent=".faq-list">
                                        <p>
                                            Peer is defined as officers reporting to the same superior(in this case your
                                            functional superior).
                                        </p>
                                    </div>
                                </li>

                                <li>
                                    <a data-bs-toggle="collapse" href="#faq4" class="collapsed" style="font-size:24pt;">
                                        Who is an Internal Stakeholder?<i class="icofont-simple-up"></i></a>
                                    <div id="faq4" class="collapse" data-parent=".faq-list">
                                        <p>
                                            An Internal Stakeholder may be a Tata Steel/Non Tata Steel professional with
                                            whom you have direct/indirect business relations.
                                        </p>
                                    </div>
                                </li>
                                </ul>--%>

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

                    <%-- <div class="container d-md-flex py-4">
                        <div class="mr-md-auto text-center text-md-left">
                            <div class="copyright">
                                &copy; Copyright <strong><span>Tata Steel</span></strong>.
                            </div>
                        </div>
                        </div>--%>
                </footer><!-- End Footer -->

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
            </form>
        </body>

        </html>