<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="AdminExamHistory.aspx.cs" Inherits="Admin_AdminExamHistory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../UserControl/SearchBar.ascx" tagname="SearchBar" tagprefix="uc1" %>

<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
	<asp:Label ID="lblHeading" runat="server" Text="Courses"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
	<uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
	<asp:Panel runat="server" ID="pnlFilter" Visible="true">
		<div id="filterContainer">
			<a href="#" id="filterButton"><span></span><em></em></a>
			<div style="clear: both">
			</div>
			<div id="filterBox" align="left">
				<div id="filterPannel">
					<asp:UpdatePanel EnableViewState="true" RenderMode="Inline" ID="filterPnal_upnlFilter"
						UpdateMode="Conditional" runat="server">
						<ContentTemplate>
							<table cellpadding="0" id="body1" cellspacing="0" width="100%">
								<tr>
									<td>
										<asp:Label ID="lblFiler" Width="60%" runat="server" Font-Bold="true" Font-Size="12pt"
											Text="Filter Panel"></asp:Label>
										<asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
											Text="" OnClick="ResetFilterPanel" />
										<asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
											Text="" OnClick="AllyFilter" />
									</td>
								</tr>
								<tr>
									<td>
										<asp:Label ID="lblUserType" Width="100%" runat="server" Text="User Type"></asp:Label>
										<asp:DropDownList ID="ddlSearchUserType" Width="100%" runat="server">
											<asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
										</asp:DropDownList>
										
									</td>
								</tr>
							</table>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
				<script language="javascript" type="text/javascript">
					var box = $('#filterBox');
					shortcut.add("Ctrl+Shift+F", function () {
						box.show();
					});
					shortcut.add("Esc", function () {
						box.hide();
					});
				</script>
			</div>
		</div>
	</asp:Panel>
	   <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by object name or parent name"
		OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
		AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
		AutoCompleteCompletionSetCount="10" />   
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">

<ul class="crumbs">
		<li class="first"><a href="AdminRegstud.aspx" style="z-index:9;"><span></span>Registered Students</a></li>
		<li><a href="AdminRegstud.aspx?key1=<%=Session["regno"].ToString() %>&name=<%=Session["name"].ToString() %>" style="z-index:8;"><%=Session["regno"].ToString() %></a></li>
		<li id="licourse" runat="server"><a href="AdminCourses.aspx" style="z-index:7;"> Courses:</a></li>
		<li id="liLevel" runat="server"><a href="AdminCourses.aspx" style="z-index:6;">  <asp:Label  ID="Label2" runat="server" Text=""></asp:Label></a></li>
		<li><a href="AdminExamDetail.aspx" style="z-index:5;">Exam detail</a></li>
		<li><a href="#" style="z-index:4;"><%= Request.QueryString["exam"]%></a></li>
		
	</ul>





<%--<a href="AdminRegstud.aspx">Registered Students</a> : 
<a href="AdminRegstud.aspx?key1=<%= Request.QueryString["key1"] %>&name=<%=Request.QueryString["name"] %>"><%= Request.QueryString["key1"] %></a>
 >> 
<a href="AdminCourses.aspx?key1=<%= Request.QueryString["key1"] %>&name=<%=Request.QueryString["name"] %>">Courses</a> :
<a href="AdminCourses.aspx?key1=<%= Request.QueryString["key1"] %>&level=<%=Request.QueryString["level"] %>&name=<%=Request.QueryString["name"] %>">  <%= Request.QueryString["level"] %>  </a>
 >> 
