<%@ Page Title="" Language="VB" MasterPageFile="~/AdminMaster.master" AutoEventWireup="false" CodeFile="RawData.aspx.vb"
    Inherits="RawData" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
            <!-- Google Fonts -->
            <link href="assets/css/googlefont.css" rel="stylesheet" />

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
                        <div class="row">
                            <div class="col-lg-12">
                                <h3>Download Raw data</h3>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div class="row form-group p-1">

                                <div class="col-md-2 text-left">
                                    <label class="font-weight-bold">Year</label>
                                    <asp:DropDownList runat="server" ID="ddlFinYr" CssClass="form-control" />
                                </div>
                                <div class="col-md-2 text-left">
                                    <label class="font-weight-bold">Cycle</label>
                                    <asp:DropDownList runat="server" ID="ddlCycle" CssClass="form-control" />
                                </div>
                                <div class="col-md-2 text-left">
                                    <label class="font-weight-bold">Level</label>
                                    <asp:DropDownList runat="server" ID="ddlLevel" CssClass="form-control" />
                                </div>
                                <div class="col-md-2 text-left">
                                    <label class="font-weight-bold">Executive Head</label>
                                    <asp:DropDownList runat="server" ID="ddlExecutive" CssClass="form-control" />
                                </div>
                                <div class="col-md-2 text-left">
                                    <label class="font-weight-bold">Assessee Per. No.</label>
                                    <asp:TextBox runat="server" ID="txtAssess" CssClass="form-control" MaxLength="6"
                                        placeholder="Assesse Per. No" />
                                </div>
                            </div>
                            <div class="form-group" style="text-align: center;">
                                <div class="d-inline-block">

                                    <asp:UpdatePanel ID="upanel" runat="server">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="btnsearch" runat="server"
                                                CssClass="btn btn-primary btn-md mt-4"><i
                                                    class="fa fa-search-minus"></i>&nbsp;Search</asp:LinkButton>
                                            <asp:LinkButton ID="lbtnExport" runat="server"
                                                CssClass="btn btn-warning btn-md mt-4 text-white" Visible="false"><i
                                                    class="fa fa-file-excel-o"></i>&nbsp;Export</asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="lbtnExport" />
                                        </Triggers>
                                    </asp:UpdatePanel>

                                </div>
                            </div>
                            <div class="row content">
                                <div class="col-md-12 col-lg-12">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvself" runat="server" Visible="false"
                                            AutoGenerateColumns="False"
                                            CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                            Font-Names="verdana" EmptyDataText="No Record Found" BorderStyle="None"
                                            BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                            RowStyle-CssClass="rows" AllowPaging="true" PageSize="20">
                                            <HeaderStyle BackColor="#e43c5c" Font-Bold="True" ForeColor="Black" />
                                            <AlternatingRowStyle BackColor="#FFB6C1" />

                                            <Columns>
                                                <asp:TemplateField HeaderText="Slr. No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblslrno" runat="server"
                                                            Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Year">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSS_YEAR" runat="server"
                                                            Text='<%# Eval("SS_YEAR")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Cycle">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCycle" runat="server"
                                                            Text='<%# Eval("SS_SERIAL")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Assessee Per. No. ">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblpno" runat="server"
                                                            Text='<%# Eval("SS_ASSES_PNO")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Assessee Name ">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnlname" runat="server"
                                                            Text='<%# Eval("EMA_ENAME")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Respodent Per. No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnlRespodent" runat="server"
                                                            Text='<%# Eval("SS_PNO")%>'></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Category">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSS_CATEG"
                                                            Text='<%# Eval("SS_CATEG_Desc")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Q. Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQTEXT" runat="server"
                                                            Text='<%# Eval("SS_QCODE")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Response">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblstats"
                                                            Text='<%# Eval("SS_QOPTN")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Question Category">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSS_DEPT" runat="server"
                                                            Text='<%# Eval("IRC_DESC")%>'></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Level">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSS_QLEVEL"
                                                            Text='<%# Eval("SS_QLEVEL")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Created By">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCreatedBy"
                                                            Text='<%# Eval("SS_CREATED_BY")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Created Dt">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblcreatedt"
                                                            Text='<%# Eval("SS_CREATED_DT")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Modified By">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblModified"
                                                            Text='<%# Eval("SS_MODIFIED_BY")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Modified Dt">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblModifiedDt"
                                                            Text='<%# Eval("SS_MODIFIED_DT")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Serial No">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSerial"
                                                            Text='<%# Eval("SS_SRL_NO")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Flag">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblFlag"
                                                            Text='<%# Eval("SS_FLAG")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>

                                            <PagerStyle BackColor="#999999" ForeColor="Black"
                                                HorizontalAlign="Center" />
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
            <%-- <div class="modal fade" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false"
                tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
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
                </div>--%>
        </asp:Content>