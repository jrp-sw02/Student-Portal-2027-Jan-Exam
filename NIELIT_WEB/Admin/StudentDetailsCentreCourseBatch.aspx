<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/main.master" CodeFile="StudentDetailsCentreCourseBatch.aspx.cs"
     Inherits="Admin_StudentDetailsCentreCourseBatch" Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Virtual Academy Student Details For LMS"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {
            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category Name"))
                return false;
            if (!isSelected("<%=ddlcourseName.ClientID %>", "Course Name"))
                return false;           

            if (!isSelected("<%=ddlBatchName.ClientID %>", "Batch Name"))
                return false;
            return true;
        }

        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp1, Sender, CheckBoxName)
        }
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp2, Sender, CheckBoxName)
        }
    </script>

    <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
        <tr>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td style="width: 33%;" valign="top">
                <asp:DropDownList ID="ddlcoursecategory" Width="100%" runat="server" SkinID="ddl250" AutoPostBack="true" OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlcourseName" Width="100%" runat="server" SkinID="ddl250" AutoPostBack="true" OnSelectedIndexChanged="ddlcourseName_SelectedIndexChanged1" >
                            <asp:ListItem>---Select One---</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="trisactive" runat="server">
            <td style="width: 33%;" valign="top">
                <asp:Label ID="LabelBatch" runat="server" SkinID="CaptionLabel" Text="Batch &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">                
            </td>
        </tr>

        <tr class="even">            
            <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlBatchName" Width="100%" runat="server" SkinID="ddl250" AutoPostBack="true">
                            <asp:ListItem>---Select One---</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>

    </table>
    <div style="text-align: right; margin-top: 10px">
         <asp:Button ID="btnSave" OnClientClick="return ValidateLogin();" runat="server" Text="Download" OnClick="btnSave_Click"/>&nbsp;
        <asp:Button ID="btnEmail" OnClientClick="return ValidateLogin();" runat="server" Text="Send Email" OnClick="btnEmail_Click" />&nbsp;
        <asp:Button ID="btnCancel" runat="server" Text="Reset" OnClick="btnCancel_Click" />
        
    </div>
    <asp:hiddenfield runat="server" id="hffilename" />
    <asp:hiddenfield runat="server" id="hffilepath" />

</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
