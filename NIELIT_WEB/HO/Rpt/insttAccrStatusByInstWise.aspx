<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="insttAccrStatusByInstWise.aspx.cs" Inherits="insttAccrStatusByInstWise" Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 264px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Accreditation Status"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">


        function ValidateFormFields() {


            if (!isSelected("DdlAccState", " State"))
                return false;
            if (!isSelected("DdlAccCentre", " Centre of Accredited Institute"))
                return false;
        }



    </script>


    <table>
        <tr>
            <td align="left">&nbsp;</td>
        </tr>
        <tr id="t1" runat="server" visible="false">
            <td>
                <asp:Label ID="Label5" Width="100%" runat="server" Text="Course Category"></asp:Label>
            </td>
            <td class="auto-style1">
                <asp:DropDownList ID="ddlcategry" Width="100%" runat="server"
                    Enabled="false">
                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" Width="100%" runat="server" Text="Select State &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td class="auto-style1">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="DdlAccState" Width="275px" runat="server" OnSelectedIndexChanged="DdlAccState_SelectedIndexChanged"
                            AutoPostBack="True" Enabled="true" BackColor="Cornsilk">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>

                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" Width="100%" runat="server" Text="Select Centre Name of Accredited Institute &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td class="auto-style1">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="DdlAccCentre" Width="526px" runat="server"
                            Enabled="true" BackColor="HoneyDew">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="DdlAccState" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="t11" runat="server" visible="false">
            <td>
                <asp:Label ID="Label8" Width="100%" runat="server" Text="Accreditation No."></asp:Label>
            </td>
            <td class="auto-style1">
                <asp:TextBox ID="txtAccrNo" runat="server" ToolTip="Accreditation Number" MaxLength="12" BorderStyle="Solid"></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td></td>
            <td class="auto-style1">
                <asp:Button ID="btnShow" runat="server" Text="Check Status" OnClick="btnShow_Click" OnClientClick="return ValidateFormFields();" />
                &nbsp;&nbsp;&nbsp;
                                   &nbsp;&nbsp;&nbsp;
                                     <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div></td>

        </tr>
        <tr>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblName" runat="server" Visible="False" Font-Bold="True">Name</asp:Label></td>
            <td class="auto-style1">
                <asp:TextBox ID="txtName" runat="server" Visible="false" Enabled="False" Height="16px" TextMode="MultiLine" Width="277px"></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblAddress" runat="server" Visible="False" Font-Bold="True">Address</asp:Label></td>
            <td class="auto-style1">
                <asp:TextBox ID="txtAddress" runat="server" Visible="false" Enabled="False" Height="70px" TextMode="MultiLine" Width="273px"></asp:TextBox></td>
        </tr>
    </table>
    <br />

    <asp:UpdatePanel ID="uPnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                OnRowDataBound="gvMain_RowDataBound" Height="100%" AutoGenerateColumns="False" Width="100%" Visible="False" EmptyDataText="No record found" Caption="&lt;b&gt;&lt;center&gt;Accreditation Status&lt;/centre&gt;&lt;/b&gt;">
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="5%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:HyperLinkField HeaderStyle-Width="40%"
                        DataTextField="Name" HeaderText="Course Name" SortExpression="Name"
                        Target="_self">
                        <HeaderStyle Width="20%" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField HeaderStyle-Width="40%"
                        DataTextField="AccreditationNumber" HeaderText="Accreditation Number" SortExpression="Name"
                        Target="_self">
                        <HeaderStyle Width="20%" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField HeaderStyle-Width="40%"
                        DataTextField="EffectiveFromDate" DataTextFormatString="{0:dd-MMM-yyyy}" HeaderText="Effective From Date" SortExpression="EffectiveFromDate "
                        Target="_self">
                        <HeaderStyle Width="20%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField HeaderStyle-Width="40%"
                        DataTextField="EffectiveToDate" DataTextFormatString="{0:dd-MMM-yyyy}" HeaderText="Effective To Date" SortExpression="EffectiveToDate"
                        Target="_self">
                        <HeaderStyle Width="20%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:HyperLinkField>
                    <%-- <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Name" HeaderText="Course" SortExpression="Name" Target="_self"><HeaderStyle Width="20%" /><ItemStyle HorizontalAlign="Left" /></asp:HyperLinkField>--%>
                    <asp:HyperLinkField HeaderStyle-Width="40%"
                        DataTextField="SNAME" HeaderText="Status" SortExpression="SNAME" Target="_self">
                        <HeaderStyle Width="40%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField HeaderStyle-Width="40%"
                        DataTextField="WithdrawlDate" DataTextFormatString="{0:dd-MMM-yyyy}" HeaderText="WithdrawlDate" SortExpression="WithdrawlDate"
                        Target="_self">
                        <HeaderStyle Width="20%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:HyperLinkField>
                    <%-- <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Temp_Blocked" HeaderText="Blocked" SortExpression="Temp_Blocked" Target="_self"><HeaderStyle Width="40%" /><ItemStyle HorizontalAlign="Left" /></asp:HyperLinkField>--%>
                    <%--  <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Address" HeaderText="Address" SortExpression="Address" Target="_self" />--%>
                </Columns>
                <PagerSettings Visible="true" />
            </asp:GridView>
        </ContentTemplate>

    </asp:UpdatePanel>
    <div id="divNavigation" runat="server">
        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:HiddenField ID="hfActionID" runat="server" Value="" />
    <asp:HiddenField ID="hfAccreID" runat="server" />
    <asp:HiddenField ID="hfAccName" runat="server" />
    </ContentTemplate>
                </asp:UpdatePanel>
            
           
                    
       
</asp:Content>

