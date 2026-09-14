<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PuraskarDocsUploadForm.aspx.cs" Inherits="PuraskarDocsUploadForm"
    MasterPageFile="~/MasterPages/MyInfo.master" Debug="false" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" Text="Puraskar Doucuments Upload Form" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script src="../Script/jquery-1.7.1.js" type="text/javascript"></script>   
    <script src="../Scripts/swAlert.js"></script>
  <link rel="stylesheet" type="text/css" href="../Scripts/swAlert.css" />
 <style type="text/css">
     .swal-wide 
{
   width: 310px;
   height: 330px;
}
     .swal-wides 
{
   width: 410px;
   height: 330px;
}
      .swal-widess 
{
   width: 450px;
   height: 330px;
}
     </style>
        <script type="text/javascript" language="javascript">
            function ValidateEmpty() {
             
                if (!isBlank("<%=TxtCompanyName.ClientID  %>", "Company Name"))
                    return false;
                
                if (!CheckNumberPresent("<%=TxtCompanyName.ClientID  %>", "Numeric characters are not allowed"))
                    return false;
                if (!isSpecialCharacter("<%=TxtCompanyName.ClientID  %>", "Special characters are not allowed"))
                    return false;
               

                if (!isBlank("<%=txtAddress1.ClientID  %>", "Company Address1"))
                    return false;

                if (!isBlank("<%=txtAddress2.ClientID  %>", "Company Address2"))
                    return false;

                if (!isSelected("<%=ddlContryName.ClientID  %>", "Contry Name"))
                    return false;
                var c2;
                c2 = document.getElementById("<%=ddlContryName.ClientID %>").value;
                if (c2 == "2") {
                    if (!isBlank("<%=txtContryName.ClientID  %>", "Company Country Name"))
                        return false;

                    if (!CheckNumberPresent("<%=txtContryName.ClientID  %>", "Numeric characters are not allowed"))
                        return false;
                    if (!isSpecialCharacter("<%=txtContryName.ClientID  %>", "Special characters are not allowed"))
                        return false;
                 }

                if (!isBlank("<%=txtCityName.ClientID  %>", "City Name"))
                    return false;
                if (!CheckNumberPresent("<%=txtCityName.ClientID  %>", "Numeric characters are not allowed"))
                    return false;
                if (!isSpecialCharacter("<%=txtCityName.ClientID  %>", "Special characters are not allowed"))
                    return false;

                var d2;
                d2 = document.getElementById("<%=ddlCmpState.ClientID %>").value;
                 if (d2 == "0") {
                     if (!isBlank("<%=txtContryState.ClientID  %>", "Company State Name"))
                         return false;

                     if (!CheckNumberPresent("<%=txtContryState.ClientID  %>", "Numeric characters are not allowed"))
                         return false;
                     if (!isSpecialCharacter("<%=txtContryState.ClientID  %>", "Special characters are not allowed"))
                         return false;
                }

                var d3;
                d3 = document.getElementById("<%=ddlCmpDistrict.ClientID %>").value;
                 if (d3 == "0") {
                     if (!isBlank("<%=txtContryDis.ClientID  %>", "Company District Name"))
                         return false;

                     if (!CheckNumberPresent("<%=txtContryDis.ClientID  %>", "Numeric characters are not allowed"))
                         return false;
                     if (!isSpecialCharacter("<%=txtContryDis.ClientID  %>", "Special characters are not allowed"))
                         return false;
                 }

                if (!isBlank("<%=txtCmpPinCode.ClientID  %>", "Company PinCode"))
                    return false;
                if (!isNumber("<%=txtCmpPinCode.ClientID  %>"))
                    return false;
             
                var e2;
                if (!isBlank("<%=txtCompanyEmail.ClientID  %>", "Company Email"))
                    return false;
                e2 = document.getElementById("<%=txtCompanyEmail.ClientID %>").value;
                 if (e2 != "") {
                     if (!isValidEmail("<%=txtCompanyEmail.ClientID %>", "Not A Valid Email Address"))
                     return false;
                 }

                if (!isBlank("<%=txtCompanyPhone.ClientID  %>", "Company Phone"))
                    return false;
                if (!isNumber("<%=txtCompanyPhone.ClientID  %>"))
                    return false;

                if (!isBlank("<%=txtOfferletterno.ClientID  %>", "Offer Letter Number"))
                    return false;

                if (!isBlankDate("<%=txtPlacementDate.ClientID %>", "Start Date", "dd-MMM-yyyy"))
                    return false;

                if (!isBlankDate("<%=txtPlacementUpto.ClientID %>", "End Date", "dd-MMM-yyyy"))
                    return false;

                var frdate = document.getElementById("<%=txtPlacementDate.ClientID %>").value;
                var todate = document.getElementById("<%=txtPlacementUpto.ClientID %>").value;
                if (!CompareDates(frdate, todate, "Placement Date should be less then Placement Upto date", true))
                    return false;

                if (!isBlank("<%=txtSalarybeingGiven.ClientID  %>", "Offer Letter Number"))
                    return false;
                if (!isNumber("<%=txtSalarybeingGiven.ClientID  %>"))
                    return false;

                var e2;
                e2 = document.getElementById("<%=txtCompanyEmail.ClientID %>").value;
             if (e2 != "") {
                 if (!isValidEmail("<%=txtCompanyEmail.ClientID %>", "Not A Valid Email Address"))
                    return false;
            }
                if (!isBlank("<%=txtBankAcNo.ClientID  %>", "Bank Account Number"))
                    return false;
                if (!isNumber("<%=txtBankAcNo.ClientID  %>"))
                    return false;

                if (!isBlank("<%=txtBankName.ClientID  %>", "Bank Name"))
                    return false;

                if (!CheckNumberPresent("<%=txtBankName.ClientID  %>", "Numeric characters are not allowed"))
                    return false;
                if (!isSpecialCharacter("<%=txtBankName.ClientID  %>", "Special characters are not allowed"))
                    return false;

                if (!isBlank("<%=txtBankCity.ClientID  %>", "Bank City Name"))
                    return false;

                if (!CheckNumberPresent("<%=txtBankCity.ClientID  %>", "Numeric characters are not allowed"))
                    return false;
                if (!isSpecialCharacter("<%=txtBankCity.ClientID  %>", "Special characters are not allowed"))
                    return false;

                if (!isSelected("<%=ddlBankState.ClientID  %>", "Bank State Name"))
                    return false;

                if (document.getElementById('<%=AppointmentLetterUpload.ClientID%>').value == "") {

                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Latest Appointment Letter !',
                        customClass: 'swal-wide',
                    });
                    return false;
                }
                    if (document.getElementById('<%=SalarySlipFileUpload.ClientID%>').value == "") {
                
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Salary Slip not older than three months !',
                            customClass: 'swal-wides',                        
                        });
                        return false;
                    }
                    if (document.getElementById('<%=BankStatementFileUpload.ClientID%>').value == "") {
                
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Bank Statement where salary is being received !',
                            customClass: 'swal-widess',                        
                        });
                        return false;                        
                }
                return true;               
            }
    </script>
 
    <script type="text/javascript">
        function check_Number(id) {
           
        }
        </script>
    <div id="maindiv" runat="server" style="height: 1200px;">
    <div id="PuraskarDocUpload" runat="server" visible="true" >
       
        <table class="sample3" style="width: 100%; height:100%; text-align: left" border="0" cellpadding="3"
            cellspacing="1">             
            <tr>
                <td align="center" colspan="2">
                    <strong style="text-align: center" class="headfont">PURASKAR DOCUMENTS UPLOAD FORM                        
                    </strong>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;
                     <asp:Label ID="Label7" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>          

            <tr class="head1">
                <td align="left" colspan="3">Applicant's Personal Details
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="36%">Name
                </td>
                <td width="44%">
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="36%">Parent / Guardian Details
                </td>
                <td width="44%">
                    <asp:Label ID="lblParent_GuardianDetails" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="36%">Gender
                </td>
                <td width="44%">
                    <asp:Label ID="lblGender" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="36%">Date of birth
                </td>
                <td width="44%">
                    <asp:Label ID="lblDOB" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="36%">Caste
                </td>
                <td width="44%">
                    <asp:Label ID="LblCaste" runat="server" Text=""></asp:Label>
                </td>
            </tr>
               <tr class="gdalternate1">
                <td width="36%">
                    <asp:Label ID="lblRegnNo1" runat="server" EnableTheming="True" Text="Regn No"></asp:Label>
                </td>
                <td colspan="3" width="44%">
                    <asp:Label ID="LblRegnNo" runat="server"></asp:Label>
                </td>
            </tr>  
            <tr class="gdrow1" id="trdob" runat="server">
                <td width="36%">
                    <asp:Label ID="Label12" runat="server" Text="Level "></asp:Label>
                </td>
                <td colspan="3" width="44%">                   
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlLevel" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlLevel_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">---Select Level---</asp:ListItem>
                                <asp:ListItem Value="1">O Level</asp:ListItem>
                                <asp:ListItem Value="2">A Level</asp:ListItem>
                                <asp:ListItem Value="3">B Level</asp:ListItem>
                                <asp:ListItem Value="4">C Level</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="Label1" runat="server" EnableTheming="True" Text="Required " Visible="false"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
              <tr id="trExamId" runat ="server" class="gdalternate1" visible="false">
                <td width="36%">
                    <asp:Label ID="Label5" runat="server" Text="Exam"></asp:Label>
                </td>
                <td colspan="3" width="44%">
                    <asp:Label ID="LblExam" runat="server"></asp:Label>
                </td>
            </tr>

            </table>
        </div>
         <div id="divDoc" runat="server" visible="true">
             <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
            <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
            cellspacing="1" runat="server" id="tblDoc" visible="false"> 
             <tr id="Tr1"  runat="server" class="head1" >
                <td align="left" colspan="3">Company Details
                </td>
            </tr>      
                 <tr id="tr2" runat ="server" class="gdalternate1" >
                <td width="36%">
                    <asp:Label ID="Label3" runat="server" Text="Compnay Name <b class='mandatory'>*</b>"></asp:Label>
                </td>
                <td colspan="3" width="44%">
                   <asp:TextBox ID="TxtCompanyName" runat="server" Width="520px" TabIndex="34" MaxLength="30"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                </td>
            </tr>

                 <tr id="tr3" runat ="server" class="gdrow1" >
                <td width="36%">
                    <asp:Label ID="Label4" runat="server" Text="Address Line1 <b class='mandatory'>*</b>">
                                </asp:Label>
                </td>
                <td colspan="3" width="44%">
                   <asp:TextBox ID="txtAddress1" runat="server" Width="520px" TabIndex="34" MaxLength="30"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                </td>
            </tr>
                <tr id="tr4" runat ="server" class="gdalternate1" >
                <td width="36%">
                    <asp:Label ID="Label6" runat="server" Text="Address Line2 <b class='mandatory'>*</b>">
                                </asp:Label>
                </td>
                <td colspan="3" width="44%">
                   <asp:TextBox ID="txtAddress2" runat="server" Width="520px" TabIndex="34" MaxLength="30"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                </td>
            </tr>
                <tr id="tr5" runat ="server" class="gdrow1" >
                <td width="36%">
                    <asp:Label ID="Label8" runat="server" Text="Address Line3 ">
                                </asp:Label>
                </td>
                <td colspan="3" width="44%">
                   <asp:TextBox ID="txtAddress3" runat="server" Width="520px" TabIndex="34" MaxLength="30"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                </td>
            </tr>
                 <tr id="tr6" runat ="server" class="gdalternate1" >
                <td width="36%">
                    <asp:Label ID="Label9" runat="server" Text="Country <b class='mandatory'>*</b> ">
                                </asp:Label>
                </td>
                <td colspan="3" width="44%">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate> 
                    <asp:DropDownList ID="ddlContryName" runat="server" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlContryName_SelectedIndexChanged">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">India</asp:ListItem>
                                    <asp:ListItem Value="2">Others</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp &nbsp &nbsp
                   <asp:TextBox ID="txtContryName" runat="server" Width="200px" TabIndex="34" MaxLength="30"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;" Visible="false">
                                </asp:TextBox>
                                </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSave" />
                            </Triggers>
                        </asp:UpdatePanel>
                </td>
            </tr>
                 <tr id="tr7" runat ="server" class="gdrow1" >
                <td width="36%">
                    <asp:Label ID="Label10" runat="server" Text="City Name <b class='mandatory'>*</b>">
                                </asp:Label>
                </td>
                <td colspan="3" width="44%">
                   <asp:TextBox ID="txtCityName" runat="server" Width="520px" TabIndex="34" MaxLength="30"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                </td>
            </tr>
                 <tr id="tr8" runat ="server" class="gdalternate1" >
                <td width="36%">
                    <asp:Label ID="Label11" runat="server" Text="Company State <b class='mandatory'>*</b>">
                                </asp:Label>
                </td>
                <td  width="40%">
                  <asp:DropDownList ID="ddlCmpState" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCmpState_SelectedIndexChanged"
                                    TabIndex="30" Width="240px" Enabled="true">
                                </asp:DropDownList>
                   </td><td>
                   <asp:TextBox ID="txtContryState" runat="server" Width="200px" TabIndex="34" MaxLength="100"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;" Visible="false">
                                </asp:TextBox>
                </td>
            </tr>

                 <tr id="tr9" runat ="server" class="gdrow1" >
                <td width="36%">
                    <asp:Label ID="lblCmpDistrict" runat="server" Text="District <b class='mandatory'>*</b><font color='RED'></font>">
                                </asp:Label>
                </td>
                <td  width="40%">
                  <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlCmpDistrict" runat="server" TabIndex="31" Width="240px" Enabled="true">
                                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCmpState" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                   </td><td>
                   <asp:TextBox ID="txtContryDis" runat="server" Width="200px" TabIndex="34" MaxLength="100"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;" Visible="false">
                                </asp:TextBox>
                </td>
            </tr>

                 <tr id="tr10" runat ="server" class="gdalternate1" >
                <td width="36%">
                    <asp:Label ID="lblCmpPinCode" runat="server" Text="Pin Code <b class='mandatory'>*</b> <font color='RED'></font>">
                                </asp:Label>
                </td>
                <td colspan="2" width="44%">
                   <asp:TextBox ID="txtCmpPinCode" runat="server" oncopy="return false" oncut="return false"
                                    onkeypress="checkNumber(this,6,0,event);" onpaste="return false" TabIndex="32"
                                    Width="93px" MaxLength="6" Enabled="true"></asp:TextBox>
                </td>
            </tr>

                 <tr id="tr11" runat ="server" class="gdrow1" >
                <td width="36%">
                    <asp:Label ID="Label13" runat="server" Text="Company Email <b class='mandatory'>*</b>">
                                </asp:Label>
                </td>
                <td colspan="3" width="44%">
                   <asp:TextBox ID="txtCompanyEmail" runat="server" Width="520px" TabIndex="34" MaxLength="30"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                </td>
            </tr>

                 <tr id="tr12" runat ="server" class="gdalternate1" >
                <td width="36%">
                    <asp:Label ID="Label14" runat="server" Text="Company Phone <b class='mandatory'>*</b>">
                                </asp:Label>
                </td>
                <td colspan="3" width="44%">
                   <asp:TextBox ID="txtCompanyPhone" runat="server" Width="520px" TabIndex="34" MaxLength="12"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                </td>
            </tr>

