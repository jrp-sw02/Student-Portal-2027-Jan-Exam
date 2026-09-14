<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdmitCardUpload_DLC.aspx.cs" Inherits="Admin_CertificateExamAdmitCard" %>--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" Async="true" AutoEventWireup="true"
    CodeFile="BSBSchoolDataUpload.aspx.cs" Inherits="Admin_BSBSchoolDataUpload" Debug="true" Culture="auto" UICulture="auto" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
    </style>
</asp:Content>
    
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">    
    <asp:Label ID="lblHeading" runat="server" Text="Import BSB School Data" meta:resourcekey="lblHeadingResource1"></asp:Label>
    <br />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    
    
    <script language="javascript" type="text/javascript">
        function ValidateLogin() {
            return true;
        }

        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }

        function ValidateFormFields() {
<%--            if (!isSelected("<%=ddlProject.ClientID %>", "Project"))
                return false;--%>
            if (!isBlank("<%=flUpload.ClientID %>", "Browse File Upload"))
                return false;
        }
    </script>


    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">

        <asp:View ID="New" runat="server">

            <table class="sample2" cellpadding="2" cellspacing="0">
               <%--
                   <tr>
                        <td class="auto-style1">
                            <div style="width: 33%; float: left">
                                <asp:Label ID="lblProject" runat="server" Text="Project &lt;font color='RED'&gt;*&lt;/font&gt; : "></asp:Label>
                            </div>
                        </td>

                        <td class="auto-style1">
                            <div style="width: 33%; float: left">
                                <asp:DropDownList ID="ddlProject" runat="server" SkinID="ddl253" Width="100%" AutoPostBack="True">
                                    <asp:ListItem>-- Select Project --</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                   --%>


                <tr>
                  
                    <td>
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Upload MS-Excel File (.XLS/.XLSX)" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                    <td>
                        <asp:FileUpload ID="flUpload" Width="485px" runat="server" />
                    </td>
                      <td>
                        <asp:HyperLink 
                            ID="lnkDownloadFormat" runat="server" NavigateUrl="~/download/BSB_Template.xlsx" Text="Download Excel Format" Target="_blank" />
                     </td>
                </tr>


                <tr>
                    <td colspan="3" style="text-align: right">
                        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload File" OnClientClick="return ValidateFormFields()" />
                        <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" />
                    </td>
                </tr>


                <tr class="even">
                    <td colspan="3" valign="top">
                        <asp:Label ID="lblCount" runat="server" SkinID="CaptionLabel" Text=""></asp:Label>
                        <asp:Label ID="lblLastId" runat="server" SkinID="CaptionLabel" Text="" Visible="false"></asp:Label>
                        <asp:Button ID="DownloadFailureData" runat="server" OnClick="DownloadFailureData_Click" Text="Download UnImported Data" Visible="false" />
                    </td>
                </tr>


            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
