<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="AdminCourseLevelDuration.aspx.cs" Inherits="AdminCourseLevelDuration"
    Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
      <style type="text/css">
        .auto-style1 {
            width: 33%;
            height: 23px;
        }

        .table-container {
            overflow-x: auto; 
            width: 800px;
            max-width: 100%; 
            height: auto; 
        }

        .table-container th {
            font-family: Arial, sans-serif;
            font-size: 10px;
            /*font-weight: bold;*/
            background: linear-gradient(to bottom, #E0F0FC, #6b849a);
        }

        .table-container tr {
            font-family: Arial, sans-serif;
            font-size: 10px;
        }
    
     .table-container td, .table-container th {
            white-space: nowrap;
            padding: 5px;
        }


        .table-container td a,
        .table-container th a {
            /*text-decoration: none !important;*/
            color: black !important;
            font-weight: inherit;
        }

        .my-grid tr:nth-child(even) {
            background-color: #E6F0F0;
        }

        .my-grid tr:nth-child(odd) {
            background-color: #c9d7e2;
            transition: background-color 0.2s ease;
        }
    </style>


    <script type="text/javascript" language="javascript">  

        function validateFormFields() {
            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlcourseName.ClientID %>", "Course Name"))
                return false;
            if (!isBlank("<%=txtcourseLevelRevisionNo.ClientID %>", "Course Level Revision No. "))
                return false;
            return false;
        }

    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Courses Level Duration"></asp:Label>
    <input id="lblHdnCourse" type="hidden" runat="server">
    <%--<asp:Label ID="lblHdnCourse" runat ="server" Visible="false"></asp:Label>--%>
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
                                        <asp:Label ID="lblFiler3" Width="100%" runat="server" Text="Course Category Name"></asp:Label>
                                        <asp:DropDownList ID="ddlcoursecategoryF" Width="100%" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlcoursecategoryF_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblcourseNameF" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlCourseNameF" Width="100%" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlCourseNameF_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Course Level No." Visible="true"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlCourseLevelNo" runat="server" Width="100%" Visible="true">
                                                    <asp:ListItem Value="0">--All--</asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                            </Triggers>
                                        </asp:UpdatePanel>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by course name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {
            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category Name"))
                return false;
            if (!isSelected("<%=ddlcourseName.ClientID %>", "Course Name"))
                return false;
            if (!isBlank("<%=txtcourseLevelRevisionNo.ClientID  %>", "Course Level Revision No. "))
                return false;

            if (!isBlank("<%=txtcourseLevelDurationHrs.ClientID  %>", "Course Level Duration Hrs"))
                return false;

            if (!isBlankDate("<%=txtEffectiveFromDate.ClientID %>", "Effective From Date", "dd-MMM-yyyy"))
                return false;

            if (!isNumber("<%=txtcourseLevelRevisionNo.ClientID %>", "Numeric characters are  allowed"))
                return false;

            if (!isNumber("<%=txtCourseLevelNo.ClientID %>", "Numeric characters are  allowed"))
                return false;

            if (!isNumber("<%=txtcourseLevelDurationHrs.ClientID %>", "Numeric characters are  allowed"))
                return false;

            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlcourseName.ClientID %>", "Course Name"))
                return false;

            if (!isBlank("<%=txtcourseLevelRevisionNo.ClientID %>", "Course Level Revision No. "))
                return false;

            // Added by amit start

            if (!isBlank("<%=txtthrs.ClientID %>", "Theory Hours "))
                return false;

            if (!isBlank("<%=txtphrs.ClientID %>", "Practical Hours "))
                return false;
            if (!isBlank("<%=txteshrs.ClientID %>", "ES hours"))
                return false;
            if (!isBlank("<%=txtdomainskill.ClientID %>", "domainskill No. "))
                return false;
            if (!isBlank("<%=txtOJThrs.ClientID %>", "OJT Project No. "))
                return false;
            if (!isBlank("<%=txtawardingbodyid.ClientID %>", "Awarding Body ID "))
                return false;

            if (!isBlank("<%=txtawardname.ClientID %>", "Course Level Revision No. "))
                return false;
            if (!isBlank("<%=txtlabcnt.ClientID %>", "Course Level Revision No. "))
                return false;
            if (!isBlank("<%=txtclasscount.ClientID %>", "Course Level Revision No. "))
                return false;

            // Added by amit end



            return true;
        }
        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1]
            ShowHideMenu(obj, tableid);
        }



        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp1, Sender, CheckBoxName)
        }

        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp1, Sender, CheckBoxName)
        }

    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">


                <div class="table-container">
                    <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>

