<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="UploadEmpData.aspx.vb" Inherits="UploadEmpData" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link type="text/css" href="plugins/Bootstrap-Slider/bootstrap-slider.min.css" rel="stylesheet" />
    <link type="text/css" href="styles/bootstrap-select.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="styles/fileInput.css" />

    <style type="text/css">
        .nav-tabs > li.active > a, .nav-tabs > li.active > a:focus {
            background-color: #efefef !important;
            color: #000 !important;
            border: 2px solid #5bc0de !important;
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
        /*.wrap-Text{
          word-wrap: normal; 
          word-break: break-all;
          width:30px;
      }*/
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

        .error {
            color: red;
            font-size: smaller;
            font-weight: bold;
        }
    </style>

    <script type="text/javascript" src="plugins/Bootstrap-Slider/bootstrap-slider.min.js"></script>
    <script type="text/javascript" src="scripts/bootstrap-select.min.js"></script>
    <script type="text/javascript" src="scripts/fileinput.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js" type="text/javascript"></script>

    <script type="text/javascript">
        function pageLoad() {
            initializeFormTab();
            initializeFormControls();
        }

        function initializeFormTab() {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "tabExclude";
            $('.nav-tabs a[href="#' + tabName + '"]').tab('show');
        };

        $(document).ready(function () {
            $(".nav-tabs a").click(function () {
                $(this).tab('show');
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                initializeFormControls();
                attachDatepicker();

            }
        });


        function initializeFormControls() {
            $('.selectpicker').selectpicker();
            $('.fileinput').fileinput();
            $.app.initCheckbox();
            $.app.initDateTimePicker();
        };
        function showGenericMessageModal(type, message) {
            swal('', message, type);
        }

        //$(document).ready(function () {
        //    initializeFormControls();
        //});

        //check numeric
        function onlyNum() {
            var carCode = event.keyCode;
            if ((carCode < 48) || (carCode > 57)) {
                alert('Enter only number !');
                event.returnValue = false;
            }
        }

        //check numeric
        function CheckNumeric() {
            return event.keyCode >= 48 && event.keyCode <= 57 || event.key == '.';
        }


        $(function () {
            attachDatepicker()
        });
        function attachDatepicker() {
            $('#<%=txtStartDt.ClientID %>').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: "dd-M-yy",
                //yearRange: "c-100:c"
            });
            $('#<%=txtToDate.ClientID %>').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: "dd-M-yy",
                //yearRange: "c-100:c"
            });
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <div class="control-panel box blue">
                <div class="control-panel-title">
                    <div class="caption">
                        <i class="fa fa-cogs "></i><span class="caption-subject bold uppercase">Employee Master Data Management</span>
                    </div>
                </div>
                <div class="control-panel-body">

                    <asp:UpdateProgress ID="updtProgressPnlMain" AssociatedUpdatePanelID="updtPnlMain" runat="server">
                        <ProgressTemplate>
                            <div class="divWaiting">
                                <center>
                                    <div style="text-align: center" class="loader_body">
                                        <img src="Images/loader.gif" alt="Loading..."  id="ifMobile1"/>
                                    </div>
                                </center>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                    <asp:UpdatePanel ID="updtPnlMain" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="control-panel primary ">
                                <div class="control-panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-body">
                                            <div class="row">
                                                <div class="col-md-12 text-right" style="padding-bottom: 10px; padding-right: 0px;">
                                                    <asp:LinkButton ID="lnkRefresh" runat="server" Style="align-content: center; background-color: transparent; border: none; padding: 15px" data-toggle="tooltip" ToolTip="Refresh All"><i class="fa fa-refresh" style="color: dodgerblue; font-size:20px;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>

                                            <div class="panel-heading">
                                                <asp:HiddenField ID="TabName" runat="server" />
                                                <ul class="nav nav-tabs" style="border: none;">
                                                    <li class="active"><a href="#tabExclude" class="bg-color" data-toggle="tab" id="Exclu"><b>Exclude Employee</b></a></li>
                                                    <li><a href="#tabInclude" class="bg-color" data-toggle="tab" id="Include"><b>Include Employee</b></a></li>
                                                    <li><a href="#tabDataManagement" class="bg-color" data-toggle="tab" id="EmplDtMgt"><b>Employee Data Management</b></a></li>
                                                    <li><a href="#tabSurveyMgt" class="bg-color" data-toggle="tab" id="SurveyMgt"><b>Survey Status Management</b></a></li>
                                                    <%--<li><a href="#tabSend" class="bg-color" data-toggle="tab" id="SndRemd"><b>Send Reminder Mail</b></a></li>--%>
                                                    <li><a href="#tabAdmin" class="bg-color" data-toggle="tab" id="Sup Admin"><b>Super Admin Role</b></a></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel-body">
                                        <div class="tab-content clearfix">
                                            <div class="tab-pane active" id="tabExclude">
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        <label class="control-label">Year<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:DropDownList ID="ddlExYr" runat="server" CssClass="selectpicker" data-width="100%"
                                                            data-live-search="true">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvExYr" runat="server" ControlToValidate="ddlExYr" ErrorMessage="*Required" Display="Dynamic" CssClass="error" ValidationGroup="vgDisplay" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <label class="control-label">Cycle<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:DropDownList ID="ddlExCycle" runat="server" CssClass="selectpicker" data-width="100%"
                                                            data-live-search="true">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvExCycle" runat="server" ControlToValidate="ddlExCycle" ErrorMessage="*Required" Display="Dynamic" CssClass="error" ValidationGroup="vgDisplay" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-1" style="padding-right: 0px; padding-top: 8px;">
                                                        <label class="control-label">Executive Head</label>
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:DropDownList ID="ddlExecHd" runat="server" CssClass="selectpicker" data-width="100%"
                                                            data-live-search="true" AutoPostBack="true" OnSelectedIndexChanged="ddlExecHd_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="row" style="padding-top: 15px;">
                                                    <div class="col-md-1" style="padding-top: 8px;">
                                                        <label class="control-label">Level</label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:DropDownList ID="ddlExLevel" runat="server" CssClass="selectpicker" data-width="100%"
                                                            data-live-search="true" AutoPostBack="true" OnSelectedIndexChanged="ddlExLevel_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-1" style="padding-top: 8px;">
                                                        <label class="control-label">BUHR P No.</label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox runat="server" ID="txtBuhr" CssClass="form-control" onkeypress="onlyNum();" MaxLength="6" placeholder="Enter BUHR P No." AutoPostBack="true" OnTextChanged="txtBuhr_TextChanged" />
                                                    </div>
                                                    <div class="col-md-1 text-center" style="padding-top: 8px;">
                                                        <label class="control-label">Personal No.</label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox runat="server" ID="txtPerno" CssClass="form-control" onkeypress="onlyNum();" MaxLength="6" placeholder="Enter P No." AutoPostBack="true" OnTextChanged="txtPerno_TextChanged" />
                                                    </div>
                                                </div>
                                                <div class="row" style="padding-top: 20px;">
                                                    <div class="col-md-2 col-md-offset-4" style="margin-right: 0px;">
                                                        <asp:Button ID="btnDisplay" runat="server" Text="Search" CssClass="btn btn-success" ForeColor="White" Width="100%" CausesValidation="true" ValidationGroup="vgDisplay" />
                                                    </div>
                                                    <div class="col-md-2" style="margin-left: 0px;">
                                                        <asp:Button ID="btnAdd" runat="server" Text="Exclude Record" CssClass="btn aqua-gradient" BackColor="#25bac5" ForeColor="White" Width="100%" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="GvExclude" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
                                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                            <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                                            <AlternatingRowStyle BackColor="#DCDCDC" />
                                                            <Columns>

                                                                <asp:TemplateField HeaderText="Select">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox runat="server" ID="chksel" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Year">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblYear" runat="server" Text='<%# Eval("ema_year")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Cycle">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCycle" runat="server" Text='<%# Eval("ema_cycle")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="P No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblpno" runat="server" Text='<%# Eval("ema_perno")%>'></asp:Label>
                                                                        <asp:Label ID="lblComp" runat="server" Text='<%# Eval("ema_comp_code")%>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblname" runat="server" Text='<%# Eval("ema_ename")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Level">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllevel" runat="server" Text='<%# Eval("ema_empl_sgrade")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Equivalent Level">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEqvlvl" runat="server" Text='<%# Eval("ema_eqv_level")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Department">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldept" runat="server" Text='<%# Eval("ema_dept_desc")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Email ID">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblemail" runat="server" Text='<%# Eval("ema_email_id")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                            <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                            <SortedDescendingHeaderStyle BackColor="#000065" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane" id="tabInclude">
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        <label class="control-label">Year<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:DropDownList ID="ddlIncYr" runat="server" CssClass="selectpicker" data-width="100%"
                                                            data-live-search="true">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvIncYr" runat="server" ControlToValidate="ddlIncYr" ErrorMessage="*Required" Display="Dynamic" CssClass="error" ValidationGroup="vgShow" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <label class="control-label">Cycle<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:DropDownList ID="ddlIncCycle" runat="server" CssClass="selectpicker" data-width="100%"
                                                            data-live-search="true">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvIncCyc" runat="server" ControlToValidate="ddlIncCycle" ErrorMessage="*Required" Display="Dynamic" CssClass="error" ValidationGroup="vgShow" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-1" style="padding-right: 0px; padding-top: 8px;">
                                                        <label class="control-label">Executive Head</label>
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:DropDownList ID="ddlExecutive" runat="server" CssClass="selectpicker" data-width="100%"
                                                            data-live-search="true" AutoPostBack="true" OnSelectedIndexChanged="ddlExecutive_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="row" style="padding-top: 15px;">
                                                    <div class="col-md-1" style="padding-top: 8px;">
                                                        <label class="control-label">Level</label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:DropDownList ID="ddlIncLvl" runat="server" CssClass="selectpicker" data-width="100%"
                                                            data-live-search="true" AutoPostBack="true" OnSelectedIndexChanged="ddlIncLvl_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-1" style="padding-top: 8px;">
                                                        <label class="control-label">BUHR P No.</label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox runat="server" ID="txtBUR" CssClass="form-control" onkeypress="onlyNum();" MaxLength="6" placeholder="Enter BUHR P No." AutoPostBack="true" OnTextChanged="txtBUR_TextChanged" />
                                                    </div>
                                                    <div class="col-md-1 text-center" style="padding-top: 8px;">
                                                        <label class="control-label">Personal No.</label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox runat="server" ID="txtPNo" CssClass="form-control" onkeypress="onlyNum();" MaxLength="6" placeholder="Enter P No." AutoPostBack="true" OnTextChanged="txtPNo_TextChanged" />
                                                    </div>
                                                </div>
                                                <div class="row" style="padding-top: 20px;">
                                                    <div class="col-md-2 col-md-offset-4" style="margin-right: 0px;">
                                                        <asp:Button ID="btnShow" runat="server" Text="Search" CssClass="btn btn-success" ForeColor="White" Width="100%" CausesValidation="true" ValidationGroup="vgShow" />
                                                    </div>
                                                    <div class="col-md-2" style="margin-left: 0px;">
                                                        <asp:Button ID="btnRemove" runat="server" Text="Include Record" CssClass="btn aqua-gradient" BackColor="#25bac5" ForeColor="White" Width="100%" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="GvInclude" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
                                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                            <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                                            <AlternatingRowStyle BackColor="#DCDCDC" />
                                                            <Columns>

                                                                <asp:TemplateField HeaderText="Select">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox runat="server" ID="chkseln" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Year">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblYr" runat="server" Text='<%# Eval("ema_year")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Cycle">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCyle" runat="server" Text='<%# Eval("ema_cycle")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="P No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblperno" runat="server" Text='<%# Eval("ema_perno")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblename" runat="server" Text='<%# Eval("ema_ename")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Level">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllvl" runat="server" Text='<%# Eval("ema_empl_sgrade")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Equivalent Level">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEqvl" runat="server" Text='<%# Eval("ema_eqv_level")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Department">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldepart" runat="server" Text='<%# Eval("ema_dept_desc")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                            </Columns>
                                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                            <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                            <SortedDescendingHeaderStyle BackColor="#000065" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane" id="tabAdmin">
                                                <div class="row">
                                                    <div class="margintop">
                                                        <div class="col-md-1" style="padding-right: 0px;">
                                                            <label class="control-label">Personal No.<span style="color: red; font-weight: bold; font-size: 14pt">*</span></label>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:TextBox runat="server" ID="txtSuperAdmin" CssClass="form-control" placeholder="Enter P No." MaxLength="10" />
                                                            <asp:RequiredFieldValidator ID="rfvadmin" runat="server" ControlToValidate="txtSuperAdmin" ErrorMessage="*Required" Display="Dynamic" CssClass="error" ValidationGroup="vgAdmin"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" style="padding-top: 25px;">
                                                    <div class="col-md-2 col-md-offset-3" style="margin-right: 0px;">
                                                        <asp:Button ID="btnDisAdm" runat="server" Text="Search" CssClass="btn btn-success" ForeColor="White" Width="100%" CausesValidation="true" ValidationGroup="vgAdmin" />
                                                    </div>
                                                    <div class="col-md-2" style="margin-right: 0px;">
                                                        <asp:Button ID="btnSave" runat="server" Text="Add" CssClass="btn aqua-gradient" BackColor="#25bac5" ForeColor="White" Width="100%" CausesValidation="true" ValidationGroup="vgAdmin" />
                                                    </div>
                                                    <div class="col-md-2" style="margin-left: 0px;">
                                                        <asp:Button ID="btnDelete" runat="server" Text="Remove" CssClass="btn btn-danger" ForeColor="White" Width="100%" Enabled="false" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="gvSuperAdm" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
                                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                            <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                                            <AlternatingRowStyle BackColor="#DCDCDC" />
                                                            <Columns>

                                                                <asp:TemplateField HeaderText="Select">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox runat="server" ID="chkAdmim" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcd" runat="server" Text='<%# Eval("irc_code")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Description">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldescr" runat="server" Text='<%# Eval("irc_desc")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Group ID">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("igp_group_id")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                            </Columns>
                                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                            <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                            <SortedDescendingHeaderStyle BackColor="#000065" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane" id="tabDataManagement">
                                                <div class="row">
                                                    <div class="col-md-5">
                                                        <asp:RadioButtonList ID="rblupdate" runat="server" RepeatDirection="Horizontal" BorderStyle="None" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="rblupdate_SelectedIndexChanged">
                                                            <asp:ListItem Value="1">&nbsp;Employee Wise&nbsp;&nbsp;</asp:ListItem>
                                                            <asp:ListItem Value="2">&nbsp;Step Date Wise</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </div>
                                                <br />
                                                <div id="UpdateEmployeeData" runat="server">
                                                    <div class="row">
                                                        <div class="col-md-1 text-center">
                                                            <label class="control-label">Year<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:DropDownList ID="ddlDMYear" runat="server" CssClass="selectpicker" data-width="100%"
                                                                data-live-search="true">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvyear" runat="server" ControlToValidate="ddlDMYear" ErrorMessage="*Required" Display="Dynamic" CssClass="error" ValidationGroup="vgSearch" InitialValue="0"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-1 text-center">
                                                            <label class="control-label">Cycle<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:DropDownList ID="ddlDMCycle" runat="server" CssClass="selectpicker" data-width="100%"
                                                                data-live-search="true">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvCycle" runat="server" ControlToValidate="ddlDMCycle" ErrorMessage="*Required" Display="Dynamic" CssClass="error" ValidationGroup="vgSearch" InitialValue="0"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-1 text-center" style="padding-left: 0px;">
                                                            <label class="control-label">Personal No.<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:TextBox runat="server" ID="txtPersonalno" CssClass="form-control" MaxLength="6" onkeypress="onlyNum();" placeholder="Enter P No." />
                                                            <asp:RequiredFieldValidator ID="rfvpno" runat="server" ControlToValidate="txtPersonalno" ErrorMessage="*Required" Display="Dynamic" CssClass="error" ValidationGroup="vgSearch"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                                                                  
                                                    <div class="row" style="padding-top: 20px;">
                                                        <div class="col-md-2 col-md-offset-4" style="margin-right: 0px;">
                                                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-success" ForeColor="White" Width="100%" CausesValidation="true" ValidationGroup="vgSearch" />
                                                        </div>
                                                        <div class="col-md-2" style="margin-left: 0px;">
                                                            <asp:Button ID="btn_clear" runat="server" Text="Clear" CssClass="btn purple-gradient" BackColor="#915c99" ForeColor="White" Width="100%" />
                                                        </div>
                                                    </div>
                                                    </br>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <asp:GridView ID="GridMstrDataMgt" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" OnRowEditing="GridMstrDataMgt_RowEditing"
                                                                OnRowUpdating="GridMstrDataMgt_RowUpdating" OnRowCancelingEdit="GridMstrDataMgt_RowCancelingEdit">
                                                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                                <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                                                <AlternatingRowStyle BackColor="#DCDCDC" />
                                                                <Columns>

                                                                    <asp:TemplateField HeaderText="P No.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_perno" runat="server" Text='<%# Eval("ema_perno")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_ename" runat="server" Text='<%# Eval("ema_ename")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Year">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_Yr" runat="server" Text='<%# Eval("ema_year")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Cycle">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_Cyle" runat="server" Text='<%# Eval("ema_cycle")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Department">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_Dept" runat="server" Text='<%# Eval("ema_dept_desc")%>'></asp:Label>
                                                                            <asp:HiddenField ID="hdndpt" runat="server" Value='<%# Eval("ema_dept_code") %>' />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddl_Dept" runat="server" CssClass="form-control">
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Designation">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_Desgn" runat="server" Text='<%# Eval("ema_desgn_desc")%>'></asp:Label>
                                                                            <asp:HiddenField ID="hdndesgn" runat="server" Value='<%# Eval("ema_desgn_code") %>' />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddl_Desgn" runat="server"  CssClass="form-control">
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Email ID" ItemStyle-Width="20%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_email" runat="server" Text='<%# Eval("ema_email_id")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txt_email" runat="server" Text='<%#Eval("ema_email_id") %>' CssClass="form-control" Width="100%"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="revemail" runat="server"
                                                                                ErrorMessage="Please Enter a valid Email ID" ControlToValidate="txt_email" CssClass="error" ValidationGroup="vgUpdatelist" Display="Dynamic" SetFocusOnError="true"
                                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Eqv Level">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_eqvlvl" runat="server" Text='<%# Eval("ema_eqv_level")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddl_eqvlvl" runat="server"  CssClass="form-control">
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Reporting P No.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_reporting" runat="server" Text='<%# Eval("ema_reporting_to_pno")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txt_ReptPNo" runat="server" CssClass="form-control" onkeypress="return CheckNumeric();"  MaxLength="6" Text='<%#Eval("ema_reporting_to_pno") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Dotted P No.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_Dotted" runat="server" Text='<%# Eval("ema_dotted_pno")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txt_Dotted" runat="server" CssClass="form-control" onkeypress="return CheckNumeric();" MaxLength="6" Text='<%#Eval("ema_dotted_pno") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Edit" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkbtnEdit" Text="Edit" CommandName="Edit" runat="server" CssClass="btn btn-primary"><i class="fa fa-pencil-square-o fa"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:LinkButton ID="btn_Update" runat="server" Text="Update" CommandName="Update" CssClass="btn btn-success" CausesValidation="true" ValidationGroup="vgUpdatelist"><i class="fa fa-floppy-o fa"></i></asp:LinkButton>
                                                                            <asp:LinkButton ID="lnkbtncancel" runat="server" Text="Cancel" CommandName="Cancel" CssClass="btn btn-danger"><i class="fa fa-times"></i></asp:LinkButton>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                                <SortedDescendingHeaderStyle BackColor="#000065" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" id="StepUpdataData" runat="server">
                                                    <div class="col-md-1">
                                                        <label class="control-label">Level<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:DropDownList ID="ddlDMlevel" runat="server" CssClass="selectpicker" data-width="100%"
                                                            data-live-search="true" AutoPostBack="true" OnSelectedIndexChanged="ddlDMlevel_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvdmlvl" runat="server" ControlToValidate="ddlDMlevel" ErrorMessage="*Required" Display="Dynamic" CssClass="error" ValidationGroup="vgView" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-1" style="padding-right: 0px; padding-top: 5px;">
                                                        <label class="control-label">Equivalent level</label>
                                                    </div>
                                                    <div class="col-md-2 text-center">
                                                        <asp:DropDownList ID="ddlDMEqv" runat="server" CssClass="selectpicker" data-width="100%"
                                                            data-live-search="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <label class="control-label">Step<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:DropDownList ID="ddlStep" runat="server" CssClass="selectpicker" data-width="100%"
                                                            data-live-search="true" AutoPostBack="true" OnSelectedIndexChanged="ddlStep_SelectedIndexChanged">
                                                            <asp:ListItem Text="Please Select" Value="0" />
                                                            <asp:ListItem Text="Step 1" Value="ST1" />
                                                            <asp:ListItem Text="Step 2" Value="ST2" />
                                                            <asp:ListItem Text="Step 3" Value="ST3" />
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvstep" runat="server" ControlToValidate="ddlStep" ErrorMessage="*Required" Display="Dynamic" CssClass="error" ValidationGroup="vgView" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="row" style="padding-top: 15px;" id="Stepdates" runat="server">
                                                     <div class="col-md-1" style="padding-top: 8px;">
                                                        <label class="control-label">Start Date<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="txtStartDt" data-placement="top"
                                                            CssClass="form-control cursor-pointer" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <label class="control-label">End Date<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="txtToDate" data-placement="top"
                                                            CssClass="form-control cursor-pointer" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row" style="padding-top: 20px;" id="Stepbtn" runat="server">
                                                    <div class="col-md-2 col-md-offset-4" style="margin-right: 0px;">
                                                        <asp:Button ID="btn_view" runat="server" Text="Search" CssClass="btn btn-success" ForeColor="White" Width="100%" CausesValidation="true" ValidationGroup="vgView" />
                                                    </div>
                                                    <div class="col-md-2" style="margin-left: 0px;">
                                                        <asp:Button ID="btnupdate" runat="server" Text="Update" CssClass="btn aqua-gradient" BackColor="#25bac5" ForeColor="White" Width="100%" Enabled="false" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="GridStep" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" AllowPaging="true" PageSize="20" OnPageIndexChanging="GridStep_PageIndexChanging">
                                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                            <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                                            <AlternatingRowStyle BackColor="#DCDCDC" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ema_year" HeaderText="Year" />
                                                                <asp:BoundField DataField="ema_cycle" HeaderText="Cycle" />
                                                                <asp:BoundField DataField="ema_perno" HeaderText="P No." />
                                                                <asp:BoundField DataField="ema_ename" HeaderText="Name" />
                                                                <asp:BoundField DataField="ema_dept_desc" HeaderText="Department" />
                                                                <asp:BoundField DataField="ema_empl_sgrade" HeaderText="level" />
                                                                <asp:BoundField DataField="ema_eqv_level" HeaderText="Eqv level" />
                                                                <asp:BoundField DataField="ema_step1_stdt" HeaderText="Step1 Start Date" />
                                                                <asp:BoundField DataField="ema_step1_enddt" HeaderText="Step1 End Date" />
                                                                <asp:BoundField DataField="ema_step2_stdt" HeaderText="Step2 Start Date" />
                                                                <asp:BoundField DataField="ema_step2_enddt" HeaderText="Step2 End Date" />
                                                                <asp:BoundField DataField="ema_step3_stdt" HeaderText="Step3 Start Date" />
                                                                <asp:BoundField DataField="ema_step3_enddt" HeaderText="Step3 End Date" />
                                                            </Columns>
                                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                            <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                            <SortedDescendingHeaderStyle BackColor="#000065" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane" id="tabSurveyMgt">
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        <label class="control-label">Year<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:DropDownList ID="ddlSurYr" runat="server" CssClass="selectpicker" data-width="100%"
                                                            data-live-search="true">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvSurYr" runat="server" ControlToValidate="ddlSurYr" ErrorMessage="*Required" Display="Dynamic" CssClass="error" ValidationGroup="vgSurvey" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <label class="control-label">Cycle<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:DropDownList ID="ddlSurCycle" runat="server" CssClass="selectpicker" data-width="100%"
                                                            data-live-search="true">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvSurCyc" runat="server" ControlToValidate="ddlSurCycle" ErrorMessage="*Required" Display="Dynamic" CssClass="error" ValidationGroup="vgSurvey" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-1" style="padding-right: 0px; padding-top: 8px;">
                                                        <label class="control-label">Personal No.<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox runat="server" ID="txtSurPno" CssClass="form-control" onkeypress="onlyNum();" MaxLength="6" placeholder="Enter P No." />
                                                        <asp:RequiredFieldValidator ID="rfvurPno" runat="server" ControlToValidate="txtSurPno" ErrorMessage="*Required" Display="Dynamic" CssClass="error" ValidationGroup="vgSurvey"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-top: 25px;">
                                                    <div class="col-md-2 col-md-offset-3" style="margin-right: 0px;">
                                                        <asp:Button ID="btnSurDisplay" runat="server" Text="Search" CssClass="btn btn-success" ForeColor="White" Width="100%" CausesValidation="true" ValidationGroup="vgSurvey" />
                                                    </div>
                                                    <div class="col-md-2" style="margin-right: 0px;">
                                                        <asp:Button ID="btnSurEdit" runat="server" Text="Edit" CssClass="btn aqua-gradient" BackColor="#25bac5" ForeColor="White" Width="100%" />
                                                    </div>
                                                    <div class="col-md-2" style="margin-left: 0px;">
                                                        <asp:Button ID="btnSurRemove" runat="server" Text="Remove" CssClass="btn btn-danger" ForeColor="White" Width="100%" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="GridSurvey" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
                                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                            <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                                            <AlternatingRowStyle BackColor="#DCDCDC" />
                                                            <Columns>

                                                                <asp:TemplateField HeaderText="Select">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox runat="server" ID="chkSurvey" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Year">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSurYear" runat="server" Text='<%# Eval("ss_year")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Cycle">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSurCycle" runat="server" Text='<%# Eval("ss_srlno")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Assessment PNo.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAssPno" runat="server" Text='<%# Eval("ss_asses_pno")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Feedback PNo.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFdPno" runat="server" Text='<%# Eval("ss_pno")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Feedback Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFdname" runat="server" Text='<%# Eval("ss_name")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Category">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCatg" runat="server" Text='<%# Eval("ss_categ")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                            </Columns>
                                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
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
                                </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

