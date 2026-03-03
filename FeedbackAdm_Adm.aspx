<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FeedbackAdm_Adm.aspx.vb" Inherits="FeedbackAdm_Adm"  MaintainScrollPositionOnPostback="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <!-- New Library Versions -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-4.0.0-beta.2.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

  <meta charset="utf-8"/>
  <meta content="width=device-width, initial-scale=1.0" name="viewport"/>


  <title>360 Survey</title>
  <meta content="" name="descriptison"/>
  <meta content="" name="keywords"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

  <!-- Favicons -->
  <link href="assets/img/favicon.png" rel="icon"/>
  <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon"/>
 
    <%-- <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet"/> --%>
  <!-- Google Fonts -->
    <link href="assets/css/googlefont.css" rel="stylesheet" />

  <!-- Vendor CSS Files -->
  <%-- <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/> --%>
  <link href="assets/vendor/icofont/icofont.min.css" rel="stylesheet"/>
  <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet"/>
  <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet"/>
  <link href="assets/vendor/venobox/venobox.css" rel="stylesheet"/>
  <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet"/>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
  <!-- Template Main CSS File -->
  <link href="assets/css/styleIL3.css" rel="stylesheet"/>
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
             .option{
                 margin-left:200px;
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
            .option{
                 margin-left:200px;
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
            .option{
                 margin-left:200px;
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
            .option{
                 margin-left:100px;
             }
        }
        @media only screen and (max-width: 373px) {
            .rbl td input[type="radio"] {
                margin-left: 10px;
                margin-right: 10px;
                margin-top: 5px;
                transform: scale(1, 1);
                -ms-transform: scale(1, 1);
                -webkit-transform: scale(1, 1);
                align-items: center;
            }

            table {
                width: 350px;
                font-size:small;
            }
            .option{
                 margin-left:40px;
             }
        }
          @media only screen and (min-width: 374px) and (max-width: 395px){
            .rbl td input[type="radio"] {
                margin-left: 10px;
                margin-right: 10px;
                margin-top: 5px;
                transform: scale(1, 1);
                -ms-transform: scale(1, 1);
                -webkit-transform: scale(1, 1);
                align-items: center;
            }

            table {
                width: 360px;
                font-size:small;
            }
            .option{
                 margin-left:45px;
             }
        }
         @media only screen and (min-width: 410px) and (max-width:480) {
            .rbl td input[type="radio"]
            {
               margin-left: 30px;
               margin-right: 10px;
               margin-top:5px;
               transform: scale(1, 1);
        -ms-transform: scale(1, 1);
        -webkit-transform: scale(1, 1);
        align-items:center;
            }
            table{
                width:375px;
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
             .option{
                 margin-left:50px;
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
        .auto-style1 {
            width: 216px;
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


    <%--<script>

      (function(d){

         var s = d.createElement("script");

         /* uncomment the following line to override default position*/

         /* s.setAttribute("data-position", 1);*/

         /* uncomment the following line to override default size (values: small, large)*/

         /* s.setAttribute("data-size", "large");*/

         /* uncomment the following line to override default language (e.g., fr, de, es, he, nl, etc.)*/

         /* s.setAttribute("data-language", "null");*/

         /* uncomment the following line to override color set via widget (e.g., #053f67)*/

         /* s.setAttribute("data-color", "#2d68ff");*/

         /* uncomment the following line to override type set via widget (1=person, 2=chair, 3=eye, 4=text)*/

         /* s.setAttribute("data-type", "1");*/

         /* s.setAttribute("data-statement_text:", "Our Accessibility Statement");*/

         /* s.setAttribute("data-statement_url", "http://www.example.com/accessibility";*/

         /* uncomment the following line to override support on mobile devices*/

         /* s.setAttribute("data-mobile", true);*/

         /* uncomment the following line to set custom trigger action for accessibility menu*/

         /* s.setAttribute("data-bs-trigger", "triggerId")*/

         s.setAttribute("data-account", "XMXIaBPVwG");

         s.setAttribute("src", "https://cdn.userway.org/widget.js");

         (d.body || d.head).appendChild(s);})(document)

</script>--%>

<%--<noscript>

Please ensure Javascript is enabled for purposes of

<a href="https://userway.org">website accessibility</a>

</noscript>--%>


</head>

<body>

   
    <form id="form1" runat="server">
   
  <!-- ======= Header ======= -->
  <header id="header" class="fixed-top ">
    <div class="container d-flex align-items-center">

      <h1 class="logo mr-auto"><a href="FeedbackAdm_OPR.aspx">360 DEGREE FEEDBACK SURVEY</a></h1>
      <!-- Uncomment below if you prefer to use an image logo -->
      <!-- <a href="index.html" class="logo mr-auto"><img src="assets/img/logo.png" alt="" class="img-fluid"></a>-->

      <nav class="nav-menu d-none d-lg-block">
           <ul>
          <li class="active"><a href="AddRespondent.aspx">Home</a></li>
                <%--<li><a href="Images/Step -3_Filling up Survey.pdf" target="_blank">Help</a></li>--%>
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
     
        <h2 class="table-responsive1">As we aspire to be the most valuable and respected steel company globally in the next 5-10 years, we are developing agile behaviours across the organization. We will measure this as an integral part of our Performance Management System for all the officers through a 360 degree feedback survey</h2>

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
          <div class="row">
              <div style="text-align:center;margin:auto;color:red;font-weight:bold;">
                  <p>
                      You may wish to reject any survey triggered to you if you feel that you are not informed enough to respond for the particular officer. The option to reject would remain open only for the first 5 days from the date of triggering of the survey. Please note that this option should be exercised very carefully because it may affect an individual's report generation criteria
                  </p>
              </div>
          </div>
            <div class="row content form-group">
        <div class="col-md-3 col-sm-12 col-lg-3">
        <%-- <label>Assess Per. no.</label>
                        <asp:TextBox ID="txtAssespno" runat="server" Text="" CssClass="form-control" placeholder="Assess Per. No." MaxLength="6" />      --%> 
        </div>
        <div class="col-md-2 col-sm-12 col-lg-2">
            <div style="margin-top:5px;">
                <label>Respondent Per. no.</label>  
            </div>                               
        </div> 
        <div class="col-md-3 col-sm-12 col-lg-3">
           <asp:TextBox ID="txtRespondent" runat="server" Text="" CssClass="form-control" placeholder="Respondent Per. No." />       
        </div>  
        <div class="col-md-2 col-sm-12 col-lg-2">
         <div>
                <asp:LinkButton runat="server" CssClass="btn btn-primary form-control" ID="lbtnShow" CausesValidation="false">Show</asp:LinkButton>
            </div>      
        </div>
            </div>
          <div class="row  content form-group">
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
                                                              <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center">
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

                                <asp:Label runat="server" ID="aspno" Visible="false"></asp:Label>
              <div class="container">       
                            <div class="section-title">    
                           <h3>      Rate  <span> <asp:Label ID="lblRateNm" runat="server" Text=""/></span> on the following statements</span></h3>  
        </div>      
                  <div class="row">
                      <div style="text-align:center; margin:auto;">
                          <asp:LinkButton ID="btn_reject" runat="server" class="btn-learn-more" style="background-color:#F2F2F2" Text="Insufficient Exposure to provide feedback" data-bs-toggle="modal" data-bs-target="#staticBackdrop1" Visible="false"/>
                      </div>
                  </div>
                                

<div class="row">
                        
                                          <div class="col-md-11 col-sm-11 col-12">
                     
                           
                            <h5>&nbsp;<asp:Label ID="lblRecipientNm1" runat="server" Text="" Visible="false" /></h5>
                            
                        </div>
                                          </div>
                       <div class="row" visible="false">
                           
                           <div class="col-6"  >
                           
                            <asp:RadioButtonList ID="rblQ1a" runat="server" RepeatDirection="Horizontal" BorderStyle="None" CssClass="form-control rbl" Visible="false">
                                <asp:ListItem Text="" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="" Value="2"></asp:ListItem>
                                <asp:ListItem Text="" Value="3"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                               
                           </div>
                            
                           
                   
                        <div class="row">
                            
                            
                            
                            <div class="col-6">
                                <asp:RadioButtonList ID="rblQ1b" runat="server" BorderStyle="None" class="align-items-center" CssClass="form-control rbl" RepeatDirection="Horizontal" Visible="false">
                                    <asp:ListItem Selected="True" Text="" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="" Value="3"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                   
                       
                           
                        </div>
                        <div class="row">
                            <div class="col-md-1 col-sm-12 col-12">
                                <div style="margin-top:15px;">
                                <span class="badge badge-pill badge-primary ml-2" style="width:50px; font-size:large; font-weight:bold;">Q1</span>
                                </div>
                            </div>
                            <div class="col-md-11 col-sm-12 col-12 ">
                                <div style="margin-top:15px;">
                                    <%--Commented and Added by TCS on 291222, Change question text--%>
                                <%--<b>If <asp:Label ID="lblRcptNm" Text="" runat="server" /> consistently displays either of the mentioned behaviours, then select the corresponding circle below the statement,else select the circle in the middle</b>--%>
                                    <b>If <asp:Label ID="lblRcptNm" Text="" runat="server" /> consistently displays one of the mentioned behaviours, then select the corresponding circle below the statement.</b>
                                </div>
                            </div>
                        </div>
                       
                        <div class="row">
                            <div class="col-12" style="margin-left:1px">
                            <table class="table table-borderless col-12" >

                                     <tbody class="col-12">
                                         
                                        
                                      <%--  <tr>
                                             <td class="col-5" style="width:46%">
                                               <span id="span1" runat="server"></span>
                                             </td>
                                            <td style="width:8%;padding-right: 107px;">
                                            </td>
                                              <td class="col-5" style="width:46%">
                                               <span id="span1P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                           <tr>
                                             <td class="col-4" style="width:30%">
                                               <span id="span1" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="span1M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="span1P" runat="server"></span>
                                             </td>
                                         </tr>
                                         <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                       <asp:RadioButtonList ID="rblQ1" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                                            <asp:ListItem Text="" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                                            <asp:ListItem Text="" Value="3" ></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                       <%--  <tr>
                                             <td class="col-5" style="width:46%">
                                               <span id="span2" runat="server"></span>
                                             </td>
                                             <td style="width:8%;padding-right: 107px;">
                                            </td>
                                             <td class="col-5" style="width:46%">
                                                 <span id="span2P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                           <tr>
                                             <td class="col-4" style="width:30%">
                                               <span id="span2" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="span2M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="span2P" runat="server"></span>
                                             </td>
                                         </tr>
                                         <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                       <asp:RadioButtonList ID="rblQ2" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                                            <asp:ListItem Text="" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                                            <asp:ListItem Text="" Value="3" ></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                         <%-- <tr>
                                             <td class="col-5" style="width:46%">
                                               <span id="Span3" runat="server"></span>
                                             </td>
                                              <td style="width:8%;padding-right: 107px;">
                                             <td class="col-5" style="width:46%">
                                                 <span id="Span3P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                         <tr>
                                             <td class="col-4" style="width:30%">
                                               <span id="Span3" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="Span3M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="Span3P" runat="server"></span>
                                             </td>
                                         </tr>
                                         <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                       <asp:RadioButtonList ID="rblQ3" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                       <%--  <tr>
                                             <td class="col-5" style="width:46%">
                                               <span id="Span4" runat="server"></span>
                                             </td>
                                             <td style="width:8%;padding-right: 107px;">
                                            </td>
                                             <td class="col-5" style="width:46%">
                                                <span id="Span4P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                          <tr>
                                             <td class="col-4" style="width:30%">
                                               <span id="Span4" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="Span4M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="Span4P" runat="server"></span>
                                             </td>
                                         </tr>
                                          <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                        <asp:RadioButtonList ID="rblQ4" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                         <%--  <tr>
                                             <td class="col-5" style="width:46%">
                                               <span id="Span5" runat="server"></span>
                                             </td>
                                               <td class="col-2" style="width:8%;padding-right: 107px;">
                                            </td>
                                             <td class="col-5" style="width:46%">
                                                 <span id="Span5P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                         <tr>
                                             <td class="col-4" style="width:30%">
                                               <span id="Span5" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="Span5M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="Span5P" runat="server"></span>
                                             </td>
                                         </tr>
                                         <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                        <asp:RadioButtonList ID="rblQ5" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                          <%--<tr>
                                             <td class="col-5" style="width:46%">
                                               <span id="Span6" runat="server"></span>
                                             </td>
                                              <td style="width:8%;padding-right: 107px;">
                                            </td>
                                             <td class="col-5" style="width:46%">
                                                <span id="Span6P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                         <tr>
                                             <td class="col-4" style="width:30%">
                                               <span id="Span6" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="Span6M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="Span6P" runat="server"></span>
                                             </td>
                                         </tr>
                                         <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                         <asp:RadioButtonList ID="rblQ6" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                       <%--    <tr>
                                             <td class="col-5" style="width:46%">
                                               <span id="Span7" runat="server"></span>
                                             </td>
                                               <td style="width:8%;padding-right: 107px;">
                                            </td>
                                             <td class="col-5" style="width:46%">
                                                 <span id="Span7P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                          <tr>
                                             <td class="col-4" style="width:30%">
                                               <span id="Span7" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="Span7M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="Span7P" runat="server"></span>
                                             </td>
                                         </tr>
                                          <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                         <asp:RadioButtonList ID="rblQ7" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                           <%-- <tr>
                                             <td class="col-5" style="width:46%">
                                               <span id="Span8" runat="server"></span>
                                             </td>
                                               <td style="width:8%;padding-right: 107px;">
                                            </td>
                                             <td class="col-5"  style="width:46%">
                                                 <span id="Span8P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                         <tr>
                                             <td class="col-4" style="width:30%">
                                               <span id="Span8" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="Span8M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="Span8P" runat="server"></span>
                                             </td>
                                         </tr>
                                         <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                         <asp:RadioButtonList ID="rblQ8" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                        <%--  <tr>
                                             <td class="col-5" style="width:46%">
                                               <span id="Span9" runat="server"></span>
                                             </td>
                                             <td style="width:8%;padding-right: 107px;">
                                            </td>
                                              <td class="col-5" style="width:46%">
                                               <span id="Span9P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                         <tr>
                                             <td class="col-4" style="width:30%">
                                               <span id="Span9" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="Span9M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="Span9P" runat="server"></span>
                                             </td>
                                         </tr>
                                         <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                          <asp:RadioButtonList ID="rblQ9" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                      <%--   <tr id="tr10" runat="server">
                                             <td class="col-5" style="width:46%">
                                               <span id="Span10" runat="server"></span>
                                             </td>
                                            <td style="width:8%;padding-right: 107px;">
                                            </td>
                                              <td class="col-5" style="width:46%">
                                               <span id="Span10P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                         <tr id="tr10" runat="server">
                                             <td class="col-4" style="width:30%">
                                               <span id="Span10" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="Span10M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="Span10P" runat="server"></span>
                                             </td>
                                         </tr>
                                         <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                          <asp:RadioButtonList ID="rblQ10" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                        <%-- <tr id="tr11" runat="server">
                                             <td class="col-5" style="width:46%">
                                               <span id="Span11" runat="server"></span>
                                             </td>
                                             <td style="width:8%;padding-right: 107px;">
                                            </td>
                                             <td class="col-5" style="width:46%">
                                               <span id="Span11P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                         <tr id="tr11" runat="server">
                                             <td class="col-4" style="width:30%">
                                               <span id="Span11" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="Span11M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="Span11P" runat="server"></span>
                                             </td>
                                         </tr>
                                         <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                          <asp:RadioButtonList ID="rblQ11" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                        <%-- <tr id="tr12" runat="server">
                                             <td class="col-5" style="width:46%">
                                               <span id="Span12" runat="server"></span>
                                             </td>
                                             <td style="width:8%;padding-right: 107px;">
                                            </td>
                                             <td class="col-5" style="width:46%">
                                               <span id="Span12P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                          <tr id="tr12" runat="server">
                                             <td class="col-4" style="width:30%">
                                               <span id="Span12" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="Span12M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="Span12P" runat="server"></span>
                                             </td>
                                         </tr>
                                         <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                         <asp:RadioButtonList ID="rblQ12" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                            <%--  <tr id="tr13" runat="server">
                                             <td class="col-5" style="width:46%">
                                               <span id="Span13" runat="server"></span>
                                             </td>
                                                  <td style="width:8%;padding-right: 107px;">
                                            </td>
                                                  <td class="col-5" style="width:46%">
                                               <span id="Span13P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                         <tr id="infoPeopleDevelopmentQuestion13" runat="server">
                                             <td colspan="3" class=" col-12" style="font-size:medium; font-weight:bold; color:blue;">
                                                 <i class="fa fa-info-circle"></i>
                                                 <asp:Label ID="lblPeopleDvlpmnt13" runat="server" Text="You may skip this in case you are not aware of this practice."></asp:Label>
                                             </td>
                                         </tr>
                                          <tr id="tr13" runat="server">
                                             <td class="col-4" style="width:30%">
                                               <span id="Span13" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="Span13M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="Span13P" runat="server"></span>
                                             </td>
                                         </tr>
                                         <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                         <asp:RadioButtonList ID="rblQ13" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                        <%--  <tr id="tr14" runat="server">
                                             <td class="col-5" style="width:46%">
                                               <span id="Span14" runat="server"></span>
                                             </td>
                                              <td style="width:8%;padding-right: 107px;">
                                            </td>
                                              <td class="col-5" style="width:46%">
                                               <span id="Span14P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                          <tr id="infoPeopleDevelopmentQuestion14" runat="server">
                                             <td colspan="3" class=" col-12" style="font-size:medium; font-weight:bold; color:blue;">
                                                 <i class="fa fa-info-circle"></i>
                                                 <asp:Label ID="lblPeopleDvlpmnt14" runat="server" Text="You may skip this in case you are not aware of this practice."></asp:Label>
                                             </td>
                                         </tr>
                                           <tr id="tr14" runat="server">
                                             <td class="col-4" style="width:30%">
                                               <span id="Span14" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="Span14M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="Span14P" runat="server"></span>
                                             </td>
                                         </tr>
                                         <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                         <asp:RadioButtonList ID="rblQ14" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                         <%-- <tr id="tr15" runat="server">
                                             <td class="col-5" style="width:46%">
                                               <span id="Span15" runat="server"></span>
                                             </td>
                                             <td style="width:8%;padding-right: 107px;">
                                            </td>
                                             <td class="col-5" style="width:46%">
                                               <span id="Span15P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                          <tr id="infoPeopleDevelopmentQuestion15" runat="server">
                                             <td colspan="3" class=" col-12" style="font-size:medium; font-weight:bold; color:blue;">
                                                 <i class="fa fa-info-circle"></i>
                                                 <asp:Label ID="lblPeopleDvlpmnt15" runat="server" Text="You may skip this in case you are not aware of this practice."></asp:Label>
                                             </td>
                                         </tr>
                                         <tr id="tr15" runat="server">
                                             <td class="col-4" style="width:30%">
                                               <span id="Span15" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="Span15M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="Span15P" runat="server"></span>
                                             </td>
                                         </tr>
                                          <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                         <asp:RadioButtonList ID="rblQ15" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>

                                       <%--   <tr id="tr16" runat="server">
                                             <td class="col-5" style="width:46%">
                                               <span id="Span16" runat="server"></span>
                                             </td>
                                              <td style="width:8%;padding-right: 107px;">
                                            </td>
                                             <td class="col-5" style="width:46%">
                                               <span id="Span16P" runat="server"></span>
                                             </td>
                                         </tr>--%>
                                          <tr id="infoPeopleDevelopmentQuestion16" runat="server">
                                             <td colspan="3" class=" col-12" style="font-size:medium; font-weight:bold; color:blue;">
                                                 <i class="fa fa-info-circle"></i>
                                                 <asp:Label ID="lblPeopleDvlpmnt16" runat="server" Text="You may skip this in case you are not aware of this practice."></asp:Label>
                                             </td>
                                         </tr>
                                           <tr id="tr16" runat="server">
                                             <td class="col-4" style="width:30%">
                                               <span id="Span16" runat="server"></span>
                                             </td>
                                            <td class="col-4" style="width:30%">
                                                 <span id="Span16M" runat="server"></span>
                                            </td>
                                              <td class="col-4" style="width:30%">
                                               <span id="Span16P" runat="server"></span>
                                             </td>
                                         </tr>
                                         <tr>
                                               <td colspan="3" class="col-12">
                                                   <div class="option">
                                                         <asp:RadioButtonList ID="rblQ16" runat="server" RepeatDirection="Horizontal"  BorderStyle="None"  style="width:100%" class="col-12">
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                                <asp:ListItem Text="" Value="2" ></asp:ListItem>
                                <asp:ListItem Text="" Value="3" ></asp:ListItem>
                            </asp:RadioButtonList>
                                                   </div>
                                             </td>
                                         </tr>
                                     </tbody>
                                     </table>
                                </div>

                           </div>
                      
                        
                        <div class="row">
                           
                           
                            
                            <div class="col-12" style="margin-left:10px" >
                            <asp:RadioButtonList ID="rblQ1d" runat="server"  RepeatDirection="Horizontal"  BorderStyle="None" CssClass="form-control rbl" Visible="false">
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
                        <div class="col-md-8 col-sm-8 col-lg-8 form-group">
                            <div style="margin-top:15px;">
                            <label>What have you seen <asp:Label ID="lblRecipientNm3" runat="server" Text="" ></asp:Label> do that is exemplary on any of the behaviours? (Accountability, Collaboration, Responsiveness,<asp:Label ID="lblRecipientNm5" runat="server" Text="" ></asp:Label> )<font color="Red">*</font></label>
                            <asp:TextBox ID="txtAns1" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"
                                onkeypress="return setLength1(event);" onkeyup="return setCharacters1(event);" MaxLength="500" ></asp:TextBox>                            
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAns1"
                Display="Dynamic" ErrorMessage="Please enter 500s characters or less."  ValidationGroup="vgFeedback" ForeColor="Red" SetFocusOnError="true"
                ValidationExpression="[\s\S]{1,500}"></asp:RegularExpressionValidator>
                                 <asp:Label ID="Label1" Text="" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAns1" ValidationGroup="vgFeedback" SetFocusOnError="true" ErrorMessage="Please provide your response" ForeColor="#E43C5C"></asp:RequiredFieldValidator>
                               
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
                        <div class="col-md-8 col-sm-8 col-lg-8  form-group">
                            <div style="margin-top:15px;">
                            <label>How can <asp:Label ID="lblRecipientNm4" runat="server" Text=""></asp:Label> improve on behaviours that enhance agility?<font color="Red">*</font></label>
                            <asp:TextBox ID="txtAns2" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6"
                                onkeypress="return setLength(event);" onkeyup="return setCharacters(event);" MaxLength="500"></asp:TextBox>                           
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtAns2"
                Display="Dynamic" ErrorMessage="Please enter 500s characters or less."  ValidationGroup="vgFeedback" ForeColor="Red" SetFocusOnError="true"
                ValidationExpression="[\s\S]{1,500}"></asp:RegularExpressionValidator>
                                  <asp:Label ID="lblCountChar" Text="" runat="server" />
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAns2" ValidationGroup="vgFeedback" ErrorMessage="Please provide your response" ForeColor="#E43C5C" SetFocusOnError="true"></asp:RequiredFieldValidator>
                              
                            </div>
                        </div>
                    </div>
                                <div class="box-footer" style="text-align:center;">
        <asp:LinkButton ID="btn_submit" runat="server" class="btn-learn-more" style="background-color:#F2F2F2" Width="120px" Text="Submit" ValidationGroup="vgFeedback" data-bs-toggle="modal" data-bs-target="#staticBackdrop"/>

       &nbsp;&nbsp; 
                         
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
        </div>
          </div>
      </div>

          <div class="modal fade" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      
      <div class="modal-body">
      Are you sure you want to submit the form?
      </div>
      <div class="modal-footer">
         
          <asp:Button runat="server" ID="btnyesclick" OnClick="Submit" Text="Yes" class="btn btn-primary"  />
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">No</button>
        
      </div>
    </div>
  </div>
</div>

             <div class="modal fade" id="staticBackdrop1" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      
      <div class="modal-body">
        Are you sure you want to reject the form and go ahead with the option of insufficient exposure?
      </div>
      <div class="modal-footer">
         
          <asp:Button runat="server" ID="Button1" OnClick="reject" Text="Yes" class="btn btn-primary"  />
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">No</button>
        
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
<span>In case of any queries or issues, please reach out to your HRBP. </span>
            <span>In case of any system specific queries or IT issues, please reach out to<b> IT helpdesk (it_helpdesk@tatasteel.com)</b></span>
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
            <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script> --%>  
        <script type="text/javascript"> 
    if (window.history.replaceState) {
        window.history.replaceState(null, null, window.location.href);
    }
    $(function () {  
        $(document).keydown(function (e) {  
            return (e.which || e.keyCode) != 116;  
        });  
    });  
</script>
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
