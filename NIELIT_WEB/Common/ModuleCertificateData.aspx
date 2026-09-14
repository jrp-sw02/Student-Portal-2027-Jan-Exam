<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="ModuleCertificateData.aspx.cs" Inherits="ModuleCertificateData" %>

<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    O/A/B/C Module Certificate Data Download
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        var dtga = "<%= gbapplicant.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtga, Sender, CheckBoxName)
        }
        function SelectheaderCheckboxes(headerchk) {
            var gvcheck = document.getElementById("<%=gbapplicant.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }

        function Selectchildcheckboxes(header) {
            var ck = header;
            var count = 0;
            var gvcheck = document.getElementById("<%=gbapplicant.ClientID %>");
            var headerchk = document.getElementById(header);
            var rowcount = gvcheck.rows.length;
            //By using this for loop we will count how many checkboxes has checked
            for (i = 1; i < gvcheck.rows.length; i++) {
                var inputs = gvcheck.rows[i].getElementsByTagName('input');
                if (inputs[0].checked) {
                    count++;
                }
            }
            //Condition to check all the checkboxes selected or not
            if (count == rowcount - 1) {
                headerchk.checked = true;
            }
            else {
                headerchk.checked = false;
            }
        }

    </script>
    <script type="text/javascript" language="javascript">
        function Validate() {

            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Level"))
                return false;
            if (!isSelected("<%=ddlBatchNumber.ClientID %>", "Batch Number"))
                return false;
            if (!isSelected("<%=ddlDataFile.ClientID %>", "Data File"))
                return false;
            return true;
        }
    </script>
 
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="width: 33%;">
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;">
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Batch Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;">
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Data File &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 33%;">
                <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250"
                    AutoPostBack="True" OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 33%;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlBatchNumber" runat="server" Height="22px" SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td style="width: 33%;">
                <asp:DropDownList ID="ddlDataFile" runat="server" Height="22px" SkinID="ddl250">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                    <asp:ListItem Value="1">Candidate Module Data</asp:ListItem>
                    <asp:ListItem Value="2">Candidate Personal Data</asp:ListItem>
                    <asp:ListItem Value="3">Candidate Photo</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnDownload" runat="server" Text="Download Data" OnClick="btnDownload_Click"
            OnClientClick="return Validate();" />
    </div>
    <br />
    <br />
    <div>
        <br />
        <br />
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid1" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="1" width="100%" id="tbl1" runat="server">
                    <tr>
                        <td>
                            <asp:Label ID="LblError" runat="server" CssClass="error" EnableTheming="false" Visible="false"
                                Width="99%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="bottom">
                            <asp:Button ID="btnProcess" Visible="false" runat="server" Text="Assign Batch" OnClientClick="return Validate_Checkbox('Are you sure you want to process the selected applications!')"
                                OnClick="btnProcess_Click" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gbapplicant" runat="server" AutoGenerateColumns="False" DataKeyNames="CourseId" OnRowDataBound="gbapplicant_RowDataBound"
                    Width="100%" HeaderStyle-Font-Size="11px">
                    <Columns>
                        <asp:BoundField HeaderText="#">
                            <HeaderStyle Width="5%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="10%" DataField="CourseName" HeaderText="Course Name">
                            <HeaderStyle Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="10%" DataField="RequestCount" HeaderText="New Request">
                            <HeaderStyle Width="10%" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-Width="5%" HeaderText="">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkheader" runat="server" onclick="javascript:SelectheaderCheckboxes(this)" /></HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkchild" runat="server" onclick="javascript:Selectchildcheckboxes(chkheader)" /></ItemTemplate>
                            <HeaderStyle Width="5%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                     <PagerSettings Visible="False" />
                    <EmptyDataTemplate>
                        No New request Found.</EmptyDataTemplate>
                    <EmptyDataRowStyle  CssClass="error" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel><asp:HiddenField ID="hfActionID" runat="server" Value="" />
        <div id="divNavigation" runat="server">
            <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
