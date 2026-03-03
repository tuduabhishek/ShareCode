<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SurveyRpt.aspx.vb" Inherits="SurveyRpt" MaintainScrollPositionOnPostback="true"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html lang="en">

<head>
    <!-- New Library Versions -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-4.0.0-beta.2.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

  <meta charset="utf-8">
  <meta content="width=device-width, initial-scale=1" name="viewport">

  <title>360 Survey</title>
  <meta content="" name="descriptison">
  <meta content="" name="keywords">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
  <!-- Favicons -->
  <link href="assets/img/favicon.png" rel="icon">
  <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">

  <!-- Google Fonts -->
    <link href="assets/css/googlefont.css" rel="stylesheet" />

  <!-- Vendor CSS Files -->
  <!-- <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"> -->
  <link href="assets/vendor/icofont/icofont.min.css" rel="stylesheet">
  <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
  <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">
  <link href="assets/vendor/venobox/venobox.css" rel="stylesheet">
  <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet">
  <!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"> -->
     <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
    <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>

    <!-- <link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css"> -->
<!-- <script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script> -->
<!-- <script src="//code.jquery.com/jquery-1.11.1.min.js"></script> -->
     <%-- <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet"> --%>
     <!-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script> -->
  <!-- Include all compiled plugins (below), or include individual files as needed -->
  <!-- <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script> -->


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

