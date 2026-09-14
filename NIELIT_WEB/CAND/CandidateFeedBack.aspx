<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CandidateFeedBack.aspx.cs" Inherits="CAND_CandidateFeedBack" MasterPageFile="~/MasterPages/MyInfo.master" %>


<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .Weak
        {
            color:Blue;
            font-size:10pt;
        }
        .Poor
        {
            color:Red;
            font-size:10pt;
        }
        .Strong
        {
            color:Green;
            font-size:10pt;
        }
        .Good
        {
            color:Gray;
            font-size:10pt;
        }
        .Excellent
        {
            color:Maroon;
            font-size:10pt;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <%--<uc1:NormalHeader ID="NormalHeader1" runat="server" />--%>
    <asp:Label ID="lblHeading" runat="server" Text="Feedback / Suggestions"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc2:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateLogin() {
            if (!isBlank("<%=Txtfeedback.ClientID %>", "Feedback"))
                return false;
            return true;
        }
        function textCounter() {
            var field = document.getElementById("<%=Txtfeedback.ClientID %>");
            var maxlimit = 1000;
            var countfield = document.getElementById("<%=lblCount.ClientID %>");
            if (field.value.length > maxlimit)
                field.value = field.value.substring(0, maxlimit);
            else
                countfield.innerHTML = maxlimit - field.value.length;
        }
        function textCounter1() {
            var field = document.getElementById("<%=TxtSuggestions.ClientID %>");
            var maxlimit = 1000;
            var countfield = document.getElementById("<%=lblCount1.ClientID %>");
            if (field.value.length > maxlimit)
                field.value = field.value.substring(0, maxlimit);
            else
                countfield.innerHTML = maxlimit - field.value.length;
        }
    </script>
    <div id="divfilter" runat="server" width="100%">
        <table style="width: 550px;">
            <tr>
                <td align="left" colspan="3">
                    <table class="sample2" style="width: 100%; text-align: left" border="0" cellpadding="1"
                        cellspacing="0">
                        <tr class="even">
                            <td width="100%" colspan="2">
                                <asp:Label ID="LblTitle" runat="server"  style="font-size:10pt; font-weight:bold;"></asp:Label>
                            </td>
                        </tr>
                        <tr class="odd">
                            <td valign="top" width="80%">
                                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Feedback&lt;b class='mandatory'&gt;*&lt;/b&gt; (Maximum 1000 characters only)"></asp:Label>
                                </td>
                            <td width="20%" valign="top">
                                <asp:Label ID="lblCount" runat="server" SkinID="CaptionLabel" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr class="even">
                            <td colspan="2">
                                <asp:TextBox ID="Txtfeedback" runat="server" SkinID="txt756" TextMode="MultiLine"
                                    Height="70px" ToolTip="Feedback" onkeyup="textCounter();" onkeydown="textCounter();"
                                    MaxLength="1000"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="odd">
                            <td valign="top" width="80%">
                                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Suggestions (Maximum 1000 characters only)"></asp:Label>
                            </td>
                            <td width="20%" valign="top">
                                <asp:Label ID="lblCount1" runat="server" SkinID="CaptionLabel" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr class="even">
                            <td colspan="2">
                                <asp:TextBox ID="TxtSuggestions" runat="server" TextMode="MultiLine" SkinID="txt756"
                                    Height="70px" ToolTip="Suggestion" onkeyup="textCounter1();" onkeydown="textCounter1();"
                                    MaxLength="10000"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <div style="text-align: right; width: 100%; margin-top: 10px;" align="center">
                        <asp:Button ID="btnsubmit" runat="server" Text="Submit" OnClick="btnsubmit_Click"
                            OnClientClick="return ValidateLogin();" />
                        <asp:Button ID="btnreset" runat="server" Text="Reset" 
                            onclick="btnreset_Click" />
                        <asp:Button ID="btncancel" runat="server" Text="Cancel" 
                            onclick="btncancel_Click" />
                       
                    </div>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
  <div id="divfinal" runat="server" width="100%" visible="false">
        <table border="0" cellpadding="3" class="sample3" width="100%">
            <tr class="gdrow1">
                <td colspan="2">
                    <asp:Label ID="LblWelcome" runat="server"></asp:Label>
                    <br />
                    <br />
                    Your feedback and suggestions have been successfully saved.Thanks for your valuable suggestions and feedback ,we will take care of your feedback and suggestions while designing our application 
                    in future.
                    <br />
                    Please click here to go back to <a href="../frmDashBoard.aspx">Home Page</a>
                    <br />
                    Thanks<br />
                    NIELIT Team
                </td>
            </tr>
            <tr class="gdalternate1">
                <td align="right" colspan="2">
                    <a href="../frmDashBoard.aspx">Back to Home Page</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