<tr id="tr13" runat ="server" class="gdrow1" >
                <td width="36%">
                    <asp:Label ID="Label15" runat="server" Text="Offer Letter No <b class='mandatory'>*</b>">
                                </asp:Label>
                </td>
                <td colspan="3" width="44%">
                   <asp:TextBox ID="txtOfferletterno" runat="server" Width="520px" TabIndex="34" MaxLength="30"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                </td>
            </tr>

                 <tr id="tr14" runat ="server" class="gdalternate1" >
                <td width="36%">
                    <asp:Label ID="Label16" runat="server" Text="Placement Date <b class='mandatory'>*</b>">
                                </asp:Label>
                </td>
                <td colspan="3" width="44%">                
                     <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtPlacementDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ReadOnly="false"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtPlacementDate">
                                </asp:CalendarExtender>
                                <img id="img2" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </td>
            </tr>
                <tr id="tr15" runat ="server" class="gdrow1" >
                <td width="36%">
                    <asp:Label ID="Label17" runat="server" Text="Placement Upto <b class='mandatory'>*</b>">
                                </asp:Label>
                </td>
                <td colspan="3" width="44%">
                                     <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtPlacementUpto" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ReadOnly="false"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtPlacementUpto">
                                </asp:CalendarExtender>
                                <img id="img1" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </td>
            </tr>

                 <tr id="tr16" runat ="server" class="gdalternate1" >
                <td width="36%">
                    <asp:Label ID="Label18" runat="server" Text="Salary being Given <b class='mandatory'>*</b>">
                                </asp:Label>
                </td>
                <td colspan="3" width="44%">
                   <asp:TextBox ID="txtSalarybeingGiven" runat="server" Width="200px" TabIndex="34" MaxLength="30"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                </td>
            </tr>
                 <tr id="Tr17"  runat="server" class="head1" >
                <td align="left" colspan="3">Salary Details
                </td>
            </tr>      
                 <tr id="tr18" runat ="server" class="gdalternate1" >
                <td width="36%">
                    <asp:Label ID="Label19" runat="server" Text="Bank Account Number <b class='mandatory'>*</b>"></asp:Label>
                </td>
                <td colspan="3" width="44%">
                   <asp:TextBox ID="txtBankAcNo" runat="server" Width="520px" TabIndex="34" MaxLength="25"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                </td>
            </tr>
                 <tr id="tr19" runat ="server" class="gdrow1" >
                <td width="36%">
                    <asp:Label ID="Label20" runat="server" Text="Bank Name <b class='mandatory'>*</b>"></asp:Label>
                </td>
                <td colspan="3" width="44%">
                   <asp:TextBox ID="txtBankName" runat="server" Width="520px" TabIndex="34" MaxLength="30"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                </td>
            </tr>
                <tr id="tr20" runat ="server" class="gdalternate1" >
                <td width="36%">
                    <asp:Label ID="Label21" runat="server" Text="Bank City <b class='mandatory'>*</b>"></asp:Label>
                </td>
                <td colspan="3" width="44%">
                   <asp:TextBox ID="txtBankCity" runat="server" Width="520px" TabIndex="34" MaxLength="30"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                </td>
            </tr>
                 <tr id="tr21" runat ="server" class="gdrow1" >
                <td width="36%">
                    <asp:Label ID="Label22" runat="server" Text="Bank State <b class='mandatory'>*</b>"></asp:Label>
                </td>
                <td colspan="3" width="44%">
                    <asp:DropDownList ID="ddlBankState" runat="server" 
                                    TabIndex="30" Width="240px" Enabled="true">
                                </asp:DropDownList>
                </td>
            </tr>

                 <tr  runat="server" class="head1" >
                <td align="left" colspan="3">Documents Upload
                </td>
            </tr>           
                <tr class="gdalternate1">
                    <td width="36%" class="auto-style1">
                        <asp:Label ID="LblAppointmentLetterUpload" runat="server" EnableTheming="True" Text=" Latest Appointment Letter Upload &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td colspan="2" width="44%" class="auto-style1">                       
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>                                
                                <asp:FileUpload ID="AppointmentLetterUpload" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="360px" /><br />
                                ( PDF file with size upto 100 KB ) 
                                <asp:RegularExpressionValidator ID="regPHCertUpload" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="AppointmentLetterUpload" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSave" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            
               
                <tr class="gdrow1">
                    <td width="36%">
                        <asp:Label ID="LblSalarySlip" runat="server" EnableTheming="True"  Text="Salary Slip &lt;b class='mandatory'&gt;*&lt;/b&gt; &lt;br/&gt;&lt;font color=&quot;red&quot;&gt; Salary Slip not older than three months.&lt;/font&gt;"></asp:Label>
                    </td>
                    <td colspan="2" width="44%">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>

                                <asp:FileUpload ID="SalarySlipFileUpload" runat="server" onkeypress="return false;" TabIndex="45" /><br />
                                ( PDF file with size upto 100 KB  )  
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                ControlToValidate="SalarySlipFileUpload" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                Display="Dynamic" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSave" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
           
                     
            <tr class="gdalternate1">
                <td width="36%">
                    <asp:Label ID="LblBankStatementUpload" runat="server" EnableTheming="True" Text= " Bank Statement &lt;b class='mandatory'&gt;*&lt;/b&gt; &lt;br/&gt;&lt;font color=&quot;red&quot;&gt; where salary is being received.&lt;/font&gt;"></asp:Label>
                    <br />
                </td>
                <td colspan="2" width="44%">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>

                            <asp:FileUpload ID="BankStatementFileUpload" runat="server" onkeypress="return false;" TabIndex="45" /><br />
                            ( PDF file with size upto 100 KB  ) 
            <asp:RegularExpressionValidator ID="BankStatementFileU" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                ControlToValidate="BankStatementFileUpload" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                Display="Dynamic" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSave" />
                        </Triggers>
                    </asp:UpdatePanel>

                </td>
            </tr>
            

            <tr class="head1">
                                <td colspan="3" id="tddeclarartion" runat="server">Declaration
                                </td>
                    </tr>
                            <tr class="gdrow1">
                                <td align="left" valign="top" colspan="3">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td class="style107"></td>
                                            <td style="text-align: justify;">
                                                <asp:CheckBox ID="chkdisclamier" runat="server" TabIndex="42" Text="<font color='RED'>*</font>" />
                                                I
                                            <asp:Label ID="Label2" runat="server" Text=""></asp:Label>,
                                            <asp:Label ID="Lblsalutation" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="LblDecMName" runat="server" Text=""></asp:Label>                                              
                                                ,  solemnly declare that all the facts and figures provided in the online form is true and correct to the best of my knowledge and belief. I have submitted the latest Appointment letter, salary slip and bank statement as per criteria / norms.
                                            
                                            
                                           
                                            </td>
                                        </tr>


        </table>
                                    </td></tr>


 <tr class="head1">
                                <td colspan="3" id="td1" runat="server">Disclaimer
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td class="style107" style="text-align: justify;" colspan="3">                                           
                                               &nbsp;&nbsp;&nbsp;&nbsp; 
                                    <b>“In any case, during verification any discrepancy is found in facts and figures mentioned in Appointment letter, salary slip and bank statement, your claim will be summarily rejected and claim will not be settled. And NIELIT shall have the right to take any action against the applicant as per law/ and it deemed fit”</b>
                                            </td>
                                        </tr>
