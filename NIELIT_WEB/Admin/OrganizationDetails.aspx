<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="OrganizationDetails.aspx.cs" Inherits="Admin_OrganizationDetails" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
<asp:Label ID="lblHeading" runat="server" Text="Organization Detail"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
 <uc5:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
<script language="javascript" type="text/javascript">
    function ValidateFormFields() {
        if (!isBlank("<%=txtOrgName.ClientID %>", "Organization Name"))
            return false;
        var Pin;
        Pin = document.getElementById("<%=txtPinNumber.ClientID %>").value;
        if (Pin != "") {
            if (!isBlankNumber("<%=txtPinNumber.ClientID %>", "Pin Code"))
                return false;
            if (!isNumber("<%=txtPinNumber.ClientID %>"))
                return false;
            if (!IsValidMinMaxLenght("<%=txtPinNumber.ClientID %>", 6, 6, "Invalid Pin Code"))
                return false;
            
        }
        var e1;
        e1 = document.getElementById("<%=txtEmail.ClientID %>").value;
        if (e1 != "") {
            if (!isValidEmail("<%=txtEmail.ClientID %>", "Not A Valid Email Address"))
                return false;
        }
        var techEmail = document.getElementById("<%=txtTechEmail.ClientID %>").value;
        var techMobile = document.getElementById("<%=txtTechMobile.ClientID %>").value;
        if (techEmail != "") {
            if (!isValidEmail("<%=txtTechEmail.ClientID %>", "Not A Valid  Email Address"))
                return false;
        }
        if (techMobile != "") {
            if (!isBlankNumber("<%=txtTechMobile.ClientID %>", "Mobile Number"))
                return false;
            if (!isNumber("<%=txtTechMobile.ClientID %>"))
                return false;
            if (!chekMobNo("<%=txtTechMobile.ClientID %>"))
                return false;
        }
    }
 </script>
 <table class="sample2" cellpadding="0" cellspacing="1" width="100%">
                <tr class="heading">
                    <td colspan="3">
                        Organization Detail
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="3">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" 
                            Text="Organization Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td colspan="3" valign="top">
                        <asp:TextBox ID="txtOrgName" runat="server" SkinID="txt756"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="3">
                        <asp:Label ID="Label22" runat="server" SkinID="CaptionLabel" 
                            Text="Main Heading &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" colspan="3">
                        <asp:TextBox ID="txtMainHeading" SkinID="txt756" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="3">
                        <asp:Label ID="Label23" runat="server" SkinID="CaptionLabel" 
                            Text="Sub Heading &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" colspan="3">
                        <asp:TextBox ID="txtSubHeading" runat="server" SkinID="txt756"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label15" runat="server" SkinID="CaptionLabel" 
                            Text="Address-1 &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        
                        <asp:Label ID="Label24" runat="server" SkinID="CaptionLabel" 
                            Text="Address-2 &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                                            
                    <td valign="top">
                        <asp:Label ID="Label25" runat="server" SkinID="CaptionLabel" 
                            Text="State &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtAddress1" SkinID="txt248" runat="server"></asp:TextBox>
                    </td>
                    <td valign="top" style="height: 25%;">
                        <asp:TextBox ID="txtAddress2" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="height: 25%;" valign="top">
                    <asp:DropDownList ID="ddlState" runat="server" SkinID="ddl250">
                    </asp:DropDownList>
                   </td>
                </tr>
                 <tr>
                    <td valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" 
                            Text="City Name &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label21" runat="server" SkinID="CaptionLabel" 
                            Text="Pin Number &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                     </td>
                    <td valign="top">
                        &nbsp;</td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                            <asp:TextBox ID="txtCityName" runat="server" SkinID="txt248"></asp:TextBox>            
                    </td>
                    <td valign="top" style="height: 25%;">
                        
                        <asp:TextBox ID="txtPinNumber" runat="server" SkinID="txt248" MaxLength="6" 
                            onkeypress="checkNumber(this,6,0,event)"></asp:TextBox>
                        
                    </td>
                    <td style="height: 25%;" valign="top">
                        
                    </td>
                </tr>
                <tr class="heading">
                    <td colspan="3">
                        Contact Details
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label26" runat="server" SkinID="CaptionLabel" 
                            Text="Phone Number 1 &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label27" runat="server" SkinID="CaptionLabel" 
                            Text="Phone Number 2 &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                       </td>
                    <td valign="top">
                        <asp:Label ID="Label28" runat="server" SkinID="CaptionLabel" 
                            Text="Phone Number 3 &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtPhoneNumber1" runat="server" SkinID="txt248" MaxLength="15"></asp:TextBox>
                    </td>
                    <td valign="top" style="height: 25%;">
                        <asp:TextBox ID="txtPhoneNumber2" runat="server" SkinID="txt248" MaxLength="15"></asp:TextBox>
                    </td>
                    <td style="height: 25%;" valign="top">
                        <%--<asp:DropDownList ID="ddlcity" runat="server" SkinID="ddl250" 
                                    OnSelectedIndexChanged="ddlstate_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>--%>
                        <asp:TextBox ID="txtPhoneNumber3" runat="server" SkinID="txt248" MaxLength="15"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label29" runat="server" SkinID="CaptionLabel" 
                            Text="Phone Number 4 &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label30" runat="server" SkinID="CaptionLabel" 
                            Text="Fax &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label31" runat="server" SkinID="CaptionLabel" 
                            Text="Email &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtPhoneNumber4" runat="server" SkinID="txt248" MaxLength="15"></asp:TextBox>
                    </td>
                    <td valign="top" style="height: 25%;">
                        
                        <asp:TextBox ID="txtFaxNumber" runat="server" SkinID="txt248" MaxLength="15"></asp:TextBox>
                        
                    </td>
                    <td style="height: 25%;" valign="top">
                      
                        <asp:TextBox ID="txtEmail" runat="server" SkinID="txt248" MaxLength="100"></asp:TextBox>
                      
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label32" runat="server" SkinID="CaptionLabel" 
                            Text="Web Site &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                    </td>
                    <td valign="top">
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtWebSite" runat="server" SkinID="txt248" MaxLength="100"></asp:TextBox>
                    </td>
                    <td valign="top" style="height: 25%;">
                    </td>
                    <td style="height: 25%;" valign="top">
                    </td>
                </tr>
                <tr class="heading">
                    <td colspan="3">
                        Bank Account Details
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="2">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" 
                            Text="Bank Account Name &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" 
                            Text="Branch Name &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" colspan="2">
                        <asp:TextBox ID="txtBankAcName" runat="server" SkinID="txt502"></asp:TextBox>
                    </td>
                    <td style="height: 25%;" valign="top">
                        <asp:TextBox ID="txtBranchName" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
                 <tr class="heading">
                    <td colspan="3">
                        Technical Administrator Details
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" 
                            Text="Email &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" 
                            Text="Mobile Number &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top">
                        <asp:TextBox ID="txtTechEmail" runat="server" SkinID="txt248" MaxLength="70"></asp:TextBox>
                    </td>
                    <td style="height: 25%;" valign="top">
                        <asp:TextBox ID="txtTechMobile" runat="server" SkinID="txt248" MaxLength="10" onkeypress="checkNumber(this,10,0,event)"></asp:TextBox>
                    </td>
                    <td valign="top">
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave"  runat="server"
                    Text="Save" onclick="btnSave_Click" OnClientClick="return ValidateFormFields()" />
                </div>
                <asp:HiddenField ID="hMainID" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

