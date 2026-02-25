<%@ Page Title="" Language="VB" MasterPageFile="~/AdminMaster.master" AutoEventWireup="false" CodeFile="Mail_Management.aspx.vb" Inherits="Mail_Management" ValidateRequest="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- Google Fonts -->
    <link href="assets/css/googlefont.css" rel="stylesheet" />

    <!-- Vendor CSS Files -->
    <!-- <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"> -->
    <link href="./assets/vendor/icofont/icofont.min.css" rel="stylesheet">
    <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
    <%-- Start WI368  by Manoj Kumar on 30-05-2021--%>
    <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">
    <%-- WI368 one line added --%>
    <%-- End  by Manoj Kumar on 30-05-2021--%>
    <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
    <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">
    <!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"> -->
    <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
    <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>

    <!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css"> -->

    <!-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script> -->
    <!-- <script src="https://code.jquery.com/ui/1.10.4/jquery-ui.js"></script> -->
    <!-- <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script> -->

    <%--<!-- <link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css"> -->--%>
    <!-- <script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script> -->
    <%--<!-- <script src="//code.jquery.com/jquery-1.11.1.min.js"></script> -->--%>
    <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet">
    <!-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script> -->
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <!-- <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script> -->
    <!-- <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script> -->

    <!-- Template Main CSS File -->
    <link href="assets/css/styleIL3.css" rel="stylesheet">
    <!-- <script src="//code.jquery.com/jquery-1.11.0.min.js"></script> -->
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
            
      $('#<%=txtEnd_Date.ClientID %>').datepicker({
          language: 'pt-BR',
          dateFormat: 'dd/mm/yy'
      });
      $('#<%=txtStart_Date.ClientID %>').datepicker({
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
            <div class="row">
                <div class="col-md-12 text-center">
                 <h1>Mail Management</h1>
                </div>
                 </div>
    <div runat="server" id="div_ext" visible="true">
<div class="row form-group p-1">
                        
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Description<span class="text-danger">*</span> </label>
                            <asp:TextBox runat="server" ID="txtDesc" CssClass="form-control" placeholder="Description" AutoPostBack="true" />
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="txtDesc"
                                                        ServiceMethod="SearchFor_MailADMIN" MinimumPrefixLength="1" CompletionInterval="100"
                                                        DelimiterCharacters="" Enabled="True" ServicePath=""
                                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListCssClass="AutoExtender"
                                                        CompletionListItemCssClass="AutoExtenderList">
                                                    </cc1:AutoCompleteExtender>
                            </div>
    <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Status<span class="text-danger">*</span></label>
                            <asp:DropDownList runat="server" ID="ddl_Status" CssClass="form-control" >
                                 <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                                                        <asp:ListItem Text="Active" Value="A"></asp:ListItem>
                                                                        <asp:ListItem Text="Deactive" Value="D"></asp:ListItem>
                                </asp:DropDownList>
                        </div>
    
                        <div class="col-md-2 text-left">
                            <label class="font-weight-bold">Start Date<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txtStart_Date" CssClass="form-control" autocomplete="off" MaxLength="10" placeholder="Start Date(dd/MM/yyyy)"></asp:TextBox>
                            
                        </div>
                        <div class="col-md-2 text-left">
                            <label class="font-weight-bold">End Date<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txtEnd_Date" CssClass="form-control" autocomplete="off" MaxLength="10" placeholder="End Date(dd/MM/yyyy)"></asp:TextBox>
                            
                        </div>
   
                <div class="form-group" style="text-align: center;">
                        <div class="d-inline-block">
                            <asp:Button ID="btnAction" runat="server" CssClass="btn btn-primary btn-md mt-4" Text="ADD"></asp:Button>
                            
                        </div>
                    <div class="d-inline-block">
                            <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary btn-md mt-4" Text="Reset"></asp:Button>
                            
                        </div>
                    </div>
            </div>
        </div>
       <div class="row">
                            <div class="container">
                                <div class="row">
                                    <div class="col-md-12 col-lg-12">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gdv_Mail" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer" Font-Names="verdana"
                                                EmptyDataText="No Record Found" BackColor="#ffccff" BorderColor="Black" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" PageSize="50" AllowPaging="true" RowStyle-CssClass="rows" HeaderStyle-Wrap="false">
                                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#FFB6C1" Font-Bold="True" ForeColor="Black" />
                                                <AlternatingRowStyle BackColor="#FFB6C1" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Value">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTP_VALUE" runat="server" Text='<%# Eval("TP_VALUE")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTP_DESC" runat="server" Text='<%# Eval("TP_DESC")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("status")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="TP_ACTIVE" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTP_ACTIVE" runat="server" Text='<%# Eval("TP_ACTIVE")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Start Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTP_START_DT"  runat="server" Text='<%# Eval("TP_START_DT")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="End Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTP_END_DT" runat="server" Text='<%# Eval("TP_END_DT")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Email Id" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTP_CREATED_DT"  runat="server" Text='<%# Eval("TP_CREATED_DT")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="TP_MODIFIED_DT" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTP_MODIFIED_DT"  runat="server" Text='<%# Eval("TP_MODIFIED_DT")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText="Action" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btn_Update" runat="server" CommandName="_UPDATE" Text="Update" Visible="true" CommandArgument='<%# Eval("TP_VALUE")%>'></asp:Button>
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
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>

