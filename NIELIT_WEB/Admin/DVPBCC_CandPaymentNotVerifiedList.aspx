<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="DVPBCC_CandPaymentNotVerifiedList.aspx.cs" Inherits="Admin_DVPBCC_CandPaymentNotVerifiedList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">

    <asp:Label ID="Label1" runat="server" Text="DVP BCC data  download"></asp:Label>
    &nbsp;
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">

    <div>
        <asp:Label ID="Lblerror" runat="server" EnableTheming="false" CssClass="error" Width="99%"
            Visible="false"></asp:Label>
        <table runat="server" align="center" border="0" cellpadding="3" class="sample3" width="100%" visible="true">
       
             <tr class="gdrow1">
                <td><asp:Label ID="lblExamMonth" runat="server" Text="Exam Month"></asp:Label></td>
                <td><asp:Label ID="lblExamYear" runat="server" Text="Exam Year"></asp:Label>
                     </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlMonth" runat="server"  Height="22px" Width="260px" >
                                <asp:ListItem Value="0"> -- Select One --</asp:ListItem>
                                <asp:ListItem Value="1"> January </asp:ListItem>
                                <asp:ListItem Value="2"> February</asp:ListItem>
                                <asp:ListItem Value="3"> March</asp:ListItem>
                                <asp:ListItem Value="4"> April</asp:ListItem>
                                <asp:ListItem Value="5"> May</asp:ListItem>
                                <asp:ListItem Value="6"> June</asp:ListItem>
                                <asp:ListItem Value="7"> July</asp:ListItem>
                                <asp:ListItem Value="8"> August</asp:ListItem>
                                <asp:ListItem Value="9"> September</asp:ListItem>
                                <asp:ListItem Value="10"> October</asp:ListItem>
                                <asp:ListItem Value="11"> November</asp:ListItem>
                                <asp:ListItem Value="12"> December</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack=" true" Height="22px" Width="260px">
                                <asp:ListItem Value="0"> -- Select One --</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel></td>
            </tr>
            <tr class="gdrow1">
                
                <td colspan="2" align="center">
                    <asp:Button ID="btnShow" runat="server" Text="Show candidates" OnClick="btnShow_Click" />
                    &nbsp;&nbsp;&nbsp;<asp:Button ID="btnDownload" runat="server" Text="Download candidates data" OnClick="btnDownload_Click" />
&nbsp;&nbsp;<asp:Button ID="btnDownload1" runat="server" Text="Download candidates data(Excel)" OnClick="btnDownload1_Click"  />
                </td>
            </tr>
            <tr >
                <td>&nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
           
            <tr>

                <td colspan="2">
                    <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>

                            <asp:Label ID="lblReportHeading" runat="server" Font-Bold="True" ForeColor="#3399FF" ></asp:Label>
                            <asp:GridView ID="gvMain" runat="server" DataKeyNames="Roll_Number"
                                AutoGenerateColumns="False" Width="100%" EmptyDataText="No data available" CellPadding="4" ForeColor="#333333" GridLines="None" allowpaging=true pagesize=100000>
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField   HeaderText="SrNo." DataField="SrNo">
                                        <HeaderStyle Width="1%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Roll_Number" HeaderText="Roll Number" />
                                    <asp:BoundField DataField="Name" HeaderText="Candidate Name" />
                                    <asp:BoundField DataField="Father_Name" HeaderText="Father Name" />
                                    <asp:BoundField DataField="Dob" HeaderText="Dob" />
                                    <asp:BoundField DataField="Demand_Note_ID" HeaderText="DemandNote No." />
                                    <asp:BoundField DataField="Exam_Centre_Name" HeaderText="Exam Centre" /> 
                                    <asp:BoundField DataField="Date_of_Exam" HeaderText="Date of Exam" />
                                   

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
            
            <tr >
                <td>&nbsp;</td>
                <td>
                    <div id="divpdf" class="text-center" style="font-size: small; color: darkslateblue" runat="server" visible="false">
                        
                            <asp:Label ID="lblExamDate" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label><br />
                            <asp:Label ID="lblExamTime" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label><br />
                         
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

