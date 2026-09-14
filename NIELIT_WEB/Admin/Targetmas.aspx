<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Targetmas.aspx.cs" Inherits="Admin_Targetmas"
    Debug ="False"
    MasterPageFile="~/MasterPages/MyInfo.master" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <%--<uc1:NormalHeader ID="NormalHeader1" runat="server" />--%>
    <asp:Label ID="lblHeading" runat="server" Text="Target Master"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc2:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateFields() {
              if (!isBlank("<%=txttargetname.ClientID %>", "Target Name"))
                return false;

                if (!isSelected("<%=ddlfrequency.ClientID %>", "Frequency "))
                    return false;

            return true;
        }

        function validatename(input) {
            input.value = input.value.replace(/[^A-Za-z0-9\s-]/g, ''); 
        }
    </script>
    <div id="divfilter" runat="server" width="100%">
        <asp:Label ID="lblstart" SkinID="CaptionLabel" runat="server"><span style='color:red'>*</span> fields are compulsory</asp:Label>
        <table>
            <tr>
                <td>
                    <table class="sample2" style="width: 100%; text-align: left" border="0" cellpadding="1"
                        cellspacing="0">

                        <tr class="odd">
                            <td valign="top" width="80%">
                                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel">Target Name<span style="color:red">*</span></asp:Label>
                                <asp:Label ID="Label1" runat="server" Style="font-style:italic" Text="(Only Alphabets, Number and -)"></asp:Label>
                            </td>
                            <td width="20%" valign="top">
                                <asp:TextBox runat="server" ID="txttargetname" Placeholder="Enter Target Name (Only Alphabets, Number and -)" MaxLength="60" oninput="validatename(this)"  Width="300px"></asp:TextBox>
                            </td>
                        </tr>

                        <tr class="odd">
                            <td valign="top" width="80%">
                                <asp:Label ID="lblfrequency" runat="server" SkinID="CaptionLabel" >Frequency<span style="color:red">*</span></asp:Label>
                            </td>
                            <td width="20%" valign="top">
                                <asp:DropDownList ID="ddlfrequency" runat="server" Width="300px" TabIndex="1">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr class="odd">
                            <td valign="top" width="80%">
                                <asp:Label ID="lblflupload" runat="server" SkinID="CaptionLabel" >File Upload Required<span style="color:red">*</span></asp:Label>
                            </td>
                            <td width="20%" valign="top">
                                <asp:RadioButtonList ID="radflupload" runat="server" Width="300px" TabIndex="1">
                                    <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <div style="text-align: right; width: 100%; margin-top: 10px;" align="center">
                        <asp:Button ID="btnsubmit" runat="server" Text="Submit" OnClick="btnsubmit_Click"
                            OnClientClick="return ValidateFields();" />

                        <asp:Button ID="btnreset" runat="server" Text="Reset"
                            OnClick="btnreset_Click" />
                        <asp:Button ID="btncancel" runat="server" Text="Cancel"
                            OnClick="btncancel_Click" />
                    </div>
                </td>
                <td></td>
            </tr>
        </table>
    </div>

</asp:Content>
