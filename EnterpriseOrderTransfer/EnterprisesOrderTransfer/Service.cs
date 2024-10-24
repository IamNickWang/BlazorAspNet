using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Configuration;
using System.Drawing;
using System;
using System.Net;
using Serilog;
using Serilog.Events;
using System.Security.Authentication;
using System.Data.SqlClient;
using System.Net.Mail;
using static System.Net.WebRequestMethods;
using ServiceStack;
using System.Formats.Asn1;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using EnterprisesOrderTransfer.ServiceLibaray;


namespace EnterprisesOrderTransfer
{
    public interface IService
    {
    }

    public class Service : IService
    {
        //private readonly ILogger<Service> _log;


        private string _timer;
        private readonly IService _services;
        //private static HttpClient _httpClient;
        private SqlConnection _dbcon;
        //private string _anaplanToken;
        private IConfiguration Configuration;


        //string DBString = @"Data Source=entbidb03;Initial Catalog=DWStagingDB;User Id=dataOpsjob01;Password=KAC!V6x63xwwiPZzBv$TNC;"; string ServerName = "ENTBIDB03";
        string DBString = @"Data Source=entbidbstg03;Initial Catalog=DWStagingDB;User Id=dataOpsjob01;Password=KAC!V6x63xwwiPZzBv$TNC;"; string ServerName = "ENTBIDBSTG03";

        string transferName = @"erpTransferOrderFlat";







        string[] url_transfer =
        {
            "https://5391094.restlets.api.netsuite.com/app/site/hosting/restlet.nl?script=1128&deploy=1"


        };






        HttpWebRequest request = null;
        HttpWebResponse response = null;

        ///// notification reccipient email list
        string toRecipients = "nwang@chumashenterprises.com";

        public async Task ExcuteAsync(CancellationToken stoppingToken)
        {
            ///initiate Method class
            var mm = new Method();

            Console.WriteLine("The servicee has started.");
            System.IO.File.AppendAllText(mm.@_logPath, "The servicee has started." + Environment.NewLine);

            //... setup protocl
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;


            ///////****** below used in the third part - Budget period and service approval, Procure To Pay         ******///
            mm.getTableTruncate(DBString, transferName);

            ///extract transferOrder data and insert into database and send out notification
            int transfer_rows = 0;
            for (int i = 0; i < url_transfer.Length; i++)
            {

                DataTable dta = await mm.getTransferOrder(url_transfer[i], i);
                string rows = mm.getTableInsert(DBString, transferName, dta);
                transfer_rows = transfer_rows + Int32.Parse(rows);

                var htmlString = "<html>erp (PROD) TransferOrder Data Extraction finished inserting rows: ( Part_" + i.ToString() + "=" + rows;
                htmlString = htmlString + " ) into dbo.erpTransferOrderFlat table. </html>";
                string[] recipients = toRecipients.Split(";");
            }
            var p2pFinalString = "<html>erp (PROD) TransferOrder Data Extraction finished inserting total rows = ( " + transfer_rows.ToString() + " ) into (" + ServerName + ") dbo.erpProcureToPayFlat table. Data Extraction job finished. </html>";
            string[] p2pFinalRecipients = toRecipients.Split(";");
            foreach (var address in p2pFinalRecipients)
            {
                mm.Email(p2pFinalString, new MailAddress(address));
            };




            //////------ final message --------------------------------------------------------------------------------------------------------------------//////
            /******       this is the final notification of the execution.             *******/
            ///print message to notify service finished
            Console.WriteLine(DateTime.Now + " The servicee has finished.");
            System.IO.File.AppendAllText(mm.@_logPath, DateTime.Now + " The servicee has finished." + Environment.NewLine);
            var finishString = "Netsuite Data extraction job (" + ServerName + ") has finished.";
            string[] finishfinalRecipients = toRecipients.Split(";");
            foreach (var address in finishfinalRecipients)
            {
                Email(finishString, new MailAddress(address));
            };




        }

