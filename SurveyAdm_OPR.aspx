<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SurveyAdm_OPR.aspx.vb" Inherits="SurveyAdm_OPR"
    MaintainScrollPositionOnPostback="true" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
        <meta http-equiv="Pragma" content="no-cache" />
        <meta http-equiv="Expires" content="-1" />
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
                    <link href="./assets/vendor/icofont/icofont.min.css" rel="stylesheet">
                    <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
                    <%-- Start WI368 by Manoj Kumar on 30-05-2021--%>
                        <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet"> <%-- WI368 one line added
                            --%>
                            <%-- End by Manoj Kumar on 30-05-2021--%>
                                <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
                                <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">
                                <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
                                <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>

                                <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css"
                                    rel="stylesheet">

                                <!-- Template Main CSS File -->
                                <link href="assets/css/styleIL3.css" rel="stylesheet">

                                <link
                                    href='https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/ui-lightness/jquery-ui.css'
                                    rel='stylesheet'>

                                <!-- <script src="//code.jquery.com/jquery-1.11.0.min.js"></script> -->


                                <style type="text/css">
                                    #header .sessionNm a {
                                        color: #fff;
                                        font-size: 15px;
                                        font-weight: bold;
                                    }

                                    #header.header-scrolled .sessionNm a {
                                        color: #493c3e;
                                        font-size: 15px;
                                        font-weight: bold;
                                    }

                                    .ui-datepicker {
                                        width: 12em;
                                    }

                                    h1 {
                                        color: green;
                                    }

                                    #txtStartDt {
                                        text-align: center;
                                    }

                                    #txtEndDt {
                                        text-align: center;
                                    }

                                    #txtCycleStartDt {
                                        text-align: center;
                                    }

                                    #txtCycleEndDt {
                                        text-align: center;
                                    }

                                    .ui-datepicker-trigger {
                                        padding: 0px;
                                        padding-left: 5px;
                                        vertical-align: baseline;

                                        position: relative;
                                        top: -27px;
                                        height: 22px;
                                    }

                                    a:hover {
                                        color: white !important;
                                        cursor: pointer;
                                    }

                                    .dropdown-menu>li>a {
                                        background-color: #fff !important;
                                    }

                                    .dropdown-menu>li>a:hover {
                                        background: white !important;
                                        color: black !important;
                                    }

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
                                    function openModal() {
                                        var myModal = new bootstrap.Modal(document.getElementById('staticBackdroppend'));
                                        myModal.show();
                                    }

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
                                        debugger;
                                        //setDatePicker();
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
                                    function showConfirmMessageModal(type, message) {
                                        swal({
                                            title: "Are you sure you want to Approve this Record?",
                                            text: "",
                                            icon: "info",
                                            //buttons: true,
                                            //dangerMode: true
                                            showCancelButton: true,
                                            confirmButtonColor: "#DD6B55",
                                            confirmButtonText: "Yes, approve it!",
                                            closeOnConfirm: false
                                        }, function (isConfirm) {
                                            if (isConfirm) {
                                                var pno = $('#<%=hdfSession.ClientID%>').val();
                                                var yr = $('#<%=hdfYear.ClientID%>').val();
                                                var cyc = $('#<%=hdfCycle.ClientID%>').val();
                                                $.ajax({
                                                    type: "POST",
                                                    url: "SurveyAdm_opr.aspx/SubmitClick",
                                                    data: "{User:'" + message + "',Syear:'" + yr + "',Scyc:'" + cyc + "',UserId:'" + pno + "'}",
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function (r) {
                                                        if (r.d == true) {
                                                            //swal(r.d, 'success');
                                                            swal({
                                                                title: "",
                                                                text: "Record approved successfully",
                                                                icon: "success"
                                                            }, function (isConfirm) {
                                                                location.reload();
                                                            });
                                                        }
                                                    }
                                                });
                                            }
                                        });
                                        return false;
                                    }

                                    function Check_Click(objRef) {
                                        debugger;

                                        var row = objRef.parentNode.parentNode;

                                        if (objRef.checked) {
                                        }

                                        else {


                                            if (row.rowIndex % 2 == 0) {

                                            }

                                            else { }

                                        }


                                        var GridView = row.parentNode;

                                        var inputList = GridView.getElementsByTagName("input");

                                        for (var i = 0; i < inputList.length; i++) {

                                            var headerCheckBox = inputList[0];

                                            var checked = true;

                                            if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {

                                                if (!inputList[i].checked) {

                                                    checked = false;

                                                    break;

                                                }

                                            }

                                        }

                                        //headerCheckBox.checked = checked;
                                    }


                                    function checkAll(objRef) {

                                        var GridView = objRef.parentNode.parentNode.parentNode;

                                        var inputList = GridView.getElementsByTagName("input");

                                        for (var i = 0; i < inputList.length; i++) {

                                            var row = inputList[i].parentNode.parentNode;

                                            if (inputList[i].type == "checkbox" && objRef != inputList[i]) {

                                                if (objRef.checked) {
                                                    inputList[i].checked = true;
                                                }

                                                else {

                                                    if (row.rowIndex % 2 == 0) {
                                                    }

                                                    else {

                                                    }

                                                    inputList[i].checked = false;

                                                }

                                            }

                                        }

                                    }
        <% -- function setDatePicker() {
                                        $('#<%=txt_dor.ClientID%>').datepicker({ format: 'dd-M-yyyy', endDate: '+0d', autoclose: true });
                                    }; --%>
                                        function ShowApprovePopup() {
                                            var myModal = new bootstrap.Modal(document.getElementById('staticBackdroppend'));
                                            myModal.show();
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

                            <h1 class="logo mr-auto"><a>360&deg; <span style="font-size:20px"> FEEDBACK
                                        SURVEY</span></a></h1>
                            <div style="float:right;">
                                <ul style="list-style:none;">
                                    <li>
                                        <div style="padding:0 5px;text-align: center;">
                                            <img alt="USER IMAGE" width="40" height="30" class="rounded-circle"
                                                src='<%="https://intranet.corp.tatasteel.com/GIH001.ashx?pf=YY&cid=" + Session("ADM_USER") %>' />
                                        </div>
                                    </li>
                                    <li>
                                        <div class="sessionNm">
                                            <a href="#">
                                                <asp:Label ID="lblSessionNm" runat="server" Text=""></asp:Label>
                                            </a>
                                        </div>

                                    </li>
                                </ul>
                                <div>

                                </div>
                                <div style="font-size:20px;">

                                </div>
                            </div>




                            <!-- Uncomment below if you prefer to use an image logo -->
                            <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->
                        </div>
                        <div class="container d-flex align-items-center">
                            <nav class="nav-menu d-none d-lg-block">
                                <ul>
                                    <li class="dropdown">
                                        <a class="dropdown-toggle" data-bs-toggle="dropdown" href="#">Step-1 (Select
                                            Respondent)</a>
                                        <ul class="dropdown-menu">
                                            <li><a href="SurveyAdm_OPR.aspx">Add/Remove/Submit Respondent</a></li>
                                            <li><a href="" data-bs-toggle="modal" data-bs-target="#myModal">Return To
                                                    Assesse </a></li>
                                            <li><a href="" data-bs-toggle="modal" data-bs-target="#myModal2"
                                                    visible="false" runat="server">Send Communication To Assesse To
                                                    Select Respondent</a></li>
                                        </ul>
                                    </li>
                                    <li class="dropdown">
                                        <a class="dropdown-toggle" data-bs-toggle="dropdown" href="#">Step-2 (Approve
                                            Respondent)</a>
                                        <ul class="dropdown-menu">
                                            <li><a href="SurveyAdm_opr.aspx?adm=1">Approve Respondent</a></li>
                                            <li><a href="" data-bs-toggle="modal" data-bs-target="#myModalE">Return To
                                                    Approver</a></li>
                                            <li><a href="" data-bs-toggle="modal" data-bs-target="#myModal3"
                                                    runat="server" visible="false">Send communication to approver to
                                                    approve respodents</a></li>
                                        </ul>
                                    </li>
                                    <li class="dropdown">
                                        <a class="dropdown-toggle" data-bs-toggle="dropdown" href="#">Step-3 (Provide
                                            Feedback)</a>
                                        <ul class="dropdown-menu">
                                            <%--<li><a href="FeedbackAdm_opr.aspx">Survey Feedback</a>
                                    </li>--%>
                                    <li><a href="" data-bs-toggle="modal" data-bs-target="#myModal1">Revert Feedback</a>
                                    </li>
                                    <li><a href="" data-bs-toggle="modal" data-bs-target="#myModalR" runat="server"
                                            id="limenuR" visible="true">Remove Respondent</a></li>
                                    <li><a href="FeedbackAdm_Adm.aspx" runat="server" id="SurveyFeedback"
                                            visible="false">Survey Feedback</a></li>
                                    <li><%--<a href="SurveyAdm_opr.aspx?adm=3" visible="false">Add Respondent For
                                            Rejected Case</a>--%></li>
                                </ul>
                                </li>
                                <%--WI447 end of code--%>

                                    <li class="dropdown">
                                        <a class="dropdown-toggle" data-bs-toggle="dropdown" href="#">Report</a>
                                        <ul class="dropdown-menu">
                                            <%--<li><a
                                                    href="http://webappsdev01.corp.tatasteel.com/Feedback360/MinimumCriteriaRpt.aspx"
                                                    target="_blank">Minimum Criteria (Generation of Report)</a>
                                    </li>--%>
                                    <li><a href="MinimumCriteriaRpt.aspx" target="_blank">Minimum Criteria Report</a>
                                    </li>
                                    <li><a href="SelectAssesor_Rpt.aspx" target="_blank">Selection of respondent status
                                            ( Assesse survey status)</a></li>
                                    <li><a href="SurveyCompletionStatusRpt.aspx" target="_blank">Survey completion
                                            status (individual respondent status)</a></li>
                                    <li><a href="" data-bs-toggle="modal" data-bs-target="#myModal5" id="la"
                                            runat="server" visible="false">individual Report</a></li>
                                    <li><a href="" data-bs-toggle="modal" data-bs-target="#myModal7" id="la1"
                                            runat="server" visible="false">Overall Report</a></li>
                                    <li><a href="" data-bs-toggle="modal" data-bs-target="#myModal6" id="la2"
                                            runat="server" visible="false">Feedback report(pdf)</a></li>
                                    </ul>
                                    </li>
                                    <li class="dropdown">
                                        <a class="dropdown-toggle" data-bs-toggle="dropdown" href="#">Setting</a>
                                        <ul class="dropdown-menu">
                                            <li><a href="#" data-bs-toggle="modal" data-bs-target="#TimelineSetting"
                                                    id="A4" runat="server" visible="false">Timeline Page</a></li>
                                            <li><a href="#" data-bs-toggle="modal" data-bs-target="#YearSetting" id="A5"
                                                    runat="server" visible="false">Year Setting</a></li>
                                            <li><a href="#" data-bs-toggle="modal" data-bs-target="#ChangeBUHR" id="A1"
                                                    runat="server" visible="true">Change BUHR</a></li>
                                            <li><a href="#" data-bs-toggle="modal" data-bs-target="#ChangeApproval"
                                                    id="A2" runat="server" visible="true">Change Approver</a></li>
                                            <%-- <li><a href="FreezeEmpRecord.aspx" target="_blank">Freeze Employee
                                                    Record</a>
                                    </li>--%>
                                    </ul>
                                    </li>
                                    <li><a href="Images/User_manual_360_Degree Admin.pdf" target="_blank">Help</a></li>
                                    <%-- <li visible="false">
                                        <a href="selectassesornew_OPR.aspx" target="_blank"
                                            visible="false">Exception</a>
                                        </li>--%>
                                        <%--<li class="" visible="false"><a href="SurveyAdm_opr.aspx?adm=2"
                                                visible="false">Overall</a></li>--%>

                                            </ul>
                            </nav><!-- .nav-menu -->

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
                                company globally in the next 5-10 years, we are developing agile behaviours across the
                                organization. We will measure this as an integral part of our Performance Management
                                System for all the officers through a 360 degree feedback survey</h2>
                            <a href="#about" class="btn-get-started scrollto" id="resp" runat="server"></a>
                        </div>
                    </section><!-- End Hero -->

                    <main id="main">

                        <!-- ======= About Section ======= -->
                        <section id="about" class="about">
                            <div class="container-fluid">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">

                                                <div class="col-md-12">
                                                    <label>Executive Head</label>
                                                    <asp:DropDownList runat="server" ID="ddlExecutive"
                                                        CssClass="form-control" AutoPostBack="true" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">

                                                <div class="col-md-12">
                                                    <label>Department</label>
                                                    <asp:DropDownList runat="server" ID="ddlDept"
                                                        CssClass="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">

                                                <div class="col-md-12">
                                                    <label>BUHR Per. No.</label>
                                                    <asp:TextBox runat="server" ID="txtBuhr" CssClass="form-control"
                                                        MaxLength="6" placeholder="BUHR Per. No" />

                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">

                                                <div class="col-md-12">
                                                    <label>Officer Per. No.</label>
                                                    <asp:TextBox runat="server" ID="txtperno1" CssClass="form-control"
                                                        MaxLength="6" placeholder="Officer Per. No."></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-10">
                                            <div style="margin-top:25px;margin-left:30px;float:left;">
                                                <asp:LinkButton ID="btnsearch" runat="server" class="btn btn-primary"
                                                    type="submit" CausesValidation="false">
                                                    Show
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
                                                    <div class="col-10"></div>
                                                    <div class="col-4">

                                                    </div>
                                                    </br>
                                                    <%-- <asp:HiddenField runat="server" ID="hdfnfy" />--%>
                                                    <div class="col-4" style="margin-top:-10px">
                                                        <%--<asp:Button runat="server" ID="btnsearch" Text="Go"
                                                            class="btn-learn-more"></asp:Button>--%>
                                                    </div>
                                                    <asp:HiddenField ID="hdfSession" runat="server" />
                                                    <asp:HiddenField ID="hdfYear" runat="server" />
                                                    <asp:HiddenField ID="hdfCycle" runat="server" />
                                                    <asp:GridView ID="gvself" runat="server" Visible="false"
                                                        AutoGenerateColumns="False"
                                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                        Font-Names="verdana" EmptyDataText="No Record Found"
                                                        BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                        GridLines="Vertical" RowStyle-CssClass="rows">
                                                        <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                        <HeaderStyle BackColor="#e43c5c" Font-Bold="True"
                                                            ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />

                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Assesse P.no">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblpno" runat="server"
                                                                        Text='<%# Eval("SS_ASSES_PNO")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Assesse Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lnlname" runat="server"
                                                                        Text='<%# Eval("EMA_ENAME")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Assesse Designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbllevel" runat="server"
                                                                        Text='<%# Eval("ema_desgn_desc")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Approver">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblappr" runat="server"
                                                                        Text='<%# Eval("Approver")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Criteria Completed">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblstats" Text="">
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Click to View Details">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server"
                                                                        ID="lbpendingapproval" Text=""
                                                                        OnClick="lbpendingapproval_Click"
                                                                        CssClass="btn-learn-more"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

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

                                                    <asp:GridView ID="gvoverall" runat="server"
                                                        AutoGenerateColumns="False"
                                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                        Font-Names="verdana" EmptyDataText="No Record Found"
                                                        BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                        GridLines="Vertical" RowStyle-CssClass="rows">
                                                        <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                        <HeaderStyle BackColor="#e43c5c" Font-Bold="True"
                                                            ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />

                                                        <Columns>

                                                            <asp:TemplateField HeaderText="P.No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfpno1" runat="server"
                                                                        Text='<%# Eval("SS_ASSES_PNO")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Employee Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfname1" runat="server"
                                                                        Text='<%# Eval("EMA_ENAME")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Level">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfmail1" runat="server"
                                                                        Text='<%# Eval("EMA_EMPL_SGRADE")%>'>
                                                                    </asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblflvl1" runat="server"
                                                                        Text='<%# Eval("EMA_DESGN_DESC")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Executive Head">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblfdesg1"
                                                                        Text='<%# Eval("EMA_EXEC_HEAD_DESC")%>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Accountability (Score)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblfexc1"
                                                                        Text='<%# Eval("a")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Collaboration(Score)">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsts1" runat="server"
                                                                        Text='<%# Eval("b")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Responsiveness(Score)">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcateg1" runat="server"
                                                                        Text='<%# Eval("c")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="People Dev(Score)">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblapno1" runat="server"
                                                                        Text='<%# Eval("d")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Number Of respondent">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblaname1" runat="server"
                                                                        Text='<%# Eval("NO_RECORDS")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Combination">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblaexe1"
                                                                        Text='<%# Eval("ALL1")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField
                                                                HeaderText="Accountability (Category G/A/U)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblaemail1"
                                                                        Text='<%# Eval("r1")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField
                                                                HeaderText="Collaboration(Category G/A/U)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lbladsg1"
                                                                        Text='<%# Eval("r2")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField
                                                                HeaderText="Responsiveness(Category G/A/U)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblaexe1"
                                                                        Text='<%# Eval("r3")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="People Dev(Category G/A/U)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblaexe1"
                                                                        Text='<%# Eval("r4")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>




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



                                                    <asp:GridView ID="GridView3" runat="server"
                                                        AutoGenerateColumns="False"
                                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                        Font-Names="verdana" EmptyDataText="No Record Found"
                                                        BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                        GridLines="Vertical" RowStyle-CssClass="rows">
                                                        <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                        <HeaderStyle BackColor="#e43c5c" Font-Bold="True"
                                                            ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />

                                                        <Columns>

                                                            <asp:TemplateField HeaderText="P.No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfpno1" runat="server"
                                                                        Text='<%# Eval("EMA_PERNO")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Employee Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfname1" runat="server"
                                                                        Text='<%# Eval("EMA_ENAME")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Level">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfmail1" runat="server"
                                                                        Text='<%# Eval("EMA_EMPL_SGRADE")%>'>
                                                                    </asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblflvl1" runat="server"
                                                                        Text='<%# Eval("ema_desgn_desc")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Executive Head">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblfdesg1"
                                                                        Text='<%# Eval("ema_exec_head_desc")%>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="BUHR Pno">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblbhrpno"
                                                                        Text='<%# Eval("ema_bhr_pno")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Buhr Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblbhrnam"
                                                                        Text='<%# Eval("EMA_BHR_NAME")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Respondent Pno">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblrespno"
                                                                        Text='<%# Eval("ss_pno")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Respondent name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblresnm"
                                                                        Text='<%# Eval("ss_name")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Category">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblcateg"
                                                                        Text='<%# Eval("ss_categ")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Accountability (Score)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblfexc1"
                                                                        Text='<%# Eval("a")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Collaboration(Score)">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsts1" runat="server"
                                                                        Text='<%# Eval("c")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Responsiveness(Score)">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcateg1" runat="server"
                                                                        Text='<%# Eval("r")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Team Building(score)">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblapno1" runat="server"
                                                                        Text='<%# Eval("T")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>



                                                            <asp:TemplateField
                                                                HeaderText="Accountability (Category G/A/U)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblaemail1"
                                                                        Text='<%# Eval("ac")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField
                                                                HeaderText="Collaboration(Category G/A/U)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lbladsg1"
                                                                        Text='<%# Eval("col")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField
                                                                HeaderText="Responsiveness(Category G/A/U)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblaexe1"
                                                                        Text='<%# Eval("res")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField
                                                                HeaderText="Team Building(Category G/A/U)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblaexe1"
                                                                        Text='<%# Eval("team")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Combination">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblcomb"
                                                                        Text='<%# Eval("Combination")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


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

                                                    <asp:GridView ID="GridView4" runat="server"
                                                        AutoGenerateColumns="False"
                                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                        Font-Names="verdana" EmptyDataText="No Record Found"
                                                        BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                        GridLines="Vertical" RowStyle-CssClass="rows">
                                                        <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                        <HeaderStyle BackColor="#e43c5c" Font-Bold="True"
                                                            ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />

                                                        <Columns>

                                                            <asp:TemplateField HeaderText="P.No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfpno1" runat="server"
                                                                        Text='<%# Eval("EMA_PERNO")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Employee Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfname1" runat="server"
                                                                        Text='<%# Eval("EMA_ENAME")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Level">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfmail1" runat="server"
                                                                        Text='<%# Eval("EMA_EMPL_SGRADE")%>'>
                                                                    </asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblflvl1" runat="server"
                                                                        Text='<%# Eval("EMA_DESGN_DESC")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Executive Head">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblfdesg1"
                                                                        Text='<%# Eval("EMA_EXEC_HEAD_DESC")%>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="BUHR Pno">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblbhrpno"
                                                                        Text='<%# Eval("EMA_BHR_PNO")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Buhr Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblbhrnam"
                                                                        Text='<%# Eval("EMA_BHR_NAME")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Category">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblcateg"
                                                                        Text='<%# Eval("SS_CATEG")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Accountability (Score)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblfexc1"
                                                                        Text='<%# Eval("a")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Collaboration(Score)">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsts1" runat="server"
                                                                        Text='<%# Eval("c")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Responsiveness(Score)">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcateg1" runat="server"
                                                                        Text='<%# Eval("r")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Team Building(Score)">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblapno1" runat="server"
                                                                        Text='<%# Eval("T")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>



                                                            <asp:TemplateField
                                                                HeaderText="Accountability (Category G/A/U)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblaemail1"
                                                                        Text='<%# Eval("ac")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField
                                                                HeaderText="Collaboration(Category G/A/U)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lbladsg1"
                                                                        Text='<%# Eval("col")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField
                                                                HeaderText="Responsiveness(Category G/A/U)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblaexe1"
                                                                        Text='<%# Eval("res")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField
                                                                HeaderText="Team Building(Category G/A/U)">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblaexe1"
                                                                        Text='<%# Eval("team")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Combination">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblcomb"
                                                                        Text='<%# Eval("Combination")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


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


                                                    <asp:GridView ID="GridView1" runat="server" Visible="false"
                                                        AutoGenerateColumns="False"
                                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                        Font-Names="verdana" EmptyDataText="No Record Found"
                                                        BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                        GridLines="Vertical" RowStyle-CssClass="rows">
                                                        <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                        <HeaderStyle BackColor="#e43c5c" Font-Bold="True"
                                                            ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />

                                                        <Columns>

                                                            <asp:TemplateField HeaderText="RESPONDENT P.no">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblpno1" runat="server"
                                                                        Text='<%# Eval("SS_ASSES_PNO")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="RESPONDENT Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lnlname1" runat="server"
                                                                        Text='<%# Eval("EMA_ENAME")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="RESPONDENT Designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbllevel1" runat="server"
                                                                        Text='<%# Eval("ema_desgn_desc")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Approver">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblappr1" runat="server"
                                                                        Text='<%# Eval("Approver")%>'></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="lbpend"
                                                                        Text="Click to Approve" OnClick="lbpend_Click"
                                                                        CssClass="btn-learn-more"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

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

                                                    <asp:GridView ID="gdvMiniCriteria" runat="server" Visible="true"
                                                        AutoGenerateColumns="False"
                                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                        Font-Names="verdana" EmptyDataText="No Record Found"
                                                        BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                        GridLines="Vertical" RowStyle-CssClass="rows">
                                                        <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                        <HeaderStyle BackColor="#e43c5c" Font-Bold="True"
                                                            ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />

                                                        <Columns>

                                                            <asp:BoundField DataField="ss_asses_pno" HeaderText="P.no"
                                                                SortExpression="ss_asses_pno" />
                                                            <asp:BoundField DataField="ema_ename" HeaderText="Name"
                                                                SortExpression="ema_ename" />
                                                            <asp:BoundField DataField="ss_categ"
                                                                HeaderText="Respondent Category"
                                                                SortExpression="ss_categ" />
                                                            <asp:BoundField DataField="minmum" HeaderText="Minimum"
                                                                SortExpression="minmum" />
                                                            <asp:BoundField DataField="Pending" HeaderText="Pending"
                                                                SortExpression="Pending" />
                                                            <asp:BoundField DataField="Completed" HeaderText="Completed"
                                                                SortExpression="Completed" />
                                                            <asp:BoundField DataField="Rejected"
                                                                HeaderText="Insufficient Exposure"
                                                                SortExpression="Rejected" />
                                                            <asp:BoundField DataField="Status" HeaderText="Criteria"
                                                                SortExpression="Status" />
                                                            <asp:BoundField DataField="ema_bhr_pno"
                                                                HeaderText="BUHR P No." SortExpression="ema_bhr_pno" />
                                                            <asp:BoundField DataField="ema_bhr_name"
                                                                HeaderText="BUHR Name" SortExpression="ema_bhr_name" />
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
                                                    <div class="modal fade" id="staticBackdroppend"
                                                        data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1"
                                                        aria-labelledby="staticBackdropLabel" aria-hidden="true">
                                                        <div class="modal-dialog">
                                                            <div class="modal-content">

                                                                <div class="modal-body">
                                                                    Do you want to approve?
                                                                </div>
                                                                <div class="modal-footer">

                                                                    <asp:Button runat="server" ID="Button1"
                                                                        OnClick="SubmitPend" Text="Yes"
                                                                        class="btn btn-primary" />
                                                                    <button type="button" class="btn btn-secondary"
                                                                        data-bs-dismiss="modal">No</button>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </ContentTemplate>
                                                <Triggers>

                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>

                                    </div>

                                </div>
                                <asp:UpdatePanel ID="uppanelbtn" runat="server">
                                    <ContentTemplate>
                                        <div class="section-title" runat="server" id="divtitle" visible="false">
                                            <h3>RESPONDENT Selected List for<span><strong>
                                                        <asp:Label runat="server" ID="lblassname"></asp:Label>
                                                    </strong> </span></h3>
                                        </div>

                                        <div class="row content">
                                            <div class="col-md-3">


                                                <asp:Button runat="server" ID="btnaddpeertsl" Text="Add Tata Steel"
                                                    class="btn-learn-more" OnClick="btnaddpeertsl_Click"
                                                    Visible="false"></asp:Button>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Button runat="server" ID="btnaddnontsl" Text="Add Non Tata Steel"
                                                    class="btn-learn-more" OnClick="btnaddnontsl_Click" Visible="false">
                                                </asp:Button>
                                            </div>

                                        </div><br />
                                        <div class="row">
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <asp:Label runat="server" ID="lbls" CssClass="label" Visible="false"
                                                    style="font-size:12pt; color:black;"></asp:Label>
                                            </div>
                                        </div><br />

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
                                                                            CssClass="form-control" placeholder="P. No"
                                                                            OnTextChanged="txtpnoI_TextChanged"
                                                                            AutoPostBack="true" />
                                                                        <cc1:AutoCompleteExtender
                                                                            ID="AutoCompleteExtender1" runat="server"
                                                                            TargetControlID="txtpnoI"
                                                                            ServiceMethod="SearchPrefixesForApprover"
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
                                                                            placeholder="Designation" Enabled="false" />
                                                                        <%--<asp:HiddenField runat="server"
                                                                            ID="hdfnin" /> --%>
                                                                        <asp:Label runat="server" ID="lbllevel"
                                                                            Visible="false"></asp:Label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="form-group">

                                                                    <div class="col-md-12">
                                                                        <asp:TextBox runat="server" ID="txtemailI"
                                                                            CssClass="form-control " placeholder="Email"
                                                                            Enabled="false" />
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
                                                                            placeholder="Org Name" Enabled="false" />

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
                                                                            data-width="100%" data-live-search="true">
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
                                                                    class="btn-learn-more" OnClick="btnAddP_Click">
                                                                </asp:Button>
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
                                                                            CssClass="form-control" placeholder="Name"
                                                                            MaxLength="50" />

                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="form-group">

                                                                    <div class="col-md-12">
                                                                        <asp:TextBox runat="server" ID="txtdesgnI"
                                                                            CssClass="form-control"
                                                                            placeholder="Designation" MaxLength="50" />

                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="form-group">

                                                                    <div class="col-md-12">
                                                                        <asp:TextBox runat="server" ID="txtemailnI"
                                                                            CssClass="form-control " placeholder="Email"
                                                                            MaxLength="50" />
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
                                                                            placeholder="Org Name" MaxLength="50" />

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div><br />
                                                        <div class="row">
                                                            <br />
                                                            <div class="col-md-3">
                                                                <div class="form-group">

                                                                    <div class="col-md-12">
                                                                        <asp:DropDownList ID="ddlrole" runat="server"
                                                                            CssClass="form-control" data-width="100%"
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
                                                                <asp:Button runat="server" ID="btnaddnorgI" Text="Add"
                                                                    class="btn-learn-more" OnClick="btnaddnorgI_Click">
                                                                </asp:Button>
                                                            </div>

                                                        </div>

                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <%-- <Triggers>
                                        <asp:PostBackTrigger ControlID="divtsl" />
                                        <asp:PostBackTrigger ControlID="divntsl" />
                                        </Triggers>--%>
                                </asp:UpdatePanel>
                                <div class="row">
                                    <div class="col-md-12 col-lg-12">
                                        <div class="table-responsive">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gvfinal" runat="server"
                                                        AutoGenerateColumns="False" Visible="false"
                                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                        Font-Names="verdana" EmptyDataText="No Record Found"
                                                        BackColor="#ffccff" BorderColor="Black" BorderStyle="None"
                                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                                        RowStyle-CssClass="rows" OnRowDataBound="GvCateg_RowDataBound"
                                                        OnRowDeleting="gvfinal_RowDeleting" DataKeyNames="SS_PNO">
                                                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                        <HeaderStyle CssClass="bg-clouds segoe-light"
                                                            BackColor="#FFB6C1" Font-Bold="True" ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="P.no">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblpno" runat="server"
                                                                        Text='<%# Eval("SS_PNO")%>'></asp:Label>
                                                                    <asp:Label ID="lblapno" runat="server"
                                                                        Text='<%# Eval("SS_ASSES_PNO")%>'
                                                                        Visible="false"></asp:Label>
                                                                    <%-- <asp:HiddenField runat="server"
                                                                        Value='<%# Eval("SS_ASSES_PNO") %>'
                                                                        ID="hdfnperno" />--%>
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
                                                                        Text='<%# Eval("SS_EMAIL")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Category">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDelTag" runat="server"
                                                                        Visible="false" Text='<%# Eval("SS_DEL_TAG")%>'>
                                                                    </asp:Label>
                                                                    <asp:Label ID="lblCate" runat="server"
                                                                        Visible="false" Text='<%# Eval("SS_CATEG")%>'>
                                                                    </asp:Label>
                                                                    <asp:Label ID="lblcategory" runat="server"
                                                                        Text='<%# Eval("Category")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:CommandField ButtonType="Button"
                                                                ShowDeleteButton="true" selecttext="Remove"
                                                                HeaderText="Remove">
                                                                <ControlStyle CssClass="btn-learn-more" />
                                                            </asp:CommandField>

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


                                                    <asp:GridView ID="GridView2" runat="server"
                                                        AutoGenerateColumns="False" Visible="false"
                                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                        Font-Names="verdana" EmptyDataText="No Record Found"
                                                        BackColor="#ffccff" BorderColor="Black" BorderStyle="None"
                                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                                        RowStyle-CssClass="rows">
                                                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                        <HeaderStyle CssClass="bg-clouds segoe-light"
                                                            BackColor="#FFB6C1" Font-Bold="True" ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="P.no">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblpno" runat="server"
                                                                        Text='<%# Eval("SS_PNO")%>'></asp:Label>
                                                                    <asp:HiddenField runat="server"
                                                                        Value='<%# Eval("SS_ASSES_PNO") %>'
                                                                        ID="hdfnperno" />
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
                                                                        Text='<%# Eval("SS_EMAIL")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Category">
                                                                <ItemTemplate>

                                                                    <asp:Label ID="lblcategory" runat="server"
                                                                        Text='<%# Eval("Category")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>



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



                                                </ContentTemplate>
                                                <Triggers>

                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>


                                </div>
                                <asp:UpdatePanel runat="server" ID="upappbtn">
                                    <ContentTemplate>


                                        <div class="row content">
                                            <div class="col-lg-5">

                                            </div>
                                            <div class="col-lg-5">
                                                <asp:Button runat="server" ID="btnsubmit" Text="Submit"
                                                    class="btn-learn-more" data-bs-toggle="modal"
                                                    data-bs-target="#staticBackdrop" Visible="false"></asp:Button>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
                        <!-- End About Section -->

                    </main><!-- End #main -->


                    <div class="modal fade" id="myModal" role="dialog">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Return To Assesse</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <%-- <p>Some text in the modal.</p>--%>

                                        <div class="row">
                                            <div class="">
                                                <div class="col-sm-8">
                                                    <asp:TextBox runat="server" ID="txtperno" CssClass="form-control"
                                                        placeholder="P.No" />
                                                </div>

                                                <div class="col-sm-4 form-group">
                                                    <asp:Button runat="server" ID="btnreset" Text="Return"
                                                        class="btn-learn-more" OnClick="btnreset_Click"></asp:Button>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-default"
                                                data-bs-dismiss="modal">Close</button>
                                        </div>
                                </div>

                            </div>
                        </div>

                    </div>

                    <div class="modal fade" id="myModal1" role="dialog">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Revert Feedback</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <%-- <p>Some text in the modal.</p>--%>

                                        <div class="row">
                                            <div class="">
                                                <div class="col-sm-4">
                                                    <asp:TextBox runat="server" ID="txtassespno" CssClass="form-control"
                                                        placeholder="Assesee P.No" />

                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox runat="server" ID="txtrespno" CssClass="form-control"
                                                        placeholder="Resp.Pno" />
                                                </div>

                                                <div class="col-sm-4 form-group">
                                                    <asp:Button runat="server" ID="btnrevert" Text="Return"
                                                        class="btn-learn-more" OnClick="btnrevert_Click"></asp:Button>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-default"
                                                data-bs-dismiss="modal">Close</button>
                                        </div>
                                </div>

                            </div>
                        </div>

                    </div>
                    <div class="modal fade" id="myModalR" role="dialog">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Remove Respondent</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <%-- <p>Some text in the modal.</p>--%>

                                        <div class="row">
                                            <div class="">
                                                <div class="col-sm-4">
                                                    <asp:TextBox runat="server" ID="txt_Rasspno" CssClass="form-control"
                                                        placeholder="Assesee P.No" />

                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox runat="server" ID="txt_Rpno" CssClass="form-control"
                                                        placeholder="Resp.Pno" />
                                                </div>

                                                <div class="col-sm-4 form-group">
                                                    <asp:Button runat="server" ID="btnR" Text="Remove Respondent"
                                                        class="btn-learn-more" OnClick="btnR_Click"></asp:Button>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-default"
                                                data-bs-dismiss="modal">Close</button>
                                        </div>
                                </div>

                            </div>
                        </div>

                    </div>
                    <div class="modal fade" id="myModal2" role="dialog">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Send Communication To Assesse</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <%-- <p>Some text in the modal.</p>--%>

                                        <div class="row">
                                            <div class="">
                                                <div class="col-sm-4">
                                                    <asp:TextBox runat="server" ID="txt_pnocom" CssClass="form-control"
                                                        placeholder="Assesee P.No" />

                                                </div>


                                                <div class="col-sm-4 form-group">
                                                    <asp:Button runat="server" ID="btn_comm"
                                                        Text="Send Communication To Assesse" class="btn-learn-more"
                                                        OnClick="btncomm_Click"></asp:Button>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-default"
                                                data-bs-dismiss="modal">Close</button>
                                        </div>
                                </div>

                            </div>
                        </div>

                    </div>

                    <div class="modal fade" id="myModal5" role="dialog">
                        <div class="modal-dialog modal-lg">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Individual Report For Assesse - Final Data (Tag)</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="upnlIndivial" runat="server">
                                        <ContentTemplate>
                                            <div class="row form-group">
                                                <div class="col-sm-2">
                                                    <label>Year</label><span style="color:red;">*</span>
                                                    <asp:TextBox ID="txtIyear" runat="server" CssClass="form-control"
                                                        Text="" MaxLength="4" />
                                                    <asp:RequiredFieldValidator ID="rfvtxtIyear" runat="server"
                                                        ControlToValidate="txtIyear" ValidationGroup="IndivisualRpt"
                                                        SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-sm-2">
                                                    <label>Cycle</label><span style="color:red;">*</span>
                                                    <asp:TextBox ID="txtIcycle" runat="server" CssClass="form-control"
                                                        Text="" MaxLength="1" />
                                                    <asp:RequiredFieldValidator ID="rfvtxtIcycle" runat="server"
                                                        ControlToValidate="txtIcycle" ValidationGroup="IndivisualRpt"
                                                        SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-sm-4">
                                                    <label>Category</label>
                                                    <asp:DropDownList runat="server" ID="ddltype"
                                                        CssClass="form-control" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                                        <asp:ListItem Value="Rp">--Select Option--</asp:ListItem>
                                                        <asp:ListItem Value="pno">Enter Pno</asp:ListItem>
                                                        <asp:ListItem Value="IL3">Overall IL1-3</asp:ListItem>
                                                        <asp:ListItem Value="IL4">Overall IL4-6</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-4">
                                                    <label>Assesee P. No.</label>
                                                    <asp:TextBox runat="server" ID="TextBox1" CssClass="form-control" />
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="modal-footer">
                                    <div style="text-align:center;margin:auto;">
                                        <asp:LinkButton runat="server" ID="Button3" class="btn btn-primary"
                                            OnClick="Button3_Click" ValidationGroup="IndivisualRpt"><i
                                                class="fa fa-file-text"></i>&nbsp;Download</asp:LinkButton>&nbsp;
                                        <button type="button" class="btn btn-danger"
                                            data-bs-dismiss="modal">Close</button>
                                        <asp:Button runat="server" ID="Button4" Text="Download(.pdf)"
                                            class="btn-learn-more" OnClick="Button4_Click" Visible="false"></asp:Button>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>

                    <div class="modal fade" id="myModal7" role="dialog">
                        <div class="modal-dialog modal-lg">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Over All Report For Assesse</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="upnlOverall" runat="server">
                                        <ContentTemplate>
                                            <div class="row form-group">
                                                <div class="col-sm-2">
                                                    <label>Year</label><span style="color:red;">*</span>
                                                    <asp:TextBox ID="txtOyear" runat="server" CssClass="form-control"
                                                        Text="" MaxLength="4" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4"
                                                        runat="server" ControlToValidate="txtOyear"
                                                        ValidationGroup="OverallRpt" SetFocusOnError="true">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-sm-2">
                                                    <label>Cycle</label><span style="color:red;">*</span>
                                                    <asp:TextBox ID="txtOCycle" runat="server" CssClass="form-control"
                                                        Text="" MaxLength="1" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5"
                                                        runat="server" ControlToValidate="txtOCycle"
                                                        ValidationGroup="OverallRpt" SetFocusOnError="true">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-sm-3">
                                                    <label>Category</label>
                                                    <asp:DropDownList runat="server" ID="DropDownList2"
                                                        CssClass="form-control" AutoPostBack="true"
                                                        OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged">
                                                        <asp:ListItem Value="Rp">--Select Option--</asp:ListItem>
                                                        <asp:ListItem Value="pno">Enter Pno</asp:ListItem>
                                                        <asp:ListItem Value="IL1">Overall IL1</asp:ListItem>
                                                        <asp:ListItem Value="IL2">Overall IL2</asp:ListItem>
                                                        <asp:ListItem Value="IL3">Overall IL3</asp:ListItem>
                                                        <asp:ListItem Value="IL4">Overall IL4</asp:ListItem>
                                                        <asp:ListItem Value="IL5">Overall IL5</asp:ListItem>
                                                        <asp:ListItem Value="IL6">Overall IL6</asp:ListItem>
                                                        <asp:ListItem Value="TG">Overall TSGC</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-3">
                                                    <label>Assesee P.No</label>
                                                    <asp:TextBox runat="server" ID="TextBox3" CssClass="form-control" />
                                                </div>
                                                <div class="col-sm-2">
                                                    <label>Tag</label>
                                                    <asp:DropDownList runat="server" ID="ddlTag"
                                                        CssClass="form-control">
                                                        <asp:ListItem Value="Y" Selected="True">With Tag Only
                                                        </asp:ListItem>
                                                        <asp:ListItem Value="N">All</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="modal-footer">
                                        <div style="text-align:center;margin:auto;">
                                            <asp:LinkButton runat="server" ID="Button7" Text="Download(excel)"
                                                class="btn btn-primary" OnClick="Button7_Click"
                                                ValidationGroup="OverallRpt"><i
                                                    class="fa fa-file-text"></i>&nbsp;Download</asp:LinkButton>&nbsp;
                                            <button type="button" class="btn btn-default"
                                                data-bs-dismiss="modal">Close</button>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                    </div>

                    <div class="modal fade" id="myModal6" role="dialog">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Feedback report</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <div class="row form-group">
                                        <div class="col-sm-3">
                                            <label>Year</label><span style="color:red;">*</span>
                                            <asp:TextBox ID="txtFyear" runat="server" CssClass="form-control" Text=""
                                                MaxLength="4" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                ControlToValidate="txtFyear" ValidationGroup="FeedbackRpt"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-sm-3">
                                            <label>Cycle</label><span style="color:red;">*</span>
                                            <asp:TextBox ID="txtFcycle" runat="server" CssClass="form-control" Text=""
                                                MaxLength="1" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                                ControlToValidate="txtFcycle" ValidationGroup="FeedbackRpt"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-sm-6">
                                            <label>Assesee P.No</label>
                                            <asp:TextBox runat="server" ID="TextBox2" CssClass="form-control"
                                                placeholder="Assesee P.No" />
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <div style="text-align:center;margin:auto;">
                                            <asp:LinkButton runat="server" ID="Button5" Text="Download"
                                                class="btn btn-primary" OnClick="Button5_Click"
                                                ValidationGroup="FeedbackRpt"></asp:LinkButton>&nbsp;
                                            <button type="button" class="btn btn-default"
                                                data-bs-dismiss="modal">Close</button>
                                            <asp:Button runat="server" ID="Button6" Text="Download(.pdf)"
                                                class="btn-learn-more" OnClick="Button4_Click" Visible="false">
                                            </asp:Button>
                                        </div>

                                    </div>
                                </div>

                            </div>
                        </div>

                    </div>

                    <div class="modal fade" id="myModalE" role="dialog">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Return To Approver</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <%-- <p>Some text in the modal.</p>--%>

                                        <div class="row">
                                            <div class="">
                                                <div class="col-sm-4">
                                                    <asp:TextBox runat="server" ID="txt_revertapp"
                                                        CssClass="form-control" placeholder="Assesee P.No" />

                                                </div>


                                                <div class="col-sm-4 form-group">
                                                    <asp:Button runat="server" ID="btn_revertapp"
                                                        Text="Return To Approver" class="btn-learn-more"
                                                        OnClick="btn_revertapp_click"></asp:Button>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-default"
                                                data-bs-dismiss="modal">Close</button>
                                        </div>
                                </div>

                            </div>
                        </div>

                    </div>
                    <div class="modal fade" id="myModal3" role="dialog">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Send communication to assessor to approve respodents</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <%-- <p>Some text in the modal.</p>--%>

                                        <div class="row">
                                            <div class="">
                                                <div class="col-sm-4">
                                                    <asp:TextBox runat="server" ID="txt_approvemail"
                                                        CssClass="form-control" placeholder="Approver P.No" />

                                                </div>


                                                <div class="col-sm-4 form-group">
                                                    <asp:Button runat="server" ID="Button2"
                                                        Text="Send Communication To Approver" class="btn-learn-more"
                                                        OnClick="btncommapp_Click"></asp:Button>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-default"
                                                data-bs-dismiss="modal">Close</button>
                                        </div>
                                </div>

                            </div>
                        </div>

                    </div>
                    <div class="modal fade" id="TimelineSetting">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Timeline Setting</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <div class="row form-group">
                                        <div class="col-md-4">
                                            <div style="margin-top: 8px;">
                                                <asp:Label ID="lblpageName" runat="server" Text="Page Name" />
                                            </div>
                                        </div>
                                        <div class="col-md-8">
                                            <asp:DropDownList runat="server" ID="ddlTimelinePage"
                                                CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row form-group">
                                        <div class="col-md-4">
                                            <div style="margin-top: 8px;">
                                                <asp:Label ID="Label1" runat="server" Text="Start Date" />
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtStartDt" runat="server" class="form-control float-end">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row form-group">
                                        <div class="col-md-4">
                                            <div style="margin-top: 8px;">
                                                <asp:Label ID="Label2" runat="server" Text="End Date" />
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtEndDt" runat="server" class="form-control float-end">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row" style="justify-content:center;">
                                        <asp:Button runat="server" ID="btnTimeline" Text="Submit"
                                            OnClick="btnTimeline_Click" class="btn-learn-more"></asp:Button>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div
                                            style="align-content:center; align-items:center; overflow:scroll;margin-left: 50px;">
                                            <asp:GridView ID="gdvTimeline" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                Font-Names="verdana" EmptyDataText="No Record Found" BorderStyle="None"
                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                                RowStyle-CssClass="rows">
                                                <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                <HeaderStyle BackColor="#e43c5c" Font-Bold="True" ForeColor="Black" />
                                                <AlternatingRowStyle BackColor="#FFB6C1" />

                                                <Columns>
                                                    <asp:BoundField DataField="CODE" HeaderText="Step"
                                                        SortExpression="CODE" />
                                                    <asp:BoundField DataField="irc_start_dt" HeaderText="Start Date"
                                                        SortExpression="irc_start_dt" />
                                                    <asp:BoundField DataField="irc_end_dt" HeaderText="End Date"
                                                        SortExpression="irc_end_dt" />
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
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade" id="YearSetting" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Year Setting</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="updPnl" runat="server">
                                        <ContentTemplate>
                                            <div class="row form-group">
                                                <div class="col-md-3">
                                                    <div style="margin-top: 8px;">
                                                        <asp:Label ID="Label3" runat="server" Text="Year" />
                                                    </div>
                                                </div>
                                                <div class="col-md-5">
                                                    <asp:DropDownList runat="server" ID="ddlYearSetting"
                                                        AutoPostBack="true" CssClass="form-control">
                                                        <asp:ListItem>--Select--</asp:ListItem>
                                                        <asp:ListItem>2021</asp:ListItem>
                                                        <asp:ListItem>2022</asp:ListItem>
                                                        <asp:ListItem>2023</asp:ListItem>
                                                        <asp:ListItem>2024</asp:ListItem>
                                                        <asp:ListItem>2025</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <div class="col-md-3">
                                                    <div style="margin-top: 8px;">
                                                        <asp:Label ID="Label4" runat="server" Text="Cycle" />
                                                    </div>
                                                </div>
                                                <div class="col-md-5">
                                                    <asp:TextBox ID="txtCycle" runat="server" Text="" Enabled="false"
                                                        CssClass="form-control" />
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="row" style="justify-content:center;">
                                    <asp:Button runat="server" ID="btnCycle" OnClick="btnCycle_Click" Text="Submit"
                                        class="btn-learn-more"></asp:Button>
                                </div>
                                <br />
                                <div class="row">
                                    <div style="align-content:center; align-items:center;margin-left: 50px;">
                                        <asp:GridView ID="gdvCycle" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                            Font-Names="verdana" EmptyDataText="No Record Found" BorderStyle="None"
                                            BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                            RowStyle-CssClass="rows">
                                            <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                            <HeaderStyle BackColor="#e43c5c" Font-Bold="True" ForeColor="Black" />
                                            <AlternatingRowStyle BackColor="#FFB6C1" />

                                            <Columns>
                                                <asp:BoundField DataField="Year" HeaderText="Year"
                                                    SortExpression="Year" />
                                                <asp:BoundField DataField="CycleTime" HeaderText="Cycle"
                                                    SortExpression="CycleTime" />
                                                <asp:BoundField DataField="irc_start_dt" HeaderText="Start Date"
                                                    SortExpression="irc_start_dt" Visible="false" />
                                                <asp:BoundField DataField="irc_end_dt" HeaderText="End Date"
                                                    SortExpression="irc_end_dt" Visible="false" />
                                            </Columns>

                                            <PagerStyle BackColor="#999999" ForeColor="Black"
                                                HorizontalAlign="Center" />
                                            <RowStyle BackColor="White" ForeColor="Black" />
                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                            <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                            <SortedDescendingHeaderStyle BackColor="#000065" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade  bd-example-modal-lg" id="ChangeBUHR" role="dialog">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Change BUHR</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="pnlChangeBUHR" runat="server">
                                        <ContentTemplate>
                                            <div class="row" id="pnlCngPnoScreen" runat="server">
                                                <div class="col-md-4 form-group">
                                                    <label>Assess Per. No. : &nbsp;<span
                                                            style="color :red;">*</span></label>
                                                    <asp:TextBox ID="txtCngAssessPerNo" runat="server" MaxLength="6"
                                                        Text="" ToolTip="Enter Assess Per. No."
                                                        placeholder="Assess Per. No." CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6"
                                                        runat="server" ErrorMessage="Please enter Assess Per. No."
                                                        Display="Dynamic" ControlToValidate="txtCngAssessPerNo"
                                                        SetFocusOnError="true" ForeColor="Red" ValidationGroup="A">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-8 form-group">
                                                    <div style="margin-top:23px;">
                                                        <asp:Button runat="server" ID="btnCngAdd" Text="Add"
                                                            OnClick="btnCngAdd_Click" class="btn btn-primary"
                                                            ValidationGroup="A"></asp:Button>&nbsp;&nbsp;
                                                        <asp:Button runat="server" ID="btnCngConfirmList"
                                                            Text="Confirm list" OnClick="btnCngConfirmList_Click"
                                                            class="btn btn-primary"></asp:Button>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" id="pnlBuhrChangeScreen" runat="server" visible="false">
                                                <div class="col-md-4 form-group">
                                                    <label>New BUHR Per. No. : &nbsp;<span
                                                            style="color :red;">*</span></label>
                                                    <asp:TextBox ID="txtCngBUHRPerno" runat="server" Text=""
                                                        MaxLength="6" CssClass="form-control"
                                                        ToolTip="Enter BUHR Per. No." placeholder="BUHR Per. No." />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                                        runat="server" ErrorMessage="Please enter New BUHR Per. No."
                                                        Display="Dynamic" ControlToValidate="txtCngBUHRPerno"
                                                        SetFocusOnError="true" ForeColor="Red" ValidationGroup="B">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-8 form-group">
                                                    <div style="margin-top:23px;">
                                                        <asp:Button runat="server" ID="btnCngSubmit" Text="Submit"
                                                            OnClick="btnCngSubmit_Click" ValidationGroup="B"
                                                            class="btn btn-primary"></asp:Button>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="container">
                                                    <div class="row">
                                                        <div class="col-md-12 col-lg-12">
                                                            <div class="table-responsive">
                                                                <asp:GridView ID="gdvChangeBUHR" runat="server"
                                                                    AutoGenerateColumns="False"
                                                                    CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                                    Font-Names="verdana" EmptyDataText="No Record Found"
                                                                    BackColor="#ffccff" BorderColor="Black"
                                                                    BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                                    GridLines="Vertical" RowStyle-CssClass="rows"
                                                                    HeaderStyle-Wrap="false">
                                                                    <FooterStyle BackColor="#CCCCCC"
                                                                        ForeColor="Black" />
                                                                    <HeaderStyle CssClass="bg-clouds segoe-light"
                                                                        BackColor="#FFB6C1" Font-Bold="True"
                                                                        ForeColor="Black" />
                                                                    <AlternatingRowStyle BackColor="#FFB6C1" />
                                                                    <Columns>
                                                                        <asp:BoundField HeaderText="Assess Per. No."
                                                                            DataField="ema_perno"
                                                                            SortExpression="ema_perno" />
                                                                        <asp:BoundField HeaderText="Assess Name"
                                                                            DataField="ema_ename"
                                                                            SortExpression="ema_ename" />
                                                                        <asp:BoundField HeaderText="BUHR Per.No."
                                                                            DataField="EMA_BHR_PNO"
                                                                            SortExpression="EMA_BHR_PNO" />
                                                                        <asp:BoundField HeaderText="BUHR Name"
                                                                            DataField="EMA_BHR_NAME"
                                                                            SortExpression="EMA_BHR_NAME" />
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
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="ChangeApproval" class="modal fade  bd-example-modal-lg" role="dialog">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Change Approver</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="pnlChangeApproval" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-4 col-sm-4 form-group">
                                                    <label>Assess Per. No. : &nbsp;<span
                                                            style="color :red;">*</span></label>
                                                    <asp:TextBox ID="txtCngAppAssesPerNo" runat="server" MaxLength="6"
                                                        Text="" CssClass="form-control" ToolTip="Enter Assess Per. No."
                                                        placeholder="Assess Per. No." AutoPostBack="true"
                                                        OnTextChanged="txtCngAppAssesPerNo_TextChanged" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                                        runat="server" ErrorMessage="Please enter Assess Per. No."
                                                        Display="Dynamic" ControlToValidate="txtCngAppAssesPerNo"
                                                        SetFocusOnError="true" ForeColor="Red" ValidationGroup="C">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-4 col-sm-4 form-group">
                                                    <label>Existing Approver Per. No. : &nbsp;<span
                                                            style="color :red;">*</span></label>
                                                    <asp:TextBox ID="txtCngExistingApprover" runat="server"
                                                        MaxLength="6" Text="" CssClass="form-control" Enabled="false"
                                                        ToolTip="Existing Approver Per. No."
                                                        placeholder="Existing Approver Per. No." />
                                                </div>
                                                <div class="col-md-4 col-sm-4 form-group">
                                                    <label>Approver Per. No. : &nbsp;<span
                                                            style="color :red;">*</span></label>
                                                    <asp:TextBox ID="txtCngAppPerNO" runat="server" MaxLength="6"
                                                        Text="" CssClass="form-control"
                                                        ToolTip="Enter Approver Per. No."
                                                        placeholder="Approver Per. No." />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3"
                                                        runat="server" ErrorMessage="Please enter Approver Per. No."
                                                        Display="Dynamic" ControlToValidate="txtCngAppPerNO"
                                                        SetFocusOnError="true" ForeColor="Red" ValidationGroup="C">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div style="margin-left:47%;">
                                                    <asp:Button runat="server" ID="btnChangeApproval" Text="Submit"
                                                        OnClick="btnChangeApproval_Click" ValidationGroup="C"
                                                        class="btn btn-primary"></asp:Button>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>


                    <!-- ======= Footer ======= -->
                    <footer id="footer">



                        <div class="container d-md-flex py-4">

                            <div class="mr-md-auto text-center">
                                <span>In case of any queries or issues, please reach out to your BUHR. </span>
                                <span>In case of any system specific queries or issues, please reach out to <b>IT
                                        helpdesk (it_helpdesk@tatasteel.com)</b></span>
                            </div>
                            <%-- <div class="mr-md-auto text-center text-md-left">
                                <div class="copyright">
                                    &copy; Copyright <strong><span>Tata Steel</span></strong>. All Rights Reserved
                                </div>
                        </div> --%>
                        </div>
                    </footer><!-- End Footer -->

                    <a href="#" class="back-to-top"><i class="ri-arrow-up-line"></i></a>

                    <!-- Vendor JS Files Commented Out -->
                    <%-- <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js"></script>
                        <script src="assets/vendor/jquery/jquery.min.js"></script>
                        <script src="assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script> --%>
                        <script src="assets/vendor/jquery.easing/jquery.easing.min.js"></script>
                        <script src="assets/vendor/php-email-form/validate.js"></script>
                        <script src="assets/vendor/isotope-layout/isotope.pkgd.min.js"></script>
                        <script src="assets/vendor/venobox/venobox.min.js"></script>
                        <script src="assets/vendor/owl.carousel/owl.carousel.min.js"></script>



                        <!-- Template Main JS File -->
                        <script src="assets/js/main.js"></script>
                        <%-- <script type="text/javascript" src="./assets/js/jquery-1.8.3.min"></script> --%>
                            <script type="text/javascript">
                                if (window.history.replaceState) {
                                    window.history.replaceState(null, null, window.location.href);
                                }
                                $(function () {
                                    $(document).keydown(function (e) {
                                        return (e.which || e.keyCode) != 116;
                                    });
                                });  
                            </script>
                </form>

                <%-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js">
                    </script>
                    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js">
                    </script> --%>
                    <script>
                        $(document).ready(function () {

                           <% --function setDatePicker() {
                                $("[id$=txt_datepicker]").datepicker({
                                    showOn: 'button',
                                    buttonImageOnly: true,
                                    buttonImage: 'calendar.gif'
                                });
                            }--%>        });

                        $(function () {
                            $("#txtStartDt").datepicker({ dateFormat: 'dd-mm-yy', showOn: 'button', buttonImageOnly: true, buttonImage: 'Images/calendar.gif' });
                        });

                        $(function () {
                            $("#txtCycleStartDt").datepicker({ dateFormat: 'dd-mm-yy', showOn: 'button', buttonImageOnly: true, buttonImage: 'Images/calendar.gif' });
                        });

                        $(function () {
                            $("#txtEndDt").datepicker({ dateFormat: 'dd-mm-yy', showOn: 'button', buttonImageOnly: true, buttonImage: 'Images/calendar.gif' });
                        });

                        $(function () {
                            $("#txtCycleEndDt").datepicker({ dateFormat: 'dd-mm-yy', showOn: 'button', buttonImageOnly: true, buttonImage: 'Images/calendar.gif' });
                        });

                        $('#txtStartDt').change(function () {
                            startDate = $(this).datepicker('getDate');
                            $("#txtEndDt").datepicker("option", "minDate", startDate);
                        })

                        $('#txtEndDt').change(function () {
                            endDate = $(this).datepicker('getDate');
                            $("#txtStartDt").datepicker("option", "maxDate", endDate);
                        })

                        $('#txtCycleStartDt').change(function () {
                            startDate = $(this).datepicker('getDate');
                            $("#txtCycleEndDt").datepicker("option", "minDate", startDate);
                        })

                        $('#txtCycleEndDt').change(function () {
                            endDate = $(this).datepicker('getDate');
                            $("#txtCycleStartDt").datepicker("option", "maxDate", endDate);
                        })
                        })
                    </script>
            </body>

        </html>