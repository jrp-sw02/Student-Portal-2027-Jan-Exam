
namespace EConnect.Utils.Common
{
    public class Validation
    {
        //public enum TextBoxValidation
        //{ 
        //    IsBlank=1,
        //    IsNumber=2,
        //    IsDate =3,
        //    MobileNumber,
        //    PhoneNumber,
        //    EmailAddress,
        //    NumericValue,
        //    StringValue,
        //    IsBlankStringValue,
        //    IsBlankNumericValue,
        //    isDate
        //}
    }

    //public enum ErrorMgsCode
    //{ 
    
    
    
    
    //}
    //public enum ValidationsForTextbox
    //{ 
    //    MobileNumber,
    //    PhoneNumber,
    //    EmailAddress,
    //    NumericValue,
    //    StringValue,
    //    IsBlankStringValue,
    //    IsBlankNumericValue,
    //    isDate
    //    }
    //public enum ValidationsForDropdown
    //{
    //   isSelected
    //}
    //public static bool VFunctions(ref TextBox tb, ValidationsForTextbox validtype)
    //{
    //    if (validtype == ValidationsForTextbox.IsBlankStringValue)
    //    {
    //        if (!string.IsNullOrEmpty(tb.Text))
    //        {
    //            return true;
    //        }
    //      }

    // //validation for Email Address
    //    if (validtype == ValidationsForTextbox.EmailAddress )
    //    {
    //        if (!string.IsNullOrEmpty(tb.Text))
    //        {
    //           Regex emailregex = new Regex("(?<user>[^@]+)@(?<host>.+)");
    //           string s = tb.Text; 
    //            Match m = emailregex.Match(s);
    //            if (m.Success) 
    //            {
    //                return true; 
    //            }

    //        }
    //      }
    //      if (validtype == ValidationsForTextbox.NumericValue )
    //      {
    //          if (!string.IsNullOrEmpty(tb.Text))
    //          {
    //              //String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
    //              String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
    //              Regex objNumberPattern = new Regex( strValidIntegerPattern );
    //              return objNumberPattern.IsMatch(tb.Text );
    //          }
    //      }
    //      if (validtype == ValidationsForTextbox.StringValue)
    //      {
    //          if (!string.IsNullOrEmpty(tb.Text))
    //          {
    //              //String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
    //              String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
    //              Regex objNumberPattern = new Regex(strValidIntegerPattern);
    //              return !objNumberPattern.IsMatch(tb.Text);
    //          }
    //      }
    //    return false;
    
    //}

    //public static bool VFunctions(ref DropDownList  db, ValidationsForDropdown validtype)
    //{
    //    return true;

    //}
}
