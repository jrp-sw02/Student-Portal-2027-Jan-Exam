<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LinkApplication.aspx.cs"
    Inherits="HO_LinkApplication" MasterPageFile="~/MasterPages/main.master" Debug="false"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    Link Application
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">

    </script>
    <asp:label id="lblError" runat="server" cssclass="error" enabletheming="false" visible="false"
        width="99%"></asp:label>
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="Label9" runat="server" skinid="CaptionLabel" text=" Registered Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                    <td valign="top">
                        <asp:label id="Label2" runat="server" skinid="CaptionLabel" text="Registered Registration Number &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="Label12" runat="server" skinid="CaptionLabel" text="Registration Type &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:dropdownlist id="ddlcrname" runat="server"
                            autopostback="true" skinid="ddl250">
                            <asp:listitem value="0">-- Select One--</asp:listitem>
                        </asp:dropdownlist>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:textbox id="txtregistrationno" runat="server" maxlength="7" skinid="txt248">
                        </asp:textbox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:dropdownlist runat="server" id="ddlregtype" skinid="ddl250">
                            <asp:listitem value="0">-- Select One--</asp:listitem>
                        </asp:dropdownlist>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="Label13" runat="server" skinid="CaptionLabel" text="Qualified Course Name">
                        </asp:label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="Label1" runat="server" skinid="CaptionLabel" text="Qualified Registration Number">
                        </asp:label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="Lbprevcoursename" runat="server" skinid="CaptionLabel" text="Qualified Course Passsing Year">
                        </asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:dropdownlist id="ddlpcrname" runat="server" autopostback="true" skinid="ddl250">
                            <asp:listitem value="0">-- Select One--</asp:listitem>
                        </asp:dropdownlist>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:textbox id="txtpregistrationno" runat="server" maxlength="7" skinid="txt248">
                        </asp:textbox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:textbox id="txtpregistrationyear" runat="server" maxlength="4" skinid="txt248">
                        </asp:textbox>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:button id="btnSave" runat="server" text="Save" onclick="btnSave_Click" />
                <asp:button id="btnCancel" runat="server" text="Cancel" onclick="btnCancel_Click" /></div>
        </asp:view>
    </asp:multiview>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
