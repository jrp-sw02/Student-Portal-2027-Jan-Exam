using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net;
using System.IO;
using System.Xml;
using BridgePG;
using System.Web;

namespace ConnectPayment
{
    [Serializable]
    public class BridgePGUtil
    {

        public string merchant_id { get; set; }
        public string csc_id { get; set; }
        public string merchant_receipt_no { get; set; }
        public string return_url { get; set; }
        public string cancel_url { get; set; }
        public string merchant_txn { get; set; }
        public string merchant_txn_date_time { get; set; }
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string txn_amount { get; set; }
        public string amount_parameter { get; set; }
        public string txn_mode { get; set; }
        public string txn_type { get; set; }
        public string csc_share_amount { get; set; }
        public string pay_to_email { get; set; }
        public string Currency { get; set; }
        public string Discount { get; set; }
        public string param_1 { get; set; }
        public string param_2 { get; set; }
        public string param_3 { get; set; }
        public string param_4 { get; set; }

        public string CreateMessage(string merchant_id, string csc_id, string txn_amount, string csc_share_amount, string merchant_txn, string merchant_receipt_no, string return_url, string cancel_url, string product_id, string product_name, string Did)
        {
            string postMessage = string.Empty;
            BridgePGUtil objBridgePGUtil = new BridgePGUtil();
            List<BridgePGUtil> objListBridgePGUtil = new List<BridgePGUtil>();
            objBridgePGUtil.merchant_id = merchant_id;
            objBridgePGUtil.csc_id = csc_id;
            objBridgePGUtil.txn_amount = txn_amount;
            objBridgePGUtil.merchant_receipt_no = merchant_receipt_no;
            objBridgePGUtil.return_url = return_url;
            objBridgePGUtil.cancel_url = cancel_url;
            objBridgePGUtil.merchant_txn_date_time = DateTime.Now.Year.ToString().PadLeft(4, '0') + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0') + " " + DateTime.Now.Hour.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Second.ToString().PadLeft(2, '0');
            objBridgePGUtil.merchant_txn = merchant_txn;
            objBridgePGUtil.product_id = product_id;
            objBridgePGUtil.product_name = product_name;
            objBridgePGUtil.amount_parameter = "NA";
            objBridgePGUtil.txn_mode = "D";
            objBridgePGUtil.txn_type = "D";
            objBridgePGUtil.csc_share_amount = csc_share_amount;
            objBridgePGUtil.pay_to_email = "na";
            objBridgePGUtil.Currency = "INR";
            objBridgePGUtil.Discount = "0";
            objBridgePGUtil.param_1 = Did;// you may send Demand Note ID value here
            objBridgePGUtil.param_2 = "NA";// you may send Othervals value here
            objBridgePGUtil.param_3 = "NA";// you may send SMER value here
            objBridgePGUtil.param_4 = "NA";

            objListBridgePGUtil.Add(objBridgePGUtil);

            for (int i = 0; i < objListBridgePGUtil.Count; i++)
            {
                postMessage = "merchant_id=" + objListBridgePGUtil[i].merchant_id + "|"
                                + "csc_id=" + objListBridgePGUtil[i].csc_id + "|"
                                + "merchant_txn=" + objListBridgePGUtil[i].merchant_txn + "|"
                                + "merchant_txn_date_time=" + objListBridgePGUtil[i].merchant_txn_date_time + "|"
                                + "product_id=" + objListBridgePGUtil[i].product_id + "|"
                                + "product_name=" + objListBridgePGUtil[i].product_name + "|"
                                + "txn_amount=" + objListBridgePGUtil[i].txn_amount + "|"
                                + "amount_parameter=" + objListBridgePGUtil[i].amount_parameter + "|"
                                + "txn_mode=" + objListBridgePGUtil[i].txn_mode + "|"
                                + "txn_type=" + objListBridgePGUtil[i].txn_type + "|"
                                + "merchant_receipt_no=" + objListBridgePGUtil[i].merchant_receipt_no + "|"
                                + "csc_share_amount=" + objListBridgePGUtil[i].csc_share_amount + "|"
                                + "pay_to_email=" + objListBridgePGUtil[i].pay_to_email + "|"
                                + "return_url=" + objListBridgePGUtil[i].return_url + "|"
                                + "cancel_url=" + objListBridgePGUtil[i].cancel_url + "|"
                                + "Currency=" + objListBridgePGUtil[i].Currency + "|"
                                + "Discount=" + objListBridgePGUtil[i].Discount + "|"
                                + "param_1=" + objListBridgePGUtil[i].param_1 + "|"
                                + "param_2=" + objListBridgePGUtil[i].param_2 + "|"
                                + "param_3=" + objListBridgePGUtil[i].param_3 + "|"
                                + "param_4=" + objListBridgePGUtil[i].param_4 + "|";

            }
            string strPRIVATE_KEY = ConfigurationManager.AppSettings["PRIVATE_KEY"];
            string strPUBLIC_KEY = ConfigurationManager.AppSettings["PUBLIC_KEY"];
            strPRIVATE_KEY = Base64Decode(strPRIVATE_KEY);
            strPUBLIC_KEY = Base64Decode(strPUBLIC_KEY);
            Crypto.privateKey = strPRIVATE_KEY;
            Crypto.publicKey = strPUBLIC_KEY;

            return Crypto.encrypt(postMessage, Crypto.publicKey, Crypto.privateKey);
        }
        public string CreateURLappendString()
        {
            Int64 a = Convert.ToInt64(DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0')) * 883 + (1000 - 883);
            string returnvalue = ConfigurationManager.AppSettings["PAY_URL"] + a.ToString();
            return returnvalue;
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        //Api Methods

        public string transaction_enquiry(string merchant_id, string merchant_txn)
        {

            string data = "merchant_id=" + merchant_id + "|merchant_txn=" + merchant_txn + "|";
            string strPRIVATE_KEY = ConfigurationManager.AppSettings["PRIVATE_KEY"];
            string strPUBLIC_KEY = ConfigurationManager.AppSettings["PUBLIC_KEY"];
            strPRIVATE_KEY = Base64Decode(strPRIVATE_KEY);
            strPUBLIC_KEY = Base64Decode(strPUBLIC_KEY);
            Crypto.privateKey = strPRIVATE_KEY;
            Crypto.publicKey = strPUBLIC_KEY;
            string request_data = Crypto.encrypt(data, Crypto.publicKey, Crypto.privateKey);


            string response = "";
            XmlDocument req = new XmlDocument();

            XmlNode docNode = req.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            req.AppendChild(docNode);
            XmlNode xml = req.AppendChild(req.CreateElement("xml"));
            XmlNode merchant_id_node = xml.AppendChild(req.CreateElement("merchant_id"));
            merchant_id_node.InnerXml = merchant_id;
            XmlNode request_data_node = xml.AppendChild(req.CreateElement("request_data"));
            request_data_node.InnerXml = request_data;


            string appuri = ConfigurationManager.AppSettings["API_URL"] + "transaction/enquiry" + "/format/xml";

            callApi(appuri, req.OuterXml, ref  response);

            XmlDocument responsestr = new XmlDocument();

            responsestr.LoadXml(response);

            string result = responsestr.GetElementsByTagName("response_status")[0].InnerXml;
            if (result == "Success")
            {
                string response_data = responsestr.GetElementsByTagName("response_data")[0].InnerXml;

                response = Crypto.decrypt(response_data, Crypto.privateKey, Crypto.publicKey, true);


            }
            return response;
        }
        //3
        public string transaction_status(string merchant_id, string merchant_txn, string csc_txn)
        {

            string data = "merchant_id=" + merchant_id + "|merchant_txn=" + merchant_txn + "|csc_txn=" + csc_txn + "|";
            string strPRIVATE_KEY = ConfigurationManager.AppSettings["PRIVATE_KEY"];
            string strPUBLIC_KEY = ConfigurationManager.AppSettings["PUBLIC_KEY"];
            strPRIVATE_KEY = Base64Decode(strPRIVATE_KEY);
            strPUBLIC_KEY = Base64Decode(strPUBLIC_KEY);
            Crypto.privateKey = strPRIVATE_KEY;
            Crypto.publicKey = strPUBLIC_KEY;
            string request_data = Crypto.encrypt(data, Crypto.publicKey, Crypto.privateKey);


            string response = "";
            XmlDocument req = new XmlDocument();

            XmlNode docNode = req.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            req.AppendChild(docNode);
            XmlNode xml = req.AppendChild(req.CreateElement("xml"));
            XmlNode merchant_id_node = xml.AppendChild(req.CreateElement("merchant_id"));
            merchant_id_node.InnerXml = merchant_id;
            XmlNode request_data_node = xml.AppendChild(req.CreateElement("request_data"));
            request_data_node.InnerXml = request_data;


            string appuri = ConfigurationManager.AppSettings["API_URL"] + "transaction/status" + "/format/xml";

            callApi(appuri, req.OuterXml, ref  response);



            XmlDocument responsestr = new XmlDocument();

            responsestr.LoadXml(response);

            string result = responsestr.GetElementsByTagName("response_status")[0].InnerXml;
            if (result == "Success")
            {
                string response_data = responsestr.GetElementsByTagName("response_data")[0].InnerXml;

                response = Crypto.decrypt(response_data, Crypto.privateKey, Crypto.publicKey, true);


            }

            return response;
        }
        //4
        public static string refund_log(string merchant_id, string merchant_txn, string csc_txn,
                   string merchant_txn_status, string merchant_reference, string refund_deduction, string refund_mode,
            string refund_type, string refund_trigger, string refund_reason)
        {

            string data = "merchant_id=" + merchant_id + "|merchant_txn=" + merchant_txn + "|csc_txn=" + csc_txn +
                   "|merchant_txn_status=" + merchant_txn_status + "|merchant_reference=" + merchant_reference +
                   "|refund_deduction=" + refund_deduction + "|refund_mode=" + refund_mode + "|refund_type=" + refund_type +
                   "|refund_trigger=" + refund_trigger + "|refund_reason=" + refund_reason + "|";
            string strPRIVATE_KEY = ConfigurationManager.AppSettings["PRIVATE_KEY"];
            string strPUBLIC_KEY = ConfigurationManager.AppSettings["PUBLIC_KEY"];
            strPRIVATE_KEY = Base64Decode(strPRIVATE_KEY);
            strPUBLIC_KEY = Base64Decode(strPUBLIC_KEY);
            Crypto.privateKey = strPRIVATE_KEY;
            Crypto.publicKey = strPUBLIC_KEY;
            string request_data = Crypto.encrypt(data, Crypto.publicKey, Crypto.privateKey);


            string response = "";
            XmlDocument req = new XmlDocument();

            XmlNode docNode = req.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            req.AppendChild(docNode);
            XmlNode xml = req.AppendChild(req.CreateElement("xml"));
            XmlNode merchant_id_node = xml.AppendChild(req.CreateElement("merchant_id"));
            merchant_id_node.InnerXml = merchant_id;
            XmlNode request_data_node = xml.AppendChild(req.CreateElement("request_data"));
            request_data_node.InnerXml = request_data;


            string appuri = ConfigurationManager.AppSettings["API_URL"] + "refund/log" + "/format/xml";

            callApi(appuri, req.OuterXml, ref  response);



            XmlDocument responsestr = new XmlDocument();

            responsestr.LoadXml(response);

            string result = responsestr.GetElementsByTagName("response_status")[0].InnerXml;
            if (result == "Success")
            {
                string response_data = responsestr.GetElementsByTagName("response_data")[0].InnerXml;

                response = Crypto.decrypt(response_data, Crypto.privateKey, Crypto.publicKey, true);


            }

            return response;
        }
        //5
        public string refund_status(string merchant_id, string merchant_txn, string csc_txn, string refund_reference)
        {

            // Input String: merchant_id=1234|csc_txn=1234556|merchant_txn=12|refund_reference=1234567890|
            string data = "merchant_id=" + merchant_id + "|merchant_txn=" + merchant_txn + "|csc_txn=" + csc_txn + "|refund_reference=" + refund_reference + "|";
            string strPRIVATE_KEY = ConfigurationManager.AppSettings["PRIVATE_KEY"];
            string strPUBLIC_KEY = ConfigurationManager.AppSettings["PUBLIC_KEY"];
            strPRIVATE_KEY = Base64Decode(strPRIVATE_KEY);
            strPUBLIC_KEY = Base64Decode(strPUBLIC_KEY);
            Crypto.privateKey = strPRIVATE_KEY;
            Crypto.publicKey = strPUBLIC_KEY;
            string request_data = Crypto.encrypt(data, Crypto.publicKey, Crypto.privateKey);



            string response = "";
            XmlDocument req = new XmlDocument();

            XmlNode docNode = req.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            req.AppendChild(docNode);
            XmlNode xml = req.AppendChild(req.CreateElement("xml"));
            XmlNode merchant_id_node = xml.AppendChild(req.CreateElement("merchant_id"));
            merchant_id_node.InnerXml = merchant_id;
            XmlNode request_data_node = xml.AppendChild(req.CreateElement("request_data"));
            request_data_node.InnerXml = request_data;


            string appuri = ConfigurationManager.AppSettings["API_URL"] + "refund/status" + "/format/xml";
            // response = "APPURL:" + appuri + "     Request XML:    " + req.OuterXml.ToString();


            callApi(appuri, req.OuterXml, ref  response);


            // return "ffff" + response;
            XmlDocument responsestr = new XmlDocument();

            responsestr.LoadXml(response);

            string result = responsestr.GetElementsByTagName("response_status")[0].InnerXml;
            if (result == "Success")
            {
                string response_data = responsestr.GetElementsByTagName("response_data")[0].InnerXml;
                try
                {
                    response = Crypto.decrypt(response_data, Crypto.privateKey, Crypto.publicKey, true);
                }
                catch (Exception)
                {
                    return response;
                }


            }
            else
            {
                return response;
            }

            return response;
        }

        //6  
        public string recon_log(string merchant_id, string merchant_txn, string csc_txn, string cscuser_id, string product_id, string txn_amount, string merchant_date, string merchant_txn_status, string merchant_reciept)
        {

            string data = "merchant_id=" + merchant_id + "|merchant_txn=" + merchant_txn + "|csc_txn=" + csc_txn +
                "|csc_id=" + cscuser_id + "|product_id=" + product_id + "|txn_amount=" + txn_amount +
                 "|merchant_txn_datetime=" + merchant_date + "|merchant_txn_status=" + merchant_txn_status + "|merchant_receipt_no=" + merchant_reciept + "|";

            //Added 10 Sept 2020
           /*string dd = DateTime.Now.ToString("yyyy-MM-dd");

         string   logFile = Convert.ToString(dd + "_13.txt");

            CommonFunctions.createLog(logFile, data);*/
            //

            string strPRIVATE_KEY = ConfigurationManager.AppSettings["PRIVATE_KEY"];
            string strPUBLIC_KEY = ConfigurationManager.AppSettings["PUBLIC_KEY"];
            strPRIVATE_KEY = Base64Decode(strPRIVATE_KEY);
            strPUBLIC_KEY = Base64Decode(strPUBLIC_KEY);
            Crypto.privateKey = strPRIVATE_KEY;
            Crypto.publicKey = strPUBLIC_KEY;
            string request_data = Crypto.encrypt(data, Crypto.publicKey, Crypto.privateKey);

            //Added 10 Sept 2020
           /* dd = DateTime.Now.ToString("yyyy-MM-dd");

            logFile = Convert.ToString(dd + "_13a.txt");

            CommonFunctions.createLog(logFile, request_data);*/
            //
            string response = "";
            XmlDocument req = new XmlDocument();

            XmlNode docNode = req.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            req.AppendChild(docNode);
            XmlNode xml = req.AppendChild(req.CreateElement("xml"));
            XmlNode merchant_id_node = xml.AppendChild(req.CreateElement("merchant_id"));
            merchant_id_node.InnerXml = merchant_id;
            XmlNode request_data_node = xml.AppendChild(req.CreateElement("request_data"));
            request_data_node.InnerXml = request_data;


            string appuri = ConfigurationManager.AppSettings["API_URL"] + "recon/log" + "/format/xml";
            //Added 10 Sept 2020
          /*  dd = DateTime.Now.ToString("yyyy-MM-dd");

            logFile = Convert.ToString(dd + "_13b.txt");

            CommonFunctions.createLog(logFile, appuri);*/
            //

            callApi(appuri, req.OuterXml, ref  response);



            XmlDocument responsestr = new XmlDocument();

            responsestr.LoadXml(response);

            string result = responsestr.GetElementsByTagName("response_status")[0].InnerXml;
            //Added 10 Sept 2020
            /*dd = DateTime.Now.ToString("yyyy-MM-dd");

            logFile = Convert.ToString(dd + "_14.txt");

            CommonFunctions.createLog(logFile, result);*/
            //

            if (result == "Success")
            {
                string response_data = responsestr.GetElementsByTagName("response_data")[0].InnerXml;

                response = Crypto.decrypt(response_data, Crypto.privateKey, Crypto.publicKey, true);


            }
            //Added 10 Sept 2020
           /* dd = DateTime.Now.ToString("yyyy-MM-dd");

            logFile = Convert.ToString(dd + "_15.txt");

            CommonFunctions.createLog(logFile, response);*/
            //

            return response;
        }


        public static bool callApi(string url, string requestXML, ref string responseXML)
        {
			//Added 10 Sept 2020
           /* string dd = DateTime.Now.ToString("yyyy-MM-dd");

            string logFile = Convert.ToString(dd + "_13x.txt");

            CommonFunctions.createLog(logFile, requestXML);*/
            //
            try
            {
                HttpWebRequest req = (HttpWebRequest)(HttpWebRequest.Create(url));
                req.Method = "POST";
                req.ProtocolVersion = HttpVersion.Version11;
                req.ContentType = "application/xml";
                string content = requestXML;
                req.ContentLength = content.Length;
                Stream wri = req.GetRequestStream();
                byte[] array = Encoding.UTF8.GetBytes(content);
                wri.Write(array, 0, array.Length);
                wri.Flush();
                wri.Close();
                HttpWebResponse HttpWResp = (HttpWebResponse)req.GetResponse();
                int resCode = (int)HttpWResp.StatusCode;
                //Added 10 Sept 2020
                 /*dd = DateTime.Now.ToString("yyyy-MM-dd");

                 logFile = Convert.ToString(dd + "_13c.txt");

                CommonFunctions.createLog(logFile, resCode.ToString());*/
                //
                StreamReader reader = new StreamReader(HttpWResp.GetResponseStream(), System.Text.Encoding.UTF8);
                string resultData = reader.ReadToEnd();
                //Added 10 Sept 2020
               /*  dd = DateTime.Now.ToString("yyyy-MM-dd");

                 logFile = Convert.ToString(dd + "_16.txt");
                ;
                CommonFunctions.createLog(logFile, resultData);*/
                //
                responseXML = resultData;
                return true;
            }
            catch (Exception ex)
            {
                responseXML = ex.Message;
                return false;
            }
        }
    }

}