</table>
</ContentTemplate>
 
                        </asp:UpdatePanel>
   </div>

        <div id="divSbtn" style="text-align: right; margin-top: 10px; height: 80px" runat="server" visible="true"> 
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnSave" runat="server" OnClick="SaveRecord" OnClientClick="return ValidateEmpty();" Style="height: 26px"
                        Text="Save" Visible="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    
    <div id="divSumbitMsg" runat="server" >
          <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                <ContentTemplate>
         <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
            cellspacing="1" id="tblmsg" runat="server" visible="false">
            <%-- <tr>
                <td align="center" colspan="2">
                    &nbsp;
                       </td>
            </tr><tr>
                <td align="center" colspan="2">
                    &nbsp;
                       </td>
            </tr><tr>
                <td align="center" colspan="2">
                    &nbsp;
                       </td>
            </tr>--%>
            <tr>
                <td align="center" colspan="2">
                    <strong style="text-align: center" class="headfont">
                        <asp:Label Width="99%" EnableTheming="False" ID="LblSubmitMessage"
            runat="server" ForeColor="Green" Font-Bold="True" Font-Size="Medium"></asp:Label>                       
                    </strong>
                </td>
            </tr>
             </table>
           </ContentTemplate>
            </asp:UpdatePanel>
    </div>
        </div>
</asp:Content>



<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" align="center" class="nav" cellspacing="0" cellpadding="0"
        id="tblNavLinks" width="97%" visible="false">
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">               
                <asp:HyperLink ID="hl1" runat="server" Target="_blank">Print Puraskar Application Form</asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content9" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1 {
            height: 9px;
        }
    </style>
</asp:Content>
