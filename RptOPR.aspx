<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RPTOPR.aspx.vb" Inherits="RPTOPR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

     <meta content="" name="descriptison"/>
  <meta content="" name="keywords"/>
     <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <%--<meta content="width=device-width, initial-scale=1" name="viewport" />--%>
  <!-- Favicons -->
  <link href="assets/img/favicon.png" rel="icon"/>
  <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon"/>

  <!-- Google Fonts -->
    <link href="assets/css/googlefont.css" rel="stylesheet" />

  <!-- Vendor CSS Files -->
  <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
  <link href="assets/vendor/icofont/icofont.min.css" rel="stylesheet"/>
  <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet"/>
  <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet"/>
  <link href="assets/vendor/venobox/venobox.css" rel="stylesheet"/>
  <link href="assets/vendor/owl.carousel/assets/owl.carousel.min.css" rel="stylesheet"/>
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"/>
    <link rel="stylesheet" type="text/css" href="styles/sweetalert2.css" />
    <script type="text/javascript" src="scripts/sweetalert2.min.js"></script>
    <%--<link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">--%>
<script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script>
<script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
     <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet"/>
     <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
  <!-- Include all compiled plugins (below), or include individual files as needed -->
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <style>
         table {
            border-spacing: 0px;
            table-layout: fixed;
            margin-left:auto; 
            margin-right:auto;
        }
        th {
            color: green;
            border: 1px solid black;
        }
        td {
            border: 1px dotted black;
            
          
        }
        /*.vendorListHeading {
  background-color: aquamarine;
  color:black;
  -webkit-print-color-adjust: exact; 
}*/

        @media print {
    tr.vendorListHeading {
        background-color: #c1e6f8 !important;
        -webkit-print-color-adjust: exact;
         height:100px; 
    }
    .frcolor{
          color:#002960 !important;
         -webkit-print-color-adjust: exact !important;
         font-weight:bold;
    }
     tr.trbg{
        background-color:#e6f4f7 !important;
         height:100px;
         -webkit-print-color-adjust: exact;
    }
     .bgcolor1
     {
         -webkit-print-color-adjust: exact !important;
         background-color:#1481c4 !important;
         color:white !important;
         text-align:center !important;
         border:1px solid green;
     }
     .footer {
   position: fixed;
   left: 0;
   bottom: 0;
   width: 100%;
   background-color: red;
   color: white;
   text-align: center;
}
    .bgcor{
          background-color: #c1e6f8 !important;
          -webkit-print-color-adjust: exact !important;
           text-align:center !important;
          border:none !important;
     }
}

        .clsWrap
        {
            /*margin-left:10px;*/
            /*white-space:normal;*/
            word-wrap: break-word;
            /*word-wrap: normal; 
            word-break: break-all;
            width:40px;*/
        }
        .auto-style1 {
            /*width: 266px;*/
        }
    </style>

