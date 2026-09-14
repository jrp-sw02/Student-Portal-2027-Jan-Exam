<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="ExamCenterList.aspx.cs" Inherits="Common_ExamCenterList" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Exam Centre List Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">
        function OpenWindow() {
            //Course Category
            var CourseCategoryID;
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;
            else
                CourseCategoryID = document.getElementById('<%=ddlCourseCategry.ClientID %>').value;
            //State
            var StateID;
            //            if (!isSelected("<%=ddlState.ClientID %>", "State"))
            //                return false;
            //            else
            StateID = document.getElementById('<%=ddlState.ClientID %>').value;

            //List Mode
            var ListModeID;
              ListModeID = document.getElementById('<%=ddlListMode.ClientID %>').value;
            //View report
              window.open("../HO/Rpt/ExamCentreListReport.aspx?StateId=" + StateID + "&CourseCategoryId=" + CourseCategoryID + "&ListModeId=" + ListModeID);
        }
    </script>
    <asp:Label ID="lblerror" runat="server"></asp:Label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                    Width="100%"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="State"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="List Mode"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td style="width: 33%;" valign="top">
                <asp:DropDownList ID="ddlCourseCategry" runat="server" Height="22px" SkinID="ddl250">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:DropDownList ID="ddlState" runat="server" Height="22px" SkinID="ddl250" 
                    AutoPostBack="True" onselectedindexchanged="ddlState_SelectedIndexChanged">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlListMode" runat="server" Height="22px" SkinID="ddl250" 
                            Enabled="False">
                            <asp:ListItem Value="0">Summary</asp:ListItem>
                            <asp:ListItem Value="1">Detailed</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlState" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" 
            OnClientClick="return OpenWindow();"  />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
