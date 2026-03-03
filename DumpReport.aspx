<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DumpReport.aspx.vb" Inherits="DumpReport"
  MaintainScrollPositionOnPostback="true" %>

  <!DOCTYPE html>

  <html xmlns="http://www.w3.org/1999/xhtml">

  <head>
    <!-- New Library Versions -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-4.0.0-beta.2.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <meta charset="utf-8">
    <meta content="width=device-width, initial-scale=1.0" name="viewport">

    <title>360 Survey</title>
    <meta content="" name="descriptison">
    <meta content="" name="keywords">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <!-- Favicons -->
    <link href="assets/img/favicon.png" rel="icon">
    <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">

    <%-- <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" /> --%>
    <!-- Google Fonts -->
    <link href="assets/css/googlefont.css" rel="stylesheet" />

    <!-- Vendor CSS Files -->
    <!-- <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"> -->
    <link href="assets/vendor/icofont/icofont.min.css" rel="stylesheet">
    <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
    <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">
    <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
    <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <!-- Template Main CSS File -->
    <link href="assets/css/style.css" rel="stylesheet">
    <style type="text/css">
      @media only screen and (min-width: 992px) {
        .rbl input[type="radio"] {
          margin-left: 210px;
          margin-right: 50px;
          margin-top: 5px;
          transform: scale(2, 2);
          -ms-transform: scale(2, 2);
          -webkit-transform: scale(2, 2);
          align-items: center;
        }


      }

      @media only screen and (max-width: 600px) {
        .rbl td input[type="radio"] {
          margin-left: 80px;
          margin-right: 20px;
          margin-top: 5px;
          transform: scale(2, 2);
          -ms-transform: scale(2, 2);
          -webkit-transform: scale(2, 2);
          align-items: center;
        }

        .divB {
          display: none;
        }

        #btn_reject {
          width: 300px;
          white-space: normal;
          word-wrap: break-word;
        }
      }

      @media only screen and (max-width: 768px) {
        .rbl td input[type="radio"] {
          margin-left: 135px;
          margin-right: 20px;
          margin-top: 5px;
          transform: scale(2, 2);
          -ms-transform: scale(2, 2);
          -webkit-transform: scale(2, 2);
          align-items: center;
        }

        #btn_reject {
          width: 300px;
          white-space: normal;
          word-wrap: break-word;
        }
      }

      @media only screen and (max-width: 800px) {
        .rbl td input[type="radio"] {
          margin-left: 150px;
          margin-right: 20px;
          margin-top: 5px;
          transform: scale(2, 2);
          -ms-transform: scale(2, 2);
          -webkit-transform: scale(2, 2);
          align-items: center;
        }

        #btn_reject {
          width: 300px;
          white-space: normal;
          word-wrap: break-word;
        }
      }

      @media only screen and (max-width: 420px) {
        .rbl td input[type="radio"] {
          margin-left: 50px;
          margin-right: 10px;
          margin-top: 5px;
          transform: scale(1, 1);
          -ms-transform: scale(1, 1);
          -webkit-transform: scale(1, 1);
          align-items: center;
        }

        .divB {
          display: none;
        }

        #btn_reject {
          width: 300px;
          white-space: normal;
          word-wrap: break-word;
        }
      }

      /*@media only screen and (min-width : 320px) and (max-width : 767px) { 
             table.rbl td input[type="radio"]
            {
               margin-left: 100px;
               margin-right: 10px;
               margin-top:5px;
               transform: scale(2, 2);
        -ms-transform: scale(2, 2);
        -webkit-transform: scale(2, 2);
            }
             }*/
      /*@media (min-width: 400px) { 
            table.rbl td input[type="radio"]
            {
               margin-left: 100px;
               margin-right: 10px;
               margin-top:5px;
                 transform: scale(2, 2);
        -ms-transform: scale(2, 2);
        -webkit-transform: scale(2, 2);
            }
             }*/

      ol {
        list-style-type: disc;
      }

      #hero .btn-get-started {
        font-size: 15px !important;
      }


      .badge-primary {
        color: #fff;
        background-color: #e43c5c !important;
      }

      .table-responsive1 {
        display: block;
        width: 100%;
        -webkit-overflow-scrolling: touch;
      }

      #btn_submit:hover {
        background-color: #E43C5C !important;
        color: white !important;
      }

      #btn_reject:hover {
        background-color: #E43C5C !important;
        color: white !important;
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
                  <asp:Label ID="lblWait" runat="server" Text="Please wait" />
                </h3>
                <img src="Images/loader.gif" alt="Loading..." id="ifMobile1" />
              </div>
            </center>
          </div>
        </ProgressTemplate>
      </asp:UpdateProgress>

      <header id="header" class="fixed-top ">
        <div class="container d-flex align-items-center">

          <h1 class="logo mr-auto"><a href="SelectAssesor.aspx">360 DEGREE FEEDBACK SURVEY</a></h1>
          <!-- Uncomment below if you prefer to use an image logo -->
          <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->

          <nav class="nav-menu d-none d-lg-block">
            <ul>
              <li class="active"><a href="Feedback.aspx">Home</a></li>
              <li><a href="#faq">Help</a></li>
            </ul>
            <ul>
              <!-- <li class="active"><a href="SelectAssesor.aspx">Home</a></li>-->
              <!--<li><a href="#about">About</a></li>
          <li><a href="#services">Services</a></li>
          <li><a href="#portfolio">Portfolio</a></li>
          <li><a href="#team">Team</a></li>
          <li><a href="blog.html">Blog</a></li>
          <li class="drop-down"><a href="">Drop Down</a>
            <ul>
              <li><a href="#">Drop Down 1</a></li>
              <li class="drop-down"><a href="#">Deep Drop Down</a>
                <ul>
                  <li><a href="#">Deep Drop Down 1</a></li>
                  <li><a href="#">Deep Drop Down 2</a></li>
                  <li><a href="#">Deep Drop Down 3</a></li>
                  <li><a href="#">Deep Drop Down 4</a></li>
                  <li><a href="#">Deep Drop Down 5</a></li>
                </ul>
              </li>
              <li><a href="#">Drop Down 2</a></li>
              <li><a href="#">Drop Down 3</a></li>
              <li><a href="#">Drop Down 4</a></li>
            </ul>
          </li>
          <li><a href="#contact">Contact</a></li>-->
              <!--    <li><a href="#faq">FAQ</a></li>-->

            </ul>
          </nav><!-- .nav-menu -->

        </div>
      </header><!-- End Header -->
      <asp:ScriptManager runat="server" ID="scpmgr">

      </asp:ScriptManager>
      <!-- ======= Hero Section ======= -->

      <section id="hero">
        <div class="hero-container">
          <h3>Welcome <strong>
              <asp:Label ID="lblname" runat="server" Text=""></asp:Label>
            </strong></h3>

          <h2 class="table-responsive1">As we aspire to be the most valuable and respected steel company globally in the
            next 5-10 years,we are developing agile behaviours in our top leadership- accountability,responsiveness,
            collaboration and people development.We will measure this as an integral part of our Performance Management
            System for IL2s through a 360 degree feedback survey.</h2>

          <a href="#about" class="btn-get-started scrollto"> VIEW REPORT</a>
        </div>
      </section><!-- End Hero -->
      <div class="col-lg-12 col-sm-12 col-md-12">
        <main id="main">

          <!-- ======= About Section ======= -->
          <section id="about" class="about">
            <div class="container-fluid">
              <div class="row">
                <div class="col-lg-12">
                  <h1>Generation of Report</h1>
                </div>
              </div>
              <asp:UpdatePanel ID="upnl1" runat="server">
                <ContentTemplate>
                  <div class="panel-body">
                    <div class="row">
                      <div class="col-md-3">
                        <div class="form-group">

                          <div class="col-md-12">
                            <label>Year</label>
                            <asp:DropDownList runat="server" ID="ddlYear" CssClass="form-control" />
                          </div>
                        </div>
                      </div>
                      <div class="col-md-3">
                        <div class="form-group">

                          <div class="col-md-12">
                            <label>Level Wise</label>
                            <asp:DropDownList runat="server" ID="ddlLevel" CssClass="form-control" AutoPostBack="true">
                              <asp:ListItem>--Select--</asp:ListItem>
                              <asp:ListItem>I1</asp:ListItem>
                              <asp:ListItem>I2</asp:ListItem>
                            </asp:DropDownList>
                          </div>
                        </div>
                      </div>

                      <div class="col-md-3">
                        <div class="form-group">

                          <div class="col-md-12">
                            <label>Officer Per. No.</label>
                            <asp:TextBox runat="server" ID="txtpnosub" CssClass="form-control" MaxLength="6"
                              placeholder="Officer Per. No" />
                          </div>
                        </div>
                      </div>
                      <div class="col-md-3">
                        <div class="form-group">

                          <div class="col-md-12">
                            <div style="margin-top:25px;margin-left:30px;float:left;">
                              <asp:LinkButton ID="btn_Show" runat="server" class="btn btn-success" Width="120px"
                                type="submit" CausesValidation="false">
                                <span aria-hidden="true" class="fa fa-eye"></span> Show
                              </asp:LinkButton>
                            </div>
                            <div style="margin-top:25px;margin-left:5px;float:left;">
                              <asp:LinkButton ID="btn_download" runat="server" class="btn btn-primary" Width="120px"
                                type="submit" CausesValidation="false">
                                <span aria-hidden="true" class="fa fa-file-excel-o"></span> Download
                              </asp:LinkButton>
                            </div>

                          </div>
                        </div>
                      </div>
                    </div>

                  </div>

                  <div class="row content">
                    <div class="col-md-12 col-lg-12">
                      <div class="table-responsive">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                          <ContentTemplate>
                            <asp:GridView ID="gdvMiniCriteria" runat="server" Visible="true" AutoGenerateColumns="False"
                              CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                              Font-Names="verdana" EmptyDataText="No Record Found" BorderStyle="None" BorderWidth="1px"
                              CellPadding="3" GridLines="Vertical" RowStyle-CssClass="rows">
                              <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                              <HeaderStyle BackColor="#e43c5c" Font-Bold="True" ForeColor="Black" />
                              <AlternatingRowStyle BackColor="#FFB6C1" />

                              <Columns>

                                <asp:BoundField DataField="SS_YEAR" HeaderText="Year" SortExpression="SS_YEAR" />
                                <asp:BoundField DataField="SS_ASSES_PNO" HeaderText="Assesee Pno."
                                  SortExpression="SS_ASSES_PNO" />
                                <asp:BoundField DataField="SS_CATEG" HeaderText="Category" SortExpression="SS_CATEG" />
                                <asp:BoundField DataField="SS_PNO" HeaderText="Assessor Pno." SortExpression="SS_PNO" />
                                <asp:BoundField DataField="SS_NAME" HeaderText="Assessor Name"
                                  SortExpression="SS_NAME" />
                                <asp:BoundField DataField="SS_DESG" HeaderText="Assessor Designation"
                                  SortExpression="SS_DESG" />
                                <asp:BoundField DataField="SS_DEPT" HeaderText="Assessor Department"
                                  SortExpression="SS_DEPT" />
                                <asp:BoundField DataField="SS_EMAIL" HeaderText="Assessor Email"
                                  SortExpression="SS_EMAIL" />
                                <asp:BoundField DataField="SS_Q1_A" HeaderText="Accountability"
                                  SortExpression="SS_Q1_A" />
                                <asp:BoundField DataField="SS_Q1_B" HeaderText="Collaboration"
                                  SortExpression="SS_Q1_B" />
                                <asp:BoundField DataField="SS_Q1_C" HeaderText="Responsiveness"
                                  SortExpression="SS_Q1_C" />
                                <asp:BoundField DataField="SS_Q1_D" HeaderText="People Development"
                                  SortExpression="SS_Q1_D" />
                                <asp:BoundField DataField="SS_Q2_A" HeaderText="Strength Q2-A"
                                  SortExpression="SS_Q2_A" />
                                <asp:BoundField DataField="SS_Q2_B" HeaderText="Improvemnets Q2-B"
                                  SortExpression="SS_Q2_B" />
                              </Columns>

                              <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                              <RowStyle BackColor="White" ForeColor="Black" />
                              <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                              <SortedAscendingCellStyle BackColor="#F1F1F1" />
                              <SortedAscendingHeaderStyle BackColor="#0000A9" />
                              <SortedDescendingCellStyle BackColor="#CAC9C9" />
                              <SortedDescendingHeaderStyle BackColor="#000065" />
                            </asp:GridView>
                          </ContentTemplate>
                        </asp:UpdatePanel>
                      </div>
                    </div>
                  </div>
                </ContentTemplate>
                <Triggers>
                  <asp:PostBackTrigger ControlID="btn_download" />
                </Triggers>
              </asp:UpdatePanel>
            </div>

          </section>
          <!-- End About Section -->

        </main><!-- End #main -->
      </div>
      <!-- ======= Footer ======= -->
      <footer id="footer">



        <div class="container d-md-flex py-4">
          <div class="mr-md-auto text-center">
            In case of any queries or issues please reach out to
            <strong>Ms. Shruti Choudhury: </strong>shruti.choudhury@tatasteel.com and <strong>Mr. Vikas Kumar :
            </strong>vikas.kumar1@tatasteel.com
          </div>
          <%--<div class="mr-md-auto text-center text-md-left">
            <div class="copyright">
              &copy; Copyright <strong><span>Tata Steel</span></strong>. All Rights Reserved
            </div>
        </div> --%>
        </div>
      </footer><!-- End Footer -->

      <a href="#" class="back-to-top"><i class="ri-arrow-up-line"></i></a>

      <!-- Vendor JS Files -->
      <%-- <script src="assets/vendor/jquery/jquery.min.js"></script> --%>
      <%-- <script src="assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script> --%>
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