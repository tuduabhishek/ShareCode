<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SurveyApproval_OPR.aspx.vb" Inherits="SurveyApproval_OPR"
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
            <%--<meta content="width=device-width, initial-scale=5.0" name="viewport" />--%>
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

                <!-- Old Vendor CSS Files Commented Out -->
                <!-- <!-- <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"> --> -->
                <link href="assets/vendor/icofont/icofont.min.css" rel="stylesheet">
                <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
                <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">
                <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
                <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">
                <!-- <!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"> -->
                -->
                <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
                <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>
                <!-- <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script> -->
                <%--<link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet"
                    id="bootstrap-css">--%>
                    <!-- <!-- <script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script> -->
                    -->
                    <!-- <!-- <script src="//code.jquery.com/jquery-1.11.1.min.js"></script> --> -->
                    <%-- <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet"> --%>
                    <!-- <!-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script> -->
                    -->
                    <!-- Include all compiled plugins (below), or include individual files as needed -->
                    <!-- <!-- <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script> -->
                    -->


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

                        .AutoExtenderList {
                            border-bottom: dotted 1px #006699;
                            cursor: pointer;
                            color: Navy;
                            width: 230px !important;
                        }

                        .wrap-Text {
                            word-wrap: normal;
                            word-break: break-all;
                            width: 30px;
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

                                var headerCheckBox = inputList[i];

                                var checked = true;

                                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {

                                    if (!inputList[i].checked) {

                                        checked = false;

                                        break;

                                    }

                                }

                            }

                            headerCheckBox.checked = checked;
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

                        function setCharacters(e) {
                            var count = document.getElementById('<%=txtremarks.ClientID%>');
                            var lblcount = document.getElementById('<%=lblCountChar.ClientID %>');
                            var total = parseInt(count.value.length);
                            lblcount.innerHTML = 'You have entered ' + total + ' characters out of 100.';
                            return false;
                        }


                        function setLength(e) {
                            var count = document.getElementById('<%=txtremarks.ClientID%>');
                            var total = parseInt(count.value.length);
                            if (total > 99) {
                                if (window.event) {
                                    window.event.returnValue = false;
                                }
                                else {
                                    if (e.which > 31) {
                                        e.preventDefault();
                                    }
                                }
                                //return false;
                            }
                            else {
                                return true;
                            }
                        }
                        function maxLengthPaste(field, maxChars) {
                            event.returnValue = false;
                            if ((field.value.length + window.clipboardData.getData("Text").length) > maxChars) {
                                alert("more than " + maxChars + " chars");
                                return false;
                            }
                            event.returnValue = true;
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

                        <h1 class="logo mr-auto" id="headings"><a>360 DEGREE FEEDBACK SURVEY</a></h1>
                        <!-- Uncomment below if you prefer to use an image logo -->
                        <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->

                        <nav class="nav-menu d-none d-lg-block">
                            <ul>
                                <li class="active"><a href="SurveyApproval_OPR.aspx">Home</a></li>
                                <li><a href="Images/User_mannual_360_Degree_Approval_of_Respondents.pdf"
                                        target="_blank">Help</a></li>
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
                                <asp:Label ID="lblname" runat="server"></asp:Label>
                            </strong></h3>
                        <!--<h1>We're Creative Agency</h1>-->
                        <h2 class="table-responsive1">As we aspire to be the most valuable and respected steel company
                            globally in the coming years, we are developing agile behaviours across the organization. We
                            will measure this as an integral part of our Performance Management System for all the
                            officers through a 360 degree feedback survey</h2>
                        <a href="#about" class="btn-get-started scrollto">APPROVE SURVEY</a>
                    </div>
                </section><!-- End Hero -->

                <main id="main">

                    <!-- ======= About Section ======= -->
                    <section id="about" class="about">
                        <div class="container">

                            <div class="section-title">
                                <h3>Approve<span> Survey</span></h3>
                            </div>

                            <div class="row content">
                                <div class="col-md-12 col-lg-12" style="text-align:justify;">
                                    <b>
                                        <asp:Label runat="server" id="lbl_hdrmsg"
                                            Text="You are urged to exercise due care while approving the form of the Officer. The Officer should have relevant working relationship with the respondents they have chosen in different respondent category for most valid inputs. Please note that changes in the respondent list should be made only after having a discussion with the assessee."
                                            style="font-size:14px;font:bold" Class="text-danger"></asp:Label>
                                    </b>
                                </div>
                                <%--Added by TCS on 09122022 to add information text--%>
                                    <div class="col-md-12 col-lg-12"
                                        style="text-align:justify; margin:5px 0px 5px 0px;">
                                        <b>
                                            <asp:Label runat="server" id="lblInfo"
                                                Text="Please approve within the set timeline else it would be auto approved. To exercise utmost caution in approving most relevant and critical stakeholders only. Please submit within - "
                                                style="font-size:14px;font:bold" Class="text-info"><span
                                                    id="spnLastapprovaldate" runat="server"
                                                    style="font-size:14px;font:bold" Class="badge badge-dark"></span>
                                            </asp:Label>
                                        </b>
                                    </div>
                                    <%--End--%>
                                        <div class="col-md-12 col-lg-12">
                                            <div class="table-responsive">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvself" runat="server"
                                                            AutoGenerateColumns="False"
                                                            CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                            Font-Names="verdana" EmptyDataText="No Record Found"
                                                            BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                            GridLines="Vertical" RowStyle-CssClass="rows"
                                                            OnRowDataBound="gvself_RowDataBound">
                                                            <%-- <FooterStyle BackColor="#CCCCCC"
                                                                ForeColor="Black" />--%>
                                                            <HeaderStyle BackColor="#e43c5c" Font-Bold="True"
                                                                ForeColor="Black" />
                                                            <AlternatingRowStyle BackColor="#FFB6C1" />

                                                            <Columns>

                                                                <asp:TemplateField HeaderText="P.no">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblpno" runat="server"
                                                                            Text='<%# Eval("SS_ASSES_PNO")%>'>
                                                                        </asp:Label>

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lnlname" runat="server"
                                                                            Text='<%# Eval("ema_ename")%>'></asp:Label>

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Designation">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllevel" runat="server"
                                                                            Text='<%# Eval("ema_desgn_desc")%>'>
                                                                        </asp:Label>

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Status">
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

                                                    </ContentTemplate>

                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                            </div>
                            <asp:UpdatePanel ID="uppanelbtn" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-striped table-hover table-responsive table-bordered dataTable no-footer"
                                                Font-Names="verdana" EmptyDataText="No Record Found" BorderStyle="None"
                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                                RowStyle-CssClass="rows">
                                                <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                <HeaderStyle BackColor="#e43c5c" Font-Bold="True" ForeColor="Black" />
                                                <AlternatingRowStyle BackColor="#FFB6C1" />

                                                <Columns>
                                                    <%-- <asp:BoundField DataField="Label"
                                                        HeaderText="Assessee Level" />--%>
                                                    <asp:BoundField DataField="irc_desc"
                                                        HeaderText="Respondent Category" />
                                                    <asp:BoundField DataField="minmum" HeaderText="Minimum" />
                                                    <asp:BoundField DataField="maximum" HeaderText="Maximum" />


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
                                    <div class="section-title" runat="server" id="divtitle" visible="false">
                                        <h3>Respondent Selected List for<span><strong>
                                                    <asp:Label runat="server" ID="lblassname"></asp:Label>
                                                </strong> </span></h3>
                                    </div>


                                    <div class="row content">
                                        <div class="col-md-3">


                                            <asp:Button runat="server" ID="btnaddpeertsl" Text="Add Tata Steel"
                                                class="btn-learn-more" OnClick="btnaddpeertsl_Click" Visible="false">
                                            </asp:Button>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Button runat="server" ID="btnaddnontsl" Text="Add Non Tata Steel"
                                                class="btn-learn-more" OnClick="btnaddnontsl_Click" Visible="false">
                                            </asp:Button>
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
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1"
                                                                        runat="server" TargetControlID="txtpnoI"
                                                                        ServiceMethod="SearchPrefixesForApprover"
                                                                        MinimumPrefixLength="1" CompletionInterval="100"
                                                                        DelimiterCharacters="" Enabled="True"
                                                                        ServicePath=""
                                                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                        CompletionListCssClass="AutoExtender"
                                                                        CompletionListItemCssClass="AutoExtenderList">
                                                                    </cc1:AutoCompleteExtender>
                                                                    <cc1:FilteredTextBoxExtender runat="server"
                                                                        ID="FilteredTextBoxExtender2"
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
                                                                    <asp:TextBox runat="server" ID="txtdesgI"
                                                                        CssClass="form-control"
                                                                        placeholder="Designation" Enabled="false" />
                                                                    <%-- <asp:HiddenField runat="server" ID="hdfnin" />
                                                                    --%>
                                                                    <asp:Label runat="server" ID="lbldesg1"
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
                                                                        ID="RegularExpressionValidator2" runat="server"
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
                                                                        CssClass="form-control " placeholder="Org Name"
                                                                        Enabled="false" />

                                                                </div>
                                                            </div>
                                                        </div>


                                                    </div><br />
                                                    <div class="row">
                                                        <br />
                                                        <div class="col-md-3">
                                                            <div class="form-group">

                                                                <div class="col-md-12">
                                                                    <asp:DropDownList ID="DropDownList1" runat="server"
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
                                                                        ID="RegularExpressionValidator1" runat="server"
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
                                                                        CssClass="form-control " placeholder="Org Name"
                                                                        MaxLength="50" />

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
                            </asp:UpdatePanel>

                            <div class="row">
                                <div class="col-md-12 col-lg-12">
                                    <div class="table-responsive">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvfinal" runat="server" AutoGenerateColumns="False"
                                                    Visible="false"
                                                    CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                    Font-Names="verdana" EmptyDataText="No Record Found"
                                                    BackColor="#ffccff" BorderColor="Black" BorderStyle="None"
                                                    BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                                    RowStyle-CssClass="rows">
                                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                    <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#FFB6C1"
                                                        Font-Bold="True" ForeColor="Black" />
                                                    <AlternatingRowStyle BackColor="#FFB6C1" />
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="P.no">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblpno" runat="server"
                                                                    Text='<%# Eval("SS_PNO")%>'></asp:Label>
                                                                <%--<asp:HiddenField runat="server"
                                                                    Value='<%# Eval("SS_ASSES_PNO") %>'
                                                                    ID="hdfnperno" />--%>
                                                                <asp:Label runat="server" ID="lblassess" Visible="false"
                                                                    Text='<%# Eval("SS_ASSES_PNO")%>'></asp:Label>
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
                                                                    Text='<%# Eval("SS_EMAIL")%>' CssClass="wrap-Text">
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Category">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblcategory" runat="server"
                                                                    Text='<%# Eval("Category")%>'></asp:Label>
                                                                <%--<asp:HiddenField runat="server" ID="hdfncateg"
                                                                    Value='<%# Eval("SS_CATEG") %>' />--%>
                                                                <asp:Label runat="server" ID="lblcatrg" Visible="false"
                                                                    Text='<%# Eval("SS_CATEG")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Select All">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox runat="server" ID="checkAll"
                                                                    onclick="checkAll(this);" visible="false" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkmgr" runat="server"
                                                                    onclick="Check_Click(this)"
                                                                    OnCheckedChanged="chkmgr_CheckedChanged"
                                                                    AutoPostBack="true" Checked="true" />
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

                                <div class="col-md-12 col-lg-12">
                                    <div class="table-responsive">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                            <ContentTemplate>
                                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                                    Visible="false"
                                                    CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                    Font-Names="verdana" EmptyDataText="No Record Found"
                                                    BackColor="#ffccff" BorderColor="Black" BorderStyle="None"
                                                    BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                                    RowStyle-CssClass="rows">
                                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                    <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#FFB6C1"
                                                        Font-Bold="True" ForeColor="Black" />
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
                                        <div class="col-lg-6">
                                            <asp:TextBox runat="server" ID="txtremarks" CssClass="form-control"
                                                TextMode="MultiLine" Rows="2"
                                                placeholder="Enter remarks for sending back"
                                                onkeypress="return setLength(event);"
                                                onkeyup="return setCharacters(event);"
                                                onpaste='return maxLengthPaste(this,"100");' Visible="false">
                                            </asp:TextBox>
                                            <asp:Label ID="lblCountChar" Text="" runat="server" />


                                        </div>
                                        <div class="col-lg-3">
                                            <asp:Button runat="server" ID="btnrej" Text="Send Back To Officer"
                                                class="btn-learn-more" Visible="false" data-bs-toggle="modal"
                                                data-bs-target="#staticBackdropRej"></asp:Button>
                                        </div>
                                        <div class="col-lg-2">
                                            <asp:Button runat="server" ID="lbOrg" Text="Approve" class="btn-learn-more"
                                                Visible="false" data-bs-toggle="modal" data-bs-target="#staticBackdrop">
                                            </asp:Button>
                                        </div>
                                    </div>
                                    <%--< /div>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel ID="updtpnlCollab" runat="server">
                                <ContentTemplate>
                                    <%-- <div class="section-title" runat="server" id="divtitleCollab" visible="false">
                                        <h3>Respondent Not Selected Collabarotr List<span><strong> </strong> </span>
                                        </h3>
                        </div>--%>
                        <div class="row content mt-5 mb-3" runat="server" id="divtitleCollab" visible="false">
                            <div class="col-md-12 col-lg-12" style="text-align:justify; margin:5px 0px 5px 0px;">
                                <b>
                                    <asp:Label runat="server" id="lblCollabTitle"
                                        Text="List of Collaborators who have not been selected as Respondent. "
                                        style="font-size:14px;font:bold" Class="text-info"></asp:Label>
                                </b><br /><br />
                                <b>
                                    <asp:Label runat="server" id="lblCollabDesc"
                                        Text="This is the indicative list of those individuals who were added as Collaborators by the Officer during UpNext process but are excluded from the respondent list above. You may discuss with Officer the reason of exclusion. Please note, any changes in the respondents list should be made only after having a discussion with the Officer."
                                        style="font-size:14px;font:bold" Class="text-danger"></asp:Label>
                                </b>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 col-lg-12">
                                <div class="table-responsive">
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False"
                                                Visible="false"
                                                CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                Font-Names="verdana" EmptyDataText="No Record Found" BackColor="#ffccff"
                                                BorderColor="Black" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                GridLines="Vertical" RowStyle-CssClass="rows">
                                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#FFB6C1"
                                                    Font-Bold="True" ForeColor="Black" />
                                                <AlternatingRowStyle BackColor="#FFB6C1" />
                                                <Columns>

                                                    <asp:TemplateField HeaderText="P.no">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblpno" runat="server"
                                                                Text='<%# Eval("SS_PNO")%>'></asp:Label>
                                                            <%--<asp:HiddenField runat="server"
                                                                Value='<%# Eval("SS_ASSES_PNO") %>'
                                                                ID="hdfnperno" />--%>
                                                            <asp:Label runat="server" ID="lblassess" Visible="false"
                                                                Text='<%# Eval("SS_ASSES_PNO")%>'></asp:Label>
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
                                                                Text='<%# Eval("SS_EMAIL")%>' CssClass="wrap-Text">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Category">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcategory" runat="server"
                                                                Text='<%# Eval("Category")%>'></asp:Label>
                                                            <%--<asp:HiddenField runat="server" ID="hdfncateg"
                                                                Value='<%# Eval("SS_CATEG") %>' />--%>
                                                            <asp:Label runat="server" ID="lblcatrg" Visible="false"
                                                                Text='<%# Eval("SS_CATEG")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Select All">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox runat="server" ID="checkAll"
                                                                onclick="checkAll(this);" visible="false" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkmgr" runat="server"
                                                                onclick="Check_Click(this)"
                                                                OnCheckedChanged="chkmgr_CheckedChanged"
                                                                AutoPostBack="true" Checked="true" />
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

                            <div class="col-md-12 col-lg-12">
                                <div class="table-responsive">

                                    <asp:GridView ID="gvCollaborator" runat="server" AutoGenerateColumns="False"
                                        Visible="false"
                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                        Font-Names="verdana" EmptyDataText="No Record Found" BackColor="#ffccff"
                                        BorderColor="Black" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        GridLines="Vertical" RowStyle-CssClass="rows">
                                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                        <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#FFB6C1"
                                            Font-Bold="True" ForeColor="Black" />
                                        <AlternatingRowStyle BackColor="#FFB6C1" />
                                        <Columns>

                                            <asp:TemplateField HeaderText="P.no">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpno" runat="server"
                                                        Text='<%# Eval("EC_COLLABORATOR_PERNO")%>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lnlname" runat="server"
                                                        Text='<%# Eval("EMA_ENAME")%>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Level">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbllevel" runat="server"
                                                        Text='<%# Eval("EMA_EQV_LEVEL")%>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Designation">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldesg" runat="server"
                                                        Text='<%# Eval("EMA_DESGN_DESC")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Department">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldept" runat="server"
                                                        Text='<%# Eval("EMA_DEPT_DESC")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email Id">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblemail" runat="server"
                                                        Text='<%# Eval("EMA_EMAIL_ID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                        </Columns>
                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
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
                        </ContentTemplate>
                        </asp:UpdatePanel>

                        </div>

                        <div class="modal fade" id="staticBackdropRej" data-bs-backdrop="static"
                            data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel"
                            aria-hidden="true">
                            <div class="modal-dialog">
                                <div class="modal-content">

                                    <div class="modal-body">
                                        Are you sure you want to Send Back To Officer?
                                    </div>
                                    <div class="modal-footer">

                                        <asp:Button runat="server" ID="Button1" OnClick="Reject" Text="Yes"
                                            class="btn btn-primary" />
                                        <button type="button" class="btn btn-secondary"
                                            data-bs-dismiss="modal">No</button>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal fade" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false"
                            tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
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

                <!-- ======= Footer ======= -->
                <footer id="footer">



                    <div class="container d-md-flex py-4">

                        <div class="mr-md-auto text-center">
                            <span>In case of any queries or issues, please reach out to your HRBP. </span>
                            <span>In case of any system specific queries or IT issues, please reach out to<b> IT
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
                <!-- <script src="assets/vendor/jquery/jquery.min.js"></script> -->
                <!-- <script src="assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script> -->
                <script src="assets/vendor/jquery.easing/jquery.easing.min.js"></script>
                <script src="assets/vendor/php-email-form/validate.js"></script>
                <script src="assets/vendor/isotope-layout/isotope.pkgd.min.js"></script>
                <script src="assets/vendor/venobox/venobox.min.js"></script>
                <script src="assets/vendor/owl.carousel/owl.carousel.min.js"></script>

                <!-- Template Main JS File -->
                <script src="assets/js/main.js"></script>
                <!-- <script type="text/javascript"
                    src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script> -->
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
        </body>

        </html>