</head>
<body onload="window.print();">
    <form id="form1" runat="server">
        <div class="row">
    <div class="col-lg-2">
        <asp:Image ID="imgLogo" ImageUrl="~/Images/Logo.JPG" runat="server" alt="" height="100" width="200"/>
    </div>
    </div>
        <div class="row">
    <div class="col-lg-2">
       <asp:Image ID="bgImg" runat="server" ImageUrl="~/Images/Feedback360.JPG"  alt="Feedback360" height="1010" width="1000" />
    </div>
           <div class="row" >
            <div class="col-lg-9" style="margin-left:838px;">
                <asp:TextBox runat="server" ID="txtbaseline" Text="Baseline" CssClass="bgcolor1"></asp:TextBox>
            </div>
        </div>
    </div><br />
        <div class="row">
            <div class="col-lg-10" style="text-align:left;">
                <h1><asp:Label ID="lblReceiptNm" runat="server" Text="" Font-Size="Larger" ></asp:Label><br /> 
                <asp:Label ID="lblDesignation" runat="server" Text="HR Manager"></asp:Label></h1>
               
            </div>
        </div>

        <div class="row" >
           <div class="col-lg-5"></div>
            <div class="col-lg-7" style="text-align:left;">
                <h2>TATA STEEL  </h2>
            </div>
        </div>
        <div class="row" >
           <div class="col-lg-5"></div>
            <div class="col-lg-7" style="text-align:left;">
                <h2> FY 22 </h2>
            </div>
        </div>
        <div class="row" >
            <div class="col-lg-9" style="text-align:right;">
                <h4> CONFIDENTIAL AND PROPRIETARY <br /> Any use of this material without specific permission is strictly prohibited </h4>
            </div>
        </div>
        <br />
        <br />
        <br />
        
        <div class="row footer">
            <div class="col-lg-12 col-md-12 col-sm-12">
                <asp:Image runat="server" ID="imgpatch" ImageUrl="~/Images/Patch.jpg" Height="50"  />
            </div>
        </div>
        <br />
        <br />
        <br />
           <div class="row">
             <div class ="col-lg-7 col-sm-7 col-md-7" style="margin-left:6.5%;">
                <h1> <asp:Label runat ="server" ID ="Label99" Text="Introduction"></asp:Label> </h1>
            </div>
            <div class ="col-lg-3 col-sm-3 col-md-3" style="margin-left:6.5%;">
                <asp:Image ID="Image17" ImageUrl="~/Images/Logo.JPG" runat="server" alt="" />
            </div>
        </div>

         <div class="row" style="margin-left:5%;">
            <div class="col-sm-12 col-md-12 col-lg-12 ">
                <h3><p><span class="frcolor">We aspire to be the most valuable and respected steel company globally in the next 5-10 years </span></p></h3>
            </div>
        </div>        
         <div class="row" style="margin-left:5%;">
            <div class="col-sm-12 col-md-12 col-lg-12 ">
                <h3><p>
                         While we are constantly making changes across the organization, we are aware that to be prepared for the future, we need to increase this pace of change. We need to change the way in which we work and become more agile. The change that we are now embarking on as much a <span class="frcolor">cultural transformation as a structural transformation.</span>
                      </p></h3>
            </div>
        </div>        
         <div class="row" style="margin-left:5%;">
            <div class="col-sm-12 col-md-12 col-lg-12 ">
                <h3><p>
                         We identified that we need to make the following 5 shifts in our culture:
                      </p></h3>
            </div>
        </div>
         <div class="row" style="margin-left:5%;">
            <div class="col-sm-12 col-md-12 col-lg-12 ">
                <h3><p>
                        <span class="frcolor"> “My Tata Steel” -> “Our Tata Steel” </span> – working together for a common goal
                      </p></h3>
            </div>
        </div>
         <div class="row" style="margin-left:5%;">
            <div class="col-sm-12 col-md-12 col-lg-12 ">
                <h3><p>
                        <span class="frcolor">“Looking up” -> “The buck stops here” </span>– holding yourself accountable to your responsibilities
                      </p></h3>
            </div>
        </div>
         <div class="row" style="margin-left:5%;">
            <div class="col-sm-12 col-md-12 col-lg-12 ">
                <h3><p>
                      <span class="frcolor"> “Incremental” -> “Bold” </span>– pushing the boundaries of excellence
                      </p></h3>
            </div>
        </div>
         <div class="row" style="margin-left:5%;">
            <div class="col-sm-12 col-md-12 col-lg-12 ">
                <h3><p>
                       <span class="frcolor">“Activity” -> “Impact at speed” </span>– having a bias to action
                      </p></h3>
            </div>
        </div>
         <div class="row" style="margin-left:5%;">
            <div class="col-sm-12 col-md-12 col-lg-12 ">
                <h3><p>
                     <span class="frcolor"> “Paternalistic” -> “Meritocracy” </span>– encouraging high performance
                      </p></h3>
            </div>
        </div>
         <div class="row" style="margin-left:5%;">
            <div class="col-sm-12 col-md-12 col-lg-12 ">
                <h3><p>
                      These shifts require us to <span class="frcolor">adopt new behaviours.</span> We have learnt from both research as well as experience of best-in-class organizations that this shift requires 4 things – role modelling by senior leaders, constant communication to enable understanding and conviction, capability building to help develop skills and process changes to help embed the new behaviours. A performance management system which is geared towards development and measures<span class="frcolor"> both the performance (what) and the behaviours (how)</span> will help us drive this <span class="frcolor"> cultural transformation.</span>
                      </p></h3>
            </div>
        </div>
         <div class="row" style="margin-left:5%;">
            <div class="col-sm-12 col-md-12 col-lg-12 ">
                <h3><p>
                    <span class="frcolor">  4 behaviours have been identified </span> considering the cultural shifts articulated for agility, FGDs conducted by the culture labs in HR and the behaviours identified by some global companies which have already undertaken the journey to become agile.
                      </p></h3>
            </div>
        </div>
         <div class="row" style="margin-left:5%;">
            <div class="col-sm-12 col-md-12 col-lg-12 ">
                <h3><p>
                      These behaviours are <span class="frcolor">“Be Accountable”</span> – ownership and accountability, <span class="frcolor">“Work Together”</span> – collaboration, <span class="frcolor">“Respond Quickly” </span>– responsiveness, <span class="frcolor">“Evolve” </span>– team building
                      </p></h3>
            </div>
        </div>
        <br />
       <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
         <br />
        <br />
         <br />
        <br />
        <br />
        <br />
         <br />
        <br />
            

          <div class="row footer">
            <div class="col-lg-10">
                <asp:Image runat="server" ID="Image2" ImageUrl="~/Images/Patch.jpg" Height="50" />
            </div>
        </div>
       
         <br />
        <br /> 
        <br />
        <br />  
        <br />
        <br /> 
        <br />
        <br />  
         <div class="row">
             <div class ="col-lg-7 col-sm-7 col-md-7" style="margin-left:6.5%;">
                <h1> <asp:Label runat ="server" ID ="Label67" Text="Selected behaviours for agility"></asp:Label> </h1>
            </div>
            <div class ="col-lg-3 col-sm-3 col-md-3" style="margin-left:6.5%;">
                <asp:Image ID="Image1" ImageUrl="~/Images/Logo.JPG" runat="server" alt="" />
            </div>
        </div>
        <br />
        <div class="row" style="margin-left:2%;">
            <div class ="col-lg-12">
                <asp:Image ID="Image4" ImageUrl="~/Images/patchI4.JPG" runat="server" alt="" Width="1000px" />
            </div>
        </div><br />
        <br />

         <div class="row" style="margin-left:2%;">
            <div class ="col-lg-12">
                <asp:Image ID="Image5" ImageUrl="~/Images/patchI5.JPG" runat="server" alt=""  Width="1000px" />
            </div>
        </div>
        <br />
        <br />
        
        
         <div class="row footer">
            <div class="col-lg-10">
                <asp:Image runat="server" ID="Image7" ImageUrl="~/Images/Patch.jpg" Height="50"  />
            </div>
        </div>
        
        <br />
        <br />
         <br />
        <br />
        <br />
        <br />
        <br />
         <div class="row">
             <div class ="col-lg-7 col-sm-7 col-md-7" style="margin-left:6.5%;">
                <h1> <asp:Label runat ="server" ID ="lbloverall" Text=""></asp:Label> </h1>
            </div>
            <div class ="col-lg-3 col-sm-3 col-md-3" style="margin-left:6.5%;">
                <asp:Image ID="Image6" ImageUrl="~/Images/Logo.JPG" runat="server" alt="" />
            </div>
        </div>

       

        <br />
        <br />
         <div class="row" >
            <div class="col-lg-1"></div> 
            <div class="col-lg-10" >
                <h4>  <asp:Label runat ="server" ID ="lbloverscore" Text=" overall"></asp:Label> </h4>
            </div>
            <div class="col-lg-1">
                
            </div>
        </div>
        <br />
          <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
          <br />
        
        <br />
       <h3> <table border="1" style="margin-left:3.7%;" width="1000">
          

             <tr class="vendorListHeading" style="font-weight:bold;">
             <td class="auto-style1" style="text-align:center; width:200px;"> </td>
             <td style="text-align:center; width:100px;">Self</td>
             <td style="text-align:center; width:100px;">Manager</td>
             <td style="text-align:center; width:100px;">Internal Stakeholder</td>
             <td style="text-align:center; width:100px;">Overall*</td>
             </tr>

             <tr class="trbg">
            <td>Accountability</td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="lblaccself"></asp:Label><asp:Label runat="server" Visible="false" ID="lblAccSelf1"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label1"></asp:Label><asp:Label runat="server" Visible="false" ID="lblAccMangr"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label2"></asp:Label><asp:Label runat="server" Visible="false" ID="lblAccIntsh"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label3"></asp:Label></td>
            </tr>
            <tr class="vendorListHeading">
            <td>Collaboration</td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label4"></asp:Label><asp:Label runat="server" Visible="false" ID="lblCooSelf"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label5"></asp:Label><asp:Label runat="server" Visible="false" ID="lblCooMangr"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label6"></asp:Label><asp:Label runat="server" Visible="false" ID="lblCooIntsh"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label7"></asp:Label></td>
            </tr>
            <tr class="trbg">
            <td>Responsiveness</td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label8"></asp:Label><asp:Label runat="server" Visible="false" ID="lblResSelf"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label9"></asp:Label><asp:Label runat="server" Visible="false"  ID="lblResMangr"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label10"></asp:Label><asp:Label runat="server" Visible="false" ID="lblResIntsh"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label11"></asp:Label></td>
            </tr>
            <tr class="vendorListHeading">
                <td>Team building</td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label12"></asp:Label><asp:Label runat="server" Visible="false" ID="lblDevSelf"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label13"></asp:Label><asp:Label runat="server" Visible="false" ID="lblDevMangr"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label14"></asp:Label><asp:Label runat="server" Visible="false" ID="lblDevIntsh"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label15"></asp:Label></td>
            </tr>
        </table>
