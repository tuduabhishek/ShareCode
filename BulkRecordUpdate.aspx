<%@ Page Title="" Language="VB" MasterPageFile="~/AdminMaster.master" AutoEventWireup="false"
    CodeFile="BulkRecordUpdate.aspx.vb" Inherits="BulkRecordUpdate" MaintainScrollPositionOnPostback="true" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
            <!-- Google Fonts -->
            <link href="assets/css/googlefont.css" rel="stylesheet" />

            <!-- New Library Versions -->
            <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
            <link href="https://code.jquery.com/ui/1.14.2/themes/ui-lightness/jquery-ui.css" rel="stylesheet">

            <script src="https://code.jquery.com/jquery-4.0.0-beta.2.min.js"></script>
            <script src="https://code.jquery.com/ui/1.14.2/jquery-ui.min.js"></script>
            <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
            <%-- <link href="https://netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
                rel="stylesheet"> --%>

                <!-- Old Vendor CSS Files Commented Out -->
                <!-- <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"> -->
                <link href="./assets/vendor/icofont/icofont.min.css" rel="stylesheet">
                <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
                <%-- Start WI368 by Manoj Kumar on 30-05-2021--%>
                    <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">
                    <%-- WI368 one line added --%>
                        <%-- End by Manoj Kumar on 30-05-2021--%>
                            <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
                            <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">
                            <!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"> -->
                            <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
                            <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>
                            <link href="styles/GridviewScroll.css" rel="stylesheet" />
                            <!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css"> -->

                            <!-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script> -->
                            <!-- <script src="https://code.jquery.com/ui/1.10.4/jquery-ui.js"></script> -->
                            <!-- <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script> -->

                            <%--<!-- <link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css"
                                rel="stylesheet" id="bootstrap-css"> -->--%>
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

                                    <!-- <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js" type="text/javascript"></script> -->
                                    <style type="text/css">
                                        .ui-datepicker-trigger {
                                            padding: 0px;
                                            padding-left: 5px;
                                            vertical-align: baseline;
                                            position: relative;
                                            top: 3px !important;
                                            height: 85%;
                                        }

                                        /*.modal-dialog,
.modal-content {
    height: 95%;
}*/

                                        /*.modal-body {
    max-height: calc(100vh - 210px);  overflow-y: auto;
}*/
                                        #mdialog,
                                        #mcontent {
                                            height: 95%;
                                        }

                                        #mbody {
                                            max-height: calc(100vh - 210px);
                                            overflow-y: auto;
                                        }

                                        .ui-datepicker .ui-datepicker-title select {
                                            font-size: .8em !important;
                                        }

                                        .ui-datepicker table {
                                            font-size: .8em !important;
                                        }

                                        .ui-datepicker {
                                            width: 14em !important;
                                            /*what ever width you want*/
                                        }

                                        .fa-edit:hover {
                                            color: red;
                                            transition: 0.7s;
                                        }

                                        body {
                                            padding-right: 0 !important;
                                        }

                                        .info-msg,
                                        .success-msg,
                                        .error-msg {
                                            margin: 10px 0;
                                            padding: 10px;
                                            border-radius: 3px 3px 3px 3px;
                                        }

                                        .info-msg {
                                            color: #059;
                                            background-color: #BEF;
                                        }

                                        .success-msg {
                                            color: #270;
                                            background-color: #DFF2BF;
                                        }

                                        .error-msg {
                                            color: #D8000C;
                                            background-color: #FFBABA;
                                        }

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

                                        /*.ui-datepicker-trigger {
            padding: 0px;
            padding-left: 5px;
            vertical-align: baseline;
            position: relative;
            top: -27px;
            height: 22px;
        }*/

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
                                        function pageLoad() {

                                            datePicker();
                                        }
                                        $(document).ready(function () {

                                            datePicker();
                                        });
                                        function removeBackdrop() {
                                            //$('body').removeClass('modal-open');
                                            $('.modal-backdrop').remove();
                                        }
                                        function datePicker() {
                                            $('#<%=txtStep1SD.ClientID %>').datepicker({
                                                changeMonth: true,
                                                changeYear: true,
                                                dateFormat: "dd-M-yy",
                                                showOn: 'button',
                                                buttonImageOnly: true,
                                                buttonImage: 'Images/calendar.gif',
                                                buttonText: "Select date",
                                                beforeShow: function (textbox, instance) {
                                                    var scrollTop = $(window).scrollTop();
                                                    var txtBoxOffset = $(this).offset();
                                                    var top = txtBoxOffset.top - scrollTop;
                                                    var left = txtBoxOffset.left;
                                                    var textBoxHeight = $(this).outerHeight();
                                                    setTimeout(function () {
                                                        instance.dpDiv.css({
                                                            top: top - $("#ui-datepicker-div").outerHeight(),
                                                            left: left
                                                        });
                                                    }, 0);
                                                }
                                                //yearRange: "c-100:c"
                                            });
                                            $('#<%=txtStep1ED.ClientID %>').datepicker({
                                                changeMonth: true,
                                                changeYear: true,
                                                dateFormat: "dd-M-yy",
                                                showOn: 'button',
                                                buttonImageOnly: true,
                                                buttonImage: 'Images/calendar.gif',
                                                buttonText: "Select date",
                                                beforeShow: function (textbox, instance) {
                                                    var scrollTop = $(window).scrollTop();
                                                    var txtBoxOffset = $(this).offset();
                                                    var top = txtBoxOffset.top - scrollTop;
                                                    var left = txtBoxOffset.left;
                                                    var textBoxHeight = $(this).outerHeight();
                                                    setTimeout(function () {
                                                        instance.dpDiv.css({
                                                            top: top - $("#ui-datepicker-div").outerHeight(),
                                                            left: left
                                                        });
                                                    }, 0);
                                                }
                                                //yearRange: "c-100:c"
                                            });
                                            $('#<%=txtStep2SD.ClientID %>').datepicker({
                                                changeMonth: true,
                                                changeYear: true,
                                                dateFormat: "dd-M-yy",
                                                showOn: 'button',
                                                buttonImageOnly: true,
                                                buttonImage: 'Images/calendar.gif',
                                                buttonText: "Select date",
                                                beforeShow: function (textbox, instance) {
                                                    var scrollTop = $(window).scrollTop();
                                                    var txtBoxOffset = $(this).offset();
                                                    var top = txtBoxOffset.top - scrollTop;
                                                    var left = txtBoxOffset.left;
                                                    var textBoxHeight = $(this).outerHeight();
                                                    setTimeout(function () {
                                                        instance.dpDiv.css({
                                                            top: top - $("#ui-datepicker-div").outerHeight(),
                                                            left: left
                                                        });
                                                    }, 0);
                                                }
                                                //yearRange: "c-100:c"
                                            });
                                            $('#<%=txtStep2ED.ClientID %>').datepicker({
                                                changeMonth: true,
                                                changeYear: true,
                                                dateFormat: "dd-M-yy",
                                                showOn: 'button',
                                                buttonImageOnly: true,
                                                buttonImage: 'Images/calendar.gif',
                                                buttonText: "Select date",
                                                beforeShow: function (textbox, instance) {
                                                    var scrollTop = $(window).scrollTop();
                                                    var txtBoxOffset = $(this).offset();
                                                    var top = txtBoxOffset.top - scrollTop;
                                                    var left = txtBoxOffset.left;
                                                    var textBoxHeight = $(this).outerHeight();
                                                    setTimeout(function () {
                                                        instance.dpDiv.css({
                                                            top: top - $("#ui-datepicker-div").outerHeight(),
                                                            left: left
                                                        });
                                                    }, 0);
                                                }
                                                //yearRange: "c-100:c"
                                            });
                                            $('#<%=txtStep3SD.ClientID %>').datepicker({
                                                changeMonth: true,
                                                changeYear: true,
                                                dateFormat: "dd-M-yy",
                                                showOn: 'button',
                                                buttonImageOnly: true,
                                                buttonImage: 'Images/calendar.gif',
                                                buttonText: "Select date",
                                                beforeShow: function (textbox, instance) {
                                                    var scrollTop = $(window).scrollTop();
                                                    var txtBoxOffset = $(this).offset();
                                                    var top = txtBoxOffset.top - scrollTop;
                                                    var left = txtBoxOffset.left;
                                                    var textBoxHeight = $(this).outerHeight();
                                                    setTimeout(function () {
                                                        instance.dpDiv.css({
                                                            top: top - $("#ui-datepicker-div").outerHeight(),
                                                            left: left
                                                        });
                                                    }, 0);
                                                }
                                                //yearRange: "c-100:c"
                                            });
                                            $('#<%=txtStep3ED.ClientID %>').datepicker({
                                                changeMonth: true,
                                                changeYear: true,
                                                dateFormat: "dd-M-yy",
                                                showOn: 'button',
                                                buttonImageOnly: true,
                                                buttonImage: 'Images/calendar.gif',
                                                buttonText: "Select date",
                                                beforeShow: function (textbox, instance) {
                                                    var scrollTop = $(window).scrollTop();
                                                    var txtBoxOffset = $(this).offset();
                                                    var top = txtBoxOffset.top - scrollTop;
                                                    var left = txtBoxOffset.left;
                                                    var textBoxHeight = $(this).outerHeight();
                                                    setTimeout(function () {
                                                        instance.dpDiv.css({
                                                            top: top - $("#ui-datepicker-div").outerHeight(),
                                                            left: left
                                                        });
                                                    }, 0);
                                                }
                                                //yearRange: "c-100:c"
                                            });
                                        }
                                        function ShowProgress() {
                                            $("#divWaiting").css("display", "");
                                        }
                                        function showGenericMessageModal(type, message) {
                                            swal('', message, type);
                                        };
                                        function openReportModal() {
                                            var myModal = new bootstrap.Modal(document.getElementById('myModalUploadReport'));
                                            myModal.show();
                                        }
                                        function openEmployeeRecordUpdateModal() {
                                            var myModal = new bootstrap.Modal(document.getElementById('myModalRecordUpdate'));
                                            myModal.show();
                                        }
                                        function closeModal() {
                                            if (Page_ClientValidate("modalpopup")) {
                                                $("#btnCloseModal").click();
                                                return true;
                                            } else {
                                                return false;
                                            }
                                        }
                                    </script>
        </asp:Content>
        <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <div class="divWaiting" id="divWaiting" style="display: none">
                <center>
                    <div>
                        <h3>
                            <asp:Label ID="lblWait" runat="server" Text="Please wait" />
                        </h3>
                        <img src="Images/loader.gif" alt="Loading..." id="ifMobile1" />
                    </div>
                </center>
            </div>

            <asp:UpdateProgress ID="updtProgressPnlMain" AssociatedUpdatePanelID="upnlMain" runat="server">
                <ProgressTemplate>
                    <div class="divWaiting" style="padding-top: 20%;">
                        <center>
                            <div>
                                <h3>
                                    <asp:Label ID="lblWaitAjax" runat="server" Text="Please wait..."
                                        CssClass="label text-midnight-blue" />
                                </h3>
                                <img src="images/ajax-loader.gif" alt="Loading..." />
                            </div>
                        </center>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="upnlMain" runat="server">
                <ContentTemplate>
                    <div class="container-fluid">
                        <div class="row form-group p-2">
                            <div class="col-md-3 text-left">
                                <label class="font-weight-bold">How many records do you want to update ? <span
                                        class="text-danger">*</span></label>
                                <asp:DropDownList CssClass="form-control" ID="ddlRecords" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlRecords_TextChanged">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Less than or Equal to 10</asp:ListItem>
                                    <asp:ListItem Value="2">More than 10</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <asp:Panel ID="pnlforlessRecord" runat="server" Visible="false">
                            <div class="row form-group p-2">
                                <div class="col-md-1 text-left">
                                    <label class="font-weight-bold">Year<span class="text-danger">*</span></label>
                                    <asp:TextBox runat="server" ID="txtYearforLessRecord" Enabled="false"
                                        CssClass="form-control" />
                                </div>
                                <div class="col-md-1 text-left">
                                    <label class="font-weight-bold">Cycle<span class="text-danger">*</span></label>
                                    <asp:TextBox runat="server" ID="txtCycleforLessRecord" Enabled="false"
                                        CssClass="form-control" />
                                </div>
                                <div class="col-md-3 text-left">
                                    <label class="font-weight-bold">Per. no.</label>
                                    <asp:TextBox ID="txtPernoforLessRecord" runat="server" CssClass="form-control"
                                        MaxLength="80"></asp:TextBox>
                                    <span style="color:blue; font-size: .75em;">For multiple personal no use commas (,)
                                        eg: 111111,222222 (Max 10 personal no)</span>
                                </div>
                                <div class="col-md-1 text-left">
                                    <label class="font-weight-bold">&nbsp;</label>
                                    <asp:LinkButton ID="lbkLessSearch" OnClick="lbkLessSearch_Click" runat="server"
                                        CssClass="btn btn-success"><i class="fa fa-search-plus"></i>&nbsp;Search
                                    </asp:LinkButton>
                                </div>
                                <div class="col-md-1 text-left">
                                    <label class="font-weight-bold">&nbsp;</label>
                                    <asp:LinkButton ID="lbkLessRefresh" OnClick="lbkLessRefresh_Click" runat="server"
                                        CssClass="btn btn-primary"><i class="fa fa-refresh"></i>&nbsp;Refresh
                                    </asp:LinkButton>
                                </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlforExcelRecord" runat="server" Visible="false">
                            <div class="row form-group p-2">
                                <div class="col-md-1 text-left">
                                    <label class="font-weight-bold">Year<span class="text-danger">*</span></label>
                                    <asp:TextBox runat="server" ID="txtYear" Enabled="false" CssClass="form-control" />
                                </div>
                                <div class="col-md-1 text-left">
                                    <label class="font-weight-bold">Cycle<span class="text-danger">*</span></label>
                                    <asp:TextBox runat="server" ID="txtCycle" Enabled="false" CssClass="form-control" />
                                </div>
                                <div class="col-md-1 text-left">
                                    <label class="font-weight-bold">Grade</label>
                                    <asp:DropDownList runat="server" ID="ddlSGrade" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 text-left">
                                    <label class="font-weight-bold">Executive Head</label>
                                    <asp:DropDownList CssClass="form-control" ID="ddlExecHead" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 text-left">
                                    <label class="font-weight-bold">Subarea</label>
                                    <asp:DropDownList CssClass="form-control" ID="ddlSubarea" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1 text-left">
                                    <label class="font-weight-bold">Per. no.</label>
                                    <asp:TextBox ID="txtPerno" runat="server" CssClass="form-control" MaxLength="6">
                                    </asp:TextBox>
                                </div>
                                <div class="col-md-4 text-left d-flex">
                                    <div class="d-flex m-3">
                                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn btn-success m-3"><i
                                                class="fa fa-search"></i>&nbsp;Search</asp:LinkButton>
                                        <asp:LinkButton ID="lbtnExport" runat="server" Visible="false"
                                            CssClass="btn btn-info m-3"><i class="fa fa-file-text"></i>&nbsp;Export to
                                            Excel</asp:LinkButton>
                                        <asp:LinkButton ID="lbtnExcelSectionRefresh"
                                            OnClick="lbtnExcelSectionRefresh_Click" runat="server"
                                            CssClass="btn btn-primary m-3"><i class="fa fa-refresh"></i>&nbsp;Refresh
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div id="pnlUpload" runat="server" visible="false" class="row form-group p-1">
                            <div class="col-md-3 col-sm-3">
                                <label>Upload Updated File</label>
                                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control btn btn-primary"
                                    ToolTip="Select Document to Upload updated file" />
                            </div>
                            <div class="col-md-1">
                                <div style="margin-top: 25px;">
                                    <%-- <asp:Button ID="btnUpload" Text="Upload" runat="server"
                                        CssClass="btn btn-primary" CausesValidation="false"
                                        OnClientClick="ShowProgress();" />--%>
                                    <asp:LinkButton ID="btnUpload" runat="server" CausesValidation="false"
                                        OnClientClick="ShowProgress();" CssClass="btn btn-primary"><i
                                            class="fa fa-upload"></i>&nbsp;Upload</asp:LinkButton>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div style="margin-top: 25px;">
                                    <asp:LinkButton ID="lbtnExcelTemplate" Visible="false" runat="server"
                                        OnClick="lbtnExcelTemplate_Click" CausesValidation="false"
                                        CssClass="btn btn-success"><i class="fa fa-download"></i>&nbsp;Excel Template
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                            <div class="row form-group p-2">
                                <div class="col-md-12 col-sm-12">
                                    <div style="width: 100%; height: 300px; overflow: scroll;"
                                        class="table table-striped">
                                        <%-- <div
                                            style="width: 100%; overflow: auto; height: 275px; border: 1px Solid Silver; float: left;">--%>
                                            <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-striped table-hover table-bordered dataTable no-footer "
                                                EmptyDataText="No Record Found" Font-Size="13px" BorderStyle="None"
                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                                RowStyle-CssClass="rows">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbkEdit" runat="server"
                                                                Text="<i class='fa fa-edit'></i>"
                                                                Style="font-size: 21px; color: blue;"
                                                                OnClick="lbkEdit_Click"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Year">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridYear" runat="server"
                                                                Text='<%#Eval("EMA_YEAR") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cycle">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridCycle" runat="server"
                                                                Text='<%#Eval("EMA_CYCLE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Personal Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridPerno" runat="server"
                                                                Text='<%#Eval("EMA_PERNO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="EMA_YEAR" HeaderText="Year"
                                                        SortExpression="EMA_YEAR" />
                                                    <asp:BoundField DataField="EMA_CYCLE" HeaderText="Cycle"
                                                        SortExpression="EMA_CYCLE" />
                                                    <asp:BoundField DataField="EMA_PERNO" HeaderText="Personal Number"
                                                        SortExpression="EMA_PERNO" />--%>
                                                    <%-- <asp:BoundField DataField="ema_ename" HeaderText="Name"
                                                        SortExpression="ema_ename" />--%>
                                                    <asp:TemplateField HeaderText="Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridName" runat="server"
                                                                Text='<%#Eval("ema_ename") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ema_desgn_desc" HeaderText="Designation"
                                                        SortExpression="ema_desgn_desc" />
                                                    <%-- <asp:BoundField DataField="ema_email_id" HeaderText="Email-id"
                                                        SortExpression="ema_email_id" />--%>
                                                    <asp:TemplateField HeaderText="Email-id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridEmail" runat="server"
                                                                Text='<%#Eval("ema_email_id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="ema_eqv_level"
                                                        HeaderText="Equivalent Level"
                                                        SortExpression="ema_eqv_level" />--%>
                                                    <%-- <asp:BoundField DataField="ema_reporting_to_pno"
                                                        HeaderText="Reporting Personal No."
                                                        SortExpression="ema_reporting_to_pno" />--%>
                                                    <%-- <asp:BoundField DataField="ema_bhr_pno"
                                                        HeaderText="BHR Personal No."
                                                        SortExpression="ema_bhr_pno" />--%>
                                                    <asp:TemplateField HeaderText="Equivalent Level">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridEqvLevel" runat="server"
                                                                Text='<%#Eval("ema_eqv_level") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reporting Personal No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridReportingPNO" runat="server"
                                                                Text='<%#Eval("ema_reporting_to_pno") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="BHR Personal No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridBHRPNO" runat="server"
                                                                Text='<%#Eval("ema_bhr_pno") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="BBHR Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridBHRName" runat="server"
                                                                Text='<%#Eval("ema_bhr_name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dotted Personal No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridDottedPerno" runat="server"
                                                                Text='<%#Eval("ema_dotted_pno") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Executive Head Personal No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridExecPerno" runat="server"
                                                                Text='<%#Eval("ema_pers_exec_pno") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%-- <asp:BoundField DataField="ema_bhr_name" HeaderText="BHR Name"
                                                        SortExpression="ema_bhr_name" />--%>
                                                    <%--<asp:BoundField DataField="ema_dotted_pno"
                                                        HeaderText="Dotted Personal No."
                                                        SortExpression="ema_dotted_pno" />--%>
                                                    <%--<asp:BoundField DataField="ema_pers_exec_pno"
                                                        HeaderText="Executive Personal No."
                                                        SortExpression="ema_pers_exec_pno" />--%>
                                                    <%-- <asp:BoundField DataField="ema_pers_exec_pno"
                                                        HeaderText="Executive Head Personal No."
                                                        SortExpression="ema_pers_exec_pno" />--%>
                                                    <%-- <asp:BoundField DataField="ema_step1_stdt"
                                                        HeaderText="Step 1 Sart Date" SortExpression="ema_step1_stdt" />
                                                    <asp:BoundField DataField="ema_step1_enddt"
                                                        HeaderText="Step 1 End Date" SortExpression="ema_step1_enddt" />
                                                    <asp:BoundField DataField="ema_step2_stdt"
                                                        HeaderText="Step 2 Start Date"
                                                        SortExpression="ema_step2_stdt" />
                                                    <asp:BoundField DataField="ema_step2_enddt"
                                                        HeaderText="Step 2 End Date" SortExpression="ema_step2_enddt" />
                                                    <asp:BoundField DataField="ema_step3_stdt"
                                                        HeaderText="Step 3 Start Date"
                                                        SortExpression="ema_step3_stdt" />
                                                    <asp:BoundField DataField="ema_step3_enddt"
                                                        HeaderText="Step 3 End Date" SortExpression="ema_step3_enddt" />
                                                    --%>
                                                    <asp:TemplateField HeaderText="Step 1 Sart Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridS1SD" runat="server"
                                                                Text='<%#Eval("ema_step1_stdt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Step 1 End Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridS1ED" runat="server"
                                                                Text='<%#Eval("ema_step1_enddt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Step 2 Start Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridS2SD" runat="server"
                                                                Text='<%#Eval("ema_step2_stdt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Step 2 End Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridS2ED" runat="server"
                                                                Text='<%#Eval("ema_step2_enddt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Step 3 Start Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridS3SD" runat="server"
                                                                Text='<%#Eval("ema_step3_stdt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Step 3 End Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridS3ED" runat="server"
                                                                Text='<%#Eval("ema_step3_enddt") %>'></asp:Label>
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
                                            <%-- </div>--%>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>


                    <div class="modal fade" id="myModalUploadReport" role="dialog">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Excel Data upload report</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="info-msg">
                                                <i class="fa fa-info-circle"></i>
                                                <label>Record uploaded by user : </label>
                                                <label id="lblRecordUploadbyUser" runat="server"></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="success-msg">
                                                <i class="fa fa-check"></i>
                                                <label>Updated record : </label>
                                                <label id="lblInsertRecord" runat="server"></label>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="error-msg">
                                                <i class="fa fa-times-circle"></i>
                                                <label>Invalid record : </label>
                                                <label runat="server" id="lblIncorrectRecord"></label>
                                                &nbsp;
                                                <asp:LinkButton ID="lbkInvalidRecordDump" runat="server"
                                                    OnClick="lbkInvalidRecordDump_Click"
                                                    ToolTip="Download Invalid Record Dump"
                                                    Text="<i class='fa fa-download'></i>"></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row " id="divUploadErroorSection" runat="server" visible="false">
                                        <div class="col-sm-12 col-lg-12 col-md-12">
                                            <div style="max-height: 170px; overflow-y: scroll;">
                                                <asp:GridView ID="grdExcelUploadReport" runat="server"
                                                    AutoGenerateColumns="false" Font-Size="12px"
                                                    CssClass="table table-striped table-hover table-bordered dataTable no-footer ">
                                                    <Columns>
                                                        <asp:BoundField DataField="Personal Number"
                                                            HeaderText="Personal Number" />
                                                        <asp:BoundField DataField="Email-id" HeaderText="Email-id" />
                                                        <asp:BoundField DataField="BHR Personal No."
                                                            HeaderText="BHR Personal No." />
                                                        <%-- <asp:BoundField DataField="Step 1 Sart Date"
                                                            HeaderText="Step 1 Sart Date" ItemStyle-Width="200" />
                                                        <asp:BoundField DataField="Step 1 End Date"
                                                            HeaderText="Step 1 End Date" ItemStyle-Width="200" />
                                                        <asp:BoundField DataField="Step 2 Start Date"
                                                            HeaderText="Step 2 Start Date" ItemStyle-Width="200" />
                                                        <asp:BoundField DataField="Step 2 End Date"
                                                            HeaderText="Step 2 End Date" ItemStyle-Width="200" />
                                                        <asp:BoundField DataField="Step 3 Start Date"
                                                            HeaderText="Step 3 Start Date" ItemStyle-Width="200" />
                                                        <asp:BoundField DataField="Step 3 End Date"
                                                            HeaderText="Step 3 End Date" ItemStyle-Width="200" />--%>
                                                        <asp:BoundField DataField="Error" HeaderText="Error"
                                                            ItemStyle-Width="200" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer" style="padding-right: 0px;">
                                        <button type="button" class="btn btn-primary"
                                            data-bs-dismiss="modal">Close</button>

                                    </div>
                                </div>

                            </div>
                        </div>

                    </div>
                    <button type="button" style="display: none;" id="btnShowPopup" class="btn btn-primary btn-lg"
                        data-bs-toggle="modal" data-bs-target="#myModalUploadReport">
                        Launch demo modal
                    </button>

                    <div class="modal fade" id="myModalRecordUpdate" role="dialog">
                        <div class="modal-dialog" id="mdialog">
                            <!-- Modal content-->
                            <div class="modal-content" id="mcontent">
                                <div class="modal-header">
                                    <h4 class="modal-title">Employee Record Update</h4>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body" id="mbody" style=" max-height: 80vh; overflow-y: auto;">
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>Personal Number</label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="txtPopupPerno" Enabled="false"
                                                CssClass="form-control" />
                                            <asp:Label ID="lblPopupYear" Visible="false" runat="server"></asp:Label>
                                            <asp:Label ID="lblPopupCycle" Visible="false" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>Name</label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="txtPopupName" Enabled="false"
                                                CssClass="form-control" />
                                        </div>
                                    </div>
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>Email-id</label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="txtPopupEmail" autocomplete="off"
                                                CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="emailReqFldVal" runat="server"
                                                ValidationGroup="modalpopup" autocomplete="off"
                                                ErrorMessage="Please enter email-id" ControlToValidate="txtPopupEmail"
                                                CssClass="errtxt" Display="Dynamic" ForeColor="#CC0066"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="regexemailReqFldVal" runat="server"
                                                ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"
                                                ControlToValidate="txtPopupEmail" ValidationGroup="modalpopup"
                                                ErrorMessage="Invalid e-mail format" CssClass="errtxt" Display="Dynamic"
                                                ForeColor="#CC0066" SetFocusOnError="True">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>Equivalent Level</label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="txtPopupEqvLvl" autocomplete="off"
                                                CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="eqvLevelReqFldVal" runat="server"
                                                ValidationGroup="modalpopup" autocomplete="off"
                                                ErrorMessage="Please enter Equivalent Level"
                                                ControlToValidate="txtPopupEqvLvl" CssClass="errtxt" Display="Dynamic"
                                                ForeColor="#CC0066" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>Reporting P.No.</label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="txtPopupReportingperno" autocomplete="off"
                                                MaxLength="6" TextMode="Number" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="reportingPernoReqFldVal" runat="server"
                                                ValidationGroup="modalpopup" autocomplete="off"
                                                ErrorMessage="Please enter Reporting Per. No."
                                                ControlToValidate="txtPopupReportingperno" CssClass="errtxt"
                                                Display="Dynamic" ForeColor="#CC0066" SetFocusOnError="True">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>BHR P.No.</label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="txtPopupBperno" autocomplete="off"
                                                MaxLength="6" TextMode="Number" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="bhrusrnoReqFldVal" runat="server"
                                                ValidationGroup="modalpopup" autocomplete="off"
                                                ErrorMessage="Please enter BHR Per. No."
                                                ControlToValidate="txtPopupBperno" CssClass="errtxt" Display="Dynamic"
                                                ForeColor="#CC0066" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>BHR Name</label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="txtPopupBhrName" autocomplete="off"
                                                CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="bhrnameReqFldVal" runat="server"
                                                ValidationGroup="modalpopup" autocomplete="off"
                                                ErrorMessage="Please enter BHR Name" ControlToValidate="txtPopupBhrName"
                                                CssClass="errtxt" Display="Dynamic" ForeColor="#CC0066"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>Dotted P.No.</label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="txtPopupDottedPno" autocomplete="off"
                                                MaxLength="6" TextMode="Number" CssClass="form-control" />
                                            <%-- <asp:RequiredFieldValidator ID="dottedPernoReqFldVal" runat="server"
                                                ValidationGroup="modalpopup" autocomplete="off"
                                                ErrorMessage="Please enter Dotted Per. No."
                                                ControlToValidate="txtPopupDottedPno" CssClass="errtxt"
                                                Display="Dynamic" ForeColor="#CC0066" SetFocusOnError="True">
                                                </asp:RequiredFieldValidator>--%>
                                        </div>
                                    </div>
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>Executive Head P.No.</label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="txtPopupExecutivePno" autocomplete="off"
                                                MaxLength="6" TextMode="Number" CssClass="form-control" />
                                            <%-- <asp:RequiredFieldValidator ID="executivePernoReqFldVal" runat="server"
                                                ValidationGroup="modalpopup" autocomplete="off"
                                                ErrorMessage="Please enter Executive Head Per. No."
                                                ControlToValidate="txtPopupExecutivePno" CssClass="errtxt"
                                                Display="Dynamic" ForeColor="#CC0066" SetFocusOnError="True">
                                                </asp:RequiredFieldValidator>--%>
                                        </div>
                                    </div>
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>Step-1 Start Date</label>
                                        </div>
                                        <div class="col-sm-8" style="display:flex">
                                            <asp:TextBox ID="txtStep1SD" data-bs-placement="top" autocomplete="off"
                                                CssClass="form-control cursor-pointer" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="regextxtStep1SDReqFldVal" runat="server"
                                                ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$"
                                                ControlToValidate="txtStep1SD" ValidationGroup="modalpopup"
                                                ErrorMessage="Invalid date format" CssClass="errtxt" Display="Dynamic"
                                                ForeColor="#CC0066" SetFocusOnError="True">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>Step-1 End Date</label>
                                        </div>
                                        <div class="col-sm-8" style="display:flex">
                                            <asp:TextBox ID="txtStep1ED" data-bs-placement="top" autocomplete="off"
                                                CssClass="form-control cursor-pointer" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="regextxtStep1EDReqFldVal" runat="server"
                                                ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$"
                                                ControlToValidate="txtStep1ED" ValidationGroup="modalpopup"
                                                ErrorMessage="Invalid date format" CssClass="errtxt" Display="Dynamic"
                                                ForeColor="#CC0066" SetFocusOnError="True">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>Step-2 Start Date</label>
                                        </div>
                                        <div class="col-sm-8" style="display:flex">
                                            <asp:TextBox ID="txtStep2SD" data-bs-placement="top" autocomplete="off"
                                                CssClass="form-control cursor-pointer" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="regextxtStep2SDReqFldVal" runat="server"
                                                ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$"
                                                ControlToValidate="txtStep2SD" ValidationGroup="modalpopup"
                                                ErrorMessage="Invalid date format" CssClass="errtxt" Display="Dynamic"
                                                ForeColor="#CC0066" SetFocusOnError="True">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>Step-2 End Date</label>
                                        </div>
                                        <div class="col-sm-8" style="display:flex;">
                                            <asp:TextBox ID="txtStep2ED" data-bs-placement="top" autocomplete="off"
                                                CssClass="form-control cursor-pointer" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="regextxtStep2EDReqFldVal" runat="server"
                                                ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$"
                                                ControlToValidate="txtStep2ED" ValidationGroup="modalpopup"
                                                ErrorMessage="Invalid date format" CssClass="errtxt" Display="Dynamic"
                                                ForeColor="#CC0066" SetFocusOnError="True">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>Step-3 Start Date</label>
                                        </div>
                                        <div class="col-sm-8" style="display:flex">
                                            <asp:TextBox ID="txtStep3SD" data-bs-placement="top" autocomplete="off"
                                                CssClass="form-control cursor-pointer" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="regextxtStep3SDReqFldVal" runat="server"
                                                ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$"
                                                ControlToValidate="txtStep3SD" ValidationGroup="modalpopup"
                                                ErrorMessage="Invalid date format" CssClass="errtxt" Display="Dynamic"
                                                ForeColor="#CC0066" SetFocusOnError="True">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="row p-1">
                                        <div class="col-sm-4">
                                            <label>Step-3 End Date</label>
                                        </div>
                                        <div class="col-sm-8" style="display:flex">
                                            <asp:TextBox ID="txtStep3ED" data-bs-placement="top" autocomplete="off"
                                                CssClass="form-control cursor-pointer" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="regextxtStep3EDReqFldVal" runat="server"
                                                ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$"
                                                ControlToValidate="txtStep3ED" ValidationGroup="modalpopup"
                                                ErrorMessage="Invalid date format" CssClass="errtxt" Display="Dynamic"
                                                ForeColor="#CC0066" SetFocusOnError="True">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-primary" id="btnCloseModal"
                                        data-bs-dismiss="modal">Close</button>
                                    <asp:Button ID="btnEmployeeRecordUpdate" OnClick="btnEmployeeRecordUpdate_Click"
                                        OnClientClick="closeModal();" runat="server" ValidationGroup="modalpopup"
                                        Text="Update" CssClass="btn btn-success" />
                                </div>
                            </div>
                        </div>

                    </div>
                    <button type="button" style="display: none;" id="btnShowRecordUpdateModal"
                        class="btn btn-primary btn-lg" data-bs-toggle="modal" data-bs-target="#myModalRecordUpdate">
                        Launch demo modal
                    </button>

                </ContentTemplate>
                <Triggers>
                    <%--<asp:PostBackTrigger ControlID="lbtnSearch" />
                    <asp:PostBackTrigger ControlID="lbkLessSearch" />--%>
                    <asp:PostBackTrigger ControlID="btnupload" />
                    <asp:PostBackTrigger ControlID="lbtnExcelTemplate" />
                    <asp:PostBackTrigger ControlID="lbtnExport" />
                    <asp:PostBackTrigger ControlID="lbkInvalidRecordDump" />
                </Triggers>
            </asp:UpdatePanel>

        </asp:Content>