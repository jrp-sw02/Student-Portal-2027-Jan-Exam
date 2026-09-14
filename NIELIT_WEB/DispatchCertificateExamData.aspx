<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DispatchCertificateExamData.aspx.cs"
    Inherits="DispatchCertificateExamData" Debug="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background-color: #ffffff">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div style="text-align: center; width: 100%;">
            <center>
                <table id="tblMain" runat='server' width="1000px" bgcolor="#E1E1E1">
                    <tr>
                        <td align="center" style="width: 100%">
                            <asp:Label ID="lblErrMsg" runat="server" ForeColor="red"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trLogin" runat="server" visible="true">
                        <td align="center" style="width: 100%">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 50%" id="tbllogin"
                                runat="server">
                                <tr>
                                    <td align="center" width="20%" style="height: 24px" valign="middle">
                                        <strong>Password :</strong>
                                    </td>
                                    <td align="center" width="40%" style="height: 24px" valign="middle">
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                                    </td>
                                    <td align="center" style="height: 24px" valign="middle">
                                        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trQuery" runat="server" visible="false">
                        <td align="left" style="width: 100%" id="tbldetail" runat="server">
                            <table border="0" cellpadding="0" cellspacing="3" style="width: 75%">
                                <tr>
                                    <td align="left" valign="top" colspan="2">
                                        <asp:Label ID="lblHeader" Text="Dispatch Certificate Exam Data" runat="server" CssClass="pageTitle"
                                            Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" style="width: 80px">Select Table Name:-
                                    </td>
                                    <td style="width: 50%">
                                        <asp:DropDownList ID="ddltablename" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddltablename_SelectedIndexChanged">
                                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                            <asp:ListItem Value="1">ConData_BCC_till_Mar2013</asp:ListItem>
                                            <asp:ListItem Value="2">ConData_CCC_EMEC_SPL</asp:ListItem>
                                            <asp:ListItem Value="3">ConData_CCC_till_Feb2013_EMEC</asp:ListItem>
                                            <asp:ListItem Value="4">condata_ccc_Till_Feb2013_regular</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" style="width: 80px">Total no of Records Exported :-
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="Lbexported" runat="server" Font-Bold="true" Style="display: inline"
                                                    Text=""></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddltablename" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" style="width: 80px">Total no of Records left to be Exported:-
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="Lbleftexported" runat="server" Font-Bold="true" Text=""></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddltablename" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" style="width: 80px">Enter no of records to export :-
                                    <asp:TextBox ID="txtRecords" runat="server" MaxLength="5"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:Button ID="btnResetSelect" runat="server" Text="Reset" Font-Bold="True" OnClick="btnResetSelect_Click" />
                                        <asp:Button ID="btnGo" runat="server" Text="Execute" Font-Bold="True" OnClick="btnGo_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
    </form>
</body>
</html>
