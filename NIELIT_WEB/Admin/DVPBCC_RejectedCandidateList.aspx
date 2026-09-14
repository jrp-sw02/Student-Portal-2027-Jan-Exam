<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="DVPBCC_RejectedCandidateList.aspx.cs"
     Inherits="Admin_DVPBCC_RejectedCandidateList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">

    <asp:Label ID="Label1" runat="server" Text="DVP BCC data  download For Rejected Candidates"></asp:Label>
    &nbsp;
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">

        <script type="text/javascript" language="javascript">

            function OpenWindow() {

                if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                    return false;


            }
        </script>

    <div>
        <br />
        <asp:Label ID="Lblerror" runat="server" EnableTheming="false" CssClass="error" Width="99%"
            Visible="false"></asp:Label>
        <table runat="server" align="center" border="0" cellpadding="3" class="sample3" width="100%" visible="true">
            <tr class="gdrow1">
                <td>
                    <asp:Label ID="lblExamName" runat="server" Text="Exam Name"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlExamName" runat="server" Height="22px" Width="260px"
                                AutoPostBack="true">
                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class="gdrow1">
                <td>
                    <asp:Label ID="lblInstitute" runat="server" Text="Institute"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlInstitute" runat="server" Height="22px" Width="260px"
                                AutoPostBack="true">
                                <asp:ListItem Value="0">--Select All--</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class="gdrow1">
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="btnShow" runat="server" Text="Show candidates" OnClick="btnShow_Click" onclientclick="return OpenWindow();" />
                    &nbsp;&nbsp;<asp:Button ID="btnDownload" runat="server" Text="Download candidates data" OnClick="btnDownload_Click" onclientclick="return OpenWindow();" />
                    &nbsp;&nbsp;<asp:button id="btnReset" runat="server" text="Reset" onclick="btnReset_Click" />
                </td>
            </tr>


            <tr class="gdrow1">
                <td>&nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr class="gdrow1">
                <td>&nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>


            <tr class="gdrow1">

                <td colspan="2">
                    <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvMain" runat="server" DataKeyNames="Roll_Number"
                                AutoGenerateColumns="False" Width="100%" EmptyDataText="No data available" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField   HeaderText="Application Number" DataField="Number"/>
                                        
                                    <asp:BoundField   HeaderText="Accreditation Number" DataField="Accreditation_Number">
                                        <HeaderStyle Width="1%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Exam_Centre_Name" HeaderText="Exam Centre Name" />
                                    <asp:BoundField DataField="Exam_Centre_Address" HeaderText="Exam Centre Address" />
                                    <asp:BoundField DataField="Roll_Number" HeaderText="Roll Number" />
                                    <asp:BoundField DataField="Name" HeaderText="Candidate Name" />
                                    <asp:BoundField DataField="Father_Name" HeaderText="Father Name" />
                                    <asp:BoundField DataField="Dob" HeaderText="Dob" />
                                    <asp:BoundField DataField="Date_of_Exam" HeaderText="Date of Exam" />
                                    <asp:BoundField DataField="Reporting_Time" HeaderText="Batch Number" />

                                    <asp:BoundField DataField="Reason" HeaderText="Rejection Reason" SortExpression="Reason"></asp:BoundField>
                                    <asp:BoundField DataField="DateOfReason" HeaderText="Date of Rejection" SortExpression="DateOfReason"></asp:BoundField>

                                </Columns> 
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </td>
            </tr>
            <tr class="gdrow1">
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr class="gdrow1">
                <td>&nbsp;</td>
                <td>
                    <div id="divpdf" class="text-center" style="font-size: small; color: darkslateblue" runat="server" visible="false">
                        
                            <asp:Label ID="lblExamDate" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label><br />
                            <asp:Label ID="lblExamTime" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label><br /><br />
                         
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>

