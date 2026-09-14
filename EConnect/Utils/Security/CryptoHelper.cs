

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;


    public class CryptoHelper
    {
        public string EncryptWithChaCha(string plaintext, string key, string nonce)
        {
            byte[] plaintextBytes = Encoding.Default.GetBytes(plaintext);
            byte[] resultKey = Encoding.UTF8.GetBytes(key);
            byte[] nonceBytes = Encoding.UTF8.GetBytes(nonce);

            byte[] chachaKey = new byte[32];
            Array.Copy(resultKey, 0, chachaKey, 0, Math.Min (resultKey.Length,chachaKey.Length ));

            byte[]  chachaNonce = new byte[12];
            Array.Copy(nonceBytes, 0, chachaNonce, 0, Math.Min(nonceBytes .Length, 12));

            ChaCha7539Engine cipher = new ChaCha7539Engine();
            Org.BouncyCastle.Crypto.Parameters.KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(chachaKey);
            Org.BouncyCastle.Crypto.Parameters.ParametersWithIV parameters = new Org.BouncyCastle.Crypto.Parameters.ParametersWithIV(keyParam, chachaNonce);

            cipher.Init(true, parameters); // true = encryption

            byte[] output = new byte[plaintextBytes.Length];
            cipher.ProcessBytes(plaintextBytes, 0, plaintextBytes.Length, output, 0);

            return Convert.ToBase64String(output);
        }

    public string DecryptWithChaCha(string encryptedData, string key, string nonce)
    {
        byte[] decodedEncryptedData = Convert.FromBase64String(encryptedData);
        byte[] resultKey = Encoding.UTF8.GetBytes(key);
        byte[] nonceBytes = Encoding.UTF8.GetBytes(nonce);

        byte[] chachaKey = new byte[32];
        Array.Copy(resultKey, 0, chachaKey, 0, Math.Min(resultKey.Length, chachaKey.Length));

        byte[] chachaNonce = new byte[12];
        Array.Copy(nonceBytes, 0, chachaNonce, 0, Math.Min(nonceBytes.Length, 12));

        ChaCha7539Engine cipher = new ChaCha7539Engine();
        KeyParameter keyParam = new KeyParameter(chachaKey);
        ParametersWithIV parameters = new ParametersWithIV(keyParam, chachaNonce);

        cipher.Init(false, parameters); // false = encryption

        byte[] decryptedText = new byte[decodedEncryptedData.Length];
        cipher.ProcessBytes(decodedEncryptedData, 0, decodedEncryptedData.Length, decryptedText, 0);

        return Encoding.UTF8.GetString(decryptedText);
    }
}

