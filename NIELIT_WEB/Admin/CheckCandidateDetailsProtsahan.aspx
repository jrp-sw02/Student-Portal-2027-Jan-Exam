<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="CheckCandidateDetailsProtsahan.aspx.cs" Inherits="Admin_CheckCandidateDetailsProtsahan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/CourseApplication.ascx" TagName="CourseApplication"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Check Candidate Details for Protsahan"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc5:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var globalvar = "0";
        function ValidateFormFields() {
            if (!isBlank("<%=TxtRegNo.ClientID  %>", "Registration number"))
                return false;
            if (!isSelected("<%=ddlLevel.ClientID  %>", "Level"))
                return false;
            if (!isBlank("<%=txtRemarks.ClientID  %>", "Remarks"))
                return false;
            var RegNo = document.getElementById('<%=TxtRegNo.ClientID %>').value;
            if (trim(RegNo, "") == "") {
                alert("Please Enter Registration number!");
                return false;
            }
            
        }

        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
        }

        
    </script>
    
    <div id="divfilter" runat="server">
        <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
            <%--<uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />--%>
            <tr>
                <td width="33%" >
                 <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Registration Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                </td>
                <td width="33%" >
                 <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Level &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                </td>

            </tr>
            <tr class="even">
                <td width="33%" >
            <asp:TextBox ID="TxtRegNo" runat="server" backcolor="LemonChiffon" onkeypress="checkNumber(this,11,0,event);" MaxLength="11" onpaste="return false;" SkinID="txt248" Width="200px" ></asp:TextBox>
                     <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlLevel" runat="server" Height="22px" SkinID="ddl250" backcolor="Lavender">
                                 <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="O LEVEL"></asp:ListItem>
                            <asp:ListItem Value="2" Text="A LEVEL"></asp:ListItem>
                            <asp:ListItem Value="3" Text="B LEVEL"></asp:ListItem>
                            <asp:ListItem Value="4" Text="C LEVEL"></asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                </td>
            </tr>
        </table>
        <div style="text-align: right; margin-top: 10px">
            <asp:Button ID="BtnView" runat="server" Text="View" OnClick="BtnView_Click" OnClientClick="return ValidateFormFields();" />
            <asp:Button ID="BtnReset" runat="server" Text="Reset" OnClick="BtnReset_Click" /></div>
    </div>
    
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
              <%--deep--%>
             
            <div id="div1" runat="server">               
                <asp:Label ID="lblcand"  runat="server" ForeColor="blue" Font-Bold="true"
                      Text="  Candidates Details " Visible="false"   ></asp:Label>

                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>                       
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblErrMsg1" Visible="false"
                runat="server"></asp:Label>
                    <br />
                        <asp:GridView ID="gvMainCandidates" runat="server" DataKeyNames="ID" OnSorting="gvMainCandidates_Sorting"
                            OnRowDataBound="gvMainCandidates_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>                             
                                 <asp:BoundField HeaderStyle-Width="2%" DataField="SLNO" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                               <asp:BoundField DataField="Regno" HeaderText="Registration" ItemStyle-Width="100" />                                 
                               <asp:BoundField DataField="CourseName" HeaderText="Course" ItemStyle-Width="80" />
                               <asp:BoundField DataField="StudentName" HeaderText="Student Name" ItemStyle-Width="100" />                               
                               <asp:BoundField DataField="canddob" HeaderText="DoB" ItemStyle-Width="100" DataFormatString="{0:dd-MM-yyyy}" />
                               <asp:BoundField DataField="GenderName" HeaderText="Gender" ItemStyle-Width="70" />
                               <asp:BoundField DataField="Castname" HeaderText="Category" ItemStyle-Width="100" />
                               <asp:BoundField DataField="phName" HeaderText="IsHandicapped" ItemStyle-Width="90" />
                               <asp:BoundField DataField="FatherName" HeaderText="Father Name" ItemStyle-Width="100" />
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID1" runat="server" Value="" />
                        <asp:HiddenField ID="hfcode1" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divNavigation1" runat="server" visible="false">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation1" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar2" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <%--deep--%>
            <div id="divGrid" runat="server">
                <br /><br />
                <asp:Label ID="lblRegh"  runat="server" ForeColor="blue" Font-Bold="true"
                      Text="  Registration Details " Visible="false"   ></asp:Label>
                <br /><br />
                <asp:Label Width="99%" Style="background-color: #EACFCE; padding-top: 4px; padding-bottom: 4px;
        padding-left: 4px; color: Red; border: 1px solid maroon; font-size: 11pt; font-variant: normal;"
        ID="Lblerror" Visible="false" runat="server" Text=""></asp:Label>
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvMain_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                               
                                <asp:HyperLinkField HeaderStyle-Width="13%" HeaderText="Registration No" DataNavigateUrlFields="courseID,Regno,ID"
                                    DataNavigateUrlFormatString="?courseID={0}&Regno={1}&Key={2}"
                                    DataTextField="Regno" SortExpression="Regno" Target="_self">
                                    <HeaderStyle Width="13%" />
                                </asp:HyperLinkField>
                                 <asp:HyperLinkField HeaderText="Course" HeaderStyle-Width="10%" DataTextField="CourseName"
                                    SortExpression="CourseName" DataNavigateUrlFields="courseID,Regno,ID"
                                    DataNavigateUrlFormatString="?courseID={0}&Regno={1}&Key={2}">
                                    <HeaderStyle Width="10%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Student Name" HeaderStyle-Width="27%" DataTextField="StudentName"
                                    DataNavigateUrlFields="courseID,Regno,ID" DataNavigateUrlFormatString="?courseID={0}&Regno={1}&Key={2}"
                                    SortExpression="StudentName">
                                    <HeaderStyle Width="27%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="DoB" HeaderStyle-Width="25%" DataTextField="canddob"
                                    DataNavigateUrlFields="courseID,Regno,ID" DataNavigateUrlFormatString="?courseID={0}&Regno={1}&Key={2}"
                                    SortExpression="date" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="25%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Gender" HeaderStyle-Width="14%" DataTextField="GenderName"
                                    DataNavigateUrlFields="courseID,Regno,ID" DataNavigateUrlFormatString="?courseID={0}&Regno={1}&Key={2}"
                                    SortExpression="GenderName">
                                    <HeaderStyle Width="14%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Category" HeaderStyle-Width="30%" DataTextField="Castname"
                                    DataNavigateUrlFields="courseID,Regno,ID" DataNavigateUrlFormatString="?courseID={0}&Regno={1}&Key={2}"
                                    SortExpression="Castname">
                                    <HeaderStyle Width="30%" />
                                </asp:HyperLinkField>
                                 <asp:HyperLinkField HeaderText="IsHandicapped" HeaderStyle-Width="30%" DataTextField="phName"
                                    DataNavigateUrlFields="courseID,Regno,ID" DataNavigateUrlFormatString="?courseID={0}&Regno={1}&Key={2}"
                                    SortExpression="phName">
                                    <HeaderStyle Width="30%" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField HeaderText="Father Name" HeaderStyle-Width="30%" DataTextField="FatherName"
                                    DataNavigateUrlFields="courseID,Regno,ID" DataNavigateUrlFormatString="?courseID={0}&Regno={1}&Key={2}"
                                    SortExpression="FatherName">
                                    <HeaderStyle Width="30%" />
                                </asp:HyperLinkField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divNavigation" runat="server" visible="false">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server">           
              <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                   <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Candidate Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                         
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Registration Number&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Level&lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                  <tr class="even">
                    <td style="width: 33%;" valign="top">                      
                       <asp:TextBox ID="txtCandNamesV" runat="server"  SkinID="txt248" Width="200px" Enabled="false" ></asp:TextBox>  
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="TxtRegV" runat="server"  SkinID="txt248" Width="200px" Enabled="false" ></asp:TextBox>
                    </td>
                    <td valign="top" style="width: 33%;">
                       <asp:TextBox ID="txtLevelV" runat="server"  SkinID="txt248" Width="200px" Enabled="false" ></asp:TextBox>
                    </td>
                </tr>     
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblCastCategory" runat="server" SkinID="CaptionLabel" Text="Cast Category &lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                         
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblHanidcapped" runat="server" SkinID="CaptionLabel" Text="Handicapped&lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="lblGender" runat="server" SkinID="CaptionLabel" Text="Gender&lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">                      
                        <asp:DropDownList ID="ddlCastCategory" runat="server"  SkinID="ddl250" backcolor="Lavender">
                                </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlPHCategory" runat="server"  SkinID="ddl250" backcolor="Lavender">
                             <asp:ListItem Value="-1" Text="--Select One--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="YES"></asp:ListItem>
                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                </asp:DropDownList>
                    </td>
                    <td valign="top" style="width: 33%;">
                       <asp:DropDownList ID="ddlGender" runat="server"  SkinID="ddl250" backcolor="Lavender">
                                </asp:DropDownList>
                    </td>
                </tr> 
                   <tr>
                    <td style="width: 33%;" valign="top" colspan="3">
                        <asp:Label ID="lblremarkss" runat="server" SkinID="CaptionLabel" Text="Remarks &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>                        
                    
                    </td>
                       <tr class="even">
                    <td style="width: 33%;" valign="top" colspan="3">                                           
                     <asp:TextBox ID="txtRemarks" runat="server"  SkinID="txt756" Width="700px" Enabled="true" ></asp:TextBox>
                    </td>
                </tr>              
            </table>
            
                <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div> 
             <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="CsId" runat="server" Value="0" />                   
                     <asp:HiddenField ID="REGID" runat="server"  Value="0"/>                    
                </ContentTemplate>
            </asp:UpdatePanel>          
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
