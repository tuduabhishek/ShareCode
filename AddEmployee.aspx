<%@ Page Title="" Language="VB" MasterPageFile="~/AdminMaster.master" AutoEventWireup="false"
    CodeFile="AddEmployee.aspx.vb" Inherits="AddEmployee" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
            <!-- Google Fonts -->
            <link href="assets/css/googlefont.css" rel="stylesheet" />

            <!-- Vendor CSS Files -->
            <!-- Vendor CSS Files Commented Out (Redundant with Master Page) -->
            <!-- <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"> -->
            <!-- <link href="./assets/vendor/icofont/icofont.min.css" rel="stylesheet"> -->
            <!-- <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet"> -->
            <%-- Start WI368 by Manoj Kumar on 30-05-2021--%>
                <!-- <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet"> -->
                <%-- WI368 one line added --%>
                    <%-- End by Manoj Kumar on 30-05-2021--%>
                        <!-- <link href="assets/vendor/venobox/venobox.css" rel="stylesheet"> -->
                        <!-- <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet"> -->
                        <!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"> -->
                        <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
                        <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>

                        <!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css"> -->

                        <!-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script> -->
                        <!-- <script src="https://code.jquery.com/ui/1.10.4/jquery-ui.js"></script> -->
                        <!-- <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script> -->

                        <%--<link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css"
                            rel="stylesheet" id="bootstrap-css">--%>
                            <!-- <script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script> -->
                            <%--<!-- <script src="//code.jquery.com/jquery-1.11.1.min.js"></script> -->--%>
                                <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css"
                                    rel="stylesheet">
                                <!-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script> -->
                                <!-- Include all compiled plugins (below), or include individual files as needed -->
                                <!-- <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script> -->
                                <!-- <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script> -->

                                <!-- Template Main CSS File -->
                                <link href="assets/css/styleIL3.css" rel="stylesheet">

                                <!-- <link href='https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/ui-lightness/jquery-ui.css'
        rel='stylesheet'> -->

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
        </asp:Content>
        <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMain" runat="server">
                <ContentTemplate>
                    <div class="container-fluid">
                        <div class="row form-group p-2">
                            <div class="col-md-3 text-left">
                                <asp:LinkButton ID="lbtnNew" runat="server" CssClass="btn btn-primary btn-md mt-4"><i
                                        class="fa fa-plus-square-o"></i>&nbsp;New</asp:LinkButton>&nbsp;&nbsp;
                                <asp:LinkButton ID="lbtnEdit" runat="server" CssClass="btn btn-secondary btn-md mt-4"><i
                                        class="fa fa-edit"></i>&nbsp;Edit</asp:LinkButton>
                            </div>
                            <div class="col-md-3 mt-4  ui-widget">
                                <asp:TextBox ID="txtSearch" runat="server" Visible="false"
                                    CssClass="form-control automplete2" placeholder="Search Per. No. or Name"
                                    AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
                            </div>
                            <div class="col-md-1 text-left">
                                <label class="font-weight-bold">Year<span class="text-danger">*</span></label>
                                <asp:TextBox runat="server" ID="txtYear" Enabled="false" CssClass="form-control" />
                            </div>
                            <div class="col-md-1 text-left">
                                <label class="font-weight-bold">Cycle<span class="text-danger">*</span></label>
                                <asp:TextBox runat="server" ID="txtCycle" Enabled="false" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="row form-group p-1">
                            <div class="col-md-3 text-left">
                                <label class="font-weight-bold">Per. no.<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtPerno" runat="server" CssClass="form-control automplete2"
                                    MaxLength="6" AutoPostBack="true" OnTextChanged="txtPerno_TextChanged">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="usrnoReqFldVal" runat="server" ValidationGroup="exp"
                                    autocomplete="off" ErrorMessage="Please enter Per. No." ControlToValidate="txtPerno"
                                    CssClass="errtxt" Display="Dynamic" ForeColor="#CC0066" SetFocusOnError="True">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-3 text-left">
                                <label class="font-weight-bold">Name<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtSelfName" runat="server" CssClass="form-control" MaxLength="80">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ValidationGroup="exp" autocomplete="off" ErrorMessage="Please enter Name"
                                    ControlToValidate="txtSelfName" CssClass="errtxt" Display="Dynamic"
                                    ForeColor="#CC0066" SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-3 text-left">
                                <label class="font-weight-bold">Designation<span class="text-danger">*</span></label>
                                <%-- <div class="input-group date datepickerSelect" data-date-format="dd-M-yyyy">
                                    <asp:TextBox ID="txtSelfDob" runat="server" Enabled="false"
                                        CssClass="input-group-addon form-control"></asp:TextBox><span
                                        class="input-group-addon ml-1 mt-2"><i class="fa fa-calendar"></i></span>
                            </div>
                            <div class='input-group date' id='datetimepicker1'>
                                <input type='text' class="form-control" />
                                <span class="input-group-addon">
                                    <span class="fa fa-calendar"></span>
                                </span>
                            </div>--%>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                ValidationGroup="exp" autocomplete="off" ErrorMessage="Please enter date of birth"
                                ControlToValidate="txtSelfDob" CssClass="errtxt" Display="Dynamic" ForeColor="#CC0066"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                <asp:DropDownList CssClass="form-control" ID="ddlDesg" runat="server">
                                </asp:DropDownList>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Email-Id<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form-group p-1">
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Equivalent Level<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtEquLvl" runat="server" Enabled="false" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Contact No<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Subarea<span class="text-danger">*</span></label>
                            <asp:DropDownList CssClass="form-control" ID="ddlSubarea" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Reporting to<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtReportingTo" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form-group p-1">
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">BUHR No.<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtBuhrNo" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">BUHR Name<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtBuhrNm" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Joining Date<span
                                    class="text-danger">&nbsp;(dd-MMM-yyyy)</span></label>
                            <asp:TextBox ID="txtJoiningDt" runat="server" Enabled="false" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Department<span class="text-danger">*</span></label>
                            <asp:DropDownList CssClass="form-control" ID="ddlDepartment" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row form-group p-1">
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Employee Sgrade<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtEmpSgrade" runat="server" Enabled="false" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Employee Class<span class="text-danger">*</span></label>
                            <asp:TextBox ID="TxtEmpClass" runat="server" Enabled="false" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Dotted Per. No.<span
                                    class="text-danger">&nbsp;(dd-MMM-yyyy)</span></label>
                            <asp:TextBox ID="txtDotted" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Executive Head<span class="text-danger">*</span></label>
                            <asp:DropDownList CssClass="form-control" ID="ddlExecHead" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row form-group p-1">
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Employee Pers Executive Per. No.<span
                                    class="text-danger">*</span></label>
                            <asp:TextBox ID="txtEmpPersExec" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Assessor Start Date<span
                                    class="text-danger">&nbsp;(dd-MMM-yyyy)</span></label>
                            <asp:TextBox ID="txtStep1StDt" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Assessor End Date<span
                                    class="text-danger">&nbsp;(dd-MMM-yyyy)</span></label>
                            <asp:TextBox ID="txtStep1EndDt" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Approver Start Date<span
                                    class="text-danger">&nbsp;(dd-MMM-yyyy)</span></label>
                            <asp:TextBox ID="txtStep2StDt" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form-group p-1">
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Approver End Date<span
                                    class="text-danger">&nbsp;(dd-MMM-yyyy)</span></label>
                            <asp:TextBox ID="txtStep2EndDt" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Survey Start Date<span
                                    class="text-danger">&nbsp;(dd-MMM-yyyy)</span></label>
                            <asp:TextBox ID="txtStep3StDt" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Survey End Date<span
                                    class="text-danger">&nbsp;(dd-MMM-yyyy)</span></label>
                            <asp:TextBox ID="txtStep3EndDt" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3 text-left">
                            <%--<label class="font-weight-bold">Approver Start Date<span
                                    class="text-danger">*</span></label>
                                <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control"></asp:TextBox>--%>
                        </div>
                    </div>
                    <hr class="solid d-none" />
                    <div id="pnlNext" runat="server" class="flex">
                        <asp:LinkButton ID="lbtnUpdate" runat="server" ValidationGroup="exp" Visible="false"
                            CssClass="btn btn-success btn-lg p-2 m-2"><i class="fa fa-edit"></i>&nbsp;Update
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnSubmit" runat="server" ValidationGroup="exp"
                            CssClass="btn btn-success btn-lg p-2 m-2"><i class="fa fa-save"></i>&nbsp;Submit
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnRefresh" runat="server" CssClass="btn btn-warning btn-lg p-2 m-2">
                            Refresh&nbsp;<i class="fa fa-refresh"></i></asp:LinkButton>
                    </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- <script src = "https://code.jquery.com/jquery-1.10.2.js"></script> -->
            <!-- <script src = "https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script> -->
            <script type="text/javascript">
                function pageLoad(sender, args) {
                    loadAutocomplete();
                };

                function loadAutocomplete() {

                    $(".automplete2").autocomplete({
                        //source: availableTutorials,
                        //autoFocus:true
                        source: function (request, response) {
                            $.ajax({
                                url: "AddEmployee.aspx/GetCustomers",
                                data: "{ 'prefix': '" + request.term + "'}",
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    response($.map(data.d, function (item) {
                                        return {
                                            label: item.split('-')[0],
                                            val: item.split('-')[1]
                                        }
                                    }))
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }
                    });
                };
                function numericOnly(e) {

                    var val = e.value.replace(/[^\d]/g, "");
                    if (val != e.value)
                        e.value = val;

                };

                function validEmail(e) {
                    var validRegex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

                    if (!e.value.match(validRegex)) {
                        swal({
                            title: 'Invalid mail',
                            text: 'Please enter valid mail id',
                            icon: 'info'
                            // type: btn_type
                        });
                    }
                };

                //$(function () {
                //    $('#datetimepicker1').datetimepicker();
                //});
            </script>
        </asp:Content>