        public void Email(string htmlString, MailAddress toAddress)
        {
            SmtpClient smtp = new SmtpClient();
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("no-Reply@chumashcasino.com");
                message.To.Add(toAddress);
                message.Subject = "Netsuite API data pull - Saved Search Transaction";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = 25;
                smtp.Host = "mail.chumashenterprises.com"; //for gmail host  
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = true;
                //smtp.Credentials = new NetworkCredential("nick.wang@chumashcasino.com", "lompoc@L221"); // NetworkCredential("FromMailAddress", "password");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                smtp.Dispose();
            }
            catch (Exception) { }
            finally
            {
                if (smtp != null)
                    smtp.Dispose();
            }
        }



    }

        public class Method
    {
        private readonly IService _services;
        private SqlConnection _dbcon;
        private string _anaplanToken;
        private IConfiguration Configuration;
        //private readonly appSettings _appSettings;
        private string directoryName = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
        public string _logPath = Path.GetFullPath(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName + "/erpDataExportLog.txt");


        ////// production
        string consumer_id = "fb4e6b3c6fb829f4c357aa33ee99fe7c671ab26e7e4a44d0f1924b5a3c36dbfa"; // (Also called Consumer Key)
        string consumer_secret = "2581b1fd3557a73a921eec2956a557973505ee323a2633601deec004a32d8b70";
        string token_id = "4c48ee9be36b3cbb62e4397545ae8615320f9e134f9212e50e4cb4e94ba228bb";
        string token_secret = "7d8a191e77049fbc8f72052893b4be82dfea3e2f0b772f00613b35a241cc0cc5";
        string NS_realm = "5391094";

        ///// <summary>
        ///// SB3
        //string consumer_id = "af8ca666e29945c446b6581565840ae371b2e69e1c7c50be821d7765a2bde426"; // (Also called Consumer Key)
        //string consumer_secret = "7be61a95c402d7676ea492106d5188f553a7abb26afe3de55f9171b936de4558";
        //string token_id = "91591f5d7e647a4c960b3a8fdc0cf30fdbecb38a1e3ef1d72b0c8d771fb6c6f6";
        //string token_secret = "63f53cac93b88c1ddeb57044a0c5876b28b76ce65a58b0f6aa05c98c3aba9eb7";
        //string NS_realm = "5391094_SB3";


        public void getTableTruncate(string DBConnection, string tableName)
        {
            //... initiate SQL connection
            _dbcon = new SqlConnection(DBConnection);
            _dbcon.Open();

            //... truncate the destination table
            string sql = $"TRUNCATE TABLE [dbo]." + tableName;
            using (SqlCommand command = new SqlCommand(sql, _dbcon))
            {
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();

            };

            _dbcon.Close();

        }

        public string getTableInsert(string DBConnection, string tableName, DataTable dataTable)
        {
            //... initiate SQL connection
            _dbcon = new SqlConnection(DBConnection);
            _dbcon.Open();


            //// ... for Bulk load
            using (SqlBulkCopy sqlCopy = new SqlBulkCopy(_dbcon))
            {

                sqlCopy.BatchSize = 1000000;
                sqlCopy.NotifyAfter = 10000;
                sqlCopy.BulkCopyTimeout = 0;
                sqlCopy.DestinationTableName = $"[dbo]." + tableName;

                try
                {

                    sqlCopy.SqlRowsCopied += (sender, eventArgs) =>
                    {


                        System.IO.File.AppendAllText(_logPath,
                            DateTime.Now.ToString() + "  ------  Rows Copied to " + tableName + " table = " + eventArgs.RowsCopied.ToString() + Environment.NewLine);

                    };

                    //while (dataTable.Rows.Count != 0) { 
                    sqlCopy.WriteToServer(dataTable);
                    //}

                    System.IO.File.AppendAllText(_logPath,
                        DateTime.Now.ToString() + "  ------  Total Rows Copied to " + tableName + " table = " + dataTable.Rows.Count.ToString() + Environment.NewLine);

                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText(_logPath,
                       DateTime.Now.ToString() + "  ------  Exception happended, no row inserted into " + tableName + " table. " + Environment.NewLine);
                }

            };

            _dbcon.Close();

            return dataTable.Rows.Count.ToString();

        }

        public void Email(string htmlString, MailAddress toAddress)
        {
            SmtpClient smtp = new SmtpClient();
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("noReply@chumashcasino.com");
                message.To.Add(toAddress);
                message.Subject = "Netsuite API data pull - Saved Search Transaction";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = 25;
                smtp.Host = "mail.chumashenterprises.com"; //for gmail host  
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = true;
                //smtp.Credentials = new NetworkCredential("nick.wang@chumashcasino.com", "lompoc@L221"); // NetworkCredential("FromMailAddress", "password");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                smtp.Dispose();
            }
            catch (Exception) { }
            finally
            {
                if (smtp != null)
                    smtp.Dispose();
            }
        }

        /// splitting large Json file into trunks
        enum SplitState
        {
            InPrefix,
            InSplitProperty,
            InSplitArray,
            InPostfix,
        }
        public static void SplitJson(TextReader textReader, string tokenName, long maxItems, Func<int, TextWriter> createStream, System.Xml.Formatting formatting)
        {
            List<JProperty> prefixProperties = new List<JProperty>();
            List<JProperty> postFixProperties = new List<JProperty>();
            List<JsonWriter> writers = new List<JsonWriter>();

            SplitState state = SplitState.InPrefix;
            long count = 0;

            try
            {
                using (var reader = new JsonTextReader(textReader))
                {
                    bool doRead = true;
                    while (doRead ? reader.Read() : true)
                    {
                        doRead = true;
                        if (reader.TokenType == JsonToken.Comment || reader.TokenType == JsonToken.None)
                            continue;
                        if (reader.Depth == 0)
                        {
                            if (reader.TokenType != JsonToken.StartObject && reader.TokenType != JsonToken.EndObject)
                                throw new System.Text.Json.JsonException("JSON root container is not an Object");
                        }
                        else if (reader.Depth == 1 && reader.TokenType == JsonToken.PropertyName)
                        {
                            if ((string)reader.Value == tokenName)
                            {
                                state = SplitState.InSplitProperty;
                            }
                            else
                            {
                                if (state == SplitState.InSplitProperty)
                                    state = SplitState.InPostfix;
                                var property = JProperty.Load(reader);
                                doRead = false; // JProperty.Load() will have already advanced the reader.
                                if (state == SplitState.InPrefix)
                                {
                                    prefixProperties.Add(property);
                                }
                                else
                                {
                                    postFixProperties.Add(property);
                                }
                            }
                        }
                        else if (reader.Depth == 1 && reader.TokenType == JsonToken.StartArray && state == SplitState.InSplitProperty)
                        {
                            state = SplitState.InSplitArray;
                        }
                        else if (reader.Depth == 1 && reader.TokenType == JsonToken.EndArray && state == SplitState.InSplitArray)
                        {
                            state = SplitState.InSplitProperty;
                        }
                        else if (state == SplitState.InSplitArray && reader.Depth == 2)
                        {
                            if (count % maxItems == 0)
                            {
                                var writer = new JsonTextWriter(createStream(writers.Count)) { Formatting = (Formatting)formatting };
                                writers.Add(writer);
                                writer.WriteStartObject();
                                foreach (var property in prefixProperties)
                                    property.WriteTo(writer);
                                writer.WritePropertyName(tokenName);
                                writer.WriteStartArray();
                            }
                            count++;
                            writers.Last().WriteToken(reader, true);
                        }
                        else
                        {
                            throw new System.Text.Json.JsonException("Internal error");
                        }
                    }
                }
                foreach (var writer in writers)
                    using (writer)
                    {
                        writer.WriteEndArray();
                        foreach (var property in postFixProperties)
                            property.WriteTo(writer);
                        writer.WriteEndObject();
                    }
            }
            finally
            {
                // Make sure files are closed in the event of an exception.
                foreach (var writer in writers)
                    using (writer)
                    {
                    }

            }

        }

        public List<int> GetPositions(string source, string searchString)
        {
            List<int> ret = new List<int>();
            int len = searchString.Length;
            int start = -len;
            while (true)
            {
                start = source.IndexOf(searchString, start + len);
                if (start == -1)
                {
                    break;
                }
                else
                {
                    ret.Add(start);
                }
            }
            return ret;
        }

        public string getOauthHeader(string url)
        {
            //Uri uri = new Uri(url); //+ "&YOUR_QUERY_STRING");  // Omit the query string part if you don't need it.
            //string url1 = @"https://5391094.restlets.api.netsuite.com/app/site/hosting/restlet.nl?script=679&deploy=1";
            var uri = new Uri(url);
            var generator = new OAuthBase();

            string nonce = generator.GenerateNonce();
            string time = generator.GenerateTimeStamp();
            OAuthBase.SignatureTypes signatureType = OAuthBase.SignatureTypes.HMACSHA256;
            string httpMethod = "GET";
            string normalized_url;
            string normalized_params;

            var signature = generator.GenerateSignature(uri, consumer_id, consumer_secret, token_id, token_secret, httpMethod, time, nonce, signatureType, out normalized_url, out normalized_params);
            //File.AppendAllText(@_logPath, "Signature after started:  " + consumer_id + _timer + Environment.NewLine);
            // URL encode any + characters generated in the signature
            if (signature.Contains("+"))
            {
                signature = signature.Replace("+", "%2B");
            }


            //	// Construct the OAuth header		
            string header = "";
            header += "oauth_signature=\"" + signature + "\",";
            header += "oauth_version=\"1.0\",";
            header += "oauth_nonce=\"" + nonce + "\",";
            header += "oauth_signature_method=\"" + "HMAC-SHA256" + "\","; //\"HMAC-SHA256\",";
            header += "oauth_consumer_key=\"" + consumer_id + "\",";
            header += "oauth_token=\"" + token_id + "\",";
            header += "oauth_timestamp=\"" + time + "\",";
            header += "realm=\"" + NS_realm + "\",";
            //header += "oauth_callback=\"" + oauth_callback + "\"";


            return header;
        }



        public async Task<DataTable> getTransferOrder(string url, int dataPullPart)
        {

            ///
            System.Data.DataTable _data = new System.Data.DataTable();
            var _header = getOauthHeader(url);
            Uri _uri = new Uri(url);
            string response = "";

            using (HttpClient _httpClient = new HttpClient())
            {

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", _header);

                var result = await _httpClient.GetAsync(_uri);

                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }

                System.IO.File.AppendAllText(@"TransferOrder_response.txt", response);
                
                ///cleaning the json string header
                List<int> pages = GetPositions(response, "{\"pagedData");
                int pageCount = pages.Count;

                for (int i = 0; i < pageCount; i++)
                {
                    int pagedDataIndex = response.IndexOf("{\"pagedData");
                    int dataIndex = response.IndexOf("\"data\"");
                    response = response.Remove(pagedDataIndex, dataIndex + 7 - pagedDataIndex);
                }

                var responseClear = response;
                var responseClear1 = responseClear.Replace("]}}]},[", "]}},");
                var responseClear2 = responseClear1.Replace(@"[[", @"[");
                var responseClear3 = responseClear2.Replace("]}}]}]", "]}}]");
                var responseClear4 = responseClear3.Replace("}}]}]", "}}]");
                var responseClear5 = responseClear4.Replace("}}]},[{", "}},{");


                result.Dispose();
                _httpClient.Dispose();
                System.IO.File.AppendAllText(@"transfer_Cleared.txt", responseClear5);

                /// convert json to list
                List<transferOrderRoot> ff = JsonConvert.DeserializeObject<List<transferOrderRoot>>(responseClear5);
                List<transferFlatList> collection = new List<transferFlatList>();
                //foreach (Root line in ff)
                for (int i = 0; i < ff.Count; i++)
                {

                    var recordTypeCheck = ff[i].recordType.Count();
                    var vRecordType = "";
                    switch (recordTypeCheck) { case 0: vRecordType = ""; break; default: vRecordType = ff[i].recordType.ToString(); break; };

                    var idCheck = ff[i].id.Count();
                    Int64? vId = null;
                    switch (idCheck) { case 0: vId = null; break; default: vId = Int64.Parse(ff[i].id); break; };

                    var internalidCheck = ff[i].values.internalid.Count();
                    string vInternalIdValue = null;
                    switch (internalidCheck) { case 0: vInternalIdValue = null; break; default: vInternalIdValue = ff[i].values.internalid[0].value.ToString(); break; };
                    string vInternalIdText = null;
                    switch (internalidCheck) { case 0: vInternalIdText = null; break; default: vInternalIdText = ff[i].values.internalid[0].ToString(); break; };


                    var tranDateCheck = ff[i].values.trandate.Count();
                    string vTranDate = null;
                    switch (tranDateCheck) { case 0: vTranDate = null; break; default: vTranDate = ff[i].values.trandate.ToString(); break; };


                    var typeCheck = ff[i].values.type.Count();
                    string vTypeValue = null;
                    switch (typeCheck) { case 0: vTypeValue = null; break; default: vTypeValue = ff[i].values.type[0].value.ToString(); break; };
                    string vTypeText = null;
                    switch (typeCheck) { case 0: vTypeText = null; break; default: vTypeText = ff[i].values.type[0].text.ToString(); break; };

                    var tranIdCheck = ff[i].values.tranid.Count();
                    string vTranId = null;
                    switch (tranIdCheck) { case 0: vTranId = null; break; default: vTranId = ff[i].values.tranid.ToString(); break; };

                    var statusCheck = ff[i].values.statusref.Count();
                    string vStatusValue = null;
                    switch (statusCheck) { case 0: vStatusValue = null; break; default: vStatusValue = ff[i].values.statusref[0].value.ToString(); break; };
                    string vStatusText = null;
                    switch (statusCheck) { case 0: vStatusText = null; break; default: vStatusText = ff[i].values.statusref[0].text.ToString(); break; };

                    var locationCheck = ff[i].values.location.Count();
                    string vLocationValue = null;
                    switch (locationCheck) { case 0: vLocationValue = null; break; default: vLocationValue = ff[i].values.location[0].value.ToString(); break; };
                    string vLocationText = null;
                    switch (locationCheck) { case 0: vLocationText = null; break; default: vLocationText = ff[i].values.location[0].text.ToString(); break; };

                    var transferLocationCheck = ff[i].values.transferlocation.Count();
                    string vTransferLocationValue = null;
                    switch (transferLocationCheck) { case 0: vTransferLocationValue = null; break; default: vTransferLocationValue = ff[i].values.transferlocation[0].value.ToString(); break; };
                    string vTransferLocationText = null;
                    switch (transferLocationCheck) { case 0: vTransferLocationText = null; break; default: vTransferLocationText = ff[i].values.transferlocation[0].text.ToString(); break; };

                    var createdBy = ff[i].values.createdby.Count();
                    string vCreatedByValue = null;
                    switch (createdBy) { case 0: vCreatedByValue = null; break; default: vCreatedByValue = ff[i].values.createdby[0].value.ToString(); break; };
                    string vCreatedByText = null;
                    switch (createdBy) { case 0: vCreatedByText = null; break; default: vCreatedByText = ff[i].values.createdby[0].text.ToString(); break; };

                    var deliveryInstructionCheck = ff[i].values.custbody_cts_delivery_instruction.Count();
                    string vDeliveryInstruction = null;
                    switch (deliveryInstructionCheck) { case 0: vDeliveryInstruction = null; break; default: vDeliveryInstruction = ff[i].values.custbody_cts_delivery_instruction.ToString(); break; };






                    collection.Add(
                    new transferFlatList
                    {
                        recordType = vRecordType,
                        id = (Int64)vId,
                        internalidValue = vInternalIdValue,
                        internalidText = vInternalIdText,
                        trandate = vTranDate,
                        typeValue = vTypeValue,
                        typeText = vTypeText,
                        tranid = vTranId,
                        statusValue = vStatusValue,
                        statusText = vStatusText,
                        locationValue = vLocationValue,
                        locationText = vLocationText,
                        transferLocationValue = vTransferLocationValue,
                        transferLocationText = vTransferLocationText,
                        createdByValue = vCreatedByValue,
                        createdByText = vCreatedByText,
                        deliveryInstruction = vDeliveryInstruction,
                        dataPullPart = dataPullPart
                    }
                    );
                };

                ////List<precFlatList> accounts = new List<precFlatList>();
                ListtoDataTable listtodt = new ListtoDataTable();
                _data = listtodt.ToDataTable(collection);

            }

            ///return a data set
            return _data;
        }


    }


    public class ListtoDataTable
    {
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
