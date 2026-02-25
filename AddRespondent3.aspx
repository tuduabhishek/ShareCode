<%@ Page Title="" Language="VB" MasterPageFile="~/AdminMaster.master" AutoEventWireup="false"
    CodeFile="AddRespondent3.aspx.vb" Inherits="AddRespondent3" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
            <!-- Google Fonts -->
            <link href="assets/css/googlefont.css" rel="stylesheet" />

            <!-- Vendor CSS Files -->
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

                                <link
                                    href='https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/ui-lightness/jquery-ui.css'
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
                                        color: purple !important;
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
                        <div class="panel-body">
                            <div class="row form-group p-1">
                                <%--<div class="col-md-1 text-left">
                                    <label class="font-weight-bold">Year<span class="text-danger">*</span></label>
                                    <asp:DropDownList runat="server" ID="ddlYear" CssClass="form-control"
                                        AutoPostBack="true" />
                            </div>
                            <div class="col-md-1 text-left">
                                <label class="font-weight-bold">Cycle<span class="text-danger">*</span></label>
                                <asp:DropDownList runat="server" ID="ddlCycle" CssClass="form-control"
                                    AutoPostBack="true" />
                            </div>--%>
                            <div class="col-md-3 text-left">
                                <label class="font-weight-bold">Executive Head</label>
                                <asp:DropDownList runat="server" ID="ddlExecutive" CssClass="form-control"
                                    AutoPostBack="true" />
                            </div>
                            <div class="col-md-3 text-left">
                                <label class="font-weight-bold">Department</label>
                                <asp:DropDownList runat="server" ID="ddlDept" CssClass="form-control" />
                            </div>
                            <div class="col-md-2 text-left">
                                <label class="font-weight-bold">BUHR Per. No.</label>
                                <asp:TextBox runat="server" ID="txtBuhr" CssClass="form-control" MaxLength="6"
                                    placeholder="BUHR Per. No" />
                            </div>
                            <div class="col-md-2 text-left">
                                <label class="font-weight-bold">Officer Per. No.</label>
                                <asp:TextBox runat="server" ID="txtperno1" CssClass="form-control" MaxLength="6"
                                    placeholder="Officer Per. No."></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="text-align: center;">
                            <div class="d-inline-block">
                                <asp:LinkButton ID="btnsearch" runat="server" CssClass="btn btn-primary btn-md mt-4"><i
                                        class="fa fa-search-minus"></i>&nbsp;Search</asp:LinkButton>
                                <asp:LinkButton ID="lbtnExport" runat="server"
                                    CssClass="btn btn-warning btn-md mt-4 text-white" Visible="false"><i
                                        class="fa fa-file-excel-o"></i>&nbsp;Export</asp:LinkButton>
                            </div>
                        </div>
                        <div class="row content">
                            <div class="col-md-12 col-lg-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvself" runat="server" Visible="false" AutoGenerateColumns="False"
                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                        Font-Names="verdana" EmptyDataText="No Record Found" BorderStyle="None"
                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" RowStyle-CssClass="rows">
                                        <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                        <HeaderStyle Font-Bold="True" ForeColor="Black" />
                                        <%--<AlternatingRowStyle BackColor="#FFB6C1" />--%>

                                        <Columns>

                                            <asp:TemplateField HeaderText="Assesse P.no">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpno" runat="server"
                                                        Text='<%# Eval("SS_ASSES_PNO")%>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Assesse Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lnlname" runat="server"
                                                        Text='<%# Eval("EMA_ENAME")%>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Assesse Designation">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbllevel" runat="server"
                                                        Text='<%# Eval("ema_desgn_desc")%>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Approver">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblappr" runat="server"
                                                        Text='<%# Eval("Approver")%>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Criteria Completed">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblstats" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Click to View Details">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lbpendingapproval" Text=""
                                                        CssClass="btn-learn-more-grid"
                                                        OnClick="lbpendingapproval_Click"></asp:LinkButton>
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
                        <div class="section-title" runat="server" id="divtitle" visible="false">
                            <h3>RESPONDENT Selected List for<span><strong>
                                        <asp:Label runat="server" ID="lblassname"></asp:Label>
                                    </strong> </span></h3>
                        </div>

                        <div class="row content">
                            <div class="col-md-3">


                                <asp:Button runat="server" ID="btnaddpeertsl" Text="Add Internal Respondent"
                                    class="btn-learn-more" OnClick="btnaddpeertsl_Click" Visible="false"></asp:Button>
                            </div>
                            <div class="col-md-3">
                                <asp:Button runat="server" ID="btnaddnontsl" Text="Add External Respondent"
                                    class="btn-learn-more" OnClick="btnaddnontsl_Click" Visible="false"></asp:Button>
                            </div>

                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <asp:Label runat="server" ID="lbls" CssClass="label" Visible="false"
                                    Style="font-size: 12pt; color: black;"></asp:Label>
                            </div>
                        </div>
                        <br />

                        <div class="row" runat="server" id="divtsl" visible="false">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="panel panel-info" id="close1">
                                    <div class="panel-heading bg-transparent">
                                        <h3 class="panel-title">Enter P.no or Name for Add</h3>
                                        <!-- Watch Out: Here We must use the effect name in the data tag-->
                                        <span class="float-end clickable">
                                            <button type="button" class="fa fa-times fa-1x" data-bs-target="#close1"
                                                aria-label="Close" data-bs-dismiss="alert">
                                            </button>
                                            </i></span>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">

                                                    <div class="col-md-12">
                                                        <asp:TextBox runat="server" ID="txtpnoI" CssClass="form-control"
                                                            placeholder="P. No" OnTextChanged="txtpnoI_TextChanged"
                                                            AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1"
                                                            runat="server" TargetControlID="txtpnoI"
                                                            ServiceMethod="SearchPrefixesForApprover"
                                                            MinimumPrefixLength="1" CompletionInterval="100"
                                                            DelimiterCharacters="" Enabled="True" ServicePath=""
                                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                            CompletionListCssClass="AutoExtender"
                                                            CompletionListItemCssClass="AutoExtenderList">
                                                        </cc1:AutoCompleteExtender>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">

                                                    <div class="col-md-12">
                                                        <asp:TextBox runat="server" ID="txtdesgI"
                                                            CssClass="form-control" placeholder="Designation"
                                                            Enabled="false" />
                                                        <%--<asp:HiddenField runat="server" ID="hdfnin" /> --%>
                                                        <asp:Label runat="server" ID="lbllevel" Visible="false">
                                                        </asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">

                                                    <div class="col-md-12">
                                                        <asp:TextBox runat="server" ID="txtemailI"
                                                            CssClass="form-control " placeholder="Email"
                                                            Enabled="false" />
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                                            runat="server" ErrorMessage="Please Enter a valid Email ID"
                                                            ControlToValidate="txtemailI"
                                                            CssClass="label label-danger fontWhite" Display="Dynamic"
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


                                        </div>
                                        <br />
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
                                                    class="btn-learn-more" OnClick="btnAddP_Click"></asp:Button>
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
                                        <span class="float-end clickable">
                                            <button type="button" class="fa fa-times fa-1x" data-bs-target="#close2"
                                                aria-label="Close" data-bs-dismiss="alert">
                                            </button>
                                            </i></span>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">

                                                    <div class="col-md-12">
                                                        <asp:TextBox runat="server" ID="txtnamenI"
                                                            CssClass="form-control" placeholder="Name" MaxLength="50" />

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">

                                                    <div class="col-md-12">
                                                        <asp:TextBox runat="server" ID="txtdesgnI"
                                                            CssClass="form-control" placeholder="Designation"
                                                            MaxLength="50" />

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">

                                                    <div class="col-md-12">
                                                        <asp:TextBox runat="server" ID="txtemailnI"
                                                            CssClass="form-control " placeholder="Email"
                                                            MaxLength="50" />
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                                            runat="server" ErrorMessage="Please Enter a valid Email ID"
                                                            ControlToValidate="txtemailnI"
                                                            CssClass="label label-danger fontWhite" Display="Dynamic"
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
                                        </div>
                                        <br />
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
                                                    class="btn-learn-more" OnClick="btnaddnorgI_Click"></asp:Button>
                                            </div>

                                        </div>

                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="row content">
                            <div class="col-md-12 col-lg-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvfinal" runat="server" AutoGenerateColumns="False"
                                        Visible="false"
                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                        Font-Names="verdana" EmptyDataText="No Record Found" BackColor="#ffccff"
                                        BorderColor="Black" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        GridLines="Vertical" RowStyle-CssClass="rows"
                                        OnRowDataBound="GvCateg_RowDataBound" OnRowDeleting="gvfinal_RowDeleting"
                                        DataKeyNames="SS_PNO">
                                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                        <HeaderStyle CssClass="bg-clouds segoe-light" Font-Bold="True"
                                            ForeColor="Black" />
                                        <%--<AlternatingRowStyle BackColor="#FFB6C1" />--%>
                                        <Columns>

                                            <asp:TemplateField HeaderText="P.no">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpno" runat="server" Text='<%# Eval("SS_PNO")%>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lblapno" runat="server"
                                                        Text='<%# Eval("SS_ASSES_PNO")%>' Visible="false"></asp:Label>
                                                    <%-- <asp:HiddenField runat="server"
                                                        Value='<%# Eval("SS_ASSES_PNO") %>' ID="hdfnperno" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lnlname" runat="server" Text='<%# Eval("SS_NAME")%>'>
                                                    </asp:Label>

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
                                                    <asp:Label ID="lbldesg" runat="server" Text='<%# Eval("SS_DESG")%>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Department">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldept" runat="server" Text='<%# Eval("SS_DEPT")%>'>
                                                    </asp:Label>
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
                                                    <asp:Label ID="lblDelTag" runat="server" Visible="false"
                                                        Text='<%# Eval("SS_DEL_TAG")%>'></asp:Label>
                                                    <asp:Label ID="lblCate" runat="server" Visible="false"
                                                        Text='<%# Eval("SS_CATEG")%>'></asp:Label>
                                                    <asp:Label ID="lblcategory" runat="server"
                                                        Text='<%# Eval("Category")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:CommandField ButtonType="Button" ShowDeleteButton="true"
                                                SelectText="Remove" HeaderText="Remove">
                                                <ControlStyle CssClass="btn-learn-more" />
                                            </asp:CommandField>

                                        </Columns>
                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                        <RowStyle BackColor="White" ForeColor="Black" />
                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#000065" />
                                    </asp:GridView>
                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"
                                        Visible="false"
                                        CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                        Font-Names="verdana" EmptyDataText="No Record Found" BackColor="#ffccff"
                                        BorderColor="Black" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        GridLines="Vertical" RowStyle-CssClass="rows">
                                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                        <HeaderStyle CssClass="bg-clouds segoe-light" Font-Bold="True"
                                            ForeColor="Black" />
                                        <%--<AlternatingRowStyle BackColor="#FFB6C1" />--%>
                                        <Columns>

                                            <asp:TemplateField HeaderText="P.no">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpno" runat="server" Text='<%# Eval("SS_PNO")%>'>
                                                    </asp:Label>
                                                    <asp:HiddenField runat="server" Value='<%# Eval("SS_ASSES_PNO") %>'
                                                        ID="hdfnperno" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lnlname" runat="server" Text='<%# Eval("SS_NAME")%>'>
                                                    </asp:Label>

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
                                                    <asp:Label ID="lbldesg" runat="server" Text='<%# Eval("SS_DESG")%>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Department">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldept" runat="server" Text='<%# Eval("SS_DEPT")%>'>
                                                    </asp:Label>
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
                        <div class="row content">
                            <div class="col-lg-5">
                            </div>
                            <div class="col-lg-5">
                                <asp:Button runat="server" ID="btnsubmit" Text="Submit" class="btn-learn-more"
                                    data-bs-toggle="modal" data-bs-target="#staticBackdrop" Visible="false">
                                </asp:Button>
                            </div>
                        </div>
                    </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="modal fade" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1"
                aria-labelledby="staticBackdropLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">

                        <div class="modal-body">
                            Are you sure you want to submit the form?
                        </div>
                        <div class="modal-footer">

                            <asp:Button runat="server" ID="btnyesclick" OnClick="Submit" Text="Yes"
                                class="btn btn-primary" />
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">No</button>

                        </div>
                    </div>
                </div>
            </div>
        </asp:Content>