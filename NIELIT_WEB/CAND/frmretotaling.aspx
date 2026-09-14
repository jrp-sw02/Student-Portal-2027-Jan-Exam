<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="frmretotaling.aspx.cs" Inherits="CAND_frmretotaling" %>

<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
<asp:Label ID="lblHeading" runat="server" Text="Retotaling"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
<uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
 
               
                                <table class="gdbody" cellspacing="0" cellpadding="0" id="cphContents_gvMain" width="100%">
                                    <tr class="gdheader">
                                        <th scope="col" style="width: 3%;">
                                            #
                                        </th>
                                        <th scope="col" style="width: 20%;">
                                            Module Code
                                        </th>
                                        <th scope="col" style="width: 20%;">
                                            Module Name
                                        </th>
                                        <th scope="col" style="width: 20%;">
                                            Status</th>
                                        <th scope="col" style="width: 15%;">
                                            Grade</th>
                                        <th scope="col" style="width: 15%;">
                                            </th>
                                    </tr>
                                    <tr class="gdrow">
                                        <td>
                                            1
                                        </td>
                                        <td>
                                            M1</td>
                                        <td>
                                            Operating system
                                        </td>
                                        <td>
                                            Pass</td>
                                        <td>
                                            A</td>
                                        <td align="center">
                                            <input id="Checkbox2" type="checkbox" /></td>
                                    </tr>
                                    <tr class="gdalternate">
                                        <td>
                                            2
                                        </td>
                                        <td>
                                            M2</td>
                                        <td>
                                            Information Technology
                                        </td>
                                        <td>
                                            Pass
                                        </td>
                                        <td>
                                            B</td>
                                        <td align="center">
                                           
                                            <input id="Checkbox3" type="checkbox" /></td>
                                    </tr>
                                    <tr class="gdalternate">
                                        <td>
                                            3</td>
                                        <td>
                                            M3</td>
                                        <td>
                                            C</td>
                                        <td>
                                            Fail</td>
                                        <td>
                                            F</td>
                                        <td align="center">
                                            <input id="Checkbox4" type="checkbox" /></td>
                                    </tr>
                                    <tr class="gdalternate">
                                        <td>
                                            4</td>
                                        <td>
                                            M4</td>
                                        <td>
                                            Internet</td>
                                        <td>
                                            Fail</td>
                                        <td>
                                            F</td>
                                        <td align="center">
                                            <input id="Checkbox5" type="checkbox" /></td>
                                    </tr>
                                    <tr class="gdalternate">
                                        <td>
                                            5</td>
                                        <td>
                                            M5</td>
                                        <td>
                                            DS</td>
                                        <td>
                                            Fail</td>
                                        <td>
                                            F</td>
                                        <td align="center">
                                            <input id="Checkbox6" type="checkbox" /></td>
                                    </tr>
                                </table>
                                <div ALIGN="right">
                                    <asp:Button ID="btnsubmit" runat="server" Text="Apply Payment" 
                                        onclick="btnsubmit_Click" />
                                    <asp:Button ID="Button2" runat="server" Text="Cancel" />
                                </div>
                         
                    
             
           
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

