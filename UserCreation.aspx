<%@ Page Title="" Language="VB" MasterPageFile="~/AdminMaster.master" AutoEventWireup="false" CodeFile="UserCreation.aspx.vb" Inherits="UserCreation" %>
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

    <link href='https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/ui-lightness/jquery-ui.css'
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
    <style>
    .radiostyle {
      height: auto;
    }

    .radiostyle label {
        margin-left: 3px !important;
        margin-right: 10px !important;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMain" runat="server" >
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12 text-center">
                 <h1>Assign Admin Role</h1>
                </div>
                 </div>
            <div class="row content">
                        <div class="col-md-12" style="margin-left:20;">
<asp:RadioButtonList ID="popiRadios" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server" AutoPostBack="true">
                        <asp:ListItem class="radio-inline" Value="I" Text="Internal Employee" ></asp:ListItem>
                        <asp:ListItem class="radio-inline" Value="E" Text="External Employee"></asp:ListItem>
                    </asp:RadioButtonList>

                           <%-- <asp:Button runat="server" ID="btnaddpeertsl" Text="Add Internal Respodent" class="btn-learn-more" OnClick="btnaddpeertsl_Click" Visible="false"></asp:Button>--%>
                        </div>
                       <%-- <div class="col-md-3">
                            <asp:Button runat="server" ID="btnaddnontsl" Text="Add External Respodent" class="btn-learn-more" OnClick="btnaddnontsl_Click" Visible="false"></asp:Button>
                        </div>--%>

                    </div>
                    <br />
            <div runat="server" id="div_int" visible="true">
            <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">

                                                <div class="col-md-12">
                                                    <label class="font-weight-bold">Type Employee ID </label>
                                                    <asp:TextBox runat="server" ID="txtpnoI" CssClass="form-control" placeholder="Employee ID" OnTextChanged="txtpnoI_TextChanged" AutoPostBack="true" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtpnoI"
                                                        ServiceMethod="SearchPrefixesForApprover" MinimumPrefixLength="1" CompletionInterval="100"
                                                        DelimiterCharacters="" Enabled="True" ServicePath=""
                                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListCssClass="AutoExtender"
                                                        CompletionListItemCssClass="AutoExtenderList">
                                                    </cc1:AutoCompleteExtender>
                                                </div>

                                            </div>
                                        </div>
                                        <%--<div class="col-md-3">
                                            <div class="form-group">

                                                <div class="col-md-12">
                                                    <asp:TextBox runat="server" ID="txtEmp_Name" CssClass="form-control" placeholder="Name" Enabled="true" />
                                                    
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="txtEmp_Name"
                                                        ServiceMethod="SearchPrefixesForApprover" MinimumPrefixLength="1" CompletionInterval="100"
                                                        DelimiterCharacters="" Enabled="True" ServicePath=""
                                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListCssClass="AutoExtender"
                                                        CompletionListItemCssClass="AutoExtenderList">
                                                    </cc1:AutoCompleteExtender>

                                                    <asp:Label runat="server" ID="lbllevel" Visible="false"></asp:Label>
                                                </div>
                                            </div>
                                        </div>--%>
                                        

                                    </div>
                                    <%--<br />--%>
                                    
                                    

                                </div>
                </div>
            <div runat="server" id="div_ext" visible="false" style="margin-left:20;">
<div class="row form-group p-1">
                        <%--<div class="col-md-1 text-left">
                            <label class="font-weight-bold">Year<span class="text-danger">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlYear" CssClass="form-control" AutoPostBack="true" />
                        </div>
                        <div class="col-md-1 text-left">
                            <label class="font-weight-bold">Cycle<span class="text-danger">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlCycle" CssClass="form-control" AutoPostBack="true" />
                        </div>--%>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Employee ID </label>
                            <asp:TextBox runat="server" ID="txtPAN" CssClass="form-control" placeholder="Employee ID or Name" Style="text-transform: uppercase" AutoPostBack="true" />
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="txtPAN"
                                                        ServiceMethod="SearchPrefixesForADMIN" MinimumPrefixLength="1" CompletionInterval="100"
                                                        DelimiterCharacters="" Enabled="True" ServicePath=""
                                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListCssClass="AutoExtender"
                                                        CompletionListItemCssClass="AutoExtenderList">
                                                    </cc1:AutoCompleteExtender>
                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPAN"
                    Display="Dynamic" ForeColor="Red" ErrorMessage="Please Enter Correct PAN Card Number" ValidationExpression="[A-Z]{5}\d{4}[A-Z]{1}"></asp:RegularExpressionValidator>--%>
                        </div>
    <div class="col-md-2 text-left">
                            <label class="font-weight-bold">Email ID</label>
                            <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" MaxLength="75" placeholder="Email ID" />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                        ErrorMessage="Please Enter a valid Email ID" ControlToValidate="txtEmail" CssClass="label label-danger fontWhite" Display="Dynamic"
                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </div>
    <div class="col-md-2 text-left">
                            <label class="font-weight-bold">Name</label>
                            <asp:TextBox runat="server" ID="txtName" CssClass="form-control" MaxLength="75" placeholder="Name" />
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Status</label>
                            <asp:DropDownList runat="server" ID="ddl_Ext_Status" CssClass="form-control" >
                                 <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                                                        <asp:ListItem Text="Active" Value="A"></asp:ListItem>
                                                                        <asp:ListItem Text="Deactive" Value="D"></asp:ListItem>
                                </asp:DropDownList>
                        </div>
                        <%--<div class="col-md-2 text-left">
                            <label class="font-weight-bold">Remarks</label>
                            <asp:TextBox runat="server" ID="txtRemarks" CssClass="form-control" MaxLength="60" placeholder="Remarks" />
                        </div>--%>
                        <%--<div class="col-md-2 text-left">
                            <label class="font-weight-bold">Officer Per. No.</label>
                            <asp:TextBox runat="server" ID="txtperno1" CssClass="form-control" MaxLength="6" placeholder="Officer Per. No."></asp:TextBox>
                        </div>--%>
                    </div>
                <div class="form-group" style="text-align: center;">
                        <div class="d-inline-block">
                            <asp:Button ID="btnAssign" runat="server" CssClass="btn btn-primary btn-md mt-4" Text="ADD" Enabled="true"></asp:Button>
                            <%--<asp:LinkButton ID="lbtnExport" runat="server" CssClass="btn btn-warning btn-md mt-4 text-white" Visible="false"><i class="fa fa-file-excel-o"></i>&nbsp;Export</asp:LinkButton>--%>
                        </div>
                    </div>
            </div>

            <div class="row content">
                                        
                                        <div class="col-md-12 text-center">
                                            <asp:Button runat="server" ID="btnReset" Text="Reset" class="btn-learn-more" OnClientClick="return confirm(' Are you sure you want to reset?');"></asp:Button>
                                        </div>

                                    </div><br />
            <div class="row">
                            <div class="container">
                                <div class="row">
                                    <div class="col-md-12 col-lg-12">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gdv_Create_User" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer" Font-Names="verdana"
                                                EmptyDataText="No Record Found" BackColor="#ffccff" BorderColor="Black" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" PageSize="50" AllowPaging="true" RowStyle-CssClass="rows" HeaderStyle-Wrap="false">
                                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#FFB6C1" Font-Bold="True" ForeColor="Black" />
                                                <AlternatingRowStyle BackColor="#FFB6C1" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="P.No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblema_perno" runat="server" Text='<%# Eval("ema_perno")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="P.No." Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_user_id" runat="server" Text='<%# Eval("user_id")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblema_ename" runat="server" Text='<%# Eval("ema_ename")%>'></asp:Label>
                                                                    <asp:TextBox ID="txt_IGP_ADM_NAME" MaxLength="75" runat="server" Text='<%# Eval("ema_ename")%>' Visible="false">
                                                                    </asp:TextBox>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="Level">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIRC_DESC" runat="server" Text='<%# Eval("IRC_DESC")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEMA_DESGN_DESC" runat="server" Text='<%# Eval("EMA_DESGN_DESC")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Department">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEMA_DEPT_DESC" runat="server" Text='<%# Eval("EMA_DEPT_DESC")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Email Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEMA_EMAIL_ID"  runat="server" Text='<%# Eval("EMA_EMAIL_ID")%>'></asp:Label>
                                                                    <asp:TextBox ID="txt_IGP_EMAIL_ID" MaxLength="75" runat="server" Text='<%# Eval("IGP_EMAIL_ID")%>' Visible="false">
                                                                                                                                        </asp:TextBox>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Email Id" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblADM_EMAIL_ID"  runat="server" Text='<%# Eval("IGP_EMAIL_ID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="User Type" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUSER_TYPE"  runat="server" Text='<%# Eval("IGP_MODE")%>'></asp:Label>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_IGP_REMARKS" Width="100" Enabled="false" runat="server" Text='<%# Eval("IGP_REMARKS")%>'>
                                                                    </asp:TextBox>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddl_Active_deactive" runat="server">
                                                                        <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                                                        <asp:ListItem Text="Active" Value="A"></asp:ListItem>
                                                                        <asp:ListItem Text="Deactive" Value="D"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Assign Admin" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnADD" runat="server" CommandName="ADD" Text="Assign" OnClientClick="return confirm(' Are you sure you want to add?');" CommandArgument='<%# Eval("ema_perno")%>'></asp:Button>
                                                                    <asp:Button ID="btn_Update_Admin" runat="server" CommandName="_UPDATE" Text="Update" Visible="false" OnClientClick="return confirm(' Are you sure you want to upadte?');" CommandArgument='<%# Eval("ema_perno")%>'></asp:Button>
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

