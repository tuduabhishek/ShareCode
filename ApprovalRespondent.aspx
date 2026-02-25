<%@ Page Title="" Language="VB" MasterPageFile="~/AdminMaster.master" AutoEventWireup="false" CodeFile="ApprovalRespondent.aspx.vb" Inherits="ApprovalRespondent" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <script type="text/javascript">
         function showGenericMessageModal(type, message) {
            swal('', message, type);
        }
        function showConfirmMessageModal(type, message) {
            swal({
                title: "Are you sure you want to Approve this Record?",
                text: "",
                icon: "info",
                //buttons: true,
                //dangerMode: true
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, approve it!",
                closeOnConfirm: false
            },function (isConfirm) {
                if (isConfirm) {
                     var pno = $('#<%=hdfSession.ClientID%>').val();
                     var yr = $('#<%=hdfYear.ClientID%>').val();
                     var cyc = $('#<%=hdfCycle.ClientID%>').val();
                    $.ajax({
                        type: "POST",
                        url: "SurveyAdm_opr.aspx/SubmitClick",
                        data: "{User:'" + message + "',Syear:'"+yr+"',Scyc:'"+cyc+"',UserId:'"+pno+"'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (r) {
                            if (r.d == true) {
                                //swal(r.d, 'success');
                                swal({
                                    title: "",
                                    text: "Record approved successfully",
                                    icon: "success"
                                }, function (isConfirm) {
                                    location.reload();
                                });
                            } else {
                                swal({
                                    title: "",
                                    text: "Unable to update the record, Please try again.",
                                    icon: "error"
                                }, function (isConfirm) {
                                    location.reload();
                                });
                            }
                        }
                    });
                }
            });
            return false;
        }
    </script>
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
                            <asp:DropDownList runat="server" ID="ddlYear" CssClass="form-control" AutoPostBack="true" />
                        </div>
                        <div class="col-md-1 text-left">
                            <label class="font-weight-bold">Cycle<span class="text-danger">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlCycle" CssClass="form-control" AutoPostBack="true" />
                        </div>--%>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Executive Head</label>
                            <asp:DropDownList runat="server" ID="ddlExecutive" CssClass="form-control" AutoPostBack="true" />
                        </div>
                        <div class="col-md-3 text-left">
                            <label class="font-weight-bold">Department</label>
                            <asp:DropDownList runat="server" ID="ddlDept" CssClass="form-control" />
                        </div>
                        <div class="col-md-2 text-left">
                            <label class="font-weight-bold">BUHR Per. No.</label>
                            <asp:TextBox runat="server" ID="txtBuhr" CssClass="form-control" MaxLength="6" placeholder="BUHR Per. No" />
                        </div>
                        <div class="col-md-2 text-left">
                            <label class="font-weight-bold">Officer Per. No.</label>
                            <asp:TextBox runat="server" ID="txtperno1" CssClass="form-control" MaxLength="6" placeholder="Officer Per. No."></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group" style="text-align: center;">
                        <div class="d-inline-block">
                            <asp:LinkButton ID="btnsearch" runat="server" CssClass="btn btn-primary btn-md mt-4"><i class="fa fa-search-minus"></i>&nbsp;Search</asp:LinkButton>
                            <asp:LinkButton ID="lbtnExport" runat="server" CssClass="btn btn-warning btn-md mt-4 text-white" Visible="false"><i class="fa fa-file-excel-o"></i>&nbsp;Export</asp:LinkButton>
                        </div>
                    </div>
                    <div class="row content">
                        <div class="col-md-12 col-lg-12">
                            <div class="table-responsive">
                                <asp:HiddenField ID="hdfSession" runat="server" />
                <asp:HiddenField ID="hdfYear" runat="server" />
                <asp:HiddenField ID="hdfCycle" runat="server" />
                                <asp:GridView ID="GridView1" runat="server" Visible="false" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer" Font-Names="verdana"
                                                        EmptyDataText="No Record Found" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" RowStyle-CssClass="rows">
                                                       <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                        <HeaderStyle  Font-Bold="True" ForeColor="Black" />
                                                        <%--<AlternatingRowStyle BackColor="#FFB6C1" />--%>
                        
                          <Columns>
                                                           
                                                            <asp:TemplateField HeaderText="RESPONDENT P.no">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblpno1" runat="server" Text='<%# Eval("SS_ASSES_PNO")%>'></asp:Label>
                                                                   
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="RESPONDENT Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lnlname1" runat="server" Text='<%# Eval("EMA_ENAME")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="RESPONDENT Designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbllevel1" runat="server" Text='<%# Eval("ema_desgn_desc")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                             <asp:TemplateField HeaderText="Approver">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblappr1" runat="server" Text='<%# Eval("Approver")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                               
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="lbpend" Text="Click to Approve"  OnClick="lbpend_Click" CssClass="btn-learn-more-grid" ></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField >        
                                                           
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

