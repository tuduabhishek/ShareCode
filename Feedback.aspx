<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Feedback.aspx.vb" Inherits="Feedback" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="utf-8">
  <meta content="width=device-width, initial-scale=1.0" name="viewport">

  <title>360 Survey</title>
  <meta content="" name="descriptison">
  <meta content="" name="keywords">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

  <!-- Favicons -->
  <link href="assets/img/favicon.png" rel="icon">
  <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">
 
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet"/>
  <!-- Google Fonts -->
    <link href="assets/css/googlefont.css" rel="stylesheet" />

  <!-- Vendor CSS Files -->
  <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
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
             .rbl input[type="radio"]
            {
               margin-left: 210px;
               margin-right: 50px;
               margin-top:5px;
                transform: scale(2, 2);
        -ms-transform: scale(2, 2);
        -webkit-transform: scale(2, 2);
        align-items:center;
            }
             

} 
        @media only screen and (max-width: 600px) {
            .rbl td input[type="radio"]
            {
               margin-left: 80px;
               margin-right: 20px;
               margin-top:5px;
               transform: scale(2, 2);
        -ms-transform: scale(2, 2);
        -webkit-transform: scale(2, 2);
        align-items:center;
            }
            .divB
             {
                 display:none;
             }
            #btn_reject
             {
                 width:300px;
                 white-space: normal;
                 word-wrap: break-word;
             }
        }
         @media only screen and (max-width: 768px) {
            .rbl td input[type="radio"]
            {
               margin-left: 135px;
               margin-right: 20px;
               margin-top:5px;
               transform: scale(2, 2);
        -ms-transform: scale(2, 2);
        -webkit-transform: scale(2, 2);
        align-items:center;
            }
           
            #btn_reject
             {
                 width:300px;
                 white-space: normal;
                 word-wrap: break-word;
             }
        }
          @media only screen and (max-width: 800px) {
            .rbl td input[type="radio"]
            {
               margin-left: 150px;
               margin-right: 20px;
               margin-top:5px;
               transform: scale(2, 2);
        -ms-transform: scale(2, 2);
        -webkit-transform: scale(2, 2);
        align-items:center;
            }
           
            #btn_reject
             {
                 width:300px;
                 white-space: normal;
                 word-wrap: break-word;
             }
        }
         @media only screen and (max-width: 420px) {
            .rbl td input[type="radio"]
            {
               margin-left: 50px;
               margin-right: 10px;
               margin-top:5px;
               transform: scale(1, 1);
        -ms-transform: scale(1, 1);
        -webkit-transform: scale(1, 1);
        align-items:center;
            }
            .divB
             {
                display:none;
             }
             #btn_reject
             {
                 width:300px;
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
      #btn_submit:hover
      {
          background-color:#E43C5C !important;
          color:white !important;
      }
       #btn_reject:hover
      {
          background-color:#E43C5C !important;
          color:white !important;
      }
    </style>
    
     <script type="text/javascript">

         function setCharacters(e)
        {
            var count = document.getElementById('<%=txtAns2.ClientID%>');
            var lblcount = document.getElementById('<%=lblCountChar.ClientID %>');
            var total = parseInt(count.value.length);
            lblcount.innerHTML = 'You have entered ' +  total + ' characters out of 500.';
            return false;
        }
        

        function setLength(e)
        {
            var count = document.getElementById('<%=txtAns2.ClientID%>');
            var total = parseInt(count.value.length);
            if(total > 499)
            {
                if (window.event)
                {
                window.event.returnValue = false;
                }
                else
                {
                    if(e.which > 31)
                    {
                        e.preventDefault();
                    }
                }
                //return false;
            }
            else
            {
            return true;
            }
        }
         function maxLengthPaste1(field, maxChars) {
             debugger;
           // event.returnValue = false;
            if ((field.value.length + window.clipboardData.getData("Text").length) > maxChars) {
                alert("more than " + maxChars + " chars");
                event.returnValue = false;
            }
            event.returnValue = true;
        }


         function setCharacters1(e)
        {
            var count = document.getElementById('<%=txtAns1.ClientID%>');
            var lblcount = document.getElementById('<%=Label1.ClientID %>');
            var total = parseInt(count.value.length);
            lblcount.innerHTML = 'You have entered ' +  total + ' characters out of 500.';
            return false;
        }
        

        function setLength1(e)
        {
            var count = document.getElementById('<%=txtAns1.ClientID%>');
            var total = parseInt(count.value.length);
            if(total > 499)
            {
                if (window.event)
                {
                window.event.returnValue = false;
                }
                else
                {
                    if(e.which > 31)
                    {
                        e.preventDefault();
                    }
                }
                //return false;
            }
            else
            {
            return true;
            }
        }
       
        function showGenericMessageModal(type, message) {
            swal('', message, type);
        }

       

    </script>

</head>

<body>

   
    <form id="form1" runat="server">
   
  <!-- ======= Header ======= -->
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
    <div class="hero-container" >
      <h3>Welcome  <strong><asp:Label ID="lblname" runat ="server" Text="" ></asp:Label></strong></h3>     
     
        <h2 class="table-responsive1">As we aspire to be the most valuable and respected steel company globally in the next 5-10 years,we are developing agile behaviours in our top leadership- accountability,responsiveness, collaboration and people development.We will measure this as an integral part of our Performance Management System for IL2s through a 360 degree feedback survey.</h2>

      <a href="#about" class="btn-get-started scrollto" > START WITH YOUR FEEDBACK</a>
    </div>
  </section><!-- End Hero -->
    <div class="col-lg-12 col-sm-12 col-md-12">
  <main id="main">

    <!-- ======= About Section ======= -->
      <section id="about" class="about">
      <div class="container">

        <div class="section-title">          
             <h3> YOUR <span> FEEDBACK</span></h3>        
        </div>
            <div class="row content">
              
        <div class="col-md-12 col-sm-12 col-lg-12">
         
                    <asp:UpdatePanel ID="upPending" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group table-responsive col-lg-12 col-sm-12 col-md-12">
                                <asp:GridView ID="gvPending" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered dataTable no-footer"
                                                        EmptyDataText="No Record Found"  ShowHeader="true"  OnRowCommand="gvPending_RowCommand" OnRowDataBound="gvPending_RowDataBound"
                                   BackColor="#ffccff"   BorderColor="Black" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" RowStyle-CssClass="rows">
                                                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                        <HeaderStyle CssClass="bg-clouds segoe-light" BackColor="#FFB6C1" Font-Bold="True" ForeColor="Black" />
                                                        
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="P.no">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblpno" runat="server" Text='<%# Eval("ss_asses_pno")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lnlname" runat="server" Text='<%# Eval("ema_ename")%>'></asp:Label>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldesg" runat="server" Text='<%# Eval("ema_desgn_desc")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField >                                                           
                                                            <asp:TemplateField HeaderText="Department">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldept" runat="server" Text='<%# Eval("ema_dept_desc")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                              <asp:TemplateField HeaderText="Approval Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                   <asp:LinkButton runat="server" CssClass="btn-learn-more" ID="lbselect" CausesValidation="false" CommandArgument='<%# Container.DataItemIndex %>' CommandName="select"></asp:LinkButton>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Department"  Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("status")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                           
                                                        </Columns>
                                   <PagerStyle BackColor="#c7c7c7" ForeColor="Black" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="White" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#e2e2e2" Font-Bold="True" ForeColor="Black" />
                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                        <SortedDescendingHeaderStyle BackColor="#000065" />
                                                    </asp:GridView>
                            </div>

                  

                            <div id="pnl" runat="server" visible="false">


              <div class="container">       
                            <div class="section-title">    
                           <h3>      Rate  <span> <asp:Label ID="lblRateNm" runat="server" Text=""/></span> on the following statements</span></h3>  
        </div>      
                                

<div class="row">
                        <div class="col-md-1 col-sm-1 col-2">
                           
                            <span class="badge badge-pill badge-primary ml-2" style="width:50px; font-size:large; font-weight:bold;">Q1</span>
                           
                        </div>
                                          <div class="col-md-11 col-sm-11 col-10">
                     
                           
                            <h5>&nbsp; If <asp:Label ID="lblRecipientNm1" runat="server" Text="" /> satisfies at least 3 out of 4 statements, then select the particular circular, else select the central circle</h5>
                            
                        </div>
                                          </div>
                  <br />
                       <div class="row">
                            <div class="col-md-1 col-sm-1 col-3">
                                 <div style="margin-left:15px; margin-top:5px;">
                                <span class="badge badge-pill badge-primary ml-2" style="width:50px; font-size:large;  font-weight:bold; font-size: 20px;">A</span>
                                </div>
                           </div>
                            <div class="col-md-2 col-sm-2 col-5">
                                 <div style="margin-left:13px; margin-top:5px; font-weight:bold; font-size: 20px;">
                                Ownership
                                </div>
                           </div>
                             <div class="col-md-10 col-sm-10 col-4">
                           </div>
                        </div>
                       <div class="row">
                           
                           <div class="col-12" style="margin-left:1px">
                            
                                 <table class="table table-borderless">
                                     <tbody>
                                     <tr>
                                         <td style="width:40%">
                                          <i class="fa fa-circle small" style="font-size:6px" ></i> Passes on the onus of decision making to others, even for own area
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:40%">
                                             <i class="fa fa-circle small" style="font-size:6px"></i> Proactively takes decisions to optimize organizational outcomes
                                         </td>
                                         </tr>
                                     <tr>
                                         <td style="width:40%">
                                            <i class="fa fa-circle small" style="font-size:6px"></i> Does not deliver on committed outcomes
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:40%">
                                            <i class="fa fa-circle small" style="font-size:6px"></i> Sets direction, follows through and get things done no matter what
                                         </td>
                                         </tr>
                                      <tr>
                                         <td style="width:40%">
                                           <i class="fa fa-circle small" style="font-size:6px"></i> Does not delegate responsibility to team members
                                         </td>
                                         <td>
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:10%">
                                            <i class="fa fa-circle small" style="font-size:6px"></i> Fully empowers teams to deliver, while retaining accountability
                                         </td>
                                         </tr>
                                      <tr>
                                         <td style="width:40%">
                                          <i class="fa fa-circle small" style="font-size:6px"></i> Finds it but waits for others to own it and fix it
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:40%">
                                           <i class="fa fa-circle small" style="font-size:6px"></i> Finds it, owns it, fixes it
                                         </td>
                                         </tr>
                                         </tbody> 
                                     </table>


                                
                                     
                               </div>
                           <div class="col-12 row"  >
                           
                            <asp:RadioButtonList ID="rblQ1a" runat="server" RepeatDirection="Horizontal" BorderStyle="None" CssClass="form-control rbl">
                                <asp:ListItem Text="" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="" Value="2"></asp:ListItem>
                                <asp:ListItem Text="" Value="3"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                               
                           </div>
                            
                           
                   
                       <div class="row">
                             <div class="offset-1">

                             </div>
                           
                        </div>
                                            
                        <div class="row">
                            <div class="col-md-1 col-sm-1 col-3">
                                <div style="margin-left:15px; margin-top:5px;">
                                <span class="badge badge-pill badge-primary ml-2" style="width:50px; font-size:large; font-weight:bold;">B</span>
                                </div>
                           </div>
                            <div class="col-md-2 col-sm-2 col-5">
                                 <div style="margin-left:13px; margin-top:5px; font-weight:bold; font-size: 20px;">
                                Collaboration
                                </div>
                           </div>
                        </div>
                        <div class="row" >
                           
                            <div class="col-12" style="margin-left:1px">
                            <table class="table table-borderless" >
                                     <tbody>
                                     <tr>
                                         <td style="width:40%">
                                          <i class="fa fa-circle small" style="font-size:6px" ></i> Outlines tasks for stakeholders without giving clarity on vision/ purpose
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:40%">
                                             <i class="fa fa-circle small" style="font-size:6px"></i> Outlines a shared vision/ purpose and clarifies organizational impact
                                         </td>
                                         </tr>
                                     <tr>
                                         <td style="width:40%">
                                            <i class="fa fa-circle small" style="font-size:6px"></i> Holds back in sharing resources (information, knowledge and talent)
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:40%">
                                            <i class="fa fa-circle small" style="font-size:6px"></i> Proactively volunteers to share resources (information, knowledge and talent)
                                         </td>
                                         </tr>
                                      <tr>
                                         <td style="width:40%">
                                           <i class="fa fa-circle small" style="font-size:6px"></i> Doesn’t acknowledge others’ contributions in achievement of outcomes
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:40%">
                                            <i class="fa fa-circle small" style="font-size:6px"></i> Promotes recognition of others’ contributions in achievement of outcomes
                                         </td>
                                         </tr>
                                      <tr>
                                         <td style="width:40%">
                                          <i class="fa fa-circle small" style="font-size:6px"></i> Raises their voice and behaves disrespectfully
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:40%">
                                           <i class="fa fa-circle small" style="font-size:6px"></i> Respectful to all individuals at all occasions
                                         </td>
                                         </tr>
                                         </tbody> 
                                     </table>
                                </div>

                          <div class="col-12"   >
                            <asp:RadioButtonList ID="rblQ1b" runat="server" RepeatDirection="Horizontal"  BorderStyle="None" CssClass="form-control rbl" class="align-items-center">
                                <asp:ListItem Text="" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="" Value="2"></asp:ListItem>
                                <asp:ListItem Text="" Value="3"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                   
                       </div>
                   
                       
                           
                        </div>
                         <br />
                         <br />
                        <div class="row">
                            <div class="col-md-1 col-sm-1 col-3">
                                <div style="margin-left:15px; margin-top:5px;">
                                <span class="badge badge-pill badge-primary ml-2" style="width:50px; font-size:large; font-weight:bold;">C</span>
                                </div>
                           </div>
                            <div class="col-md-2 col-sm-2 col-5">
                                 <div style="margin-left:13px; margin-top:5px; font-weight:bold; font-size: 20px;">
                                Responsiveness
                                </div>
                           </div>
                        </div>
                        <div class="row">
                         <%--  <div class="offset-1">
                               
                           </div>--%>
                           
                            <div class="col-12" style="margin-left:1px">
                            <table class="table table-borderless" >
                                     <tbody>
                                     <tr>
                                         <td style="width:40%">
                                          <i class="fa fa-circle small" style="font-size:6px" ></i> Holds multiple meetings/ discussions without initiating action
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:40%">
                                             <i class="fa fa-circle small" style="font-size:6px"></i> Moves quickly from discussions/ meetings to action
                                         </td>
                                         </tr>
                                     <tr>
                                         <td style="width:40%">
                                            <i class="fa fa-circle small" style="font-size:6px"></i> Doesn’t question the process even if they know it won’t achieve the outcome
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:40%">
                                            <i class="fa fa-circle small" style="font-size:6px"></i> Improves the process to achieve the desired outcome
                                         </td>
                                         </tr>
                                      <tr>
                                         <td style="width:40%">
                                           <i class="fa fa-circle small" style="font-size:6px"></i> Not open to change
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:40%">
                                            <i class="fa fa-circle small" style="font-size:6px"></i> Is a change champion
                                         </td>
                                         </tr>
                                      <tr>
                                         <td style="width:40%">
                                          <i class="fa fa-circle small" style="font-size:6px"></i> Misses out on customer commitments
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td class="align-baseline">
                                           <i class="fa fa-circle small" style="font-size:6px"></i> Anticipates customer needs and help them achieve their goals
                                         </td>
                                         </tr>
                                         </tbody> 
                                     </table>
                                </div>
                            <div class="col-12" style="margin-left:10px">
                            <asp:RadioButtonList ID="rblQ1c" runat="server" RepeatDirection="Horizontal"  BorderStyle="None" CssClass="form-control rbl" style="width:100%">
                                <asp:ListItem Text="" Value="1" Selected="True" ></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>

                           </div>
                      
                       <br />
                       <br />
                     
                        <div class="row">
                            <div class="col-md-1 col-sm-1 col-3">
                                <div style="margin-left:15px; margin-top:5px;">
                                <span class="badge badge-pill badge-primary ml-2" style="width:50px; font-size:large; font-weight:bold;">D</span>
                                </div>
                           </div>
                            <div class="col-md-3 col-sm-2 col-8">
                                 <div style="margin-left:13px; margin-top:5px; font-weight:bold; font-size: 20px;">
                                People Development
                                </div>
                           </div>
                        </div>
                        <div class="row">
                           
                           
                            <div class="col-12" style="margin-left:1px">
                            <table class="table table-borderless" style="width:100%" >
                                     <tbody>
                                     <tr>
                                         <td style="width:40%" >
                                          <i class="fa fa-circle small" style="font-size:6px" ></i> Promotes a transactional culture without any feedback and where compliant views are encouraged
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:40%">
                                             <i class="fa fa-circle small" style="font-size:6px"></i> Promotes an open trust-based culture where real-time feedback and logical dissent is encouraged
                                         </td>
                                         </tr>
                                     <tr>
                                         <td style="width:40%">
                                            <i class="fa fa-circle small" style="font-size:6px"></i> Is satisfied with team members just delivering within their scope of work 
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:40%">
                                            <i class="fa fa-circle small" style="font-size:6px"></i> Coaches and enables team members to achieve truly ambitious goals
                                         </td>
                                         </tr>
                                      <tr>
                                         <td style="width:40%">
                                           <i class="fa fa-circle small" style="font-size:6px"></i> Holds talent in the same role for long periods
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:40%">
                                            <i class="fa fa-circle small" style="font-size:6px"></i> Enables career movement and growth and does not hold talent
                                         </td>
                                         </tr>
                                      <tr>
                                         <td style="width:40%">
                                          <i class="fa fa-circle small" style="font-size:6px"></i> Has a weak following - Team members are not excited to be on their team
                                         </td>
                                         <td style="width:10%">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td style="width:40%">
                                           <i class="fa fa-circle small" style="font-size:6px"></i> Has a strong following  - Team members aspire to be on their team
                                         </td>
                                         </tr>
                                         </tbody> 
                                     </table>
                                </div>
                            <div class="col-12" style="margin-left:10px" >
                            <asp:RadioButtonList ID="rblQ1d" runat="server"  RepeatDirection="Horizontal"  BorderStyle="None" CssClass="form-control rbl">
                                <asp:ListItem Text="" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="" Value="2"></asp:ListItem>
                                <asp:ListItem Text="" Value="3"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>

                           </div>
                       <div class="row table">
                        <div class="col-md-1 col-sm-1 col-2">
                            <div style="margin-top:15px;">
                            <span class="badge badge-pill badge-primary ml-2" style="width:50px; font-size:large; font-weight:bold;">Q2</span>
                            </div>
                        </div>
                           
                        <div class="col-md-1 col-sm-1 col-1 ">
                            <div style="margin-top:15px;">
                            <span class="badge badge-pill badge-primary ml-2" style="width:50px; font-size:large; font-weight:bold;">A</span>
                            </div>
                        </div>
                        <div class="col-md-10 col-sm-10 col-12 form-group">
                            <div style="margin-top:15px;">
                            <label>What have you seen <asp:Label ID="lblRecipientNm3" runat="server" Text="" ></asp:Label> do that is exemplary of any of the 4 agile behaviours?<font color="Red">*</font></label>
                            <asp:TextBox ID="txtAns1" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"
                                onkeypress="return setLength1(event);" onkeyup="return setCharacters1(event);" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAns1" ValidationGroup="vgFeedback" SetFocusOnError="true" ErrorMessage="Please provide your response" ForeColor="#E43C5C"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAns1"
                Display="Dynamic" ErrorMessage="Please enter 500s characters or less."  ValidationGroup="vgFeedback" ForeColor="Red" SetFocusOnError="true"
                ValidationExpression="[\s\S]{1,500}"></asp:RegularExpressionValidator>
                                <asp:Label ID="Label1" Text="" runat="server" />
                            </div>
                        </div>
                           <div class="col-md-0 col-sm-0 col-2">
                            <div style="margin-top:15px;">
                           <%-- <span class="badge badge-pill badge-primary ml-2" style="width:50px; font-size:large; font-weight:bold;">Q2</span>--%>
                            </div>
                        </div>
                            
                        
                    </div>
                       <br />
                       <div class="row table">
                         <div class="col-1 divB"  >
                            <div style="margin-top:15px;">
                            <%--<span class="badge badge-pill badge-primary ml-2" style="width:50px; font-size:large; font-weight:bold;">Q2</span>--%>
                            </div>
                        </div>
                         <div class="col-md-1 col-sm-1 col-2">
                            <div style="margin-top:15px;">
                            <span class="badge badge-pill badge-primary ml-2" style="width:50px; font-size:large; font-weight:bold;">B</span>
                            </div>
                       </div>
                        <div class="col-md-10 col-sm-10 col-lg-10  form-group">
                            <div style="margin-top:15px;">
                            <label>How can <asp:Label ID="lblRecipientNm4" runat="server" Text=""></asp:Label> improve on behaviours that enhance agility?<font color="Red">*</font></label>
                            <asp:TextBox ID="txtAns2" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6"
                                onkeypress="return setLength(event);" onkeyup="return setCharacters(event);" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAns2" ValidationGroup="vgFeedback" ErrorMessage="Please provide your response" ForeColor="#E43C5C" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtAns2"
                Display="Dynamic" ErrorMessage="Please enter 500s characters or less."  ValidationGroup="vgFeedback" ForeColor="Red" SetFocusOnError="true"
                ValidationExpression="[\s\S]{1,500}"></asp:RegularExpressionValidator>
                                <asp:Label ID="lblCountChar" Text="" runat="server" />
                            </div>
                        </div>
                    </div>
                                <div class="box-footer" style="text-align:center;">
        <asp:LinkButton ID="btn_submit" runat="server" class="btn-learn-more" style="background-color:#F2F2F2" Width="120px" Text="Submit" ValidationGroup="vgFeedback" data-toggle="modal" data-target="#staticBackdrop"/>

       &nbsp;&nbsp; <asp:LinkButton ID="btn_reject" runat="server" class="btn-learn-more" style="background-color:#F2F2F2" Text="Insufficient Exposure to provide feedback" data-toggle="modal" data-target="#staticBackdrop1"/>
                         
    </div>
                  </div>                             
                                 </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                             <%--<asp:PostBackTrigger ControlID="btn_submit" />
                             <asp:PostBackTrigger ControlID="btn_reject" />--%>
                        </Triggers>
                    </asp:UpdatePanel>           
        </div>  </div> </div>

          <div class="modal fade" id="staticBackdrop" data-backdrop="static" data-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      
      <div class="modal-body">
      Are you sure you want to submit the form?
      </div>
      <div class="modal-footer">
         
          <asp:Button runat="server" ID="btnyesclick" OnClick="Submit" Text="Yes" class="btn btn-primary" ValidationGroup="vgFeedback" />
        <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
        
      </div>
    </div>
  </div>
</div>

             <div class="modal fade" id="staticBackdrop1" data-backdrop="static" data-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      
      <div class="modal-body">
        Are you sure you want to reject the form and go ahead with the option of insufficient exposure?
      </div>
      <div class="modal-footer">
         
          <asp:Button runat="server" ID="Button1" OnClick="reject" Text="Yes" class="btn btn-primary"  />
        <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
        
      </div>
    </div>
  </div>
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
 <strong>Ms. Shruti Choudhury: </strong>shruti.choudhury@tatasteel.com and <strong>Mr. Vikas Kumar : </strong>vikas.kumar1@tatasteel.com
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
  <script src="assets/vendor/jquery/jquery.min.js"></script>
  <script src="assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
  <script src="assets/vendor/jquery.easing/jquery.easing.min.js"></script>
  <script src="assets/vendor/php-email-form/validate.js"></script>
  <script src="assets/vendor/isotope-layout/isotope.pkgd.min.js"></script>
  <script src="assets/vendor/venobox/venobox.min.js"></script>
  <script src="assets/vendor/owl.carousel/owl.carousel.min.js"></script>

  <!-- Template Main JS File -->
  <script src="assets/js/main.js"></script>
        <%--<script type="text/javascript">
            function info(title_msg, text_msg, icon_type) {
                swal({
                    title: title_msg,
                    text: text_msg,
                    icon: icon_type
                    // type: btn_type
                }).then(() => {
                    location.href = 'Feedback.aspx'
                });
            }
            function msg(title_msg, text_msg, icon_type) {
                swal({
                    title: title_msg,
                    text: text_msg,
                    icon: icon_type
                    // type: btn_type
                });
            }

    </script>--%>
 </form>
</body>

</html>
