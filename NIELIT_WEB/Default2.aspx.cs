using System;
using System.Text;
using System.Web.UI;
using EConnect.Utils.Security; // Import CryptoHelper

public partial class Default2 : System.Web.UI.Page
{
    private static readonly string key = "4a0c76c0-7d79-404c-8134-6b91e7992b24"; // 32 bytes
    private static readonly string nonce = "255b2c3e-3d65-4c89-995e-c7d8e63d64cd"; // 12 bytes

    CryptoHelper ch = new CryptoHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = "EncryptedText: ";
    }

    protected void Encrypt_Click(object sender, EventArgs e)
    {
        string plaintext = TextBox1.Text;
        Label2.Text = ch.EncryptWithChaCha(plaintext, key, nonce); // Use CryptoHelper
        Session["text"] = plaintext;
    }

    protected void Decrypt_Click(object sender, EventArgs e)
    {
        string encryptedText = Label2.Text;
        Label2.Text = ch.DecryptWithChaCha(encryptedText, key, nonce); // Use CryptoHelper
    }
}
