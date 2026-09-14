<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeFile="CandidateUpdateRequest.aspx.cs"
    Inherits="CAND_CandidateUpdateRequest" Debug="true" %>

<%@ Register Src="~/UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>



<!DOCTYPE html>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Correction/Addition Form</title>
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script src="../Script/jquery-1.7.1.js" type="text/javascript"></script>
    <script type="text/javascript">

        document.onkeydown = function (e) {
            if (event.keyCode == 123) {
                return false;
            }
            if (e.ctrlKey && e.shiftKey && e.keyCode == 'I'.charCodeAt(0)) {
                return false;
            }
            if (e.ctrlKey && e.shiftKey && e.keyCode == 'J'.charCodeAt(0)) {
                return false;
            }
            if (e.ctrlKey && e.keyCode == 'U'.charCodeAt(0)) {
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>

    <script language="javascript" type="text/javascript">
        function ValidateHandicapped() {


        }
        function ValidateMaritalStatus() {

            if (!isSelected("ddlMStatus", "Marital status"))
                return false;
        }
        function ValidateForm() {

            if (ChkHandicapped.checked == true) {
                if (!isvalidateRadioButtonList("Rdhandicapped", "Disability"))
                    return false;
            }
            if (ChkMaritalStatus.checked == true) {
                if (!isSelected("ddlMStatus", "Marital status"))
                    return false;
            }
            if (ChkCategory.checked == true) {
                if (!isSelected("ddlCategory", "Cast Category"))
                    return false;
            }
            if (chkMob.checked == true) {
                if (!isBlankNumber("txtCorMobileNo", "Mobile Number"))
                    return false;
                if (!isNumber("txtCorMobileNo"))
                    return false;
                if (!chekMobNo("txtCorMobileNo"))
                    return false;
            }
            if (ChkEmail.checked == true) {
                if (!isBlank("txtEmailId", "Email Id"))
                    return false;
                if (!isValidEmail("txtEmailId", "Invalid E-Mail ID"))
                    return false;
            }
            if (ChkCorAddress.checked == true) {
                if (!isBlank("TxtCorAddressLine1", "AddressLine1"))
                    return false;
                if (!isBlank("TxtCorAddressLine2", "AddressLine2"))
                    return false;
                //if (!isBlank("TxtCorAddressLine3", "AddressLine3"))
                //    return false;
                if (!isBlank("TxtCorCity", "City Name"))
                    return false;
                if (!CheckNumberPresent("TxtCorCity", "Numeric characters are not allowed"))
                    return false;
                if (!isSpecialCharacter("TxtCorCity", "Special characters are not allowed"))
                    return false;

                if (!isSelected("ddlCorState", "Sate Name"))
                    return false;

                if (!isSelected("ddldistrict", "District Name"))
                    return false;

                if (!isBlankNumber("txtCorPinCode", "Pin Code"))
                    return false;
                if (!isNumber("txtCorPinCode"))
                    return false;

                if (!IsValidMinMaxLenght("txtCorPinCode", 6, 6, "Invalid Pin Code"))
                    return false;

            }
            if (ChkEducation.checked == true) {
                if (!isSelected("DDLeducode", "Highest Qualification"))
                    return false;
            }

        }

    </script>
    <style type="text/css">
        .modalPopup {
            border: 3px solid #31597C;
            background-color: #E6F0F0;
            padding-top: 0px;
            padding-right: 0px;
            width: 330px;
            height: 165px;
            top: -620px;
            left: 320px;
            position: relative;
        }

        .modalPopup1 {
            border: 3px solid #31597C;
            background-color: #E6F0F0;
            padding-top: 0px;
            padding-right: 0px;
            width: 330px;
            height: 165px;
            top: -10px;
            left: 320px;
            position: relative;
        }
        /*Added 25 April 2019*/
        .modalPopup5 {
            border: 3px solid #31597C;
            background-color: #E6F0F0;
            padding-top: 2px;
            padding-right: 2px;
            padding-left: 2px;
            padding-bottom: 2px;
            width: 443px;
            height: auto;
            top: 257px;
            left: 440px;
            position: relative;
        }

        .auto-style1 {
            width: 56%;
        }
    </style>
</head>
<body style="background-color: white;" oncontextmenu="return false;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px" runat="server">
                <tr>
                    <td>
                        <uc1:NormalHeader ID="NormalHeader1" Visible="false" runat="server" />

                    </td>
                </tr>

                <tr>

                    <td align="center">
                        <strong style="text-align: center" class="headfont">FORM FOR CORRECTION/UPDATION IN EXISTING REGISTGRATION DETAILS&nbsp; 
                
                        <asp:Label ID="Lblhead" runat="server" Text=""></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="Label2" runat="server" Text="<font color='red'>* Please Select one or more fields for Update Request.</font>">
                        </asp:Label>
                    </td>
                </tr>
                <tr id="trEditHead">
                    <td align="center">
                        <strong style="text-align: center" class="headfont">
                            <asp:Label ID="Label9" runat="server" Text="<font color='blue'><I>Form for Updation existing Request, Add more field logout and change.</I></font>">
                            </asp:Label></strong>
                    </td>
                </tr>

                <tr>
                    <td align="right">
                        <asp:Label ID="lblmandatory0" runat="server" Text="<font color='red'>*</font>  (Mandatory fields)">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False"
                                    Width="99%"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>

                <tr>
                    <td align="left" valign="top">
                        <table id="TblFormDetail" runat="server" class="sample3" style="width: 100%; text-align: left"
                            border="0" cellpadding="3" cellspacing="1">
                            <tr class="head1">
                                <td>SI No. </td>
                                <td>CORRECTION/UPDATION DETAILS 
                                </td>
                                <td class="auto-style1">EIXTING DETAILS 
                                </td>
                                <td>DESIRED UPDATE
                                </td>
                                <td>SELECT FOR UPDATE
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="3%" valign="top">1.
                                </td>
                                <td width="40%" valign="top">
                                    <asp:Label ID="Label8" runat="server" Text="Registration Level <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td align="left" style="width: 50%" colspan="3">
                                    <asp:Label ID="LblCourseLevel" Font-Size="Large" Font-Bold="True" BackColor="#E6FFE6"
                                        Width="143" BorderStyle="Solid"
                                        BorderWidth="1px" Height="22px" TabIndex="5" runat="server"></asp:Label>
                                </td>
                                <%--<td align="left" style="width: 50%">
                                <asp:DropDownList ID="ddlCourseLvl" runat="server" AutoPostBack="True" Width="520px" SkinID="ddl760">
                                    <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>--%>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="3%" valign="top">2.
                                </td>
                                <td width="40%" valign="top">
                                    <asp:Label ID="Label1" runat="server" Text="Registration Numbar">
                                    </asp:Label>
                                </td>
                                <td width="50%" colspan="3">
                                    <asp:Label ID="Lbregno" Font-Size="Large" Font-Bold="True" BackColor="#E6FFE6"
                                        Width="143px" BorderStyle="Solid"
                                        BorderWidth="1px" Height="22px" TabIndex="5" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <%--<tr class="gdrow1">
                          <td width="3%" valign="top">
                             3.
                              </td>
                          <td width="40%" valign="top">
                      <asp:Label ID="Label2" runat="server" Text="Description <font color='RED'>*</font>">
                                </asp:Label>
                          </td>
                           <td width="10%">
                                 <asp:Label ID="LblDescription" runat="server"></asp:Label>
                            </td>
                           <td width="10%">
                                <asp:DropDownList ID="ddlDescription" runat="server" Width="375px" 
                                    AutoPostBack="True" TabIndex="3">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                      </tr>--%>
                            <tr class="gdalternate1">
                                <td width="3%" valign="top">3.
                                </td>
                                <td width="40%" valign="top">
                                    <asp:Label ID="Label3" runat="server" Text="Handicapped (Disability) <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td width="40%" valign="top">
                                    <asp:Label ID="LblHandicapped" runat="server"></asp:Label>
                                </td>
                                <td width="50%">
                                    <asp:RadioButtonList ID="Rdhandicapped" runat="server" RepeatDirection="Horizontal"
                                        TabIndex="18">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Yes </asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkHandicapped" runat="server" Width="10%" AutoPostBack="true" OnClientClick="return ValidateHandicapped();" />

                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="3%" valign="top">4.
                                </td>
                                <td width="40%" valign="top">
                                    <asp:Label ID="Label4" runat="server" Text="Marital Status <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:Label ID="LblMaritalStatus" runat="server"></asp:Label>
                                </td>
                                <td width="10%">
                                    <asp:DropDownList ID="ddlMStatus" runat="server" Width="375px" TabIndex="16">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkMaritalStatus" runat="server" AutoPostBack="true" OnClientClick="return ValidateMaritalStatus();" /></td>
                            </tr>

                            <tr class="gdalternate1">
                                <td width="3%" valign="top">5.
                                </td>
                                <td width="40%" valign="top">
                                    <asp:Label ID="Label5" runat="server" Text="Caste <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:Label ID="LblCategory" runat="server"></asp:Label>
                                </td>
                                <td width="10%">
                                    <asp:DropDownList ID="ddlCategory" runat="server" TabIndex="17" Width="375px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkCategory" runat="server" AutoPostBack="true" /></td>
                            </tr>

                            <tr class="gdrow1">
                                <td width="3%" valign="top">6.
                                </td>
                                <td width="40%" valign="top">
                                    <asp:Label ID="Label7" runat="server" Text="Mobile Number &lt;font color='RED'&gt;*&lt;/font&gt; ">
                                    </asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:Label ID="LblMobile" runat="server"></asp:Label>
                                </td>
                                <td width="10%">
                                    <asp:TextBox ID="txtCorMobileCode" runat="server" Enabled="false" Text="+91" Width="65px"
                                        TabIndex="23" onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                    <asp:TextBox ID="txtCorMobileNo" runat="server" MaxLength="10" MinLenghth="10" oncopy="return false"
                                        onkeypress="checkNumber(this,10,10,event);" onpaste="return false"
                                        TabIndex="24" Width="280px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkMob" runat="server" AutoPostBack="true" /></td>
                            </tr>

                            <tr class="gdalternate1">
                                <td width="3%" valign="top">7.
                                </td>
                                <td width="40%" valign="top">
                                    <asp:Label ID="Label6" runat="server" Text="Email Address &lt;font color='RED'&gt;*&lt;/font&gt; ">
                                    </asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:Label ID="LblEmail" runat="server"></asp:Label>

                                </td>
                                <td width="10%">
                                    <asp:TextBox ID="txtEmailId" runat="server" MaxLength="150" TabIndex="25" Width="520px"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox><br />
                                    (e.g.abc@yahoo.com)
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEmail" runat="server" AutoPostBack="true" /></td>

                            </tr>
                            <tr class="head1">
                                <td colspan="4">8. Correspondence Address Details 
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkCorAddress" runat="server" AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>8.1
                                </td>
                                <td>
                                    <asp:Label ID="Label83" runat="server" Text="Address Line1 <b class='mandatory'>*</b>">
                                    </asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:Label ID="LblCorAddressLine1" runat="server"></asp:Label>
                                </td>
                                <td>&nbsp;<asp:TextBox ID="TxtCorAddressLine1" runat="server" Width="520px" TabIndex="34" MaxLength="30"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                                    <br />
                                </td>
                                <td>
                                    <%-- <asp:CheckBox ID="ChkAdd1" runat="server" AutoPostBack="true"/>--%></td>

                            </tr>
                            <tr class="gdalternate1">
                                <td>8.2
                                </td>
                                <td>
                                    <asp:Label ID="Label84" runat="server" Text="Address Line2<b class='mandatory'>*</b>">
                                    </asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:Label ID="LblCorAddressLine2" runat="server"></asp:Label>

                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCorAddressLine2" runat="server" Width="520px" TabIndex="35" MaxLength="30"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />
                                </td>
                                <td>
                                    <%--   <asp:CheckBox ID="ChkAdd2" runat="server" AutoPostBack="true" />--%></td>

                            </tr>
                            <tr class="gdrow1">
                                <td>8.3
                                </td>
                                <td>
                                    <asp:Label ID="Label85" runat="server" Text="Address Line3"></asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:Label ID="LblCorAddressLine3" runat="server"></asp:Label>


                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCorAddressLine3" runat="server" Width="520px" TabIndex="36" MaxLength="30"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                                <td>
                                    <%--<asp:CheckBox ID="ChkAdd3" runat="server" AutoPostBack="true"/>--%></td>

                            </tr>
                            <tr class="gdalternate1">
                                <td width="3%">8.4
                                </td>
                                <td>
                                    <asp:Label ID="LblCity" runat="server" Text="City Name  <b class='mandatory'>*</b>">
                                    </asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:Label ID="LblCorCity" runat="server"></asp:Label>

                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCorCity" runat="server" Width="520px" TabIndex="37" MaxLength="30"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />
                                    <td>
                                        <%--<asp:CheckBox ID="ChkCity" runat="server" AutoPostBack="true" />--%></td>

                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>8.5
                                </td>
                                <td>
                                    <asp:Label ID="Label33" runat="server" Text="State <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:Label ID="LblCorState" runat="server"></asp:Label>

                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCorState" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCorState_SelectedIndexChanged"
                                        TabIndex="38" Width="375px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%--<asp:CheckBox ID="ChkState" runat="server" AutoPostBack="true" />--%></td>

                            </tr>
                            <tr class="gdalternate1">
                                <td>8.6
                                </td>
                                <td>
                                    <asp:Label ID="Label34" runat="server" Text="District <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:Label ID="Lbldistrict" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddldistrict" runat="server" TabIndex="39" Width="375px">
                                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlCorState" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <%--<asp:CheckBox ID="ChkDistrict" runat="server" AutoPostBack="true" />--%></td>

                            </tr>
                            <tr class="gdrow1">
                                <td>8.7
                                </td>
                                <td>
                                    <asp:Label ID="Label39" runat="server" Text="Pin Code <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:Label ID="LblCorPinCode" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCorPinCode" runat="server" oncopy="return false" oncut="return false"
                                        onkeypress="checkNumber(this,6,0,event);" onpaste="return false" TabIndex="40"
                                        Width="93px" MaxLength="6">
                                    </asp:TextBox>
                                </td>
                                <td>
                                    <%--<asp:CheckBox ID="ChkPin" runat="server" AutoPostBack="true" />--%></td>
                            </tr>

                            <tr class="head1">
                                <td colspan="5">9. Educational / Qualification Details
                                </td>
                            </tr>
                            <tr class="gdalternate1" valign="top" id="cls1" runat="server">
                                <td>10
                                </td>
                                <td>
                                    <asp:Label ID="Label60" runat="server" Text="Highest Educational Qualification &lt;font color='RED'&gt;*&lt;/font&gt;">
                                    </asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:Label ID="LblLeducode" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDLeducode" runat="server" Width="530px" SkinID="25" TabIndex="41"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEducation" runat="server" AutoPostBack="true" /></td>

                            </tr>
                            <tr class="gdalternate1" id="TrIfother" runat="server" valign="top" visible="False">
                                <td></td>
                                <td>
                                    <asp:Label ID="Label81" runat="server" Text="If others specify">
                                    </asp:Label>
                                </td>
                                <td class="auto-style1"></td>
                                <td>
                                    <asp:TextBox ID="TxtEduIfOther" Width="520px" runat="server" MaxLength="50" TabIndex="42"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <br />
                                    <asp:Button ID="btnSave" runat="server" Text="Save/Preview" TabIndex="53"
                                        OnClientClick="return ValidateForm();" OnClick="btnSave_Click" />
                                    <%--  <asp:Button ID="btnupdate" runat="server" Text="Update" TabIndex="53" OnClick="btnupdate_Click" />--%>
                                    <asp:Button ID="btnback" runat="server" Text="Back" TabIndex="54" />
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="HiddenField2" runat="server" />
        <asp:ModalPopupExtender ID="AlertModalPopUp" runat="server" PopupControlID="PopUpPanel"
            PopupDragHandleControlID="PopupHeader" TargetControlID="HiddenField2" OkControlID="CancleBtn"
            X="300" Y="300">
        </asp:ModalPopupExtender>
    </form>
</body>
</html>
