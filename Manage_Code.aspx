<%@ Page Title="" Language="VB" MasterPageFile="~/AdminMaster.master" AutoEventWireup="false" CodeFile="Manage_Code.aspx.vb" Inherits="Manage_Code" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
    <script src="//code.jquery.com/jquery-1.11.0.min.js"></script>
    <link href='https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/ui-lightness/jquery-ui.css'
        rel='stylesheet'>
        <link href="plugins/Bootstrap-datetimepicker/css/bootstrap-datetimepicker.css" rel="stylesheet" />
    <script src="plugins/Bootstrap-datetimepicker/js/moment.js"></script>
    <script src="plugins/Bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js"></script>
    
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

    <%-- Ajax Calender Control CSS --%>

    <script type="text/javascript">
        function pageLoad()
        {
            
      $('#<%=txtIRC_End_Date.ClientID %>').datepicker({
          language: 'pt-BR',
          dateFormat: 'dd/mm/yy'
      });
      $('#<%=txtIRC_Start_Date.ClientID %>').datepicker({
          language: 'pt-BR',
          dateFormat: 'dd/mm/yy'
    });

        }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMain" runat="server" >
        <ContentTemplate>
           <div class="container-fluid">
                <div class="panel-body">
                    <div class="row form-group p-1">
                        
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Type<span class="text-danger">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlType" Enabled="true" AutoPostBack="true" CssClass="form-control"  />
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Code<span class="text-danger">*</span></label>
                            <%--<asp:TextBox runat="server" ID="txt_Code" Enabled="true" CssClass="form-control" />--%>
                            <asp:DropDownList runat="server" ID="ddl_Code" Enabled="true"  CssClass="form-control"/>
                        </div>
                        <div class="col-md-2 text-left" style="display:none;">
                            <label class="font-weight-bold">Start Date<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txt_Start_Date" CssClass="form-control" MaxLength="10" placeholder="Start Date"></asp:TextBox>
                            <%--<cc1:CalendarExtender ID="CEStart_Date" runat="server" TargetControlID="txt_Start_Date" PopupPosition="BottomLeft" Format="dd/MM/yyyy"></cc1:CalendarExtender>--%>
                        </div>
                         <div class="col-md-2 text-left" style="display:none;">
                            <label class="font-weight-bold">End Date<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txtEnd_Date" CssClass="form-control" MaxLength="10" placeholder="Respondent P.No"></asp:TextBox>
                             <%--<cc1:CalendarExtender ID="CE_EndDate" runat="server" TargetControlID="txtEnd_Date" PopupPosition="BottomLeft" Format="dd/MM/yyyy"></cc1:CalendarExtender>--%>
                        </div>
                        <div class="col-md-2 text-left">
                            <div style="margin-top:7px;">
                                <asp:Button runat="server" ID="btnSearch" Text="Search" class ="btn btn-warning btn-md mt-4 text-white" ></asp:Button>
                            </div>                            
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                            <div class="container">
                                <div class="row">
                                    <div class="col-md-12 col-lg-12">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gdvManageCode" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer" Font-Names="verdana"
                                                EmptyDataText="No Record Found" BackColor="#ffccff" BorderColor="Black" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" PageSize="50" AllowPaging="true" RowStyle-CssClass="rows" HeaderStyle-Wrap="false">
                                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#FFB6C1" Font-Bold="True" ForeColor="Black" />
                                                <AlternatingRowStyle BackColor="#FFB6C1" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIRC_TYPE" runat="server" Text='<%# Eval("IRC_TYPE")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Type Desc">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_TYPE_DESC" runat="server" Text='<%# Eval("IRC_TYPE_DESC")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIRC_CODE" runat="server" Text='<%# Eval("IRC_CODE")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Code Desc">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_CODE_DESC" runat="server" Text='<%# Eval("IRC_CODE_DESC")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Code Value/Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIRC_DESC" runat="server" Text='<%# Eval("IRC_DESC")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Start Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIRC_START_DT" Width="100" runat="server" Text='<%# Eval("Start_Date")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="End Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIRC_END_DT" runat="server" Width="100" Text='<%# Eval("End_date")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valid Tag">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblValid_TAG" runat="server" Text='<%# Eval("Valid_TAG")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valid Tag" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIRC_VALID_TAG" runat="server" Text='<%# Eval("IRC_VALID_TAG")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Update Code" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnUpdateCode" runat="server" CommandName="UPDATE" Text="Update" CommandArgument='<%# Eval("IRC_CODE")%>'></asp:Button>
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
                            </div>
                        </div>
            <div class="container-fluid">
                <div class="panel-body">
                    <div class="row form-group p-1">
                        <div class="col-md-1 text-left">
                            <label class="font-weight-bold">Type<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txtIRC_TYPE" Enabled="true" CssClass="form-control" />
                            <asp:DropDownList runat="server" ID="ddl_IRC_TYPE" Enabled="true" CssClass="form-control" Visible="false" />
                        </div>
                        <div class="col-md-2 text-left">
                            <label class="font-weight-bold">Type Desc<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txt_TYPE_DESC" Enabled="true" CssClass="form-control" />
                            <asp:DropDownList runat="server" ID="DropDownList1" Enabled="true" CssClass="form-control" Visible="false" />
                        </div>
                        <div class="col-md-1 text-left">
                            <label class="font-weight-bold">Code<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txtIRC_Code" Enabled="true" CssClass="form-control" />
                        </div>
                        <div class="col-md-2 text-left">
                            <label class="font-weight-bold">Code Desc<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txt_CODE_DESC" Enabled="true" CssClass="form-control" />
                        </div>
                        <div class="col-md-2 text-left">
                            <label class="font-weight-bold">Start Date<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txtIRC_Start_Date" CssClass="form-control" MaxLength="10" autocomplete="off" placeholder="IRC State Date"></asp:TextBox>
                            
                        </div>
                         <div class="col-md-2 text-left">
                            <label class="font-weight-bold">End Date<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txtIRC_End_Date" data-format="dd/MM/yyyy" autocomplete="off" CssClass="form-control" MaxLength="10" placeholder="IRC End Date"></asp:TextBox>
                             
                        </div>
                        <div class="col-md-2 text-left">
                            <label class="font-weight-bold">Code Value/Description<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txtIRC_Description" CssClass="form-control"  placeholder="IRC Description" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <div class="col-md-2 text-left">
                            <label class="font-weight-bold">Valid Tag<span class="text-danger">*</span></label>
                            <asp:DropDownList runat="server" ID="ddl_Valid_Tag" Enabled="true" CssClass="form-control" >
                                <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                <asp:ListItem Text="Activate" Value="A"></asp:ListItem>
                                <asp:ListItem Text="Deactivate" Value="D"></asp:ListItem>
                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                
                                </asp:DropDownList>
                        </div>
                        <div class="col-md-4 text-left">
                            <div style="margin-top:7px;">
                                <asp:Button runat="server" ID="btn_Action" Text="Add New Code" class ="btn btn-warning btn-md mt-4" ></asp:Button>
                                <asp:Button runat="server" ID="btnReset" Text="Reset" class ="btn btn-warning btn-md mt-4" ></asp:Button>
                            </div>    
                                                  
                        </div>
                        
                    </div>
                </div>
            </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>