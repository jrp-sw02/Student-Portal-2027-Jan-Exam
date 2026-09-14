<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="ConfirmLock.aspx.cs" Inherits="CAND_ConfirmLock" %>

<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Confirm Lock
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <div class="sample3">
        <table width="100%" class="box" style="margin-top: 20px" cellpadding="3" cellspacing="0">
            <tr class="gdalternate1">

                <td align="left" style="font-size:large;">
                    Profile Details: Confirm Lock
                </td>
            </tr>
            <tr class="gdalternate1">
                <td align="left" style="color: #800000; font-size: 17px;">
                    <table style="text-align: left" class="sample3" border="0" cellpadding="3" cellspacing="1"
                        width="100%">
                        <tr class="gdalternate1">
                            <td colspan="3" align="left" style="color: #FF0000; font-size: 13px;">
                                Note: Once you have locked your profile,you will not be able to update your profile detail in
                                future.<br />
                                If you wish to continue. Please click on Confirm Lock button to lock the profile
                                details else click on Cancel button to go back to profile page.
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="3">
                                <asp:Label ID="Label69" runat="server" Text="Personal Details"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdalternate1" id="TrName" runat="server">
                            <td style="width: 40%">
                                <asp:Label ID="Label70" runat="server" Text=" Full Name"></asp:Label>
                            </td>
                            <td style="width: 60%">
                                <asp:Label ID="LblAppName" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1" id="TrFatherName" runat="server">
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Father's Name"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblFatherName" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdalternate1" id="TrMotherName" runat="server">
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Mother's Name"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblMotherName" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdalternate1" id="TrGardianName" runat="server" visible="false">
                            <td>
                                Guardian&#39;s Name
                            </td>
                            <td>
                                <asp:Label ID="LblGuardianName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Gender"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="LblGender" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Date of Birth "></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblDob" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="3">
                                <asp:Label ID="Label8" runat="server" Text="Contact Details"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="Phone with STD Code"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="LblPhone" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td style="width: 36%">
                                <asp:Label ID="Label40" runat="server" Text="Mobile"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="LblMobile" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td style="width: 36%">
                                <asp:Label ID="Label21" runat="server" EnableTheming="True" Text="Email"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="LblEmail" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="3">
                                <asp:Label ID="Label74" runat="server" Text="Correspondence Address Details"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td style="width: 36%">
                                <asp:Label ID="Label32" runat="server" Text="Address Line1"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="LblCorAddressLine1" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td style="width: 36%">
                                <asp:Label ID="Label20" runat="server" Text="Address Line2"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="LbCorlAddressLine2" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td style="width: 36%">
                                <asp:Label ID="Label22" runat="server" Text="Address Line3"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="LblCorAddressLine3" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td style="width: 36%">
                                City
                            </td>
                            <td colspan="2">
                                <asp:Label ID="LblCityName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td style="width: 36%">
                                <asp:Label ID="Label34" runat="server" Text="District"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="LblDistrict" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td style="width: 36%">
                                <asp:Label ID="Label33" runat="server" Text="State"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="LblState" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td style="width: 36%">
                                <asp:Label ID="Label39" runat="server" Text="Pin Code"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="LblPincode" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td align="left" style="color: #800000;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/rightMark2.jpg" />
                    I
                    <asp:Label ID="LblCandName" runat="server"></asp:Label>
                    &nbsp;declare that the above statement and information are correct to the best of
                    my knowledge and belief and I agree and abide by the information above.
                </td>
            </tr>
            <tr class="gdalternate1">
                <td align="left" style="color: #FF0000">
                    &nbsp;
                </td>
            </tr>
            
            <tr class="gdalternate1">
                <td align="left" style="color: #FF0000">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="BtnLock" runat="server" Text="Confirm Lock" OnClientClick="return confirm('Are you sure you want to lock your personal detail?');"
            OnClick="BtnLock_Click" />
        <asp:Button ID="LnkBtnNo" runat="server" OnClick="LnkBtnNo_Click" Text="Cancel" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
