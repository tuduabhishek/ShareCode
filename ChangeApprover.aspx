<%@ Page Title="" Language="VB" MasterPageFile="~/AdminMaster.master" AutoEventWireup="false" CodeFile="ChangeApprover.aspx.vb" Inherits="ChangeApprover" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
    <!-- Google Fonts -->
    <link href="assets/css/googlefont.css" rel="stylesheet" />

    <!-- Vendor CSS Files -->
    <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="./assets/vendor/icofont/icofont.min.css" rel="stylesheet">
    <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
    <%-- Start WI368  by Manoj Kumar on 30-05-2021--%>
    <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">
    <%-- WI368 one line added --%>
    <%-- End  by Manoj Kumar on 30-05-2021--%>
    <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
    <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
    <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

    <%--<link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">--%>
    <script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script>
    <%--<script src="//code.jquery.com/jquery-1.11.1.min.js"></script>--%>
    <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>

    <!-- Template Main CSS File -->
    <link href="assets/css/styleIL3.css" rel="stylesheet">

    <link href='https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/ui-lightness/jquery-ui.css'
        rel='stylesheet'>

    <script src="//code.jquery.com/jquery-1.11.0.min.js"></script>


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

        .dropdown-menu > li > a {
            background-color: #fff !important;
        }

            .dropdown-menu > li > a:hover {
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMain" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="panel-body">
                    <div class="row form-group p-1">
                        <div class="col-md-1 text-left">
                            <label class="font-weight-bold">Year<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txtYear" Enabled="false" CssClass="form-control" />
                        </div>
                        <div class="col-md-1 text-left">
                            <label class="font-weight-bold">Cycle<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txtCycle" Enabled="false" CssClass="form-control" />
                        </div>
                        <div class="col-md-3 col-sm-4 form-group">
                            <label>Assessee Per. No. : &nbsp;<span style="color: red;">*</span></label>
                            <asp:TextBox ID="txtCngAppAssesPerNo" runat="server" MaxLength="6" Text="" CssClass="form-control" ToolTip="Enter Assess Per. No." placeholder="Assess Per. No." AutoPostBack="true" OnTextChanged="txtCngAppAssesPerNo_TextChanged" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter Assess Per. No." Display="Dynamic" ControlToValidate="txtCngAppAssesPerNo" SetFocusOnError="true" ForeColor="Red" ValidationGroup="C"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-3 col-sm-4 form-group">
                            <label>Existing Approver Per. No. : &nbsp;<span style="color: red;">*</span></label>
                            <asp:TextBox ID="txtCngExistingApprover" runat="server" MaxLength="6" Text="" CssClass="form-control" Enabled="false" ToolTip="Existing Approver Per. No." placeholder="Existing Approver Per. No." />
                        </div>
                        <div class="col-md-3 col-sm-4 form-group">
                            <label>Approver Per. No. : &nbsp;<span style="color: red;">*</span></label>
                            <asp:TextBox ID="txtCngAppPerNO" runat="server" MaxLength="6" Text="" CssClass="form-control" ToolTip="Enter Approver Per. No." placeholder="Approver Per. No." />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter Approver Per. No." Display="Dynamic" ControlToValidate="txtCngAppPerNO" SetFocusOnError="true" ForeColor="Red" ValidationGroup="C"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-1 form-group">
                            <div style="margin-top: 23px;">
                                <asp:Button runat="server" ID="btnChangeApproval" Text="Submit" OnClick="btnChangeApproval_Click" ValidationGroup="C" class ="btn btn-primary"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

