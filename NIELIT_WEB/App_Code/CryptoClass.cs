using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using EConnect.DAL;
using EConnect.URM;


//namespace CypherExample
//{

    public class CryptoClass
    {
        protected static byte[] keybytes = new byte[16];
        protected AesManaged encryted;
        private static string encryptionKey = "bbdeaa53-deb9-45a1-b24f-8fbd28265113";
        //bbdeaa53-deb9-45a1-b24f-8fbd28265113
        private static string initialisationVector;
        // Singleton pattern used here with ensured thread safety
        protected static readonly CryptoClass _instance = new CryptoClass();
         public static CryptoClass Instance
        {
            get { return _instance; }
        }
        public CryptoClass()
        {

        }
        public class Employeedata
        {
            public string password;
            public string txnRequestID;
            public string demoAuth;
            public string bioAuth;
            public string aadhaarNumber;
            public string name;
            public string dob;
            public string gender;
            public string biodata;
            //public string Name;
            //public int Age;

        }

        public static void setSecretkeys(string keyvalue)
        {
            StringBuilder builder = new StringBuilder();
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  

                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(keyvalue));
                // sbyte[] signed = Array.ConvertAll(bytes, b => unchecked((sbyte)b));

                Array.Copy(bytes, keybytes, 16);

                //byte[] bytes = Encoding.Default.GetBytes(keyvalue);
                //keyvalue = Encoding.UTF8.GetString(bytes);

                //byte[] result;
                //SHA1 shaM = new SHA1Managed();
                //result = shaM.ComputeHash(bytes);                        

                //Array.Copy(result, keybytes, 16);

                //Console.WriteLine("This is the encryption key    " + Encoding.UTF8.GetString(keybytes));
            }
        }

//        public static string AESEncrypt(string plainText,string keyValue )//byte[] Key, byte[] IV
//        {
//            byte[] encrypted;

//            setSecretkeys(keyValue);

//            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
//            // Create a new AesManaged.    
//            using (AesManaged aes = new AesManaged())
//            {
//                aes.Key = keybytes; //UTF8.GetBytes(keyValue); 
//                aes.Mode = CipherMode.ECB;
//                aes.Padding = PaddingMode.PKCS7;
                

//                // Create encryptor    
//                ICryptoTransform encryptor = aes.CreateEncryptor(); //Key, IV
//                // Create MemoryStream    
//                using (MemoryStream ms = new MemoryStream())
//                {
//                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
//                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
//                    // to encrypt 
//                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
//                    {
//                        // Create StreamWriter and write data to a stream    
//                        using (StreamWriter sw = new StreamWriter(cs))
//                            sw.Write(plainText);
//                        encrypted = ms.ToArray();
//                    }
//                }
//            }

//            // Return encrypted data    

//            return System.Text.Encoding.UTF8.GetString(encrypted);
//          // return  ByteArrayToHexString(encrypted);
//           // return encrypted;
//        }
//}



//Added for changed version 17 may 2023
 public static string Encrypt(string PlainText, string secretKey)
        {
            AesManaged aes = new AesManaged();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Mode = CipherMode.ECB;
           // aes.Padding = PaddingMode.PKCS7;
            byte[] keyArr = generateAES256Key(secretKey);
            byte[] KeyArrBytes32Value = new byte[16];
            Array.Copy(keyArr, KeyArrBytes32Value, 16);
            aes.Key = KeyArrBytes32Value;
            ICryptoTransform encrypto = aes.CreateEncryptor();
            byte[] plainTextByte = ASCIIEncoding.UTF8.GetBytes(PlainText);
            byte[] CipherText = encrypto.TransformFinalBlock(plainTextByte, 0, plainTextByte.Length);
            return Convert.ToBase64String(CipherText);
        }

//Added for decryption for response by Epramaan
 public static string Decrypt(string PlainText, string secretKey)
 {
     AesManaged aes = new AesManaged();
     aes.BlockSize = 128;
     aes.KeySize = 256;
     aes.Mode = CipherMode.ECB;
     // aes.Padding = PaddingMode.PKCS7;
     byte[] keyArr = generateAES256Key(secretKey);
     byte[] KeyArrBytes32Value = new byte[16];
     Array.Copy(keyArr, KeyArrBytes32Value, 16);
     aes.Key = KeyArrBytes32Value;
     string plaintext = "";
     byte[] cipherText = Convert.FromBase64String(PlainText);
     // ICryptoTransform decrypto = aes.CreateDecryptor ();
     //byte[] plainTextByte = ASCIIEncoding.UTF8.GetBytes(PlainText);
     //byte[] CipherText = decrypto.TransformFinalBlock(plainTextByte, 0, plainTextByte.Length);
     // Create a decryptor to perform the stream transform.
     ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

     // Create the streams used for decryption.
     using (MemoryStream msDecrypt = new MemoryStream(cipherText))
     {
         using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
         {
             using (StreamReader srDecrypt = new StreamReader(csDecrypt))
             {

                 // Read the decrypted bytes from the decrypting stream
                 // and place them in a string.
                 plaintext = srDecrypt.ReadToEnd();
             }
         }



         return plaintext;
     }

     //  return Convert.ToBase64String(CipherText);
 }



        public static byte[] generateAES256Key(string seed)
        {
            SHA256 sha256 = SHA256CryptoServiceProvider.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(seed));
        }





      public static String AESEncrypt(string plainText, string keyValue)
         {
             setSecretkeys(keyValue);
            using (var aes = new AesManaged())
            {
                aes.Key = keybytes;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] data = Encoding.UTF8.GetBytes(plainText);
                        cs.Write(data, 0, data.Length);
                    }

                    byte[] encrypted = ms.ToArray();
                    String encryptedText = Convert.ToBase64String(encrypted);
                    return (encryptedText);
                }

