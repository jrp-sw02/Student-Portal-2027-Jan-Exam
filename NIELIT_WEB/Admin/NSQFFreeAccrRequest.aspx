<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NSQFFreeAccrRequest.aspx.cs"
    Inherits="Admin_NSQFFreeAccrRequest" MasterPageFile="~/MasterPages/main.master"
    Debug="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>

    <script type="text/javascript">

        function SearchNielitFreeCourses(txtSearch, cbNielitFreeCourses) {
            if ($(txtSearch).val() != "") {
                var count = 0;
                $(cbNielitFreeCourses).children('tbody').children('tr').each(function () {
                    var match = false;
                    $(this).children('td').children('label').each(function () {
                        if ($(this).text().toUpperCase().indexOf($(txtSearch).val().toUpperCase()) > -1)
                            match = true;
                    });
                    if (match) {
                        $(this).show();
                        count++;
                    }
                    else { $(this).hide(); }
                });
                $('#spnCount').html((count) + ' match').css({ "color": "green", "font-size": "100%", "font-weight": "bold" });
            }
            else {
                $(cbNielitFreeCourses).children('tbody').children('tr').each(function () {
                    $(this).show();
                });
                $('#spnCount').html('');
            }
        }


    </script>

    <script type="text/javascript">
        function checkBoxList1OnCheck(listControlRef) {
            var inputItemArray = listControlRef.getElementsByTagName('input');

            for (var i = 0; i < inputItemArray.length; i++) {
                var inputItem = inputItemArray[i];

                if (inputItem.checked) {
                    inputItem.parentElement.style.backgroundColor = 'PaleTurquoise ';
                }
                else {
                    inputItem.parentElement.style.backgroundColor = 'White';
                }
            }
            var inputElems = listControlRef.getElementsByTagName('input'),
    count = 0;
            for (var i = 0; i < inputElems.length; i++) {
                if (inputElems[i].type === "checkbox" && inputElems[i].checked === true) {
                    count++;
                }
            }

        }
    </script>
    <script type="text/javascript">
        $(function () {
            $('#multiselect').multiselect();
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $('#multiselect').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                filterPlaceholder: 'Search for something...'
            });
        });
    </script>
