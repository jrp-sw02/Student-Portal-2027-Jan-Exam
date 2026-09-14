using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for QuertStringModule
/// </summary>
namespace EConnect.Utils.Security
{
	public class QuertStringModule : System.Web.IHttpModule, System.Web.SessionState.IRequiresSessionState 
	{
		static SymmetricAlgorithm mobjCryptoService;

		static QuertStringModule()
		{
			mobjCryptoService = new DESCryptoServiceProvider();
		}
		public void Dispose()
		{
		}
		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(context_BeginRequest);
			context.EndRequest += new EventHandler(context_EndRequest);
		}
		public static string Encrypt(string url)
		{
			try
			{
				string tempUrl = "";
                if (url.Contains("userAuthenticate.php?") || url.Contains("projectDashboard") ||url.Contains("AccrAPI")|| url.Contains ("blogName"))
                    return (url);
				if (url.Contains('?'))
				{
					tempUrl = url.Substring(0, url.IndexOf('?')) + "?qs=" + Encrypting(url.Substring(url.IndexOf('?') + 1), "test");
				}
				else
					tempUrl = url;
				try
				{
					Decrypt(tempUrl);    
				}
				catch (Exception)
				{
					tempUrl = url;
				}
				return tempUrl;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public static string Decrypt(string url)
		{
			try
			{
				if (url.Contains("?qs="))
				{
					string path = HttpContext.Current.Request.RawUrl;
					path = path.Substring(0, path.IndexOf("?") + 1);
					path = path.Substring(path.LastIndexOf("/") + 1);

					url = path + Decrypting(url.Substring(url.IndexOf('=') + 1), "test");
				}
				return url;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		void context_EndRequest(object sender, EventArgs e)
		{
			try
			{
				try
				{
					HttpContext.Current.Response.Headers.Remove("Server");
				}
				catch (Exception)
				{
				}
				if (HttpContext.Current.Response.IsRequestBeingRedirected)
				{
					if(!HttpContext.Current.Response.RedirectLocation.Contains("?qs"))
						HttpContext.Current.Response.RedirectLocation = Encrypt(HttpContext.Current.Response.RedirectLocation);
				}
			}
			catch (Exception)
			{
				 
			}
		}
		void context_BeginRequest(object sender, EventArgs e)
		{
			try
			{
				Boolean isXSS =false;
				System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"<[^>]*>");
				try
				{
					foreach (String key in HttpContext.Current.Request.Form.Keys)
					{
						if (reg.IsMatch(HttpContext.Current.Request.Form[key].ToString()))
						{
							isXSS = true;
							break;
						}
					}
					if (isXSS == false)
					{
						if (HttpContext.Current.Request.Url.ToString().Contains("?"))
						{
							if (reg.IsMatch(HttpContext.Current.Request.QueryString.ToString()))
								isXSS = true;
						}
					}
				}
				catch (Exception)
				{
					isXSS = true;
				}
				if (isXSS == true)
				{
					if (HttpContext.Current.Request.UrlReferrer != null)
						HttpContext.Current.Response.Redirect(HttpContext.Current.Request.UrlReferrer.AbsoluteUri);
					else
					{
						if (HttpContext.Current.Session != null)
							HttpContext.Current.Response.Redirect("~/FrmDashBoard.aspx");
						else
							HttpContext.Current.Response.Redirect("~/Home.aspx");
					}
				}
				//System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^(?:http|https|ftp)://[a-zA-Z0-9\.\-]+(?:\:\d{1,5})?(?:[A-Za-z0-9\.\;\:\@\&\=\+\$\,\?/]|%u[0-9A-Fa-f]{4}|%[0-9A-Fa-f]{2})*$");
				if (HttpContext.Current.Request.Url.ToString().Contains("?qs="))
				{
					HttpContext.Current.RewritePath(Decrypt(HttpContext.Current.Request.Url.ToString()));
				}
				else if (HttpContext.Current.Request.Url.ToString().Contains("?"))
				{
					//throw new HttpException("Not Valid Request");
				}

				
			}
			catch (Exception)
			{
				if (HttpContext.Current.Request.UrlReferrer != null)
					HttpContext.Current.Response.Redirect(HttpContext.Current.Request.UrlReferrer.AbsoluteUri);
				else
				{
					if(HttpContext.Current.Session!=null)
						HttpContext.Current.Response.Redirect("~/FrmDashBoard.aspx");
					else
						HttpContext.Current.Response.Redirect("~/Home.aspx");
				}
				//throw ex;
			}
		}
		static string Encrypting(string Source, string Key)
		{
			try
			{
				byte[] bytIn = System.Text.ASCIIEncoding.ASCII.GetBytes(Source);
				System.IO.MemoryStream ms = new System.IO.MemoryStream();

				byte[] bytKey = GetLegalKey(Key);

				mobjCryptoService.Key = bytKey;
				mobjCryptoService.IV = bytKey;

				ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();

				CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

				cs.Write(bytIn, 0, bytIn.Length);
				cs.FlushFinalBlock();

				byte[] bytOut = ms.GetBuffer();
				int i = 0;
				for (i = 0; i < bytOut.Length; i++)
					if (bytOut[i] == 0)
						if (bytOut[i + 1] != null)
							if (bytOut[i + 1] == 0)
								if (bytOut[i + 2] != null)
									if (bytOut[i + 2] == 0)
										break;
				if (i % 2 == 1)
					i++;
				return System.Convert.ToBase64String(bytOut, 0, i);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		static string Decrypting(string Source, string Key)
		{
			try
			{
				byte[] bytIn = System.Convert.FromBase64String(Source);

				System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);
				ms.Position = 0;

				byte[] bytKey = GetLegalKey(Key);

				mobjCryptoService.Key = bytKey;
				mobjCryptoService.IV = bytKey;

				ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();

				CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

				System.IO.StreamReader sr = new System.IO.StreamReader(cs);
				return sr.ReadToEnd();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		static byte[] GetLegalKey(string Key)
		{try{
			string sTemp;
			if (mobjCryptoService.LegalKeySizes.Length > 0)
			{
				int lessSize = 0, moreSize = mobjCryptoService.LegalKeySizes[0].MinSize;

				while (Key.Length * 8 > moreSize)
				{
					lessSize = moreSize;
					moreSize += mobjCryptoService.LegalKeySizes[0].SkipSize;
				}
				sTemp = Key.PadRight(moreSize / 8, ' ');
			}
			else
				sTemp = Key;
			return ASCIIEncoding.ASCII.GetBytes(sTemp);
		}
		catch (Exception ex)
		{
			throw ex;
		}
		}
	}
}