<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="FrmViewDetail.aspx.cs" Inherits="FrmViewDetail" Debug="true"%>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc3" %>
<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc1" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
    .modalPopup 
    { 
     border: 3px solid #31597C; 
     background-color: #E6F0F0; 
     padding-top:0px; 
     padding-right: 0px; 
     width: 330px; 
     height:255px; 
     top:-40px; 
     left:90px; 
     position:relative;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Course Details"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
   
     <%--<div id="div1" class="summary_block" runat="server">
        <ul>
            <li><a href="#">Your Application For Course Has Reveived By NILET Center.Please Keep In Touch With The Site For Further Information.</a></li>
            <li><a href="#">Your O Level Result Will Be Declared on 01/01/2013.Please Keep In Touch With The Site For Further Information.</a> </li>
            <li><a href="#">New Courses Will Be Started From July 2012.Please Keep In Touch With The Site For Further Information.</a></li>
        </ul>
    </div>--%>
  

    <div id="div_O" class="summary_block" runat="server">
        <span id="summeryHeading" runat="server" >Modules Summary As Per </span>
       <%-- <div id="notnsqfisummery" style="text-align:justify" runat="server">--%>
        <table align="center" width="100%" cellpadding="3" cellspacing="1">
            <tr>
                <td width="40%">
                    <b>Module Status</b>
                </td>
                <td align="center" width="30%" id="td1">
                    <b>Theory(Comp. + Elect. + Bridge)</b>
                </td>
                <td align="center" width="15%" id="td2">
                    <b>Practical</b>
                </td>
                <td align="center" width="15%" id="td3">
                    <b>Project</b>
                </td>
            </tr>
            <tr>
                <td>
                     Total number of modules to be passed
                </td>
                <td align="center" id="tdTotalTheoryModules" runat="server">
                </td>
                <%--<td align="center" id="tdTotalElectiveModules" runat="server">
                </td>--%>
                <td align="center" id="tdTotalPracticalModules" runat="server"> </td>
                <td align="center" id="tdTotalProjectModules" runat="server"> </td>
            </tr>
            <tr>
                <td>
                    Total number of modules attempted till date
                </td>
                <td align="center" id="tdAttemptedTheoryModules" runat="server">
                </td>
                <%--<td align="center" id="tdAttemptedElectiveModules" runat="server">
                </td>--%>
                <td align="center" id="tdAttemptedPracticalModules" runat="server">
                </td>
                <td align="center" id="tdAttemptedProjectModules" runat="server">
                </td>
            </tr>
            <tr>
                <td >
                    Total number of modules passed till date
                </td>
                <td align="center" id="tdPassedTheoryModules" runat="server">
                </td>
                <%--<td align="center" id="tdPassedElectiveModules" runat="server">
                </td>--%>
                <td align="center" id="tdPassedPracticalModules" runat="server">
                </td>
                <td align="center" id="tdPassedProjectModules" runat="server">
                </td>
            </tr>
         <tr id="exemptdetlswindow" runat="server" visible="false">
               <td >
                    Total number of modules exempted till date
                </td>
                <td align="center" id="tdtotalexemptthory" runat="server">
                </td>
                <%--<td align="center" id="tdRemainingElectiveModules" runat="server">
                </td>--%>
                <td align="center" id="tdtotalexemptpract" runat="server">
                </td>
                <td align="center" id="tdtotalexemptproject" runat="server">
                </td>
            </tr>
             <tr id="remaingdetlswindow" runat="server" visible="false">
               <td >
                    Total number of remaining modules to be passed
                </td>
                <td align="center" id="tdtotalremaingthory" runat="server">
                </td>
                <%--<td align="center" id="tdRemainingElectiveModules" runat="server">
                </td>--%>
                <td align="center" id="tdtotalremaingpract" runat="server">
                </td>
                <td align="center" id="tdtotalremaingproject" runat="server">
                </td>
            </tr>
            <tr>
                <td>
                    Exams attempted till date
                </td>
                <td colspan="3" id="tdAttemptedExams" runat="server">
                </td>
            </tr>
			 <tr>
                <td>
                    Projects attempted till date
                </td>
                <td colspan="3" id="tdformstatus" runat="server">

                </td>
            </tr>
			
        </table>
       <%--</div>--%>
     <%-- <div id="nsqfisummery" runat="server" style="text-align:left;">
          <table align="center" width="100%" cellpadding="2" cellspacing="1">
               <tr>
                <td>
                    Exams attempted till date
                </td>
                <td colspan="1" id="NsqftdAttemptedExams" runat="server">
                </td>
            </tr>
			 <tr>
                <td>
                    Projects attempted till date
                </td>
                <td colspan="1" id="Nsqftdformstatus" runat="server">

                </td>
            </tr>
          </table>
      </div>--%>
    </div>

    <div style="background-color:#c7dded;width:100%;padding:10px 10px 4px 10px; color:#666666; margin-top:8px; min-height:20px;">
    <span style="font:normal 18px arial; color:#003366;">Module Status (Passed/Exempted)
        <asp:ImageButton ID="ImgBtnPopupFee" runat="server" ImageUrl="~/images/DisablePopup.PNG"
            Enabled="False" Style="float: right; margin-right: 15px;" />
        <asp:Label ID="lblFeeDetail" runat="server" Font-Size="Small" Style="text-align:right; float:right;"
            Text="View Grade Legends &nbsp;"></asp:Label>
     </span>
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" DropShadow="true"
            CancelControlID="ImgCancle1" TargetControlID="ImgBtnPopupFee" PopupControlID="InfoDiv">
        </asp:ModalPopupExtender>
        <div id="InfoDiv" runat="server" class="modalPopup">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="5%">
                        &nbsp;
                    </td>
                    <td width="90%">
                    </td>
                    <td width="5%">
                        <asp:ImageButton ID="ImgCancle1" runat="server" ImageUrl="~/images/cancel.gif" Style="float: right;"
                            ToolTip="click to close" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                    </td>
                    <td colspan="2" style="color:Navy; font-weight:bold;" >
                       Grade Legends
                    </td>
                     
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                    </td>
                    <td align="center" >
                        <div class="box" id="divreport" runat="server"></div>
                    </td>
                    <td width="5%">
                        &nbsp;
                    </td>
                </tr>
              </table>                           
        </div>
    </div>
        
   
                        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvMainNSQResultView" runat="server" DataKeyNames="Registration_no"
                            OnRowDataBound="gvMainNSQResultView_RowDataBound" AutoGenerateColumns="False" Width="100%">
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
                                                          
                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" Visible="false"><HeaderTemplate><asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></HeaderTemplate><ItemTemplate><asp:CheckBox runat="server" ID="CheckBox1" SkinID="CheckAllInGridView" /></ItemTemplate><HeaderStyle Width="2%" /></asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                       
                
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
           
   
    <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
        Width="100%" OnRowDataBound="gvMain_RowDataBound" AllowPaging="false" PageSize="60">
        <Columns>
            <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                <HeaderStyle Width="2%" />
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:HyperLinkField HeaderStyle-Width="13%" HeaderText="Code" DataTextField="Code"
                Target="_self" DataNavigateUrlFields="CourseID,ID"  DataNavigateUrlFormatString="FrmModuleHistory.aspx?CourseID={0}&ModuleID={1}">
                <HeaderStyle Width="10%" />
            </asp:HyperLinkField>
            <asp:HyperLinkField HeaderStyle-Width="33%" HeaderText="Module Name" DataTextField="name"
                Target="_self" DataNavigateUrlFields="CourseID,ID" DataNavigateUrlFormatString="FrmModuleHistory.aspx?CourseID={0}&ModuleID={1}">
                <HeaderStyle Width="48%" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:HyperLinkField>
            <asp:HyperLinkField HeaderText="Module Type" HeaderStyle-Width="13%" DataTextField="MType"
                DataNavigateUrlFields="CourseID,ID" DataNavigateUrlFormatString="FrmModuleHistory.aspx?CourseID={0}&ModuleID={1}">
                <HeaderStyle Width="13%" />
            </asp:HyperLinkField>
            <asp:HyperLinkField HeaderText="Exam Name" HeaderStyle-Width="13%" DataTextField="doexam"
                DataNavigateUrlFields="CourseID,ID" DataTextFormatString="{0:MMM, yyyy}" DataNavigateUrlFormatString="FrmModuleHistory.aspx?CourseID={0}&ModuleID={1}">
                <HeaderStyle Width="12%" />
            </asp:HyperLinkField>
            <asp:HyperLinkField HeaderText="Result" HeaderStyle-Width="13%" DataTextField="Result"
                DataNavigateUrlFields="CourseID,ID" DataNavigateUrlFormatString="FrmModuleHistory.aspx?CourseID={0}&ModuleID={1}">
                <HeaderStyle Width="10%" />
            </asp:HyperLinkField>
            <asp:HyperLinkField HeaderText="Grade" HeaderStyle-Width="13%" DataTextField="Grade"
                DataNavigateUrlFields="CourseID,ID" DataNavigateUrlFormatString="FrmModuleHistory.aspx?CourseID={0}&ModuleID={1}">
                <HeaderStyle Width="5%" />
            </asp:HyperLinkField>
        </Columns>
        <PagerSettings Visible="False" />
    </asp:GridView>
    <br/>
    <div id="intralblExemptDiv" runat="server" style="background-color:#c7dded;width:100%;padding:1px; color:#666666; margin-top:8px; min-height:20px;" Visible="false">
     <span style="font:normal 18px arial; color:#003366;"> Intra-Level Exemption Detail </span><br/><br/>
         <%--<p style="font:normal 13px arial; color:red;">Note - This Module will not be counted as in passed Module in revision Instead the Module Exempted in new revision will be counted. </p>--%>
          <asp:Label ID="LblexempNote" runat="server" style="font:normal 13px arial; color:red;"></asp:Label>(<a style="font:normal 13px arial; color:navy;" target="_blank" href="https://nielit.gov.in/content/computer-course-0">Refer the syllabus for more details </a>)<br/><br/>
        <%-- <a style="font:normal 14px arial; color:navy;" target="_blank" href="https://nielit.gov.in/content/computer-course-0">===> Refer the syllabus for more detauls </a> <br/> <br/>--%>
    </div>
  
    <asp:GridView ID="gvIntraLevelExemption" runat="server"  AutoGenerateColumns="False"
        Width="100%" AllowPaging="false" PageSize="60">
        <Columns>
          <asp:TemplateField HeaderText="Old Revision Module against which Exemption availed" HeaderStyle-Width="50%">
              <ItemTemplate>
                  <asp:hyperlink runat="server" Text='<%#Eval("ExempttedModule")%>' />
              </ItemTemplate>
           </asp:TemplateField>
            
            <asp:TemplateField HeaderText="New Revsion Module Exempted" HeaderStyle-Width="50%">
              <ItemTemplate>
                  <asp:hyperlink runat="server" Text='<%#Eval("ExemptionModule")%>' />
              </ItemTemplate>
           </asp:TemplateField>
          </Columns>
        <PagerSettings Visible="False" />
    </asp:GridView>

   <div id="divgvModules" style="background-color: #c7dded; width: 100%; padding: 10px 10px 4px 10px;
        color: #666666; margin-top: 8px; min-height: 20px;" visible="false"  runat="server" >
        <span id="summeryHeading1" runat="server" style="font: normal 18px arial; color: #003366;">Module Status (Remaining to Pass As Per </span>
    </div>

    <asp:GridView ID="gvModules" runat="server" DataKeyNames="ID" AllowPaging="false"
        AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvModules_RowDataBound"
        PageSize="60" Visible ="false">
        <Columns>
            <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                <HeaderStyle Width="2%" />
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField HeaderText="Code" DataField="Code" >
                <HeaderStyle Width="10%" />
            </asp:BoundField>
            <asp:BoundField HeaderText="Module Name" DataField="name">
                <HeaderStyle Width="73%" />
            </asp:BoundField>
            <asp:BoundField HeaderText="Module Type" DataField="MType">
                <HeaderStyle Width="15%" />
            </asp:BoundField>
            <asp:BoundField HeaderText="Last Exam Name" Visible="false"  DataField="doexam">
                <HeaderStyle Width="15%" />
            </asp:BoundField>
        </Columns>
        <PagerSettings Visible="False" />
    </asp:GridView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
<uc2:SideLink ID="Sidelink" runat="server" />
<uc3:SideLink ID="Sidelink1" runat="server" />
 <%--<div style="height:6px;"> </div> --%>
</asp:Content>

