<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="PhaseDetailRegisterFilter.aspx.cs" Inherits="PhaseDetailRegisterFilter" Debug="true" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    Download Phase Detail Register
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server"> 

    <script type="text/javascript" language="javascript">

        function OpenWindow() {

           

            //Course Category
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlDataDownloadedSequence.ClientID %>", "Data Download Sequence Number"))
                return false;

            alert("Download started it may take time. Please Check download folder after download finished !");
            /* JavaScript
  code  */
 
            //var dataDownloadedSequenceId
            //if (document.getElementById('<%=ddlDataDownloadedSequence.ClientID %>') && document.getElementById('<%=ddlDataDownloadedSequence.ClientID %>').value == "0")
                // dataDownloadedSequenceId = 0;
            // else
                // dataDownloadedSequenceId = document.getElementById('<%=ddlDataDownloadedSequence.ClientID %>').value;

           // var courseID
            //if (document.getElementById('<%=ddlCourseName.ClientID %>') && document.getElementById('<%=ddlCourseName.ClientID %>').value == "0")
               // courseID = 0;
            //else
                //courseID = document.getElementById('<%=ddlCourseName.ClientID %>').value;

            //var phaseId
           // if (document.getElementById('<%=ddlPhaseNumber.ClientID %>') && document.getElementById('<%=ddlPhaseNumber.ClientID %>').value == "0")
              // phaseId = 0;
           //else
               // phaseId = document.getElementById('<%=ddlPhaseNumber.ClientID %>').value;


            //window.open("DownloadPhaseDetailRegister.aspx?dataDownloadedSequenceId=" + dataDownloadedSequenceId + "&courseId=" + courseID + "&phaseId=" + phaseId, 'report', 'width=1100,height=600', false);
            //window.open("DownloadPhaseDetailRegister.aspx?dataDownloadedSequenceId=" + dataDownloadedSequenceId + "&courseId=" + courseID , 'report', 'width=1100,height=600', false);
            //window.open("DownloadPhaseDetailRegister.aspx?dataDownloadedSequenceId=" + dataDownloadedSequenceId , 'report', 'width=1100,height=600', false);
        }
    </script>
     <style type="text/css">
         .PromptCSS {
             color: Blue;
             font-size: small;
             font-style: italic;
             font-weight: bold;
             font-family: CourierNew;
             height: 20px;
             margin-left: 100px;
         }
     </style> 

    <asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Data Downloaded Sequence &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                      <asp:DropDownList ID="ddlCourseCategry" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged"  Enabled="false"  >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                         <asp:DropDownList ID="ddlDataDownloadedSequence" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlDataDownloadedSequence_SelectedIndexChanged" AutoPostBack="True" >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>                 
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                    <ContentTemplate>
                       <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDataDownloadedSequence" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
     
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Phase Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
               <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Certificate Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label> 
            </td>
            <td>
               
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                       <asp:DropDownList ID="ddlPhaseNumber" runat="server" Height="22px" SkinID="ddl250">
                           
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
               <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                       <asp:DropDownList ID="ddlType" runat="server" Height="22px" SkinID="ddl250">                           
                           <asp:ListItem Value="0">--Select One--</asp:ListItem>
                           <asp:ListItem Value="1"> NCVET</asp:ListItem>
                           <asp:ListItem Value="2">NON-NCVET</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </td>
            <td>
              
            </td>
        </tr>
        <tr>
            <td >             
            </td>
            <td colspan="2">
                <asp:updatepanel id="Updatepanel13" runat="server">
                    <contenttemplate>
                        <asp:label id="lblNoRecord" runat="server"   visible="False" Font-Bold="True" ForeColor="Red"></asp:label>
                 </contenttemplate> 
                                      
                </asp:updatepanel>
            </td>
        </tr>
    </table>  
    <div style="text-align: right; margin-top: 10px">
         
        <asp:button id="btnCertifcateDownload" runat="server" text="Download MarkSheet" onclientclick="return OpenWindow();" 
           onclick="btnCertifcateDownload_Click" />&nbsp;&nbsp; 
        <asp:button id="btnView" runat="server" text="Phase Detail Register Download" onclientclick="return OpenWindow();" 
            onclick="btnView_Click" />
                       
        <asp:button id="btnReset" runat="server" text="Reset" onclick="btnReset_Click" />
       <label id="lblwait" style="display:none" runat="server"> Please Wait... </label>
    </div>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
