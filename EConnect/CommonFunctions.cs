using System;
using System.Text;

namespace EConnect
{
    public class CommonFunctions
    {
        public static String GenerateRandomNumber(Int32 numberLenght)
        { 
            Random random = new Random();
            String randomNumber = "";
            for(Int32 index =0; index <numberLenght; index++)
            {
                randomNumber = string.Concat(randomNumber,random.Next(9).ToString());
            }
            return randomNumber;
        }
        // added by amit for captcha
        public static String GenerateCaptchaCode(Int32 length)
        {
            const string chars = "ABCDEFGHIJKL0123456789MNOPQRSTUVWXYZ0123456789abcdefghjkmnpqrstuvwxyz0123456789";
            StringBuilder captcha = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                captcha.Append(chars[index]);
            }

            return captcha.ToString();
        }
        // added by amit for captcha

    }
}
