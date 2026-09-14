<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProfileCandPrint.aspx.cs"
    Inherits="CAND_ProfileCandPrint_" %>

<%@ Register Src="../UserControl/Address.ascx" TagName="Address" TagPrefix="uc1" %>
<%@ Register src="../UserControl/NormalHeader.ascx" tagname="NormalHeader" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #Table2
        {
            height: 231px;
        }
    </style>
</head>
<body style="background-color: #FFFFFF;">
    <form id="form2" runat="server">
    <div>
    <uc2:NormalHeader ID="NormalHeader1" runat="server" />
    <br />
        <asp:MultiView ID="m1" runat="server">
            <asp:View ID="name" runat="server">
                <table class="sample3" cellpadding="0" cellspacing="0" width="95%" runat="server" align="center">
                    <tr class="head1">
                        <td align="center" colspan="2">
                            FORM FOR CORRECTION/ADDITION OF DETAILS
                        </td>
                    </tr>
                    <tr class="gdrow1">
                    <td align="left"  colspan="2" >PLEASE ATTACH ATTESTED COPY OF THE RELEVANT CERTIFICATES FOR CORRECTION.</td>
                    </tr>
                    <tr class="gdalternate1">
                        <td width="30%">
                            <asp:Label ID="Label2" runat="server" Text="NAME"></asp:Label>
                        </td>
                        <td>
                            Punit Babel
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td width="30%">
                            <asp:Label ID="Label3" runat="server" Text="FATHER'S NAME"></asp:Label>
                        </td>
                        <td>
                            Mr B.L.Babel
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td width="30%">
                            <asp:Label ID="Label4" runat="server" Text="MOTHER'S NAME"></asp:Label>
                        </td>
                        <td>
                            Mrs SnehLata Babel
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td width="30%">
                            <asp:Label ID="Label5" runat="server" Text="DOB(DD/MM/YYYY"></asp:Label>
                            )</td>
                        <td>
                            5/Jan/1990
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            <asp:Label ID="Label6" runat="server" Text="Phone with STD Code"></asp:Label>
                        </td>
                        <td>
                            0294-5344334
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td style="width: 36%">
                            <asp:Label ID="Label7" runat="server" Text="Mobile"></asp:Label>
                        </td>
                        <td>
                            9783582421 &nbsp;
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            <asp:Label ID="Label8" runat="server" EnableTheming="True" Text="Email"></asp:Label>
                        </td>
                        <td>
                            pbabel@gmail.com
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td>
                            Date: 5/Jan/2012
                        </td>
                        <td align="right" rowspan="4" valign="bottom">
                            [Signature of the Applicant]
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td>
                            Place: Udaipur
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="address" runat="server">
                <table id="Table1" class="sample3" cellpadding="0" cellspacing="0" width="95%" runat="server" align="center">
                        <tr class="head1">
                            <td align="center" colspan="2">
                                FORM FOR CHANGE/ CORRECTION OF ADDRESS
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td width="30%">
                                <asp:Label ID="Label1" runat="server" Text="Name"></asp:Label>
                            </td>
                            <td>
                                Punit Babel
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="2">
                                <asp:Label ID="Lbladdress" runat="server" Text="Correspondence Address"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td style="width: 36%">
                                <asp:Label ID="Label32" runat="server" Text="House No. / Street / Colony "></asp:Label>
                            </td>
                            <td>
                                112
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td style="width: 36%">
                                <asp:Label ID="Label20" runat="server" Text="City/Village"></asp:Label>
                            </td>
                            <td>
                                Udaipur
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td style="width: 36%">
                                <asp:Label ID="Label22" runat="server" Text="Tehsil/ Post"></asp:Label>
                            </td>
                            <td>
                                Aspur
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td style="width: 36%">
                                <asp:Label ID="Label34" runat="server" Text="District"></asp:Label>
                            </td>
                            <td>
                                Udaipur
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td style="width: 36%">
                                <asp:Label ID="Label33" runat="server" Text="State"></asp:Label>
                            </td>
                            <td>
                                Rajasthan
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td style="width: 36%">
                                <asp:Label ID="Label39" runat="server" Text="Pin Code"></asp:Label>
                            </td>
                            <td>
                                313001
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td style="width: 36%">
                                <asp:Label ID="Label18" runat="server" Text="Phone with STD Code"></asp:Label>
                            </td>
                            <td>
                                0294-5344334
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td style="width: 36%">
                                <asp:Label ID="Label40" runat="server" Text="Mobile"></asp:Label>
                            </td>
                            <td>
                                9783582421 &nbsp;
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td style="width: 36%">
                                <asp:Label ID="Label21" runat="server" EnableTheming="True" Text="Email"></asp:Label>
                            </td>
                            <td>
                                pbabel@gmail.com
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td align="center" colspan="2" style="text-transform:uppercase;">
                                All correspondence to be sent at the above address and change of address please
                                be recorded
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td>
                                Date: 5/Jan/2012
                            </td>
                            <td align="right" rowspan="4" valign="bottom">
                                [Signature of the Applicant]
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td>
                                Place: Udaipur
                            </td>
                        </tr>
                    </table>
            </asp:View>
        </asp:MultiView>
        <div style="text-align: center; margin-top: 10px">
            <asp:Button ID="Button2" runat="server" Text="Print" OnClientClick=" window.print();" />
            <asp:Button ID="Button3" runat="server" Text="Close" OnClientClick="window.close(); return false" />
        </div>
    </div>
    </form>
</body>
</html>