<a href="AdminExamDetail.aspx?key1=<%= Request.QueryString["key1"] %>&name=<%= Request.QueryString["name"]%>&level=<%= Request.QueryString["level"] %>">Exam detail</a>
 <a href="AdminExamHistory.aspx?key1=<%=Request.QueryString["key1"] %>&exam=<%= Request.QueryString["exam"]%>&level=<%= Request.QueryString["level"]%>">:<%= Request.QueryString["exam"]%></a> 
 
 
 <a href="#"><%= Request.QueryString["module"] %></a>--%>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
	<script language="javascript" type="text/javascript">
	   
	  
	  
	   
		  
	 
	</script>
	<asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
		<asp:View ID="List" runat="server">
			<div id="divGrid" runat="server">
				<asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
					<ContentTemplate>
						<table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
							class="ActionPopup" style="width: 132px; height: 40px;">
							<tr>
								<td align="left">
									<asp:LinkButton ID="lbResetGrid" OnClientClick="return ConfirmAction('Are you sure you want to reset password of selected user!');"
										runat="server" Text="Reset Password" ToolTip="click to reset password" SkinID="lnkbtnAction"
										CommandName="Reset" OnClick="PerformPopupAction"></asp:LinkButton>
									<asp:LinkButton ID="lbChnageStatus" OnClientClick="return ConfirmAction('Are you sure you want to change login status of selected user!');"
										runat="server" Text="Change Login Status" ToolTip="click to Change Login Status"
										SkinID="lnkbtnAction" CommandName="ChangeStatus" OnClick="PerformPopupAction"></asp:LinkButton>
								</td>
							</tr>
						</table>
						<div class="summary_block">
	 <span id="spn1" runat="server"></span>
		<table width="100%">
			<tr>
				<td>
					Registration No.</td>
				<td>
					:</td>
				<td>
					 <%= Convert.ToString(Session["regno"]) %> </td>
				<td >
					Registration Date
				</td>
				<td>
					:</td>
				<td>
					01/01/2012
				</td>
			</tr>
			<tr>
				<td>
					Exam Name</td>
				<td>
					:</td>
				<td>
					<%= Request.QueryString["exam"] %></td>
				<td>
					Date</td>
				<td>
					:</td>
				<td>
					15-jan-2012 to 17-jan-2012</td>
			</tr>
			<tr>
				<td>
					RollNo.</td>
				<td>
					:</td>
				<td>
					111</td>
				<td>
					Centre</td>
				<td>
					:</td>
				<td>
					BN Bytes Subhash Nagar Udaipur</td>
			</tr>
			<tr>
				<td colspan="6">
				<table class="gdbody" cellspacing="1" cellpadding="4" id="cphContents_gvMain" style="width: 97%;">
		<tr class="gdheader">
			<th scope="col" style="width: 3%;">
				#
			</th>
			<th scope="col" style="width: 20%;">
				Module Code</th>
			<th scope="col" style="width: 20%;">
				Module Name</th>
			<th scope="col" style="width: 20%;">
				Date of Exam
			</th>
			<th scope="col" style="width: 15%;">
				 Result
			</th>
			<th scope="col" style="width: 15%;">
				 Grade</th>
		</tr>
		<tr class="gdrow">
			<td>
				1
			</td>
			<td>
			M1
			</td>
			<%--<td>
			   <a href="AdminExamHistory.aspx?key1=<%=Request.QueryString["key1"] %>&module=M1&exam=<%= Request.QueryString["exam"]%>&level= <%= Request.QueryString["level"]%>"> M1</a></td>--%>
			<td>
				Operating system</td>
			<td>
				15-Jan-2012
			</td>
			<td>
				Pass</td>
			<td>
				A</td>
		</tr>
		<tr class="gdalternate">
			<td>
				2
			</td>
			<td>
			M2
			</td>
			<%--<td>
			   <a href="AdminExamHistory.aspx?key1=<%=Request.QueryString["key1"] %>&module=M2&exam=<%= Request.QueryString["exam"]%>&level= <%= Request.QueryString["level"]%>"> M2</a>
			</td>--%>
			<td>
				Information Technology</td>
			<td>
				17-Jan-2012</td>
			<td>
				Fail
			</td>
			<td>
				F</td>
		</tr>
	</table>
				</td>
			   
			</tr>
		</table>
	   
	
	</div>
						
						<asp:HiddenField ID="hfActionID" runat="server" Value="" />
					</ContentTemplate>
				</asp:UpdatePanel>
			</div>
			<div id="divNavigation" runat="server">
				<asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
					runat="server">
					<ContentTemplate>
						<uc3:PagingBar ID="PagingBar1" runat="server"  
							OnPageIndexChanged="PageIndexChanged" />
					</ContentTemplate>
				</asp:UpdatePanel>
			</div>
		</asp:View>
		<asp:View ID="New" runat="server">
 <div class="summary_block">
	 <span id="Span1" runat="server">Exam Details of  <%= Request.QueryString["level"] %> </span>
	 <table width="100%">
	<tr>
	 <td width="15%">
					Registration No.</td>
				<td width="2%">
					:</td>
				<td>
					 <%= Request.QueryString["key1"] %> </td>
				<td width="15%" >
					Registration Date
				</td>
				<td width="2%">
					:</td>
				<td width="20%">
					01/01/2012
				</td>
				<td width="11%">
					Roll Number</td>
				<td width="2%">
					:</td>
				<td>
					111</td>
	</tr>
	<tr>
	<td colspan="9">
				<table class="gdbody" cellspacing="1" cellpadding="4" id="Table1" style="width: 97%;">
		<tr class="gdheader">
			<th scope="col" style="width: 3%;">
				#
			</th>
			<th scope="col" style="width: 20%;">
				Exam Name</th>
			<th scope="col" style="width: 20%;">
				Module </th>
			<th scope="col" style="width: 20%;">
				Date of Exam
			</th>
			<th scope="col" style="width: 15%;">
				 Result
			</th>
			<th scope="col" style="width: 10%;" width="5%">
				 Grade</th>
			<th scope="col" style="width: 15%;" width="10%">
				 Centre</th>
		</tr>
		<tr class="gdrow">
			<td>
				1
			</td>
			<td>
				July 2012</td>
			<td>
			  <%= Request.QueryString["module"] %>  </td>
			<td>
				15-July-2012
			</td>
			<td>
				Pass</td>
			<td>
				A</td>
			<td width="10%">
				BN college</td>
		</tr>
		<tr class="gdalternate">
			<td>
				2
			</td>
			<td>
				January 2012</td>
			<td>
				 <%= Request.QueryString["module"] %></td>
			<td>
				17-Jan-2012</td>
			<td>
				Fail
			</td>
			<td>
				F</td>
			<td width="10%">
				MB College</td>
		</tr>
	</table>
	</td>
	</tr>
	</table>
		<table width="100%">
			<tr>
				<td width="11%">
					&nbsp;</td>
				<td width="2%">
					&nbsp;</td>
				<td>
					&nbsp;</td>
				<td>
					&nbsp;</td>
				<td>
					&nbsp;</td>
				<td>
					&nbsp;</td>
			</tr>
			<tr>
				<td colspan="6">
					&nbsp;</td>
			   
			</tr>
		</table>
	   
	
	</div>



			<div style="text-align:right; margin-top:10px">
			   
						<asp:Button ID="btnCancel" runat="server" 
							Text="Cancel" onclick="btnCancel_Click" /></div>
		</asp:View>

	</asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">

</asp:Content>

