<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SendMail_OPR.aspx.vb" Inherits="SendMail_OPR"  MaintainScrollPositionOnPostback="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
  <meta charset="utf-8">
  <meta content="width=device-width, initial-scale=1.0" name="viewport">

  <title>360 Survey</title>
  <meta content="" name="descriptison">
  <meta content="" name="keywords">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
  <!-- Favicons -->
  <link href="assets/img/favicon.png" rel="icon">
  <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">

  <!-- Google Fonts -->
    <link href="assets/css/googlefont.css" rel="stylesheet" />

  <!-- Vendor CSS Files -->
  <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
  <link href="assets/vendor/icofont/icofont.min.css" rel="stylesheet">
  <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
  <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">
  <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
  <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
     <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
    <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>

    
     <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet">
     <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
  <!-- Include all compiled plugins (below), or include individual files as needed -->
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    
  <!-- Template Main CSS File -->
  <link href="assets/css/style.css" rel="stylesheet">
    <style type="text/css">
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

         .panel-heading span
{
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

#hero .btn-get-started
	{
	font-size: 15px !important;
	}

.table-responsive1 {
    display: block;
    width: 100%;
    -webkit-overflow-scrolling: touch;
}
    </style>
    <script type="text/javascript">

         
        function showGenericMessageModal(type, message) {
            swal('', message, type);
        }

        function MutExChkList(chk) {
            var chkList = chk.parentNode.parentNode.parentNode;
            var chks = chkList.getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i] != chk && chk.checked) {
                    chks[i].checked = false;
                }
            }
        }

        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };

        $(function () {
            $('[id*=txtDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd/mm/yyyy",
                language: "tr"
            });
        });
    </script>
    

</head>

