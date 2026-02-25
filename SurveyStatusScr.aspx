<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SurveyStatusScr.aspx.vb" Inherits="SurveyStatusScr" %>

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
          <%--  $('#<%=txtStartDt.ClientID %>').datepicker({
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
            });--%>
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <div class="control-panel box blue">
                <div class="control-panel-title">
                    <div class="caption">
                        <i class="fa fa-cogs "></i><span class="caption-subject bold uppercase">Survey Status Data Screen</span>
                    </div>
                </div>
                <div class="control-panel-body">

                    <asp:UpdateProgress ID="updtProgressPnlMain" AssociatedUpdatePanelID="updtPnlMain" runat="server">
                        <ProgressTemplate>
                            <div class="divWaiting">
                                <center>
                                    <div style="text-align: center" class="loader_body">
                                        <img src="Images/loader.gif" alt="Loading..." id="ifMobile1" />
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
                                                        <asp:TextBox runat="server" ReadOnly="true" ID="txtYear" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <label class="control-label">Cycle<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox runat="server" ReadOnly="true" ID="txtCycle" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <label class="control-label">Asses_Pno<span style="color: red; font-weight: bold; font-size: 12pt">*</span></label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox runat="server" ID="txtAssesPno" CssClass="form-control"></asp:TextBox>
                                                    </div>

                                                    <div class="col-md-2">
                                                        <asp:Button runat="server" Text="Search" ID="btnScrh" OnClick="btnScrh_Click" CssClass="btn btn-success" />
                                                    </div>
                                                </div>
                                                <div class="row" style="overflow: auto; margin-top: 40px;">
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="GvSurStat" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" OnRowEditing="GvSurStat_RowEditing"
                                                            OnRowUpdating="GvSurStat_RowUpdating" OnRowCancelingEdit="GvSurStat_RowCancelingEdit">
                                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                            <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                                            <AlternatingRowStyle BackColor="#DCDCDC" />
                                                            <Columns>

                                                                <asp:TemplateField HeaderText="SS_YEAR">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_YEAR" runat="server" Text='<%# Eval("SS_YEAR")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SS_ASSES_PNO">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_ASSES_PNO" runat="server" Text='<%# Eval("SS_ASSES_PNO")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SS_CATEG">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_CATEG" runat="server" Text='<%# Eval("SS_CATEG")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SS_ID">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_ID" runat="server" Text='<%# Eval("SS_ID")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SS_PNO">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_PNO" runat="server" Text='<%# Eval("SS_PNO")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SS_NAME">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_NAME" runat="server" Text='<%# Eval("SS_NAME")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SS_INTSH_OTP">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_INTSH_OTP" runat="server" Text='<%# Eval("SS_INTSH_OTP")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SS_EMAIL" ItemStyle-Width="20%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_EMAIL" runat="server" Text='<%# Eval("SS_EMAIL")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_email" runat="server" Text='<%#Eval("SS_EMAIL") %>' CssClass="form-control" Width="100%" MaxLength="100"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="revemail" runat="server"
                                                                            ErrorMessage="Please Enter a valid Email ID" ControlToValidate="txt_email" ValidationGroup="vgUpdatelist" CssClass="error" Display="Dynamic" SetFocusOnError="true"
                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SS_STATUS" ItemStyle-Width="20%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_STATUS" runat="server" Text='<%# Eval("SS_STATUS")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_SS_STATUS" runat="server" Text='<%#Eval("SS_STATUS") %>' CssClass="form-control" Width="100%" MaxLength="2"></asp:TextBox>

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SS_TAG" ItemStyle-Width="20%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_TAG" runat="server" Text='<%# Eval("SS_TAG")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_SS_TAG" runat="server" Text='<%#Eval("SS_TAG") %>' CssClass="form-control" Width="100%" MaxLength="2"></asp:TextBox>

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="SS_DEL_TAG" ItemStyle-Width="20%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_DEL_TAG" runat="server" Text='<%# Eval("SS_DEL_TAG")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_SS_DEL_TAG" runat="server" Text='<%#Eval("SS_DEL_TAG") %>' CssClass="form-control" Width="100%" MaxLength="2"></asp:TextBox>

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="SS_APP_DT" ItemStyle-Width="100px" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_APP_DT" runat="server" Text='<%# Eval("SS_APP_DT")%>' Width="100px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_SS_APP_DT" runat="server" Text='<%#Eval("SS_APP_DT") %>' CssClass="form-control" Width="100%"></asp:TextBox>
                                                                        <ajax:CalendarExtender ID="CalAppDt" runat="server" TargetControlID="txt_SS_APP_DT" Format="dd/MM/yyyy"
                                                                            PopupPosition="BottomLeft">
                                                                        </ajax:CalendarExtender>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="SS_LEVEL" ItemStyle-Width="20%" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_LEVEL" runat="server" Text='<%# Eval("SS_LEVEL")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_SS_LEVEL" runat="server" Text='<%#Eval("SS_LEVEL") %>' CssClass="form-control" Width="100%" MaxLength="20"></asp:TextBox>

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="SS_APPROVER" ItemStyle-Width="20%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_APPROVER" runat="server" Text='<%# Eval("SS_APPROVER")%>' Width="150px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_SS_APPROVER" runat="server" Text='<%#Eval("SS_APPROVER") %>' CssClass="form-control" Width="100%" MaxLength="12"></asp:TextBox>

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="SS_APP_TAG" ItemStyle-Width="20%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_APP_TAG" runat="server" Text='<%# Eval("SS_APP_TAG")%>' ></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_SS_APP_TAG" runat="server" Text='<%#Eval("SS_APP_TAG") %>' CssClass="form-control" Width="100%" MaxLength="2"></asp:TextBox>

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="SS_TAG_DT" ItemStyle-Width="35%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_TAG_DT" runat="server" Text='<%# Eval("SS_TAG_DT")%>' Width="100px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_SS_TAG_DT" runat="server" Text='<%#Eval("SS_TAG_DT") %>' CssClass="form-control" Width="100%"></asp:TextBox>
                                                                        <ajax:CalendarExtender ID="CalTagDt" runat="server" TargetControlID="txt_SS_TAG_DT" Format="dd/MM/yyyy"
                                                                            PopupPosition="BottomLeft">
                                                                        </ajax:CalendarExtender>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="SS_WFL_STATUS" ItemStyle-Width="20%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_WFL_STATUS" runat="server" Text='<%# Eval("SS_WFL_STATUS")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_SS_WFL_STATUS" runat="server" Text='<%#Eval("SS_WFL_STATUS") %>' CssClass="form-control" Width="100%" MaxLength="2"></asp:TextBox>

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="SS_FLAG1" ItemStyle-Width="20%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_FLAG1" runat="server" Text='<%# Eval("SS_FLAG1")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_SS_FLAG1" runat="server" Text='<%#Eval("SS_FLAG1") %>' CssClass="form-control" Width="100%" MaxLength="1"></asp:TextBox>

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                  <asp:TemplateField HeaderText="SS_FLAG_WOTP" ItemStyle-Width="20%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_FLAG_WOTP" runat="server" Text='<%# Eval("SS_FLAG_WOTP")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_SS_FLAG_WOTP" runat="server" Text='<%#Eval("SS_FLAG_WOTP") %>' CssClass="form-control" Width="100%" MaxLength="1"></asp:TextBox>

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                 <asp:TemplateField HeaderText="SS_OTP_COUNT" ItemStyle-Width="20%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_OTP_COUNT" runat="server" Text='<%# Eval("SS_OTP_COUNT")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_SS_OTP_COUNT" runat="server" Text='<%#Eval("SS_OTP_COUNT") %>' CssClass="form-control" Width="100%"></asp:TextBox>

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                   <asp:TemplateField HeaderText="SS_FLAG4" ItemStyle-Width="20%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_SS_FLAG4" runat="server" Text='<%# Eval("SS_FLAG4")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_SS_FLAG4" runat="server" Text='<%#Eval("SS_FLAG4") %>' CssClass="form-control" Width="100%"></asp:TextBox>

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
                                        </div>
                                    </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