.square_btn{
    display: inline-block;
    padding: 7px 20px;
	border-radius: 25px;
    text-decoration: none;
    color: #FFF;
    background-image: -webkit-linear-gradient(45deg, #FFC107 0%, #ff8b5f 100%);
    background-image: linear-gradient(45deg, #FFC107 0%, #ff8b5f 100%);
    transition: .4s;
}

.square_btn:hover {
    background-image: -webkit-linear-gradient(45deg, #FFC107 0%, #f76a35 100%);
    background-image: linear-gradient(45deg, #FFC107 0%, #f76a35 100%);
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
        $(function () {
            debugger;
            $('.clickable').on('click', function () {
                var effect = $(this).data('effect');
                $(this).closest('.panel')[effect]();
            })
        });

        $(".close-button").on("click", function () {
            $(this).closest('.collapse-group').find('.collapse').collapse('hide');
        });

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

      <h1 class="logo mr-auto"><a>360 DEGREE FEEDBACK SURVEY</a></h1>
      <!-- Uncomment below if you prefer to use an image logo -->
      <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->

      <nav class="nav-menu d-none d-lg-block">
        <ul>
           <li class="active"><a href="SurveyRpt.aspx">Home</a></li>
            <li class=""><a  href="SurveyRpt.aspx?adm=1">MIS</a></li>
            
        </ul>
      </nav><!-- .nav-menu -->

    </div>
  </header><!-- End Header -->
        <asp:ScriptManager runat="server" ID="scpmgr">

        </asp:ScriptManager>
  <!-- ======= Hero Section ======= -->
  <section id="hero">
    <div class="hero-container">
     <h3>Welcome  <strong><asp:Label ID="lblname" runat ="server" Text="" ></asp:Label></strong></h3>     
      <!--<h1>We're Creative Agency</h1>-->
      <h2 class ="table-responsive1">As we aspire to be the most valuable and respected steel company globally in the next 5-10 years, we are developing agile behaviours in our top leadership - accountability, responsiveness, collaboration and people development. We will measure this as an integral part of our Performance Management System for IL2s through a 360 degree feedback survey.</h2>
      <a href="#about" class="btn-get-started scrollto">View Report</a>
    </div>
  </section><!-- End Hero -->

  <main id="main">

    <!-- ======= About Section ======= -->
      <section id="about" class="about">
      <div class="container">
          <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                <ContentTemplate>
 <div class ="row content">
     <div class ="col-md-3 col-xl-3 col-sm-3">
         <asp:TextBox runat="server" ID="txtpno" CssClass="form-control" placeholder="Enter P.No or name" Visible="false"></asp:TextBox>

         <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtpno"
                                    ServiceMethod="SearchPrefixesForApprover" MinimumPrefixLength="1" CompletionInterval="100"
                                    DelimiterCharacters="" Enabled="True" ServicePath="" 
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListCssClass="AutoExtender"
                                    CompletionListItemCssClass="AutoExtenderList"></cc1:AutoCompleteExtender>
          <cc1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender1" TargetControlID="txtpno" FilterMode="InvalidChars" InvalidChars="':;( )--#"
                                                        ></cc1:FilteredTextBoxExtender>
     </div>
     <div class ="col-md-2 col-xl-2 col-sm-2">
         <asp:DropDownList runat="server" ID ="ddlyear" CssClass="form-control" Visible="false">
            
         </asp:DropDownList>
     </div>
     <div class ="col-md-5 col-xl-5 col-sm-5" style="margin-top:-1%;">
         <asp:Button runat="server" CssClass="btn-learn-more" Text ="Find" ID="btnfind" OnClick="btnfind_Click" Visible="false"/>
          <asp:Button runat="server" CssClass="btn-learn-more" Text ="Download" ID="btndownload" OnClick="btndownload_Click" Visible="false"/>
     </div>
     <div class ="col-md-3 col-xl-3 col-sm-3">
        
     </div>
 </div>
                    
                    <div class ="row content">
     <div class ="col-md-3 col-xl-3 col-sm-3">
         
     </div>
     <div class ="col-md-2 col-xl-2 col-sm-2">
         <asp:DropDownList runat="server" ID ="DropDownList1" CssClass="form-control" Visible="false" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true">
        
         </asp:DropDownList>
     </div>
     <div class ="col-md-5 col-xl-5 col-sm-5" style="margin-top:-1%;">
        
          <asp:Button runat="server" CssClass="btn-learn-more" Text ="Download" ID="btndownloadrej" Visible="false" OnClick="btndownloadrej_Click" />
     </div>
     <div class ="col-md-3 col-xl-3 col-sm-3">
        
     </div>
 </div>
                    <br />
         <div class="row ">
      <div class="col-md-12 col-lg-12">
        <div class="table-responsive">
            
                    <asp:GridView ID="gvself" runat="server"  AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer" Font-Names="verdana"
                                                        EmptyDataText="No Record Found" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" RowStyle-CssClass="rows">
                                                       <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                        <HeaderStyle  BackColor="#e43c5c" Font-Bold="True" ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />
                        
                          <Columns>
                                                           
                                                            <asp:TemplateField HeaderText="FEEDBACK_GIVER_PNO">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfpno" runat="server" Text='<%# Eval("FEEDBACK_GIVER_PNO")%>'></asp:Label>
                                                                   
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Feedback_Giver_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfname" runat="server" Text='<%# Eval("Feedback_Giver_Name")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Feedback_Giver_Email">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfmail" runat="server" Text='<%# Eval("Feedback_Giver_Email")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                             <asp:TemplateField HeaderText="Feedback_Giver_Level">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblflvl" runat="server" Text='<%# Eval("Feedback_Giver_Level")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Feedback_Giver_Designation">
                                                                <ItemTemplate>
                                                                   <asp:Label runat="server" ID="lblfdesg" Text='<%# Eval("Feedback_Giver_Designation")%>' ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField >
                                                            <asp:TemplateField HeaderText="feedback_giver_exec_head">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblfexc" Text='<%# Eval("feedback_giver_exec_head")%>' ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField >  
                              
                                                            <asp:TemplateField HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsts" runat="server" Text='<%# Eval("Status")%>'></asp:Label>
                                                                   
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Category">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcateg" runat="server" Text='<%# Eval("Category")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Assesor_pno">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblapno" runat="server" Text='<%# Eval("Assesor_pno")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                             <asp:TemplateField HeaderText="Assesor_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblaname" runat="server" Text='<%# Eval("Assesor_Name")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Assesor_Email">
                                                                <ItemTemplate>
                                                                   <asp:Label runat="server" ID="lblaemail" Text='<%# Eval("Assesor_Email")%>' ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField >
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                     <asp:Label runat="server" ID="lbladsg" Text='<%# Eval("Assesor_Designation")%>' ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField >   
                              
                              <asp:TemplateField HeaderText="Assesor_Executive_Head">
                                                                <ItemTemplate>
                                                                   <asp:Label runat="server" ID="lblaexe" Text='<%# Eval("Assesor_Executive_Head")%>' ></asp:Label>
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


                      <div class="row ">
      <div class="col-md-12 col-lg-12">
        <div class="table-responsive">
            
                    <asp:GridView ID="GVrejectcount" runat="server" Visible="false" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer" Font-Names="verdana"
                                                        EmptyDataText="No Record Found" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" RowStyle-CssClass="rows">
                                                       <%-- <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />--%>
                                                        <HeaderStyle  BackColor="#e43c5c" Font-Bold="True" ForeColor="Black" />
                                                        <AlternatingRowStyle BackColor="#FFB6C1" />
                        
                          <Columns>
                                                           
                                                            <asp:TemplateField HeaderText="Year">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblyear" runat="server" Text='<%# Eval("SS_YEAR")%>'></asp:Label>
                                                                   
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Assesee P.no">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblassessee" runat="server" Text='<%# Eval("SS_ASSES_PNO")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Category">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcategory" runat="server" Text='<%# Eval("SS_CATEG")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                             <asp:TemplateField HeaderText="Minimum">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblmin" runat="server" Text='<%# Eval("MINIM")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Approved">
                                                                <ItemTemplate>
                                                                   <asp:Label runat="server" ID="lblapproved" Text='<%# Eval("APPROVED")%>' ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField >
                                                         <asp:TemplateField HeaderText="Completed">
                                                                <ItemTemplate>
                                                                   <asp:Label runat="server" ID="lblcompleted" Text='<%# Eval("COMPLETED")%>' ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField >
                                                            <asp:TemplateField HeaderText="INSUFFICIENT_EXPOSURE">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblrej" Text='<%# Eval("INSUFFICIENT_EXPOSURE")%>' ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField >  
                              
                                                            <asp:TemplateField HeaderText="Criteria">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsts" runat="server" Text='<%# Eval("Criteria")%>'></asp:Label>
                                                                   
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

                    </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btndownload" />
                    <asp:PostBackTrigger ControlID="btndownloadrej" />
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
              In case of any queries or issues please reach out to 
 <strong>Ms. Shruti Choudhury: </strong>shruti.choudhury@tatasteel.com and <strong>Mr. Vikas Kumar : </strong>vikas.kumar1@tatasteel.com
        </div>
     <%-- <div class="mr-md-auto text-center text-md-left">
        <div class="copyright">
          &copy; Copyright <strong><span>Tata Steel</span></strong>. All Rights Reserved
        </div>
      </div>  --%> 
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