<body>

   
    <form id="form1" runat="server">
   
  <!-- ======= Header ======= -->
  <header id="header" class="fixed-top ">
    <div class="container d-flex align-items-center">

      <h1 class="logo mr-auto"><a >360 DEGREE FEEDBACK SURVEY</a></h1>
      <!-- Uncomment below if you prefer to use an image logo -->
      <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->

      <nav class="nav-menu d-none d-lg-block">
        <ul>        

        </ul>
      </nav><!-- .nav-menu -->

    </div>
  </header><!-- End Header -->
        <asp:ScriptManager runat="server" ID="scpmgr">

        </asp:ScriptManager>
  <!-- ======= Hero Section ======= -->
  <section id="hero">
    <div class="hero-container">
     <h3>Welcome  <strong><asp:Label ID="lblname" runat ="server" Text="Admin" ></asp:Label></strong></h3>     
      <!--<h1>We're Creative Agency</h1>-->
      <h2 class ="table-responsive1">As we aspire to be the most valuable and respected steel company globally in the next 5-10 years, we are developing agile behaviours across the organization. We will measure this as an integral part of our Performance Management System for all the officers through a 360 degree feedback survey.</h2>
      <a href="#about" class="btn-get-started scrollto">SEND MAIL</a>
    </div>
  </section><!-- End Hero -->

  <main id="main">
      <asp:UpdatePanel runat="server" ID="uppnl">
          <ContentTemplate>

         
              <!-- ======= About Section ======= -->
      <section id="about" class="about">
      <div class="container">
          <div class="row">
               <div class="col-lg-2">
                  <label>
                      Year (YYYY)<span style="color:red;">*</span>
                      <asp:TextBox ID="txtYear" CssClass="form-control" runat="server" Text="" MaxLength="4" />
                  </label>
              </div> 
              <div class="col-lg-1">
                  <label>
                      Cycle<span style="color:red;">*</span>
                      <asp:TextBox ID="txtCycle" CssClass="form-control" runat="server" Text="" MaxLength="1" />
                  </label>
              </div>
              <div class="col-lg-2">
                  <label>
                      Eq. Level<span style="color:red;">*</span>
                      <asp:DropDownList ID="ddlLevel" CssClass="form-control" runat="server" AutoPostBack="true">
                          <asp:ListItem Value="0">---Select---</asp:ListItem>
                          <asp:ListItem Value="I1">I1</asp:ListItem>
                          <asp:ListItem Value="I2">I2</asp:ListItem>
                          <asp:ListItem Value="I3">I3</asp:ListItem>
                          <asp:ListItem Value="I4">I4</asp:ListItem>
                          <asp:ListItem Value="I5">I5</asp:ListItem>
                          <asp:ListItem Value="I6">I6</asp:ListItem>
                          </asp:DropDownList>
                  </label>
              </div>
              
              <div class="col-lg-3">
                   <label>
                      Mail Type&nbsp;<span style="color:red;"><asp:Label ID="lblCount" runat="server" Text=""/></span>
                      <asp:DropDownList ID="ddlMailType" CssClass="form-control" runat="server" AutoPostBack="true">
                          <asp:ListItem Value="0">---Select---</asp:ListItem>
                          <asp:ListItem Value="1">Mail to Assessee</asp:ListItem>
                          <asp:ListItem Value="2">Reminder Mail to Assessee</asp:ListItem>
                          <asp:ListItem Value="3">Mail to Approver</asp:ListItem>
                          <asp:ListItem Value="4">Reminder Mail to Approver</asp:ListItem>
                          <asp:ListItem Value="5">Mail to Respondent for Feedback</asp:ListItem>
                      </asp:DropDownList>
                  </label>
              </div>
              <div class="col-lg-2">
                  <label>
                      Per. No.
                      <asp:TextBox ID="txtPerno" CssClass="form-control" runat="server" Text=""  MaxLength="6" AutoPostBack="true" OnTextChanged="txtPerno_TextChanged"/>
                  </label>
                  <asp:CheckBoxList runat="server" ID="chklstmailtype" RepeatColumns="4" RepeatDirection="Horizontal" Visible="false" RepeatLayout="Table" CellPadding="5" CellSpacing="5"
                      AutoPostBack="true" OnSelectedIndexChanged="chklstmailtype_SelectedIndexChanged" >
                   
                     <asp:ListItem Value="mtr" onclick="MutExChkList(this);">Mail to Respondents</asp:ListItem>
                     <asp:ListItem Value="rmtoresp" onclick="MutExChkList(this);">Reminder to Respondents</asp:ListItem>
                 </asp:CheckBoxList>
              </div>
              <div class="col-lg-2">
                  <label>
                      Level
                      <asp:DropDownList ID="ddlSgrade" CssClass="form-control" runat="server" AutoPostBack="true">
                          <asp:ListItem Value="0">---Select---</asp:ListItem>
                          <asp:ListItem Value="1">JB2</asp:ListItem>
                          <asp:ListItem Value="2">JB3</asp:ListItem>
                          </asp:DropDownList>
                  </label>
                </div>
          </div>
          <div class="row content">
              <div class="col-lg-1 form-group">
                  <div style="margin-top:10px;">
                  <label>Subject : </label>
                      </div>
              </div>
              <div class="col-lg-11">
                  <asp:TextBox ID="txtSubject" runat="server" Text="" CssClass="form-control" />
              </div>
          </div>
          <br />
          <div class="row content">
              <div style="display:flex; align-content:center; text-align:center; margin:auto;">
                  <div>
                       <asp:Button runat="server" ID="btnShow" Text="Show List" CssClass="btn-learn-more" OnClick="btnShow_Click"/>
                  </div>
                  <div>
                       <asp:Button runat="server" ID="btnopt" Text="Send Mail" CssClass="btn-learn-more" OnClick="btnopt_Click" />
                  </div>
              </div>
          </div>
          <br />
           <div class="row content">
             <div class="col-md-12 col-lg-12">
                <div class="table-responsive">
                    <asp:GridView ID="gvself" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer" Font-Names="verdana"
                                                        EmptyDataText="No Record Found" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" RowStyle-CssClass="rows" >
                                                       <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                        <HeaderStyle  BackColor="#e43c5c" Font-Bold="True" ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />
                        
                          <Columns>
                                                           
                                                            <asp:TemplateField HeaderText="Assesse P.no">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblpno" runat="server" Text='<%# Eval("ema_perno")%>'></asp:Label>
                                                                   
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Assesse Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lnlname" runat="server" Text='<%# Eval("EMA_ENAME")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Assesse Designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbllevel" runat="server" Text='<%# Eval("ema_desgn_desc")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Step-1 Start Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStp1stdt" runat="server" Text='<%# Eval("Step1_Sdt")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Step-1 End Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStp1enddt" runat="server" Text='<%# Eval("Step1_Edt")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Step-2 Start Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStp2stdt" runat="server" Text='<%# Eval("Step2_Sdt")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Step-2 End Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStp2enddt" runat="server" Text='<%# Eval("Step2_Edt")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Step-3 Start Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStp3stdt" runat="server" Text='<%# Eval("Step3_Sdt")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Step-3 End Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStp3enddt" runat="server" Text='<%# Eval("Step3_Edt")%>'></asp:Label>
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

    </section>
    <!-- End About Section -->     
               </ContentTemplate>
          <Triggers>
              <asp:PostBackTrigger ControlID="btnopt" />
              <asp:PostBackTrigger ControlID="btnShow" />
          </Triggers>
      </asp:UpdatePanel>
  </main><!-- End #main -->


  <a href="#" class="back-to-top"><i class="ri-arrow-up-line"></i></a>

  <!-- Vendor JS Files -->
  <script src="assets/vendor/jquery/jquery.min.js"></script>
  <script src="assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
  <script src="assets/vendor/jquery.easing/jquery.easing.min.js"></script>
  <script src="assets/vendor/php-email-form/validate.js"></script>
  <script src="assets/vendor/isotope-layout/isotope.pkgd.min.js"></script>
  <script src="assets/vendor/venobox/venobox.min.js"></script>
  <script src="assets/vendor/owl.carousel/owl.carousel.min.js"></script>

  <!-- Template Main JS File -->
  <script src="assets/js/main.js"></script>
 </form>
</body>

</html>