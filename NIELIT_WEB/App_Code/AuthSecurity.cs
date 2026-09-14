using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



    public class AuthSecurity
    {

        public class AuthDetails
        {
            public string username { get; set; }
            public string password { get; set; }
            public string API_Path { get; set; }

        }

        public class LGDParamDetails
        {
            public string scheme_code { get; set; }
            public string state_code { get; set; }
            public string date { get; set; }
            public string authentication_token { get; set; }


        }


        public int authentication(String username,string password,string API_URL)
        {

            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            int login_valid = 0;

            try
            {
                SqlConnection objsqlconn = new SqlConnection(conn);
                objsqlconn.Open();

                string query = " select count(*) from API_User_Auth where user_id=(select id from UserAuth where UserID = @userID and pwd = @PWD) and API_id = (select API_id from API_List where api_url = @API_URL)";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(query, objsqlconn);
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@UserID", username);
                param[1] = new SqlParameter("@PWD", password);
                param[2] = new SqlParameter("@API_URL", API_URL);
                cmd.Parameters.Add(param[0]);
                cmd.Parameters.Add(param[1]);
                cmd.Parameters.Add(param[2]);

                object res = cmd.ExecuteScalar();
                if (Convert.ToInt32(res) > 0)
                {
                    login_valid++;
                }                
                objsqlconn.Close();

            }
            catch (Exception ee) { ee.ToString(); }
            return login_valid;

        }


        private const string AllowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";

        public  string GenerateToken(int length, string API_URL,string source_address)
        {
            
            var bytes = new byte[length];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(bytes);
            }
            string mytoken = new string(bytes.Select(x => AllowableCharacters[x % AllowableCharacters.Length]).ToArray());
            token_insert(mytoken, API_URL,source_address);
            //  return new string(bytes.Select(x => AllowableCharacters[x % AllowableCharacters.Length]).ToArray());
            return mytoken;
        }


        public int token_insert(String token, string API_URL,string source_ipaddr)
        {

            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            int ins_check = 0;

            try
            {
                SqlConnection objsqlconn = new SqlConnection(conn);
                objsqlconn.Open();

                string query = "Insert into Auth_url_token(Auth_token,API_id,source_ip) (select @token,(select APi_id from  Api_list where API_url = @API_Url),@source_ip)";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(query, objsqlconn);
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@token", token);
                param[1] = new SqlParameter("@API_Url", API_URL);
                param[2] = new SqlParameter("@source_ip", source_ipaddr);
                cmd.Parameters.Add(param[0]);
                cmd.Parameters.Add(param[1]);
                cmd.Parameters.Add(param[2]);

                ins_check = cmd.ExecuteNonQuery();

                objsqlconn.Close();

            }
            catch (Exception ee) { ee.ToString(); }
            return ins_check;

        }


        public int token_validate(String token, string API_URL, string consumer_ipaddr)
        {

            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            int token_valid = 0;

            try
            {
                SqlConnection objsqlconn = new SqlConnection(conn);
                objsqlconn.Open();

                string query = "select count(*) from Auth_url_token where Auth_token=@token and API_id=(select API_id from api_list where API_URL=@API_URL)";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(query, objsqlconn);
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@token", token);
                param[1] = new SqlParameter("@API_Url", API_URL);
                cmd.Parameters.Add(param[0]);
                cmd.Parameters.Add(param[1]);
                object res = cmd.ExecuteScalar();
                if (Convert.ToInt32(res) > 0)
                {
                    Console.WriteLine(token_valid);
                    token_valid++;
                    update_delete_token(token,API_URL,"U",consumer_ipaddr); // to insert consumer IP in token row
                    update_delete_token(token, API_URL, "D","NA"); // to delete token row and maintain log of it

                }

                objsqlconn.Close();

            }
            catch (Exception ee) { ee.ToString(); }
            return token_valid;

        }


        public int update_delete_token(String token, string API_URL,string update_delete,string consumer_ipaddr)
        {

            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            int update_delete_token = 0;

            try
            {
                SqlConnection objsqlconn = new SqlConnection(conn);
                objsqlconn.Open();

                string query = "";
                if (update_delete == "D")
                {
                    query = "delete Auth_url_token where Auth_token=@token and API_id=(select API_id from api_list where API_URL=@API_Url)";
                }
                else
                {
                    query = "update Auth_url_token set consume_ip=@consumer_ip where Auth_token=@token";
                }
                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(query, objsqlconn);
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@token", token);
                if (update_delete == "D")
                {

                    param[1] = new SqlParameter("@API_Url", API_URL);
                }
                else
                {
                    param[1] = new SqlParameter("@consumer_ip", consumer_ipaddr);

                }
                cmd.Parameters.Add(param[0]);
                cmd.Parameters.Add(param[1]);
                object res = cmd.ExecuteScalar();
                if (Convert.ToInt32(res) > 0)
                {
                    update_delete_token++;

                }

                objsqlconn.Close();

            }
            catch (Exception ee) { ee.ToString(); }
            return update_delete_token;

        }


    }



