<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="CHMTResultUpload.aspx.cs" Inherits="HO_CHMTResultUpload" Debug="true" EnableViewState="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style2
        {
            width: 338px;
        }
        .auto-style3
        {
        }
        .form-container
        {
            width: 50%;
            margin: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
     CHM-T O Level Result Upload
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">


    <script type="text/javascript">
        function handleClick() {


            var lblError = document.getElementById('<%=lblError.ClientID %>');l
            var lblCount = document.getElementById('<%= lblcount.ClientID %>');
            var lblPortCount = document.getElementById('<%= lblPortCount.ClientID %>');
            var lblCompileCount = document.getElementById('<%= lblCompileCount.ClientID %>');
            var theoryMarksUploaded = document.getElementById('<%= rbtnTheoryMarks.ClientID %>').checked;
            var practicalMarksUploaded = document.getElementById('<%= rbtnPracticalMarks.ClientID %>').checked;


           // var ddlExam = document.getElementById('ddlExam');
            // Hide the count label when either radio button is clicked
            if (lblError) {
                lblError.style.display = 'none';
            }

            
        if (lblCount ) {
            lblCount.style.display = 'none';
        }

        if (lblPortCount) {
            lblPortCount.style.display = 'none';
        }
        if (lblCompileCount) {
            lblCompileCount.style.display = 'none';
        }

       // if (theoryMarksUploaded) {
            //var btnTransfer = document.getElementById('<%= btnTransfer.ClientID %>') || document.querySelector('[id$="btnTransfer"]');
           // document.getElementById('<%= btnTransfer.ClientID %>').disabled = true;
      //  }
            var btnTransfer = document.getElementById('cphContents_btnTransfer');
            if (btnTransfer) {
                btnTransfer.disabled = theoryMarksUploaded || practicalMarksUploaded;
            }



        checkMarksUploadStatus();
        }

        function checkMarksUploadStatus() {
            // var btnTransfer = document.getElementById('<%= btnTransfer.ClientID %>');
            var btnTransfer = document.getElementById('<%= btnTransfer.ClientID %>') || document.querySelector('[id$="btnTransfer"]');

            //console.log('<%= btnTransfer.ClientID %>');
           // var btnTransfer =1;
           // console.log(btnTransfer);
            if (btnTransfer) {
                var theoryMarksUploaded = document.getElementById('<%= rbtnTheoryMarks.ClientID %>').checked;
        var practicalMarksUploaded = document.getElementById('<%= rbtnPracticalMarks.ClientID %>').checked;

        // Enable or disable the Transfer Marks button based on conditions
        if (theoryMarksUploaded && practicalMarksUploaded) {
            document.getElementById('<%= btnTransfer.ClientID %>').disabled = false;
            document.getElementById('<%= btnCompile.ClientID %>').disabled = false;
        } else {
            document.getElementById('<%= btnTransfer.ClientID %>').disabled = true;
            document.getElementById('<%= btnCompile.ClientID %>').disabled = true;
        }
    } else {
        console.error("Button not found in the DOM.");
    }
}

        
</script>


    <div>
        <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
            width="100%">
            <tr class="even">

                <td colspan="2">
                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                </td>

            </tr>
            <tr>
                <td >
                   <asp:HyperLink ID="lnkDownloadTh" runat="server" Text="Download Theory Exam Excel Template" NavigateUrl="~/UploadedFiles/CHMT-Theory.xlsx"></asp:HyperLink>

                </td>
                <td >
                    <asp:HyperLink ID="lnkDownloadPr" Text="Download Practical Exam Excel Template" runat="server" NavigateUrl="~/UploadedFiles/CHMT_Prac.xlsx"></asp:HyperLink>
                </td>
            </tr>
            


            <tr class="odd">
                <td colspan="1" class="auto-style2">
                    <label id="Label1" runat="server">Exam </label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlExam" runat="server" Height="21px" SkinID="ddl250"
                                OnSelectedIndexChanged="ddlExam_SelectedIndexChanged" AutoPostBack="True" Width="158px" onclick="handleClick();">
                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1"
                        runat="server"
                        ControlToValidate="ddlExam"
                        InitialValue="0"
                        ErrorMessage="Please select a value from the list."
                        Display="Dynamic"
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>            
            <tr class="odd">
                <td colspan="1" class="auto-style2">
                    <asp:RadioButton ID="rbtnTheoryMarks" runat="server" Text="Theory Marks" GroupName="MarksType" OnCheckedChanged="rbtnTheoryMarks_CheckedChanged" AutoPostBack="true"  OnClientClick="handleClick();" />

                </td>
                <td>
                    <asp:RadioButton ID="rbtnPracticalMarks" runat="server" Text="Practical Marks" GroupName="MarksType" OnCheckedChanged="rbtnPracticalMarks_CheckedChanged" AutoPostBack="true"  OnClientClick="handleClick();" />


                </td>
            </tr>
            


            <tr class="odd">
                <td colspan="2" class="auto-style3">
                    <asp:FileUpload ID="flUpload" runat="server" Width="485px" />
                    <asp:HiddenField ID="flpath" runat="server" />
                </td>
            </tr>

        </table>
    </div>

     <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnValidate" runat="server" Text="Upload" OnClick="btnValidate_Click" Width="210px" Height="26px" />
          
         <table width="100%">
             <tr>
                <td style="text-align: left">
                    <asp:Label ID="lblcount" runat="server"  Visible="false" ForeColor="Red"></asp:Label>

                </td>
            </tr>          
             <tr>
                <td style="text-align: right">
                   
                     <asp:Button ID="btnTransfer" runat="server"   Text="Validate Data"  Width="210px" Height="26px" OnClick="btnTransfer_Click"  Enabled="false" />

                </td>
            </tr>          
             <tr>
                <td style="text-align: left">
                   
                    <asp:Label ID="lblPortCount" runat="server"  Visible="false" ForeColor="Red"></asp:Label>

                </td>
            </tr>          
             <tr>
                <td style="text-align: right">
                   
              <asp:Button ID="btnCompile" runat="server" Text="Compile Result"  Width="210px" Height="26px" OnClick="btnCompile_Click" Enabled="false"   />

                </td>
            </tr>          
             <tr>
                <td style="text-align: left">
                   
                    <asp:Label ID="lblCompileCount" runat="server"  Visible="false" ForeColor="Red"></asp:Label>

                </td>
            </tr>          
                  
             <tr>
                <td style="text-align: right">                   
                    <asp:Button ID="btnFinalize" runat="server" Text="Finalize Result"  Width="210px" Height="26px"  Enabled="true"  OnClick="btnFinalize_Click"   /> 
                </td>
            </tr>          
             <tr>
                <td style="text-align: left">                   
                   <asp:Label ID="lblFinalizeCount" runat="server"  Visible="false" ForeColor="Red"></asp:Label>
                </td>
            </tr>          
         </table>
           
            </div>
    <div id="divGrid">
        <asp:GridView ID="grdMismatch" runat="server" Visible="false" AutoGenerateColumns="false" OnRowDataBound="grdMismatch_RowDataBound"  style="margin: 0 auto;" Width="100%">
            <AlternatingRowStyle CssClass="alternateRow" />
            <Columns>
                <asp:BoundField DataField="SrNo" HeaderText="SrNo." SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Reg_no" HeaderText="Registration No" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
                 <asp:BoundField DataField="Module_Code" HeaderText="Module Code/ ID" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Result" HeaderText="Error" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
               <%-- <asp:BoundField DataField="moduleCode" HeaderText="Module Code" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="levelCode" HeaderText="Level Code" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />--%>
            </Columns>
        </asp:GridView>
    </div>
      <div id="divNavigation" runat="server">
                <uc3:PagingBar ID="PagingBar1" runat="server"  />
      </div>      

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

