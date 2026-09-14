<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NSQFCandidatesResultView.aspx.cs" Inherits="NSQFCandidatesResultView"  MasterPageFile="~/MasterPages/MyInfo.master"  Debug="true"%>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" Text="NSQF Result View" runat="server"></asp:Label>
     <script language="javascript" type="text/javascript" src="../../Script/GlobalFunction.js"></script>
    <script language="javascript" type="text/javascript">
      
        function validateform() {
            if (!isBlank("txtRegNo", "Number required"))
                return false;
            if (!isNumber("txtRegNo"))
                return false;

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
<table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
                width="100%">
                <tr id="adminrow" runat="server" visible="false">
                    <td style="width: 25%;" valign="top">
                        <asp:Label id="lblregno" runat="server" visible="false" Text="<b>Registration Number :</b>" ></asp:Label>
                        </td>
                   <td style="width: 25%;" valign="top">
                       <asp:TextBox ID="txtRegNo" runat="server" visible="false"  onkeypress="checkNumber(this,3,0,event);" MaxLength ="8" ></asp:TextBox>
                        </td>
                    <td style="width: 40%;" valign="top">
                        <asp:Button ID="btnShowResult" runat="server" Text="Show Result" OnClientClick="return validateform();"  
                     OnClick="btnShowResult_Click" Visible="false" />
                        </td>
                    </tr>
                <tr>
                    <td style="width: 90%;" valign="top" colspan="3">
                        
                         <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                               
                                <asp:Label ID="lblnsqfResult" runat="server" ForeColor="Red" Font-Size="Small" Font-Italic="true" Width="60%"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                          <div id="divNavigation" runat="server">
                        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="Registration_no" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#"><HeaderStyle Width="2%" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>  
                                                             
                                <asp:HyperLinkField HeaderStyle-Width="12%"
                                     DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Registration_no" HeaderText="Registration number" SortExpression="Registration_no" Target="_self"><HeaderStyle Width="12%" /></asp:HyperLinkField>
                               
                                <asp:HyperLinkField HeaderStyle-Width="10%" 
                                     DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="NSQF_roll_no" HeaderText="Roll Number" SortExpression="NSQF_roll_no" Target="_self"><HeaderStyle Width="10%" /></asp:HyperLinkField>

                                <asp:HyperLinkField HeaderStyle-Width="23%"  DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Module_Name" HeaderText="Module Name" SortExpression="Module_Name"
                                    Target="_self"><HeaderStyle Width="23%" /></asp:HyperLinkField>                            
                             <asp:HyperLinkField HeaderStyle-Width="7%"  DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Result" HeaderText="Result" SortExpression="Result"
                                    Target="_self"><HeaderStyle Width="7%" /></asp:HyperLinkField>
                                 <asp:HyperLinkField HeaderStyle-Width="16%"  DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Exam_Cycle" HeaderText="Exam Cycle" SortExpression="Exam_Cycle"
                                    Target="_self"><HeaderStyle Width="16%" /></asp:HyperLinkField>
                                 <asp:HyperLinkField HeaderStyle-Width="18%"  DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Reslut_Declaration_Date" HeaderText="Result Declaration Date" SortExpression="Reslut_Declaration_Date"
                                    Target="_self"><HeaderStyle Width="18%" /></asp:HyperLinkField>
                                 <%--<asp:HyperLinkField HeaderStyle-Width="9%"  DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Result_upload_date" HeaderText="Result_upload_date" SortExpression="Result_upload_date"
                                    Target="_self"><HeaderStyle Width="9%" /></asp:HyperLinkField> --%>                             
                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" Visible="false"><HeaderTemplate><asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></HeaderTemplate><ItemTemplate><asp:CheckBox runat="server" ID="CheckBox1" SkinID="CheckAllInGridView" /></ItemTemplate><HeaderStyle Width="2%" /></asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                       
                
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
                    </td>
                    
                    
                </tr>
               
                
             
            </table>
             
            </asp:Content>


