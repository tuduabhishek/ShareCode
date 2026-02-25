<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MinimumCriteriaRpt.aspx.vb" Inherits="MinimumCriteriaRpt" MaintainScrollPositionOnPostback="true"   %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
  <meta charset="utf-8">
  <meta content="width=device-width, initial-scale=1" name="viewport">

  <title>360 Survey</title>
  <meta content="" name="descriptison">
  <meta content="" name="keywords">
     <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <%--<meta content="width=device-width, initial-scale=1" name="viewport" />--%>
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
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">
<script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script>
<script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
     <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet">
     <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
  <!-- Include all compiled plugins (below), or include individual files as needed -->
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>


  <!-- Template Main CSS File -->
  <link href="assets/css/styleIL3.css" rel="stylesheet"/>
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
	
	
	.badge-primary {
    color: #fff;
    background-color: #e43c5c !important ;
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
      .wrap-Text{
          word-wrap: normal; 
          word-break: break-all;
          width:30px;
      }
      #ifMobile1 {
    /*background-image: url(/images/arts/IMG_1447m.png)*/
    width:300px;
    height:200px;
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

       
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div class="divWaiting">
                <center>
                    <div>
                        <h3>
                            <asp:Label ID="lblWait" runat="server" Text="Please wait"  /></h3>
                      <img src="Images/loader.gif" alt="Loading..."  id="ifMobile1"/>
                    </div>
                </center>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
  <!-- ======= Header ======= -->
  <header id="header" class="fixed-top ">
    <div class="container d-flex align-items-center">

      <h1 class="logo mr-auto"><a href="https://irisapp-tslin.msappproxy.net/feedback_360/surveyadm_opr.aspx">360 DEGREE FEEDBACK SURVEY</a></h1>
      <!-- Uncomment below if you prefer to use an image logo -->
      <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->

      <nav class="nav-menu d-none d-lg-block">
        <ul>
          <li class="active"><a href="https://irisapp-tslin.msappproxy.net/feedback_360/surveyadm_opr.aspx">Home (Admin)</a></li>
         
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
    
        <h2 class ="table-responsive1">
            As we aspire to be the most valuable and respected steel company globally in the coming years, we are developing agile behaviours across the organization. We will measure this as an integral part of our Performance Management System for all the officers through a 360 degree feedback survey.            
</h2>
      <a href="#about" class="btn-get-started scrollto">VIEW REPORT</a>
    </div>
  </section><!-- End Hero -->

<main id="main">

    <!-- ======= About Section ======= -->
      <section id="about" class="about">
      <div class="container-fluid">
          <div class="row">
        <div class="col-lg-12">
            <h1>Minimum Criteria (Generation of Report)</h1>
        </div>
    </div>
          <asp:UpdatePanel ID="upnl1" runat="server">
        <ContentTemplate>
          <div class="panel-body">
                     <div class="row form-group">
                         <div class="col-md-1">
                             <label>Year</label><span style="color:red;">*</span>
                             <asp:TextBox runat="server" ID="txtYear" CssClass="form-control" MaxLength="4" />
                         </div>
                         <div class="col-md-1">
                             <label>Cycle</label><span style="color:red;">*</span>
                             <asp:TextBox runat="server" ID="txtCycle" CssClass="form-control" MaxLength="1" />
                         </div>
                         <div class="col-md-3">
                             <label>Executive Head</label>
                             <asp:DropDownList runat="server" ID="ddlExecutive" CssClass="form-control" AutoPostBack="true" />
                         </div>
                         <div class="col-md-3">
                              <label>Department</label>
                              <asp:DropDownList runat="server" ID="ddlDept" CssClass="form-control" />
                         </div>
                         <div class="col-md-2">
                              <label>BUHR Per. No.</label>
                              <asp:TextBox runat="server" ID="txtBuhr" CssClass="form-control" MaxLength="6" placeholder="BUHR Per. No"  />
                          </div>
                          <div class="col-md-2">
                               <label>Officer Per. No.</label>
                               <asp:TextBox runat="server" ID="txtpnosub" CssClass="form-control" MaxLength="6" placeholder="Officer Per. No"  />
                          </div>
                     </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-10" >
                              <div style="text-align:center;margin:auto;">
                                  <asp:HiddenField ID = "hfGridHtml" runat = "server" />
                                  <asp:LinkButton ID="btn_Show" runat="server" CssClass="btn btn-primary btn-block" Width="120px" type="submit" CausesValidation="false">
                                        <span aria-hidden="true" class="glyphicon glyphicon-eye-open"></span> Show
                                   </asp:LinkButton>&nbsp;
                                  <asp:LinkButton ID="btn_download" runat="server" CssClass="btn btn-primary btn-block" Width="120px" type="submit" CausesValidation="false">
                                        <span aria-hidden="true" class="glyphicon glyphicon-download"></span> Download
                                  </asp:LinkButton>&nbsp;
                                  <asp:LinkButton ID="btn_download_all" runat="server" CssClass="btn btn-primary btn-block" Width="120px" type="submit" CausesValidation="false">
                                    <span aria-hidden="true" class="glyphicon glyphicon-download"></span> Download All
                                   </asp:LinkButton> 
                              </div>
                        </div>
                    </div>
                </div>
 
         <div class="row content">
      <div class="col-md-12 col-lg-12">
        <div class="table-responsive">
            <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                <ContentTemplate>
                   <%-- <div id="Grid">
                    <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
                    </div>--%>
                    <asp:GridView ID="gdvMiniCriteria" runat="server" Visible="true" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer" Font-Names="verdana"
                                                        EmptyDataText="No Record Found" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" RowStyle-CssClass="rows"  OnDataBound = "OnDataBound">
                                                       <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                       <%-- <HeaderStyle  BackColor="#e43c5c" Font-Bold="True" ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />--%>
                        
                          <Columns>
                                                           
                                            <asp:BoundField DataField="pno" HeaderText="P.no" SortExpression="pno"/>  
                                            <asp:BoundField DataField="ema_ename" HeaderText="Name" SortExpression="ema_ename"/>  
                                            <asp:BoundField DataField="ema_empl_sgrade" HeaderText="Level" SortExpression="ema_empl_sgrade"/>  
                                            <asp:BoundField DataField="ema_eqv_level" HeaderText="Equivalent Level" SortExpression="ema_eqv_level"/>  
                                            <asp:BoundField DataField="ema_desgn_desc" HeaderText="Designation" SortExpression="ema_desgn_desc"/>  
                                            <asp:BoundField DataField="ema_dept_desc" HeaderText="Department" SortExpression="ema_dept_desc"/>  
                                            <asp:BoundField DataField="ema_email_id" HeaderText="Email Id" SortExpression="ema_email_id"/>  
                                            <asp:BoundField DataField="ema_exec_head_desc" HeaderText="Executive Head" SortExpression="ema_exec_head_desc"/> 
                                            <asp:BoundField DataField="Superior_PNo" HeaderText="Superior P No." SortExpression="Superior_PNo"/> 
                                            <asp:BoundField DataField="Superior_Name" HeaderText="Superior Name" SortExpression="Superior_Name"/> 
                                            <asp:BoundField DataField="Superior_EmaiId" HeaderText="Superior Name Email ID" SortExpression="Superior_EmaiId"/> 
                                            <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location"/> 
                                            <asp:BoundField DataField="ema_bhr_pno" HeaderText="BUHR P No." SortExpression="ema_bhr_pno"/> 
                                            <asp:BoundField DataField="ema_bhr_name" HeaderText="BUHR Name" SortExpression="ema_bhr_name"/> 
                                            <asp:BoundField DataField="Buhr_emailId" HeaderText="BUHR Email ID" SortExpression="Buhr_emailId"/> 
                                            <asp:BoundField DataField="pending1" HeaderText="Pending" SortExpression="pending1"/>  
                                            <asp:BoundField DataField="completed1" HeaderText="Completed" SortExpression="completed1"/>  
                                            <asp:BoundField DataField="pending2" HeaderText="Pending" SortExpression="pending2"/>  
                                            <asp:BoundField DataField="Completed2" HeaderText="Completed" SortExpression="completed2"/>  
                                            <asp:BoundField DataField="pending3" HeaderText="Pending" SortExpression="pending3"/>  
                                            <asp:BoundField DataField="Completed3" HeaderText="Completed" SortExpression="completed3"/>  
                                            <asp:BoundField DataField="pending4" HeaderText="Pending" SortExpression="pending4"/>  
                                            <asp:BoundField DataField="completed4" HeaderText="Completed" SortExpression="completed4"/>  
                                            <asp:BoundField DataField="pending5" HeaderText="Pending" SortExpression="pending5"/>  
                                            <asp:BoundField DataField="Completed5" HeaderText="Completed" SortExpression="completed5"/>  
                                            <asp:BoundField DataField="pending6" HeaderText="Pending" SortExpression="pending6"/>  
                                            <asp:BoundField DataField="Completed6" HeaderText="Completed" SortExpression="completed6"/>  
                                            <asp:BoundField DataField="Criterial" HeaderText="Overall Critera (OK/ Less)" SortExpression="Criterial"/>  
                                            <%--<asp:BoundField DataField="ema_bhr_name" HeaderText="BUHR Name" SortExpression="ema_bhr_name"/>--%> 
                                                        </Columns>                                

                                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="White" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                        <SortedDescendingHeaderStyle BackColor="#000065" />
                                                    </asp:GridView>
                    <asp:DataGrid ID="dgMiniCriteria" runat="server" Visible="true" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer" Font-Names="verdana" EmptyDataText="No Record Found" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" RowStyle-CssClass="rows">
                        <Columns>
                                            <asp:BoundColumn DataField="pno" HeaderText="P.no" SortExpression="pno"/>  
                                            <asp:BoundColumn DataField="ema_ename" HeaderText="Name" SortExpression="ema_ename"/>  
                                            <asp:BoundColumn DataField="ema_empl_sgrade" HeaderText="Level" SortExpression="ema_empl_sgrade"/>  
                                            <asp:BoundColumn DataField="ema_eqv_level" HeaderText="Equivalent Level" SortExpression="ema_eqv_level"/>  
                                            <asp:BoundColumn DataField="ema_desgn_desc" HeaderText="Designation" SortExpression="ema_desgn_desc"/>  
                                            <asp:BoundColumn DataField="ema_dept_desc" HeaderText="Department" SortExpression="ema_dept_desc"/>  
                                            <asp:BoundColumn DataField="ema_email_id" HeaderText="Email Id" SortExpression="ema_email_id"/>  
                                            <asp:BoundColumn DataField="ema_exec_head_desc" HeaderText="Executive Head" SortExpression="ema_exec_head_desc"/> 
                                            <asp:BoundColumn DataField="Superior_PNo" HeaderText="Superior P No." SortExpression="Superior_PNo"/> 
                                            <asp:BoundColumn DataField="Superior_Name" HeaderText="Superior Name" SortExpression="Superior_Name"/> 
                                            <asp:BoundColumn DataField="Superior_EmaiId" HeaderText="Superior Name Email ID" SortExpression="Superior_EmaiId"/> 
                                            <asp:BoundColumn DataField="Location" HeaderText="Location" SortExpression="Location"/> 
                                            <asp:BoundColumn DataField="ema_bhr_pno" HeaderText="BUHR P No." SortExpression="ema_bhr_pno"/> 
                                            <asp:BoundColumn DataField="ema_bhr_name" HeaderText="BUHR Name" SortExpression="ema_bhr_name"/> 
                                            <asp:BoundColumn DataField="Buhr_emailId" HeaderText="BUHR Email ID" SortExpression="Buhr_emailId"/> 
                                            <asp:BoundColumn DataField="pending1" HeaderText="Pending" SortExpression="pending1"/>  
                                            <asp:BoundColumn DataField="completed1" HeaderText="Completed" SortExpression="completed1"/>  
                                            <asp:BoundColumn DataField="pending2" HeaderText="Pending" SortExpression="pending2"/>  
                                            <asp:BoundColumn DataField="Completed2" HeaderText="Completed" SortExpression="completed2"/>  
                                            <asp:BoundColumn DataField="pending3" HeaderText="Pending" SortExpression="pending3"/>  
                                            <asp:BoundColumn DataField="Completed3" HeaderText="Completed" SortExpression="completed3"/>  
                                            <asp:BoundColumn DataField="pending4" HeaderText="Pending" SortExpression="pending4"/>  
                                            <asp:BoundColumn DataField="completed4" HeaderText="Completed" SortExpression="completed4"/>  
                                            <asp:BoundColumn DataField="pending5" HeaderText="Pending" SortExpression="pending5"/>  
                                            <asp:BoundColumn DataField="Completed5" HeaderText="Completed" SortExpression="completed5"/>  
                                            <asp:BoundColumn DataField="pending6" HeaderText="Pending" SortExpression="pending6"/>  
                                            <asp:BoundColumn DataField="Completed6" HeaderText="Completed" SortExpression="completed6"/>  
                                            <asp:BoundColumn DataField="Criterial" HeaderText="Overall Critera (OK/ Less)" SortExpression="Criterial"/>  
                                                        </Columns>                                

                                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    </asp:DataGrid> 

                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
          </div>
        </div>
            </ContentTemplate>
              <Triggers>
                  <asp:PostBackTrigger ControlID="btn_download" />
                  <asp:PostBackTrigger ControlID="btn_download_all" />
              </Triggers>
    </asp:UpdatePanel>
      </div>

    </section>
    <!-- End About Section -->     

  </main><!-- End #main -->
 <!-- ======= Footer ======= -->
  <footer id="footer">
    <div class="container d-md-flex py-4">

         <div class="mr-md-auto text-center">
              <span>In case of any queries or issues, please reach out to your HRBP. </span>
            <span>In case of any system specific queries or IT issues, please reach out to<b> IT helpdesk (it_helpdesk@tatasteel.com)</b></span>
        </div>
    </div>

  
  </footer><!-- End Footer -->

         
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
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $(function () {
            //$("[id*=btn_download]").click(function () {
                $("[id*=hfGridHtml]").val($("#Grid").html());
            //});
        });
    </script>
   
    </form>
</body>
</html>
