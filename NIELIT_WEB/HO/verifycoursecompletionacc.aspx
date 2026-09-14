<%@ Page Title="NIELIT" Language="C#" MasterPageFile="~/MasterPages/main.master"
    AutoEventWireup="true" CodeFile="verifycoursecompletionacc.aspx.cs" Inherits="verifycoursecompletionacc" %>

<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Verification of ACC Course Completion
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function Validate() {
            if (!ischecked("<%=chkdisclamier.ClientID %>", "Disclaimer"))
                return false;
            return true;
        }
    </script>
    <div>
        <asp:Label ID="lblerror" runat="server"></asp:Label>
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid1" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gbapplicant" runat="server" AutoGenerateColumns="False" DataKeyNames="RegistrationNo, ExamId"
                    OnRowDataBound="gbapplicant_RowDataBound" PageSize="25" Width="100%" HeaderStyle-Font-Size="12px"
                    AllowSorting="True">
                    <Columns>
                        <asp:BoundField HeaderText="#">
                            <HeaderStyle Width="2%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="5%" DataField="RegistrationNo" HeaderText="Regn Number">
                            <HeaderStyle Width="5%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="5%" DataField="RegistrationDate" HeaderText="Regn Date">
                            <HeaderStyle Width="5%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="10%" DataField="Name" HeaderText="Name">
                            <HeaderStyle Width="10%" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-Width="9%" HeaderText="Start Date">
                            <HeaderTemplate>
                                Start Date</HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox MaxLength="11" ID="txtDoStart" runat="server" SkinID="txtDate" Width="99px">
                                </asp:TextBox>
                                <img id="imgjoin1" runat="server" src="../images/calendaricon.jpg" style="width: 20px;
                                    height: 22px; vertical-align: top;" />
                                <asp:CalendarExtender ID="ceDOjoin1" TargetControlID="txtDoStart" PopupPosition="BottomLeft"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgjoin1" runat="server">
                                </asp:CalendarExtender>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="9%" HeaderText="End Date">
                            <HeaderTemplate>
                                End Date</HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox MaxLength="11" ID="txtDoEnd" runat="server" SkinID="txtDate" Width="99px">
                                </asp:TextBox>
                                <img id="imgjoin2" runat="server" src="../images/calendaricon.jpg" style="width: 20px;
                                    height: 22px; vertical-align: top;" />
                                <asp:CalendarExtender ID="ceDOjoin2" TargetControlID="txtDoEnd" PopupPosition="BottomLeft"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgjoin2" runat="server">
                                </asp:CalendarExtender>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        No New Record Found.</EmptyDataTemplate>
                    <EmptyDataRowStyle CssClass="error" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
      <%--  <div id="divNavigation" runat="server">
            <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>--%>
    </div>
    <div>
        <br />
        <table cellpadding="0" cellspacing="1" width="100%" id="tbl1" runat="server">
            <tr>
                <td>
                    <b>Declaration</b>
                </td>
            </tr>
            <tr>
                <td>
                    <p style="text-align: justify;">
                        <asp:CheckBox ID="chkdisclamier" runat="server" Text="<font color='RED'>*</font>" />
                        I undertake that the above candidate(s) has successfully completed the training
                        in 'Awareness in Computer Concepts (ACC)'. He /She has the requisite attendance
                        and recommended for issue of ACC certificate.</p>
                </td>
            </tr>
            <tr>
                <td align="center" valign="bottom">
                    <br />
                    <asp:Button ID="btnProcess" Visible="true" runat="server" Text="Submit" OnClientClick="return Validate();"
                        OnClick="btnProcess_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