</asp:Content>
<asp:content id="Content3" contentplaceholderid="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text=" NSQF Free Accreditation Request"></asp:Label>
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphAddNew" runat="Server">
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
                                    <asp:Label ID="Label1" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                    <asp:DropDownList ID="ddlCourseName" Width="100%" runat="server"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>                              
                                 
                            <tr>
                                <td></td>
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
<uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Batch Name"
    OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
    AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
    AutoCompleteCompletionSetCount="10" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
    function ValidateFormFields() {

        if (!isSelected("<%=ddlAllAccrDetails.ClientID %>", "Accr Details"))
            return false;
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
<asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
    <asp:View ID="List" runat="server">
        <div id="divGrid" runat="server">
            <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                        class="ActionPopup" style="width: 132px; height: 40px;">
                        <tr>
                            <td align="left">
                                <asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                    runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                    OnClick="PerformPopupAction"></asp:LinkButton>                                   
                            </td>
                        </tr>
                    </table>
                    <br />
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
            runat="server"></asp:Label>
                    <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                        OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                        <Columns>
                            <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                <HeaderStyle Width="5%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:HyperLinkField HeaderStyle-Width="30%" 
                                DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="CourseName"
                                HeaderText="Course Name" SortExpression="CourseName" Target="_self">
                            <HeaderStyle Width="30%" />
                            </asp:HyperLinkField> 
                             <asp:HyperLinkField HeaderStyle-Width="15%" 
                                DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="Effs"
                                HeaderText="Valid UpTo" SortExpression="Course" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                <HeaderStyle Width="15%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="10%" 
                                DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="IsActive"
                                HeaderText="Granted" SortExpression="Code" Target="_self">
                                <HeaderStyle Width="10%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>  
                                <asp:HyperLinkField HeaderStyle-Width="15%" 
                                DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="GrantDate"
                                HeaderText="Grant Date" SortExpression="Course" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                <HeaderStyle Width="15%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField HeaderStyle-Width="15%" 
                                DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="accrNo"
                                HeaderText="Accr No" SortExpression="accrNo" Target="_self" >
                                <HeaderStyle Width="15%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField HeaderStyle-Width="15%" 
                                DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="accrUpto"
                                HeaderText="Accr Upto" SortExpression="accrUpto" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                <HeaderStyle Width="15%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                                                            
                            <%--<asp:HyperLinkField HeaderStyle-Width="10%" 
                                DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="Remarks"
                                HeaderText="Remarks" SortExpression="Code" Target="_self">
                                <HeaderStyle Width="10%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>--%>                                

                         <%--   <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                <ItemTemplate>
                                    <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                        runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                        Style="cursor: pointer; border: 1px solid transparent;" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" />
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" Visible="false">
                                <HeaderTemplate>
                                    <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                </ItemTemplate>
                                <HeaderStyle Width="3%" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                    <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                    <asp:HiddenField ID="hfcode" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divNavigation" runat="server">
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
                <td colspan="3" style="width: 99%;" valign="top">
                    <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text=" Accredited Institutes"
                        Width="100%"></asp:Label>
                </td>             
            </tr>
            <tr class="even">
                <td colspan="3" style="width: 99%;" valign="top">
                    <asp:TextBox Style="width: 501px;" ID="txtInstitute" runat="server" Enabled="false" SkinID="txt248" Width="100%" ToolTip="Institute"></asp:TextBox>
                </td>                
            </tr>              
            <tr>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="All Accreditation Details  &lt;b class='mandatory'&gt;<br/>* Higher Accreditation Level selection will provide with more course choices .&lt;/b&gt;"></asp:Label>
                </td>
                <td style="width: 33%;" colspan="2" valign="top">
                    <asp:Label ID="Label22" runat="server" SkinID="CaptionLabel" Text="NSQF Free Course &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                </td>
                
            </tr>
            <tr class="even">
                <td  style="width: 33%;" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlAllAccrDetails" Width="100%" runat="server" SkinID="ddl250" AutoPostBack="true" Enabled="true" OnSelectedIndexChanged="ddlAllAccrDetails_SelectedIndexChanged">
                                <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                                <asp:PostBackTrigger ControlID="ddlAllAccrDetails" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td colspan="2" style="width: 33%;" valign="top">
                                          
                    <div style=" width: 100%" >
                                                <asp:UpdatePanel RenderMode="Inline" ID="UpdatePanel5" UpdateMode="Conditional"
                            runat="server">
                            <ContentTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" style="width: 370px;" SkinID="ddl250" onkeyup="SearchNielitFreeCourses(this,'#cbNielitFreeCourses');"
                                        visible="false"           placeholder="Search Free Courses">
                                            </asp:TextBox>
                                            <span id="spnCount"></span>
                                            <div style="height: 150px; overflow-y: auto; overflow-x: hidden">
                              
                                            <asp:CheckBox ID="allChkBox" Text="Select all" AutoPostBack="True" oncheckedchanged="allChkBox_CheckedChanged" runat="server" visible="false"  />
                                            <asp:CheckBoxList ID="cbNielitFreeCourses" runat="server" visible="true" RepeatColumns="1"
                                                    RepeatDirection="Vertical" Width="450px" ClientIDMode="Inherit" onclick="checkBoxList1OnCheck(this);"  >                                                    
                                                </asp:CheckBoxList>
                                </ContentTemplate>
                                                    <Triggers>
                                <asp:PostBackTrigger ControlID="TextBox1" />
                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                        </div>   
 
                </td>
            </tr>  
                
            <tr id ="r" runat="server" visible="false">
                <td style="width:99%;" colspan="3"  valign="top">
                    <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Remarks &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                      
                </td>                   
                                        
            </tr>
            <tr class="even" id ="r1" runat="server" visible="false">

                <td style="width:99%;" colspan="3" valign="top">
                    <asp:TextBox ID="txtRemarks" runat="server" style="width: 800px;" 
                                        visible="true"           placeholder="Search Free Courses">
                                            </asp:TextBox>

                </td>
            </tr>      
               
            <tr>
                <td style="width: 99%;" colspan="3" valign="top">
                            <asp:UpdatePanel RenderMode="Inline" ID="UpdatePanel1" UpdateMode="Conditional" 
                            runat="server" >
                            <ContentTemplate>
                        <asp:CheckBox ID="chkUnderTakingByInstitute" Text="I accept the undertaking written below" AutoPostBack="True"  runat="server" visible="TRUE"  />
                                <pre style="text-align: justify">
 1.	I agree that I am the Competent Authority of the institute, by virtue of the administrative and </pre>
                                <pre>
financial powers vested in me. I being the owner/authorized person of the Institution and as such is </pre>
                                <pre>
having the authority to sign the present undertaking and bind the institution and I have read and </pre>
                                <pre>
understood the RULES and REGULATIONS/NIELIT NORMS as mentioned in the Guidelines for the NSQF Aligned </pre>
                                <pre>
course(s) as applicable from time to time and as given on the official website of NIELIT to the </pre>
                                <pre>
institute conduction NSQF Aligned Course(s) and I agree to abide by the same.

2.	I am aware that in case any information given by me is false or misleading, NIELIT may in its </pre>
                                <pre>
sole discretion take whatever action(s) or measure(s) it deems necessary and appropriate and accreditation </pre>
                                <pre>
granted for any or all of its courses including O/A/B/C levels of courses of the institute might be </pre>
                                <pre>
withdrawn.

3.	I agree to abide by the decision(s) of the NIELIT or its designated agencies/committees/persons </pre>
                                <pre>
in respect of my applications for permission to conduct NSQF Aligned course(s). 

4.	I undertake to abide by the Rules and Regulations and Code of ethics of NIELIT as agreed by me </pre>
                                <pre>
at the time of grant of accreditation for NIELIT O/A/B/C level of courses

5.	I, further understand that,

(i)	The permission granted to the institute to conduct NSQF will automatically be withdrawn on </pre>
                                <pre>
withdrawal of accreditation of O/A/B/C level, even if, the validity with respect to NSQF Aligned course(s) </pre>
                                <pre>
is not expired.

(ii)	That in anyway,institute will not misuse this permission and shall not indulge in unfair marketing practices by </pre>
                                <pre>
exaggerating the facilities available at its institute, which, in the opinion of NIELIT, amounts to </pre>
                                <pre>
misleading the public.  

(iii)	That this permission in any case does not confer any right on the institute to enroll students </pre>
                                <pre>
for other Courses offered by NIELIT, unless specific accreditation/ permission is obtained for the purpose.

(iv)	That the institute will ensure that the NSQF course(s) in which the institute is taking student’s admission </pre>
                                <pre>
is NSQF aligned and the course is valid at the time of admission. Qualification Files consisting of </pre>
                                <pre>
validity (Date of planned review) of a NSQF course is available at National Qualification Register </pre>
                                <pre>
(https://nqr.gov.in/). NIELIT will not be responsible for any liability, if an institute takes student’s admission </pre>
                                <pre>
in a course which is not valid at the time of admission.

(v)	That, NIELIT reserves the right to discontinue/amend this permission anytime without assigning </pre>
                                <pre>
any reason. In all matters related to this permission, the decision of Competent Authority, NIELIT will </pre>
                                <pre>
be final and binding.

(vi)	In case of non-availability of the required software,hardware,faculty etc. as per NSQF Aligned Course/s </pre>
                                <pre>
applied  for ,at the time of Surprise visit by NIELIT at the institute from time to time,</pre>
                                <pre>
 are not found satisfactory, the action as deemed fit (withdrawal of accreditation/permission granted etc. </pre>
                                <pre>
(as applicable), will be taken against the institute.

(vii)	If any complaint is received against the institute with regard to non−availability of required </pre>
                                <pre>
infrastructure as per the syllabus of the course in future the action as deemed fit [withdrawal of </pre>
                                <pre>
accreditation/permission granted etc. (as applicable)], will be taken against the institute.

                                      </pre>
<%--1.	I</pre><asp:TextBox ID="txtName" runat ="server" Font-Underline="True"></asp:TextBox> <asp:Label ID="Label6" runat="server" Text="&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label> &nbsp;S/o <asp:TextBox ID="txtFatherName" runat ="server" Font-Underline="True"></asp:TextBox><asp:Label ID="Label7" runat="server" Text="&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>, 
                                Designated as <asp:TextBox ID="txtDesignation" runat ="server" Font-Underline="True"></asp:TextBox><asp:Label ID="Label8" runat="server"  Text="&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label> &nbsp;<br />
                                <br />
                                for the institute <asp:Label ID="Label5" runat ="server"></asp:Label> &nbsp;and resident of 
                             
<asp:TextBox ID="txtAddress" Width ="20%" runat ="server"></asp:TextBox><asp:Label ID="Label9" runat="server"  Text="&lt;b class='mandatory'&gt;*&lt;/b&gt;">

    </asp:Label> <pre>  certify that I am the competent authority, by virtue of the administrative and financial powers vested in me. 
    
    I being the owner/authorized person of the Institution and as such is having the authority to sign the present undertaking and 
    
    bind the institution and I have read and understood the RULES and REGULATIONS/NIELIT NORMS as mentioned in the Guidelines for 
    
    the NSQF Aligned course(s) as applicable from time to time and as given on the official website of NIELIT to the institute 
    
    conduction NSQF Aligned COURSE(S) and I agree to abide by the same.

2.  I am aware that in case any information given by me is false or misleading, NIELIT may in its sole discretion take whatever 
    
    action(s) or measure(s) it deems necessary and appropriate and accreditation/permission granted for any or all of its courses 

    including O/A/B/C levels of courses of the institute might be withdrawn.

3.	I agree to abide by the decision(s) of the NIELIT or its designated agencies/committees/persons in respect of my
    
    applications for permission to conduct NSQF Aligned course(s).

4.	I, further understand that,

(i)	    The permission granted to the institute to conduct NSQF Aligned course(s) will automatically be withdrawn on withdrawal of 
   
        accreditation of O/A/B/C levels of courses, even if, the validity with respect to NSQF Aligned course(s) is not expired.

(ii)	In anyway, not misuse this permission and shall not indulge in unfair marketing practices by exaggerating the facilities available
        
        at its institute, which, in the opinion of NIELIT, amounts to misleading the public.  

(iii)	This permission in any case does not confer any right on the institute to enroll students for other Courses offered by NIELIT, 
        
        unless specific accreditation/ permission is obtained for the purpose.

(iv)	The institute will ensure that the NSQF course(s) in which the institute is taking admission is NSQF aligned and the course is 
        
        valid at the time of admission. Qualification Files consisting of validity (Date of planned review) of a NSQF course is available 
        
        at National Qualification Register (https://nqr.gov.in/). NIELIT will not be responsible for any liability, if an institute 
        
        takes admission in a course which is not valid at the time of admission.

(v)	    That, NIELIT reserves the right to discontinue/amend this permission anytime without assigning any reason. In all matters related to 
        
        this permission, the decision of Competent Authority, NIELIT will be final and binding.

(vi)	In case, availability of the required software/hardware/faculty as per requested NSQF Aligned Course/s during the time of Surprise 
        
        visit to be done by NIELIT at the institute from time to time, are not found satisfactory, the action as deemed fit 
        
        (withdrawal of accreditation/permission granted etc. (as applicable) will be taken against the institute.

(vii)	If any complaint is received against the institute with regard to non−availability of required infrastructure as per the syllabus 
        
        of the NSQF courses in future the action as deemed fit (withdrawal of accreditation/permission granted etc. (as applicable), will be taken against the institute.--%>

                        
                                    </ContentTemplate>
                                                    <Triggers>                               
                        </Triggers>
                                                    </asp:UpdatePanel>
                </td>              
            </tr>
               
               
        </table>
        <div style="text-align: center; margin-top: 10px">

            <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                Text="Apply" OnClick="SaveRecord" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Visible="false" />
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Visible="false" />
        </div>           
    </asp:View>
</asp:MultiView>
</asp:content>
<asp:content id="Content7" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content8" contentplaceholderid="cthRightPannel" runat="Server">
    <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
    cellpadding="0" id="tblNavLinks" width="97%">
    <tr>
        <td></td>
    </tr>
</table>
</asp:content>
