
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master"  AutoEventWireup="true" CodeFile="MISDetailReport.aspx.cs" Inherits="Admin_MISDetailReport" Debug="TRUE"%>

<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../UserControl/PagingBar.ascx" tagname="PagingBar" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
MIS Detailed Report 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
<script type="text/javascript" language="javascript">
    function isValidForm() {

        //if (!isSelected("<%=ddlCenter.ClientID %>", "Center"))
           // return false;
        var curdate = new Date().format("dd-MMM-yyyy");
        if (!isBlankDate("<%=txtDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
            return false;
        if (!isDate("<%=txtDateFrom.ClientID %>", "Invalid From Date", "dd-MMM-yyyy"))
            return false;
        if (!isBlankDate("<%=txtToDate.ClientID %>", "To Date", "dd-MMM-yyyy"))
            return false;
        if (!isDate("<%=txtToDate.ClientID %>", "Invalid To Date", "dd-MMM-yyyy"))
            return false;
        var PayFromDate = document.getElementById('<%=txtDateFrom.ClientID %>').value;
        
        var PayToDate = document.getElementById('<%=txtToDate.ClientID %>').value;
        
        if (!CompareDates(PayFromDate, PayToDate, "From date should be less than To Date", true))
            return false;
        //if (!isSelected("<%=ddlProject.ClientID %>", "Project Name"))
           // return false;
       
        return true;

    }
    </script>
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
        <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
             <td style="width: 33%;" valign="top">
                 <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" 
                     Text="NIELIT Center &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
             </td>
             <td style="width: 33%;" valign="top">
                 <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" 
                     Text="From Date&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
             </td>
             <td style="width: 33%;" valign="top">
                 <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" 
                     Text="To Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
             </td>
        </tr>
        <tr class="even">
             <td style="width: 33%;" valign="top">
                 <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                <asp:DropDownList ID="ddlCenter" runat="server"  AutoPostBack="true" SkinID="ddl250">
                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                </asp:DropDownList>
                </ContentTemplate>
                <Triggers>
                </Triggers>
                </asp:UpdatePanel>
            </td>
             <td style="width: 33%;" valign="top">
                <asp:TextBox ID="txtDateFrom" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                <img runat="server" id="imgFrom" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                    vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom"
                    Format="dd-MMM-yyyy" PopupButtonID="imgFrom">
                </asp:CalendarExtender>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:TextBox ID="txtToDate" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                <img runat="server" id="imgTo" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                    vertical-align: top;" />
                     <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                    Format="dd-MMM-yyyy" PopupButtonID="imgTo">
                </asp:CalendarExtender>
                    </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" 
                    Text="Project Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr class="even" >
            <td>
                <asp:DropDownList ID="ddlProject" runat="server" SkinID="ddl250"
                    Width="250px">
                    
                  
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        </table>
        <div style="text-align: right; margin-top: 10px">
            <%--<asp:Button ID="BtnShowDetail" runat="server" onclick="BtnShowDetail_Click"  OnClientClick="return CheckSelection();"
                Text="Show Detail" />--%>
              
        <asp:Button ID="btnView" runat="server" Text="Print Report" 
                OnClientClick="return isValidForm()" OnClick="btnView_Click" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" 
            onclick="btnReset_Click" /></div>
        <div id="MainReportDiv" runat="server"></div>
            <div id="Div_GridPnl" runat ="server" visible ="false">
             <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <%--<asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False"  OnRowDataBound="gvMain_RowDataBound" 
                        DataKeyNames="demandNoteID"
                        >
                            <Columns>
                                <asp:BoundField HeaderText="#" />
                                <asp:HyperLinkField 
                                    DataTextField="TransactionNumber" HeaderText="Transaction No." SortExpression="TransactionNumber" 
                                    DataNavigateUrlFields="demandNoteID,TranID,PayModeId,AppTypeId" DataNavigateUrlFormatString="~/HO/TransactionDetails.aspx?demandNoteID={0}&TranID={1}&PayModeId={2}&AppTypeId={3}"
                                    Target="_blank" />
                                <asp:BoundField HeaderText="Transaction Date" DataField="TransactionDate" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" >
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="DemandNoteNo./Date">                                  
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("DemandNoteDetail") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Application No" DataField="ApplicationNumber" >
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="TransactionAmt" DataField="TransactionAmount" DataFormatString="{0:#0.00}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                  <asp:TemplateField  HeaderText="">
                                        <HeaderStyle  />
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chk"  SkinID="CheckAllInGridView" />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="CheckBox1"  SkinID="CheckAllInGridView" />
                                        </HeaderTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>--%>
                        <%--<asp:HiddenField ID="hfActionID" runat="server" Value="" />--%>
                          </ContentTemplate>
                </asp:UpdatePanel>
                        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <%--<uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />--%>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <%--<input id="Button1" runat="server" class="button right" onclick="return isValidForm()" onserverclick="Button1_Click" type="button" value="Update" />--%>
            
            </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>