<%--                           <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
     class="ActionPopup" style="width: 132px; height: 40px;">
     <tr>
         <td align="right">
             <asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                 runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                 OnClick="PerformPopupAction"></asp:LinkButton>
         </td>
     </tr>
 </table>--%>

                            <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                                runat="server"></asp:Label>
                            <asp:GridView
                                ID="gvMain" runat="server" CssClass="my-grid" DataKeyNames="ID,CourseId" OnSorting="gvMain_Sorting"
                                OnRowDataBound="gvMain_RowDataBound"
                                AutoGenerateColumns="False" Width="100%"
                                EnableTheming="false"
                                >
                                <Columns>
                                    <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                        <HeaderStyle Width="2%" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>

                                    <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={1}"
                                        DataTextField="coursecatName" HeaderText="Course Category Name" SortExpression="Name" Target="_self">
                                        <HeaderStyle Width="16%" />
                                    </asp:HyperLinkField>

                                    <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={1}"
                                        DataTextField="Name" HeaderText="Course Name" SortExpression="Name" Target="_self">
                                        <HeaderStyle Width="16%" />
                                    </asp:HyperLinkField>

                                    <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={1}"
                                        DataTextField="courseLevelNo" HeaderText="Course Level No." SortExpression="courseLevelNo" Target="_self">
                                        <HeaderStyle Width="10%" />
                                    </asp:HyperLinkField>

                                    <asp:HyperLinkField HeaderStyle-Width="12%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={0}"
                                        DataTextField="courseLevelRevisionNo" HeaderText="Course Level Revision No." SortExpression="courseLevelRevisionNo"
                                        Target="_self">
                                        <HeaderStyle Width="12%" />
                                    </asp:HyperLinkField>

                                    <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={1}"
                                        DataTextField="courseLevelDurationHrs" HeaderText="Course Level Duration Hrs" SortExpression="courseLevelDurationHrs"
                                        Target="_self">
                                        <HeaderStyle Width="10%" />
                                    </asp:HyperLinkField>

                                    <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={1}"
                                        DataTextField="Effective_From_Date" HeaderText="Effective From Date" SortExpression="Effective_From_Date"
                                        Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                        <HeaderStyle Width="10%" />
                                    </asp:HyperLinkField>

                                    <asp:HyperLinkField HeaderStyle-Width="10%"  DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                        DataTextField="Effective_To_Date" HeaderText="Effective To Date" SortExpression="Effective_To_Date"
                                        Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                        <HeaderStyle Width="10%" />
                                    </asp:HyperLinkField>

                                    <%-- Added by amit start --%>
                                    <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={1}"
                                        DataTextField="theoryHrs" HeaderText="Theory Hours" SortExpression="theoryHrs"
                                        Target="_self">
                                        <HeaderStyle Width="10%" />
                                    </asp:HyperLinkField>


                                    <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={1}"
                                        DataTextField="practicalHrs" HeaderText="Practical Hours" SortExpression="practicalHrs"
                                        Target="_self">
                                        <HeaderStyle Width="10%" />
                                    </asp:HyperLinkField>

                                    <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={1}"
                                        DataTextField="esHrs" HeaderText="ES Hours" SortExpression="esHrs"
                                        Target="_self">
                                        <HeaderStyle Width="10%" />
                                    </asp:HyperLinkField>

                                    <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={1}"
                                        DataTextField="OJT_ProjectHrs" HeaderText="OJT_Project Hours" SortExpression="OJT_ProjectHrs"
                                        Target="_self">
                                        <HeaderStyle Width="10%" />
                                    </asp:HyperLinkField>

                                    <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={1}"
                                        DataTextField="domainSkillsHrs" HeaderText="Domain Skill Hours" SortExpression="domainSkillsHrs"
                                        Target="_self">
                                        <HeaderStyle Width="10%" />
                                    </asp:HyperLinkField>


                                    <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={1}"
                                        DataTextField="awardingBodyID" HeaderText="Awarding Body Id" SortExpression="awardingBodyID"
                                        Target="_self">
                                        <HeaderStyle Width="10%" />
                                    </asp:HyperLinkField>

                                    <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={1}"
                                        DataTextField="awardingBodyName" HeaderText="Awarding Body Name" SortExpression="awardingBodyName"
                                        Target="_self">
                                        <HeaderStyle Width="10%" />
                                    </asp:HyperLinkField>

                                    <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={1}"
                                        DataTextField="claasroomCount" HeaderText="Class Room Count" SortExpression="claasroomCount"
                                        Target="_self">
                                        <HeaderStyle Width="10%" />
                                    </asp:HyperLinkField>

                                    <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseId" DataNavigateUrlFormatString="?Key={0}&filterCourse={1}"
                                        DataTextField="labCount" HeaderText="Lab Count" SortExpression="labCount"
                                        Target="_self">
                                        <HeaderStyle Width="10%" />
                                    </asp:HyperLinkField>
                                    <%-- Added by amit end --%>

                                    <asp:TemplateField HeaderStyle-Width="2%"  Visible="false" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                        <ItemTemplate>
                                            <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                                runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                                Style="cursor: pointer; border: 1px solid transparent;" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" Visible="false">
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="CheckBox1" SkinID="CheckAllInGridView" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="2%" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Visible="False" />
                            </asp:GridView>
                            <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                            <asp:HiddenField ID="hfAccID" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <%-- changged by amit --%>
            </div>
            <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" CurrentPageSize="200" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>


        <%-- Entry of data --%>
        <asp:View ID="New" runat="server">
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">

                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Course Level Revision No. &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">

                    <td valign="top">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcoursecategory" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                    <td valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcourseName" runat="server" SkinID="ddl250">
                                    <asp:ListItem>---Select One---</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">

                        <asp:TextBox ID="txtcourseLevelRevisionNo" runat="server" MaxLength="3" onkeypress="checkNumber(this,2,0,event);" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top" class="auto-style1">
                        <asp:Label ID="Label14" runat="server" SkinID="CaptionLabel" Text="Course Level No. (0, if no level) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" class="auto-style1">
                        <asp:Label ID="lblverifiedStatus" runat="server" SkinID="CaptionLabel" Text="Course Level Duration Hrs &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>

                    </td>
                    <td valign="top" class="auto-style1">
                        <asp:Label ID="lblNCVETQualCode" runat="server" SkinID="CaptionLabel" Text="NCVET Qualification Code"></asp:Label>
                        <%--<asp:Label ID="lblverificationMessage" runat="server" SkinID="CaptionLabel" Text="Is Active &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>--%> 
                   
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtCourseLevelNo" runat="server" MaxLength="3" onkeypress="checkNumber(this,2,0,event);" SkinID="txt248"></asp:TextBox>

                    </td>
                    <td style="width: 33%;" valign="top">
                        <%--<asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlverifiedStatus" runat="server" SkinID="ddl250" Enabled="false" >                                   
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>   --%>
                        <asp:TextBox ID="txtcourseLevelDurationHrs" runat="server" MaxLength="4" onkeypress="checkNumber(this,4,0,event);" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtNCVETQualCode" runat="server" MaxLength="150" SkinID="txt248"></asp:TextBox>

                        <%--<asp:TextBox ID="TxtverificationMessage" runat="server" BackColor="White" MaxLength="500" SkinID="txt248" Enabled="false"></asp:TextBox>--%>
                        <%--<asp:DropDownList ID="ddlIsActive" runat="server" SkinID="ddl250">
                            <asp:ListItem>---Select One---</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="2" >No</asp:ListItem>                          
                        </asp:DropDownList>--%>

                         
                    </td>
                </tr>
                <%-- Added by amit start --%>

                <tr>
                    <td valign="top" class="auto-style1">
                        <asp:Label ID="lblthrs" runat="server" SkinID="CaptionLabel" Text="Theory Hours &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" class="auto-style1">
                        <asp:Label ID="lblphrs" runat="server" SkinID="CaptionLabel" Text="Practical Hours &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" class="auto-style1">
                        <asp:Label ID="lbleshrs" runat="server" SkinID="CaptionLabel" Text="ES Hours &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr>

                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtthrs" runat="server" MaxLength="4" onkeypress="checkNumber(this,2,0,event);" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtphrs" runat="server" MaxLength="4" onkeypress="checkNumber(this,4,0,event);" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txteshrs" runat="server" MaxLength="4" onkeypress="checkNumber(this,4,0,event);" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td valign="top" class="auto-style1">
                        <asp:Label ID="lblojthrs" runat="server" SkinID="CaptionLabel" Text="OJT Project Hours &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" class="auto-style1">
                        <asp:Label ID="lbldomainskill" runat="server" SkinID="CaptionLabel" Text="Domain Skill Hours &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" class="auto-style1">
                        <asp:Label ID="lblawardingbodyid" runat="server" SkinID="CaptionLabel" Text="Awarding Body ID &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr>

                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtOJThrs" runat="server" MaxLength="4" onkeypress="checkNumber(this,2,0,event);" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtdomainskill" runat="server" MaxLength="4" onkeypress="checkNumber(this,4,0,event);" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtawardingbodyid" runat="server" onkeypress="checkNumber(this,10,0,event);" MaxLength="150" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>


                <tr>
                    <td valign="top" class="auto-style1">
                        <asp:Label ID="lblawardname" runat="server" SkinID="CaptionLabel" Text="Awarding Body Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" class="auto-style1">
                        <asp:Label ID="lblclasscount" runat="server" SkinID="CaptionLabel" Text="Classroom Count &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" class="auto-style1">
                        <asp:Label ID="lbllabcnt" runat="server" SkinID="CaptionLabel" Text="Lab Count &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr>

                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtawardname" runat="server" MaxLength="150" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtclasscount" runat="server" MaxLength="4" onkeypress="checkNumber(this,4,0,event);" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtlabcnt" runat="server" MaxLength="4" onkeypress="checkNumber(this,4,0,event);" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>

                <%-- Added by amit end --%>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblNIELITQualCode" runat="server" SkinID="CaptionLabel" Text="NIELIT Qualification Code"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label21" runat="server" SkinID="CaptionLabel" Text="Effective_From_Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Effective_To_Date "></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtNIELITQualCode" runat="server" MaxLength="150" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEffectiveFromDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ReadOnly="false"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtEffectiveFromDate">
                                </asp:CalendarExtender>
                                <img id="img2" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEffectiveToDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtEffectiveToDate">
                                </asp:CalendarExtender>
                                <img id="img3" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <%-- <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <%--<asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEffectiveFromDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ReadOnly="false" ></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtEffectiveFromDate">
                                </asp:CalendarExtender>
                                <img id="img2" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <%--<asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEffectiveToDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtEffectiveToDate">
                                </asp:CalendarExtender>
                                <img id="img3" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                     
                    </td>
                </tr> --%>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateLogin();" runat="server" Text="Save"
                    OnClick="SaveRecordCourseLevelDuration" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                <asp:Button ID="btnback" runat="server" Text="Back" TabIndex="54" OnClick="btnback_Click" Visible="False" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<%--<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>--%>