//             byte[] encrypted;
//             setSecretkeys(keyValue);
//            // using (Aes aes = Aes.Create())
//             //{
//             System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
////            // Create a new AesManaged.    
//         using (AesManaged aes = new AesManaged())
//            {
//                 aes.KeySize = 256;
//                 aes.Key = keybytes; 
//                 // Create encryptor    
//                 ICryptoTransform encryptor = aes.CreateEncryptor();
//                 // Create MemoryStream    
//                 using (MemoryStream ms = new MemoryStream())
//                 {
//                     // Create crypto stream using the CryptoStream class. This class is the key to encryption    
//                     // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
//                     // to encrypt    
//                     using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
//                     {
//                         // Create StreamWriter and write data to a stream    
//                         using (StreamWriter sw = new StreamWriter(cs))
//                             sw.Write(plainText);
//                         encrypted = ms.ToArray();
//                     }
                 }
           //  }
        // return System.Text.Encoding.UTF8.GetString(encrypted);
           //  return encrypted ;
         }

         public static string AESDecrypt(string encText, string keyValue)
         {
             string decrypted = null;
            setSecretkeys(keyValue);
            using (var aes = new AesManaged())
            {
                aes.Key = keybytes;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                byte[] src = System.Convert.FromBase64String(encText);

                using (var ms = new MemoryStream(src))
                {
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {


                        using (StreamReader sr = new StreamReader(cs))
                        {
                            decrypted = sr.ReadToEnd();
                        }
                    }
                }

            }

            return (decrypted);
        }

             //string plaintext = null;
             //// Create AesManaged    
             //using (Aes aes = Aes.Create())
             //{
             //    // Create a decryptor    
             //    ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
             //    // Create the streams used for decryption.    
             //    using (MemoryStream ms = new MemoryStream(ciphertext))
             //    {
             //        // Create crypto stream    
             //        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
             //        {
             //            // Read crypto stream    
             //            using (StreamReader reader = new StreamReader(cs))
             //                plaintext = reader.ReadToEnd();
             //        }
             //    }
             //}
             //return plaintext;

       //  }


      

        public static   byte[] EncryptText(string plainText, byte[] key, byte[] iv)
        {

            var cypher = new AesManaged();
            cypher.Mode = CipherMode.CBC;
            cypher.Padding = PaddingMode.PKCS7;
            cypher.KeySize = 128;
            cypher.BlockSize = 128;
            cypher.Key = key;
            cypher.IV = iv;

            var icTransformer = cypher.CreateEncryptor();
            var msTemp = new MemoryStream();

            var csEncrypt = new CryptoStream(msTemp, icTransformer, CryptoStreamMode.Write);
            var sw = new StreamWriter(csEncrypt);
            sw.Write(plainText);
            sw.Close();
            sw.Dispose();

            csEncrypt.Clear();
            csEncrypt.Dispose();

            byte[] bResult = msTemp.ToArray();

            return bResult;
        }

        public static  string DecryptText(byte[] ciphertext, byte[] key, byte[] iv)
        {

            var cypher = new AesManaged();
            cypher.Mode = CipherMode.CBC;
            cypher.Padding = PaddingMode.PKCS7;
            cypher.KeySize = 128;
            cypher.BlockSize = 128;
            cypher.Key = key;
            cypher.IV = iv;

            var icTransformer = cypher.CreateDecryptor();
            var msTemp = new MemoryStream(ciphertext);

            var csDecrypt = new CryptoStream(msTemp, icTransformer, CryptoStreamMode.Read);
            var sr = new StreamReader(csDecrypt);

            string plaintext = sr.ReadToEnd();

            csDecrypt.Clear();
            csDecrypt.Dispose();

            return plaintext;
        }


        public static  string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder sbHex = new StringBuilder();
            foreach (byte b in bytes)
                sbHex.AppendFormat("{0:x2}", b);
            return sbHex.ToString();
        }

    }
//}