</h3>
      
      
        
       
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        
        <br />
        <br />
        <br />
        <br />
        <br />
        <br /><br />
        <br />
         
        
        
        
        

         <div class="row" style="margin-left:3.8%;">
            <div class="col-lg-2"></div>
            <div class="col-lg-5">
                 * Overall score does not include self feedback scores 
            </div>
        </div>
        <br />
        <br />
        
        <div class="row footer">
            <div class="col-lg-10">
                <asp:Image runat="server" ID="Image12" ImageUrl="~/Images/Patch.jpg" Height="50"  />
            </div>
        </div>
        <br />
        <br />
        
        
        

         <div class="row">
             <div class ="col-lg-7 col-sm-7 col-md-7" style="margin-left:6.5%;">
                <h1> <asp:Label runat ="server" ID ="lblall" Text=""></asp:Label> </h1>
            </div>
            <div class ="col-lg-3 col-sm-3 col-md-3" style="margin-left:6.5%;">
                <asp:Image ID="Image11" ImageUrl="~/Images/Logo.JPG" runat="server" alt="" />
            </div>
        </div>

       
        <br />
        <br />

        <div class="row" style="margin-left:5%;">
            <div class="col-lg-2"></div>
            <div class="col-lg-5">
                <h2>Accountability  <%--<asp:Label runat="server" ID="lblaccountability" CssClass="bgcor"></asp:Label>--%> <asp:TextBox runat="server" ID="lblaccountability" CssClass="bgcor" Width="70px" Height="40px" ></asp:TextBox></h2>  
            </div>
        </div>
        <br />

       <h4> <table width="1000" style="margin-left:5%;">
        
           <tr CLASS="vendorListHeading" style="font-weight:bold;">
                <td class="auto-style1" style="text-align:center; width:200px;">GOLD STANDARD </td>
           <td style="text-align:center; width:100px;">Self</td>
            <td style="text-align:center; width:100px;">Manager</td>
              
            <td style="text-align:center; width:100px;">Internal Stakeholder</td>
            <td style="text-align:center; width:100px;">Overall*</td>
            </tr>
             <tr CLASS="trbg" >
             <td style="text-align:left;" class="auto-style1"><asp:Label runat="server" ID="Label32" CssClass="clsWrap"    ></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label16"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label17"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label18"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label19"></asp:Label></td>
            </tr>
            <tr CLASS="vendorListHeading">
            <td style="text-align:left; " class="auto-style1"><asp:Label runat="server" ID="Label33" CssClass="clsWrap"  ></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label20"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label21"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label22"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label23"></asp:Label></td>
            </tr>
            <tr CLASS="trbg">
            <td style="text-align:left; " class="auto-style1"><asp:Label runat="server" ID="Label34" CssClass="clsWrap"  ></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label24"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label25"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label26"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label27"></asp:Label></td>
            </tr>
                
    </table> </h4>     
        <br />
        <br />
        <br />
        <br />       

         <div class="row" style="margin-left:5%;">
            <div class="col-lg-1"></div>
            <div class="col-lg-5">
                <h2>Collaboration  <%-- <asp:Label runat="server" ID="lblcollaboration" CssClass="bgcor"></asp:Label>--%> <asp:TextBox runat="server" ID="lblcollaboration" CssClass="bgcor" Width="70px" Height="40px" ></asp:TextBox></h2>
            </div>
        </div>
        <br />
       <h4>  <table width="1000" style="margin-left:5%;">
             
             
             <tr CLASS="vendorListHeading" style="font-weight:bold;">
                <td class="auto-style1" style="text-align:center; width:200px;">GOLD STANDARD </td>
           <td style="text-align:center; width:100px;">Self</td>
            <td style="text-align:center; width:100px;">Manager</td>
               
            <td style="text-align:center; width:100px;">Internal Stakeholder</td>
            <td style="text-align:center; width:100px;">Overall*</td>
            </tr>
             <tr CLASS="trbg">
            <td style="text-align:left; " class="auto-style1"><asp:Label runat="server" ID="Label36" CssClass="clsWrap"  ></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label37"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label38"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label39"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label40"></asp:Label></td>
            </tr>
            <tr CLASS="vendorListHeading">
            <td style="text-align:left; " class="auto-style1"><asp:Label runat="server" ID="Label41" CssClass="clsWrap"  ></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label42"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label43"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label44"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label45"></asp:Label></td>
            </tr>
            <tr CLASS="trbg">
             <td style="text-align:left; " class="auto-style1"><asp:Label runat="server" ID="Label46" CssClass="clsWrap"  ></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label47"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label48"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label49"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label50"></asp:Label></td>
            </tr>

             
        </table> </h4>      
  <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
       
        
         <div class="row" style="margin-left:5%;">
            <div class="col-lg-2"></div>
            <div class="col-lg-5">
                 * Overall score does not include self feedback scores 
                <br />
                U = Unacceptable; A = Acceptable; G = Gold Standard
            </div>
        </div>
        <br />
        <br />
        <div class="row footer">
            <div class="col-lg-10">
                <asp:Image runat="server" ID="Image18" ImageUrl="~/Images/Patch.jpg" Height="50"  />
            </div>
        </div>
         <br />
        <br />
        <br />
        <br />
         <div class="row">
             <div class ="col-lg-7 col-sm-7 col-md-7" style="margin-left:6.5%;">
                <h1> <asp:Label runat ="server" ID ="Label119" Text=""></asp:Label> </h1>
            </div>
            <div class ="col-lg-3 col-sm-3 col-md-3" style="margin-left:6.5%;">
                <asp:Image ID="Image19" ImageUrl="~/Images/Logo.JPG" runat="server" alt="" />
            </div>
        </div>
        <br />
          <div class="row" style="margin-left:5%;">
            <div class="col-lg-1"></div>
            <div class="col-lg-5">
                <h2> Responsiveness  <%--<asp:Label runat="server" ID="lblresponse" CssClass="bgcor"></asp:Label>--%> <asp:TextBox runat="server" ID="lblresponse" CssClass="bgcor" Width="70px" Height="40px" ></asp:TextBox></h2>
            </div>
        </div>
        <br />
       <h4>  <table width="1000" style="margin-left:5%;">
               
             <tr CLASS="vendorListHeading" style="font-weight:bold;">
                <td class="auto-style1" style="text-align:center; width:200px;">GOLD STANDARD </td>
           <td style="text-align:center; width:100px;">Self</td>
            <td style="text-align:center; width:100px;">Manager</td>
               
            <td style="text-align:center; width:100px;">Internal Stakeholder</td>
            <td style="text-align:center; width:100px;">Overall*</td>
            </tr>
             <tr CLASS="trbg" >
            <td style="text-align:left; " class="auto-style1"><asp:Label runat="server" ID="Label68" CssClass="clsWrap"  ></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label69"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label70"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label71"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label72"></asp:Label></td>
            </tr>
            <tr CLASS="vendorListHeading">
            <td style="text-align:left; " class="auto-style1"><asp:Label runat="server" ID="Label73" CssClass="clsWrap"  ></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label74"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label75"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label76"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label77"></asp:Label></td>
            </tr>
            <tr CLASS="trbg">
            <td style="text-align:left; " class="auto-style1"><asp:Label runat="server" ID="Label78" CssClass="clsWrap"  ></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label79"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label80"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label81"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label82"></asp:Label></td>
            </tr>
             
        </table></h4>
        <br />
        <br />
        <br />
        <br />
           <div class="row" style="margin-left:5%;">
            <div class="col-lg-1"></div>
            <div class="col-lg-5">
                <h2> Team Building  <%--<asp:Label runat="server" ID="lblpeople" CssClass="bgcor"></asp:Label>--%> <asp:TextBox runat="server" ID="lblpeople" CssClass="bgcor" Width="70px" Height="40px" ></asp:TextBox></h2>
            </div>
        </div>
        <br />
       <h4>  <table width="1000" style="margin-left:5%;">
             
             <tr CLASS="vendorListHeading" style="font-weight:bold;">
                <td class="auto-style1" style="text-align:center; width:200px;">GOLD STANDARD </td>
           <td style="text-align:center; width:100px;">Self</td>
            <td style="text-align:center; width:100px;">Manager</td>
              
            <td style="text-align:center; width:100px;">Internal Stakeholder</td>
            <td style="text-align:center; width:100px;">Overall*</td>
            </tr>
             <tr CLASS="trbg">
             <td style="text-align:left; " class="auto-style1"><asp:Label runat="server" ID="Label51"  CssClass="clsWrap"  ></asp:Label></td>
             <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label52"></asp:Label></td>
             <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label53"></asp:Label></td>
             
             <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label54"></asp:Label></td>
             <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label55"></asp:Label></td>
            </tr>
            <tr CLASS="vendorListHeading">
            <td style="text-align:left; " class="auto-style1"><asp:Label runat="server" ID="Label56" CssClass="clsWrap"  ></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label57"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label58"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label59"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label60"></asp:Label></td>
            </tr>
            <tr CLASS="trbg">
            <td style="text-align:left; " class="auto-style1"><asp:Label runat="server" ID="Label61" CssClass="clsWrap"  ></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label62"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label63"></asp:Label></td>
            
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label64"></asp:Label></td>
            <td style="text-align:center; width:100px;"><asp:Label runat="server" ID="Label65"></asp:Label></td>
            </tr>

             
            
        </table> </h4>
        <br />
            <br />   
        
        <br />
        <br />
        <br />
             
        <br /> 
        <br />
        <br />
       
       
        
          <div class="row" style="margin-left:5%;">
            <div class="col-lg-2"></div>
            <div class="col-lg-5">
                 * Overall score does not include self feedback scores 
                <br />
                U = Unacceptable; A = Acceptable; G = Gold Standard
            </div>
        </div>
               
        <br />
        <br />
        
        <div class="row footer">
            <div class="col-lg-10">
                <asp:Image runat="server" ID="Image13" ImageUrl="~/Images/Patch.jpg" Height="50"  />
            </div>
        </div>
        <br />
        <br />
        <br />

         <div class="row">
             <div class ="col-lg-7 col-sm-7 col-md-7" style="margin-left:6.5%;">
                <h1> <asp:Label runat ="server" ID ="lblnmm" Text=""></asp:Label> </h1>
            </div>
            <div class ="col-lg-3 col-sm-3 col-md-3" style="margin-left:6.5%;">
                <asp:Image ID="Image15" ImageUrl="~/Images/Logo.JPG" runat="server" alt="" />
            </div>
        </div>

        <br />

          <div class="row">
            <div class="col-lg-10">
                <asp:Image ID="Image3" ImageUrl="~/Images/Strength.JPG" runat="server" alt="" height="100" />
            </div>
        </div>

         <table style="margin-left:6.5%; border:0px dotted transparent;">
           
          
            <tr style="word-break:break-all; word-wrap:normal; width :50px; text-align:left;">
                
                <td style ="border:0px dotted transparent;">
                    <asp:Literal runat ="server" ID="litans"></asp:Literal>
                </td>
            </tr>
        </table>
       
       <div class="row footer">
            <div class="col-lg-10">
                <asp:Image runat="server" ID="Image8" ImageUrl="~/Images/Patch.jpg" Height="50"  />
            </div>
        </div>
       <p style="page-break-before:always;"></p> 
        

        <div class="row">
             <div class ="col-lg-7 col-sm-7 col-md-7" style="margin-left:6.5%;">
                <h1> <asp:Label runat ="server" ID ="Label66" Text=""></asp:Label> </h1>
            </div>
            <div class ="col-lg-3 col-sm-3 col-md-3" style="margin-left:6.5%;">
                <asp:Image ID="Image16" ImageUrl="~/Images/Logo.JPG" runat="server" alt="" />
            </div>
        </div>

        <div class="row">
            <div class="col-lg-10">
               <asp:Image ID="Image10" ImageUrl="~/Images/Opprt.JPG" runat="server" alt="" height="100" />
            </div>
        </div>
         <table style="margin-left:6.5%;">          
            <tr>
                
                <td style ="border:0px dotted transparent;">
                    <asp:Literal runat ="server" ID="Literal1"></asp:Literal>
                </td>
            </tr>
        </table>

        <br />
        <br />
        <br />
        <div class="row footer">
            <div class="col-lg-10">
                <asp:Image runat="server" ID="Image14" ImageUrl="~/Images/Patch.jpg" Height="50"  />
            </div>
        </div>
    </form>
</body>
</html>