<%@ Page Title="" Language="VB" MasterPageFile="~/AdminMaster.master" AutoEventWireup="false" CodeFile="MasterData.aspx.vb" Inherits="MasterData" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!-- Google Fonts -->
    <link href="assets/css/googlefont.css" rel="stylesheet" />

    <!-- Vendor CSS Files -->
    <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="./assets/vendor/icofont/icofont.min.css" rel="stylesheet">
    <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
    <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">
    <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
    <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
    <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>
    <link href="styles/GridviewScroll.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
    <script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script>
    <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>

    <!-- Template Main CSS File -->
    <link href="assets/css/styleIL3.css" rel="stylesheet">

    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />

    <script src="//code.jquery.com/jquery-1.11.0.min.js"></script>

    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js" type="text/javascript"></script>
    <style type="text/css">
        .rbl input[type="radio"]
{
   margin-left: 15px;
   margin-right: 10px;
}
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
            width: 14em !important; /*what ever width you want*/
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
    <script type="text/javascript">
        function pageLoad() {

        }
        $(document).ready(function () {

        });
        function removeBackdrop() {
            //$('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        }
        function ShowProgress() {
            $("#divWaiting").css("display", "");
        }
        function showGenericMessageModal(type, message) {
            swal('', message, type);
        };
        function openReportModal() {
            $("#btnShowPopup").click();
        }
        function openEmployeeRecordUpdateModal() {
            $("#btnShowRecordUpdateModal").click();
        }
        function closeModal() {
            $("#btnCloseModal").click();
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="divWaiting" id="divWaiting" style="display: none">
        <center>
                    <div>
                        <h3>
                            <asp:Label ID="lblWait" runat="server" Text="Please wait"  /></h3>
                      <img src="Images/loader.gif" alt="Loading..."  id="ifMobile1"/>
                    </div>
                </center>
    </div>

    <asp:UpdateProgress ID="updtProgressPnlMain" AssociatedUpdatePanelID="upnlMain"
        runat="server">
        <ProgressTemplate>
            <div class="divWaiting" style="padding-top: 20%;">
                <center>
                    <div>
                        <h3>
                            <asp:Label ID="lblWaitAjax" runat="server" Text="Please wait..." CssClass="label text-midnight-blue" /></h3>
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
                    <div class="col-md-12 text-left">
                        <asp:RadioButtonList ID="rdbtnSelection" runat="server" RepeatDirection="Horizontal" CssClass="rbl" AutoPostBack="true"
                            OnSelectedIndexChanged="rdbtnSelection_SelectedIndexChanged" >
                            <asp:ListItem Text=" Include Employee for Assessment" Value="0"></asp:ListItem>
                            <asp:ListItem Text=" Exclude Employee for Assessment" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <asp:Panel ID="pnlIncludeEmployee" runat="server" Visible="false">
                    <div class="row form-group p-2">
                        <div class="col-md-1 text-left">
                            <label class="font-weight-bold">Year<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txtYearIE" Enabled="false" CssClass="form-control" />
                        </div>
                        <div class="col-md-1 text-left">
                            <label class="font-weight-bold">Cycle<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txtCycleIE" Enabled="false" CssClass="form-control" />
                        </div>
                        <div class="col-md-3 col-sm-3">
                        <label>Upload Master Employee Data</label>
                        <asp:FileUpload ID="flUpload" runat="server" CssClass="form-control btn btn-primary" ToolTip="Select Document to Upload updated file" />
                    </div>
                    <div class="col-md-1">
                        <div style="margin-top: 25px;">
                            <asp:LinkButton ID="btnUpload" OnClick="btnUpload_Click" runat="server"  CausesValidation="false" OnClientClick="ShowProgress();" CssClass="btn btn-primary"><i class="fa fa-upload"></i>&nbsp;Upload</asp:LinkButton>
                        </div>
                    </div>
                      <div class="col-md-1">
                        <div style="margin-top: 25px;">
                            <asp:LinkButton ID="lbtnExcelTemplate" OnClick="lbtnExcelTemplate_Click" runat="server" CausesValidation="false" CssClass="btn btn-success"><i class="fa fa-download"></i>&nbsp;Excel Template</asp:LinkButton>
                        </div>
                    </div>
                        </div>
                     <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                    <div class="row form-group p-2">
                        <div class="col-md-12 col-sm-12">
                            <u><b>Currently Uploaded Record List</b></u>
                            </div>
                        <div class="col-md-12 col-sm-12  pt-2 m-2">
                            <div style="width: 100%; height: 300px; overflow: scroll;" class="table table-striped">
                                <%--  <div style="width: 100%; overflow: auto; height: 275px; border: 1px Solid Silver; float: left;">--%>
                                <asp:GridView ID="grdIncludedData" runat="server" AutoGenerateColumns="True" CssClass="table table-striped table-hover table-bordered dataTable no-footer "
                                    EmptyDataText="No Record Found" Font-Size="13px" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" RowStyle-CssClass="rows">
                                    <%--<Columns>
                                        <asp:BoundField DataField="EMA_PERNO" HeaderText="P.No." SortExpression="EMA_PERNO" />
                                        <asp:BoundField DataField="EMA_ENAME" HeaderText="Name" SortExpression="EMA_ENAME" />
                                        <asp:BoundField DataField="EMA_DESGN_CODE" HeaderText="Design Code" SortExpression="EMA_DESGN_CODE" />                                      
                                        <asp:BoundField DataField="EMA_DESGN_DESC" HeaderText="Design Desc" SortExpression="EMA_DESGN_DESC" />
                                        <asp:BoundField DataField="EMA_EMAIL_ID" HeaderText="Email Id" SortExpression="EMA_EMAIL_ID" />
                                        <asp:BoundField DataField="EMA_EQV_LEVEL" HeaderText="Eqv Level" SortExpression="EMA_EQV_LEVEL" />                                      
                                        <asp:BoundField DataField="EMA_PHONE_NO" HeaderText="Phone No." SortExpression="EMA_PHONE_NO" />
                                        <asp:BoundField DataField="EMA_PERS_SUBAREA" HeaderText="Sub Area" SortExpression="EMA_PERS_SUBAREA" />
                                        <asp:BoundField DataField="EMA_PERS_SUBAREA_DESC" HeaderText="Sub Area Desc" SortExpression="EMA_PERS_SUBAREA_DESC" />                                      
                                        <asp:BoundField DataField="EMA_COMP_CODE" HeaderText="Comp Code" SortExpression="EMA_COMP_CODE" />
                                        <asp:BoundField DataField="EMA_REPORTING_TO_PNO" HeaderText="Reporting P.No." SortExpression="EMA_REPORTING_TO_PNO" />
                                        <asp:BoundField DataField="EMA_BHR_PNO" HeaderText="BHR P.No." SortExpression="EMA_BHR_PNO" />
                                        <asp:BoundField DataField="EMA_BHR_NAME" HeaderText="BHR Name" SortExpression="EMA_BHR_NAME" />                                      
                                        <asp:BoundField DataField="EMA_JOINING_DT" HeaderText="Joining Date" SortExpression="EMA_JOINING_DT" />
                                        <asp:BoundField DataField="EMA_DEPT_CODE" HeaderText="Dept Code" SortExpression="EMA_DEPT_CODE" />
                                        <asp:BoundField DataField="EMA_DEPT_DESC" HeaderText="Dept Desc" SortExpression="EMA_DEPT_DESC" />                                      
                                        <asp:BoundField DataField="EMA_EMPL_SGRADE" HeaderText="Sub Grade" SortExpression="EMA_EMPL_SGRADE" />
                                        <asp:BoundField DataField="EMA_EMP_CLASS" HeaderText="Class" SortExpression="EMA_EMP_CLASS" />
                                        <asp:BoundField DataField="EMA_DOTTED_PNO" HeaderText="Dotted P.No." SortExpression="EMA_DOTTED_PNO" />                                      
                                        <asp:BoundField DataField="EMA_PERS_EXEC_PNO" HeaderText="Executive P.No." SortExpression="EMA_PERS_EXEC_PNO" />
                                        <asp:BoundField DataField="EMA_EXEC_HEAD" HeaderText="Executive Head" SortExpression="EMA_EXEC_HEAD" />
                                        <asp:BoundField DataField="EMA_EXEC_HEAD_DESC" HeaderText="Executive Head Desc" SortExpression="EMA_EXEC_HEAD_DESC" />                                      
                                    </Columns>--%>
                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                    <RowStyle BackColor="White" ForeColor="Black" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
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
                </asp:Panel>

             <%--   <asp:Panel ID="pnlExcludeEmployee" runat="server" Visible="false">
                    <div class="row form-group p-2">
                        <div class="col-md-1 text-left">
                            <label class="font-weight-bold">Year<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txtYearEE" Enabled="false" CssClass="form-control" />
                        </div>
                        <div class="col-md-1 text-left">
                            <label class="font-weight-bold">Cycle<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txtCycleEE" Enabled="false" CssClass="form-control" />
                        </div>
                     
                </asp:Panel>   --%>             
            </div>

                <div class="modal fade" id="myModalUploadReport" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Excel Data upload report</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="info-msg">
                                        <i class="fa fa-info-circle"></i>
                                        <label>Total record uploaded : </label>
                                        <label id="lblRecordUploadbyUser" runat="server"></label>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="success-msg">
                                        <i class="fa fa-check"></i>
                                        <label>Inserted record : </label>
                                        <label id="lblInsertRecord" runat="server"></label>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="error-msg">
                                        <i class="fa fa-times-circle"></i>
                                        <label>Invalid record : </label>
                                        <label runat="server" id="lblIncorrectRecord"></label>
                                        &nbsp; 
                        <asp:LinkButton ID="lbkInvalidRecordDump" runat="server" OnClick="lbkInvalidRecordDump_Click" ToolTip="Download Invalid Record Dump" Text="<i class='fa fa-download'></i>"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="row " id="divUploadErroorSection" runat="server" visible="false">
                                <div class="col-sm-12 col-lg-12 col-md-12">
                                    <div style="max-height: 170px; overflow-y: scroll;">
                                        <asp:GridView ID="grdExcelUploadReport" runat="server" AutoGenerateColumns="true" Font-Size="12px" CssClass="table table-striped table-hover table-bordered dataTable no-footer ">
                                            <%--<Columns>
                                                <asp:BoundField DataField="Personal Number" HeaderText="Personal Number" />
                                                <asp:BoundField DataField="Name" HeaderText="Name" />                                  
                                                <asp:BoundField DataField="Error" HeaderText="Error" ItemStyle-Width="200" />
                                            </Columns>--%>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer" style="padding-right: 0px;">
                                <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>

                            </div>
                        </div>

                    </div>
                </div>

            </div>
            <button type="button" style="display: none;" id="btnShowPopup" class="btn btn-primary btn-lg"
                data-toggle="modal" data-target="#myModalUploadReport">
                Launch demo modal
            </button>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lbtnExcelTemplate" />
            <asp:PostBackTrigger ControlID="btnUpload" />
            <asp:PostBackTrigger ControlID="lbkInvalidRecordDump" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
