<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DuplicateCandidateList.aspx.cs"
    Inherits="CAND_DuplicateCandidateList" Title="Candidate Details" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
    </style>
</head>
<body style="background-color: #ffffff; margin-bottom: 0px;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table class="sample3" border="0" width="977px" align="center" cellpadding="2" cellspacing="1">
        <tr>
            <td colspan="3"> 
                <uc2:NormalHeader ID="NormalHeader1" runat="server" />
            </td>
        </tr>
        <tr class="head1">
            <td colspan="3">
                <asp:Label ID="Label69" runat="server" Text="Personal Details"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="TrName" runat="server">
            <td style="width: 36%">
                <asp:Label ID="Label70" runat="server" Text=" Full Name"></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblAppName" runat="server" Text=""></asp:Label>
            </td>
            <td style="width:15%" align="center" rowspan="8" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table width="90%" cellspacing="3">
                            <tr>
                                <td class="box" align="center" valign="middle" style="background-color: #c9d7e2;
                                    height: 140px;">
                                    <asp:Image ID="ImgCandidatePhoto" ImageUrl="../images/photo.jpg" Width="112" Height="130"
                                        runat="server" Style="text-align: center;" ImageAlign="Middle" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" id="TdPhotoCaption" class="box" style="background-color: #c9d7e2;">
                                    <asp:ImageButton ID="ImgBtnPrevious" runat="server" Style="float: left; width: 20px;"
                                        ImageUrl="~/images/previous.gif" CommandArgument="P" Enabled="False" OnClick="ToggleImage" />
                                    <asp:Label ID="LblPhotoCaption" runat="server" Text="Photograph"></asp:Label>
                                    <asp:ImageButton ID="ImgBtnNext" ToolTip="Click to view signature" runat="server"
                                        ImageUrl="~/images/next.gif" Style="float: right;" CommandArgument="N" OnClick="ToggleImage" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr class="gdrow1" id="TrFatherName" runat="server">
            <td style="width: 36%">
                <asp:Label ID="Label2" runat="server" Text="Father's Name"></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblFatherName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="TrMotherName" runat="server">
            <td style="width: 36%">
                <asp:Label ID="Label3" runat="server" Text="Mother's Name"></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblMotherName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="TrGardianName" runat="server" visible="false">
            <td style="width: 36%">
                Guardian&#39;s Name
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblGuardianName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label4" runat="server" Text="Gender"></asp:Label>
            </td>
            <td align="left" style="width: 44%">
                <asp:Label ID="LblGender" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label5" runat="server" Text="Marital Status"></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblMaritalStatus" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label6" runat="server" Text="Date of Birth "></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblDob" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label7" runat="server" Text="Cast Category"></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblCategory" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>
                <asp:Label ID="Label84" runat="server" Text="Occuption"></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblOccuption" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td>
                <asp:Label ID="Label85" runat="server" Text="Is Handicaped"></asp:Label>
            </td>
            <td style="width: 44%" colspan="2">
                <asp:Label ID="LblIsHandicaped" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>
                <asp:Label ID="Label86" runat="server" Text="Is Ex-Servicemane"></asp:Label>
            </td>
            <td style="width: 44%" colspan="2">
                <asp:Label ID="LblIsExServicemane" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="head1">
            <td colspan="3">
                <asp:Label ID="Label83" runat="server" Text="Course Registration Details"></asp:Label>
            </td>
        </tr>
        <tr>
        <td colspan="3">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                Width="100%">
                <Columns>
                    <asp:BoundField HeaderText="Registration Number" DataField="RegNo" >
                    <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Registration Date" DataField="RegDate" 
                        DataFormatString="{0:dd-MMM-yyyy}" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Valid Upto" DataField="ValidUpto" 
                        DataFormatString="{0:dd-MMM-yyyy}" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Current Course" DataField="Course" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Candidate Type" DataField="CandidateType" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Status" DataField="Status" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </td>
        </tr>
        <tr class="head1">
            <td colspan="3">
                <asp:Label ID="Label8" runat="server" Text="Contact Details"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>
                <asp:Label ID="Label18" runat="server" Text="Phone Number with STD Code"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPhone" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label40" runat="server" Text="Mobile Number"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblMobile" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label21" runat="server" EnableTheming="True" Text="Email Address"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblEmail" runat="server" Text="NA"></asp:Label>
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
                <asp:Label ID="LblCorAddressLine1" runat="server">NA</asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label20" runat="server" Text="Address Line2"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LbCorlAddressLine2" runat="server">NA</asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label22" runat="server" Text="Address Line3"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblCorAddressLine3" runat="server">NA</asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label9" runat="server" Text="City"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblCityName" runat="server">NA</asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label34" runat="server" Text="District"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblDistrict" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label33" runat="server" Text="State"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblState" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label39" runat="server" Text="Pin Code"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPincode" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="head1">
            <td colspan="3">
                <asp:Label ID="Label1" runat="server" Text="Permanent Address  Details"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label11" runat="server" Text="Address Line1"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPerAddressLine1" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label12" runat="server" Text="Address Line2"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPerAddressLine2" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label13" runat="server" Text="Address Line3"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPerAddressLine3" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                City
            </td>
            <td colspan="2">
                <asp:Label ID="LblPerCityName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label14" runat="server" Text="District"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPerDistrict" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label15" runat="server" Text="State"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPerState" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label16" runat="server" Text="Pin Code"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPerPinCode" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="head1">
            <td colspan="3">
                Educational/Qualification Details
            </td>
        </tr>
        <tr valign="top" class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label60" runat="server" Text="Highest Educational Qualification"></asp:Label>
            </td>
            <td colspan="2" style="width: 44%">
                <asp:Label ID="LblHeighestEducation" runat="server"></asp:Label>
            </td>
        </tr>
        <tr valign="top" class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label82" runat="server" Text="Year of Passing"></asp:Label>
            </td>
            <td colspan="2" style="width: 44%">
                <asp:Label ID="LblYrOfPassing" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
