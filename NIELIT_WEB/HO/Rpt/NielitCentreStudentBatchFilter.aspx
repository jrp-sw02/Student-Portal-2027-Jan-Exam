<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="NielitCentreStudentBatchFilter.aspx.cs" Inherits="HO_NielitCentreStudentBatchFilter" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    Nielit Centre Student Batch Filter
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <script src="../../Script/GlobalFunction.js" type="text/javascript"></script>       
    <script type="text/javascript" language="javascript">
        function OpenWindow() {
            // course category id 
            var CourseCatId;
            if (document.getElementById('<%=ddlcoursecategory.ClientID %>').value != "0")
                CourseCatId = document.getElementById('<%=ddlcoursecategory.ClientID %>').value;
            else {

                alert("Please Select Course Category !");
                return false;
            }
            // course id ddlbatchname
            var CourseId;
            if (document.getElementById('<%=ddlcourseName.ClientID %>').value != "0")
                CourseId = document.getElementById('<%=ddlcourseName.ClientID %>').value;
            else {

                alert("Please Select Course !");
                return false;
            }
            // batch name
            var BatchId;
            if (document.getElementById('<%=ddlbatchname.ClientID %>').value != "0")
                BatchId = document.getElementById('<%=ddlbatchname.ClientID %>').value;
            else {

                alert("Please Select Batch Name !");
                return false;
            }
            //Cast Category 
            var Category;
            if (document.getElementById('<%=ddlCategory.ClientID %>').value != "")
                Category = document.getElementById('<%=ddlCategory.ClientID %>').value;

            //Genger 
            var Gender;
            if (document.getElementById('<%=ddlGender.ClientID %>').value != "")
                Gender = document.getElementById('<%=ddlGender.ClientID %>').value;

            //Physically Hanidcapped 
            var PhyH;
            if (document.getElementById('<%=ddlPhyHanidcapped.ClientID %>').value != "")
                PhyH = document.getElementById('<%=ddlPhyHanidcapped.ClientID %>').value;

            //Whether Project Student
            var WhProStu;
            if (document.getElementById('<%=ddlWhProjectStudent.ClientID %>').value != "")
                WhProStu = document.getElementById('<%=ddlWhProjectStudent.ClientID %>').value;

            //Project Name
            var ProjName = "0";
            if (document.getElementById('<%=ddlProjectName.ClientID %>').value != "")
                ProjName = document.getElementById('<%=ddlProjectName.ClientID %>').value;

            // Intitute name or sub centre name
            var InstId;
            if (document.getElementById('<%=ddlSubcentreName.ClientID %>').value != "0")
                InstId = document.getElementById('<%=ddlSubcentreName.ClientID %>').value;

            if (document.getElementById('<%=RdoAffInstOrNonAffInst.ClientID %>').value == "2")
                InstId = document.getElementById('<%= NIELITCentreId.ClientID %>');

            //View report                       
             window.open("NielitCentreStudentBatchWise.aspx?CourseId=" + CourseId + "&InstId=" + InstId + "&CourseCatId=" + CourseCatId + "&BatchId=" + BatchId + "&Category=" + Category + "&Gender=" + Gender + "&PhyH=" + PhyH + "&WhProStu=" + WhProStu + "&ProjName=" + ProjName, 'report', 'width=1100,height=600,menubar=no,titlebar=no,toolbar=no,status=no,scrollbars=yes,dependent=yes,resizable=yes', false);
             return false;
         }
    </script>

    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
 <tr>
                    <td colspan="2" style="width: 66%;" valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Institutes"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">&nbsp;</td>
                </tr>
         <tr class="even">
                    <td colspan="2" style="width: 66%;" valign="top">
                        <asp:TextBox Style="width: 501px;" ID="txtInstitute" runat="server" Enabled="false" SkinID="txt248" Width="100%" ToolTip="Institute"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top"></td>
                </tr>
         <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Choose  Institute  &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top" colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:RadioButtonList ID="RdoAffInstOrNonAffInst" runat="server" RepeatDirection="Horizontal"
                                    TabIndex="2" Width="412px" AutoPostBack="True" OnSelectedIndexChanged="RdoAffInstOrNonAffInst_SelectedIndexChanged"
                                    Style="height: 27px" Font-Bold="True">
                                    <asp:ListItem Value="1">Accredited Centres</asp:ListItem>
                                    <asp:ListItem Value="0">Non Accredited Institute</asp:ListItem>
                                    <asp:ListItem Value="2">NIELIT Centre</asp:ListItem>
                                </asp:RadioButtonList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="RdoAffInstOrNonAffInst" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>         
        <tr>
             <td style="width: 33%;" valign="top">
                 <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Sub Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
             </td>
             <td style="width: 33%;" valign="top">
                 <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
             </td>
             <td style="width: 33%;" valign="top">
                 <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
             </td>
            
        </tr>
        <tr class="even">
          
             <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlSubcentreName" Width="100%" runat="server" SkinID="ddl250" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlSubcentreName_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
             </td>
            <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcoursecategory" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
            </td>
              <td style="width: 33%;" valign="top">
                 <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                        <asp:DropDownList ID="ddlcourseName" runat="server" SkinID="ddl250"  OnSelectedIndexChanged="ddlcourseName_SelectedIndexChanged"
                                    AutoPostBack="true" >
                             <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                                                </ContentTemplate>
</asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td >
                <asp:Label ID="Label22" runat="server" SkinID="CaptionLabel" Text="Batch Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Cast Category"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Gender"></asp:Label></td>
        </tr>
        <tr class="even" >
            <td>
                 <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                <asp:DropDownList ID="ddlbatchname" Width="100%" runat="server" >
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList> 
                                                </ContentTemplate>
</asp:UpdatePanel>              
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                        <asp:DropDownList ID="ddlCategory" Width="100%" runat="server" SkinID="ddl250" >
                             <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                        </asp:DropDownList>
                                                </ContentTemplate>
</asp:UpdatePanel>

                 </td> 
            <td>  <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                        <asp:DropDownList ID="ddlGender" Width="100%" runat="server" SkinID="ddl250" >
                             <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                            <asp:ListItem Value="Male" Text="Male"></asp:ListItem>
                            <asp:ListItem Value="Female" Text="Female"></asp:ListItem>
                        </asp:DropDownList>
                                                </ContentTemplate>
</asp:UpdatePanel></td> 

        </tr>
        <tr>
            <td >
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Whether Physically Handicapped"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Whether project student"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Project name"></asp:Label></td>
        </tr>
         <tr class="even" >
            <td>
                 <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                            <ContentTemplate>
                <asp:DropDownList ID="ddlPhyHanidcapped" Width="100%" runat="server" >
                                             <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                            <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                        </asp:DropDownList> 
                                                </ContentTemplate>
</asp:UpdatePanel>              
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                            <ContentTemplate>
                        <asp:DropDownList ID="ddlWhProjectStudent" Width="100%" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlWhProjectStudent_SelectedIndexChanged"
                                    AutoPostBack="true" >
                             <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                            <asp:ListItem Value="2" Text="No"></asp:ListItem>
                        </asp:DropDownList>

                                               
                                                </ContentTemplate>
</asp:UpdatePanel>

                 </td> 
            <td>  <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                            <ContentTemplate>
                        <asp:DropDownList ID="ddlProjectName" Width="100%" runat="server" SkinID="ddl250" >
                             <asp:ListItem Value="0" Text="--All--"></asp:ListItem>                           
                        </asp:DropDownList>
                                                </ContentTemplate>
</asp:UpdatePanel></td> 

        </tr>
    </table> 
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
        <asp:HiddenField ID="NIELITCentreId" runat="server" />
                    <asp:HiddenField ID="HNANFL" runat="server" />
                    <asp:HiddenField ID="HNonAfflAfflInst" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>           
         <div style="text-align: right; margin-top: 10px">             
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />

         </div>                 
</asp:content>

<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>

<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>