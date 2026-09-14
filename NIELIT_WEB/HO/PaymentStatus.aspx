<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentStatus.aspx.cs" Inherits="HO_PaymentStatus" MasterPageFile="~/MasterPages/MyInfo.master"  Debug="false"%>


<%@ Register Src="../UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc1" %>
<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Payment Status"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc2:BreadCrumb ID="BreadCrumb2" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">
        function showForm(url) {
            window.open(url, "AppForm", "width=980,height=450,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes");
            return false;
        }
        if (window.top.location.href.indexOf('FrmdashBoard.aspx') >= 0)
            window.top.location.href = 'MainPage.aspx'
    </script>
    <div id="divInstitute" runat="server">
        <div class="summary_block" id="Coursediv" runat="server">
            <span>Course Registration Application Payment Status </span>
            <br />
            <asp:Label ID="lblCourseMsg" class="error" EnableTheming="False" runat="server" ForeColor="#FF3300"
                Visible="False"></asp:Label>
            <br />
            <asp:GridView ID="gvCourse" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                Width="100%" OnRowDataBound="gvCourse_RowDataBound" OnSorting="gvCourse_Sorting">
                <Columns>
                    <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                        <HeaderStyle Width="2%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Name" SortExpression="Name" HeaderStyle-Width="20%" HeaderText="Payment Mode">
                        <HeaderStyle Width="20%" />
                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Pending" SortExpression="Pending" HeaderStyle-Width="15%"
                        HeaderText="Pending">
                        <HeaderStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Paidnotverified" SortExpression="Paidnotverified" HeaderStyle-Width="30%"
                        HeaderText="Paid But Not Verified">
                        <HeaderStyle Width="30%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Paid" SortExpression="Paid" HeaderStyle-Width="15%" HeaderText="Paid">
                    </asp:BoundField>
                </Columns>
                <PagerSettings Visible="False" />
            </asp:GridView>
        </div>
        <div class="summary_block" style="width: 100%;" id="divcertificate" runat="server">
            <span>Certificate Exam Application Payment Status </span>
            <br />
            <asp:Label ID="lblCertificateMsg" class="error" EnableTheming="False" runat="server"
                ForeColor="#FF3300" Visible="False"></asp:Label>
            <asp:GridView ID="gvCertificate" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                Width="100%" OnRowDataBound="gvCertificate_RowDataBound" OnSorting="gvCertificate_Sorting">
                <Columns>
                    <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                        <HeaderStyle Width="2%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Name" SortExpression="Name" HeaderStyle-Width="20%" HeaderText="Payment Mode">
                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Pending" SortExpression="Pending" HeaderStyle-Width="15%"
                        HeaderText="Pending">
                    </asp:BoundField>
                    <asp:BoundField DataField="Paidnotverified" SortExpression="Paidnotverified" HeaderStyle-Width="30%"
                        HeaderText="Paid But Not Verified">
                    </asp:BoundField>
                    <asp:BoundField DataField="Paid" SortExpression="Paid" HeaderStyle-Width="15%" HeaderText="Paid">
                    </asp:BoundField>
                </Columns>
                <PagerSettings Visible="False" />
            </asp:GridView>
        </div>
        <div class="summary_block" id="CourseExamdiv" runat="server">
            <span>Course Exam Application Payment Status </span>
            <asp:Label ID="lblCourseExam" class="error" EnableTheming="False" runat="server"
                ForeColor="#FF3300" Visible="False"></asp:Label>
            <br />
            <asp:GridView ID="gvCourseExam" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                Width="100%" OnSorting="gvCourseExam_Sorting" OnRowDataBound="gvCourseExam_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                        <HeaderStyle Width="2%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Name" SortExpression="Name" HeaderStyle-Width="20%" HeaderText="Payment Mode">
                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Pending" SortExpression="Pending" HeaderStyle-Width="15%"
                        HeaderText="Pending">
                    </asp:BoundField>
                    <asp:BoundField DataField="Paidnotverified" SortExpression="Paidnotverified" HeaderStyle-Width="30%"
                        HeaderText="Paid But Not Verified">
                    </asp:BoundField>
                    <asp:BoundField DataField="Paid" SortExpression="Paid" HeaderStyle-Width="15%" HeaderText="Paid">
                    </asp:BoundField>
                </Columns>
                <PagerSettings Visible="False" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
