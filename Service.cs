using System;
using System.IO;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Content;
using Android.Preferences;
using Android.Util;
using File_Tracking.Model;
using Newtonsoft.Json;

namespace File_Tracking
{
    class Service
    {

        HttpClient client;
        HttpWebRequest request;
        Cryptography cryptography;
        Geolocation geo;
        string licenceId, UserId, AppDateTime, geolocation;
        public Service()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            cryptography = new Cryptography();
            geo = new Geolocation();

        }

        public async Task<JsonValue> LoginUser2(string userid, string password, string licenceid, string geolocation)
        {
            //Items = new LoginModel();
            JsonValue jsonDoc = null;
            string url = "http://mobileapi.work121.com/api/Login/Getloginuser?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceid + "&UserId=" + userid + "&Password=" + password + "&geolocation=" + geolocation + "&licenceFor=OPDApp";

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {

                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        string decrypted_Text = await OnCryptoAsync2(text, "Decryption");
                        // Use this stream to build a JSON document object:
                        jsonDoc = await Task.Run(() => JsonObject.Parse(decrypted_Text));
                        Log.Error("Response: {0}", jsonDoc.ToString());

                        // Return the JSON document:
                        // Items = JsonConvert.SerializeObject(jsonDoc);

                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error:", e.Message);
            }
            return jsonDoc;

        }



        public async Task<string> GetLicenceId(string geoLocation, string gcmid)
        {
            string licenceId = "";

            string url = "http://mobileapi.work121.com/api/Login/GetLicenceId?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceFor=OPDApp&geolocation=" + geoLocation + "&gcmid=" + gcmid;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {

                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        string[] tokens = text.Split(',');
                        if (tokens[0].Contains("Success"))
                        {
                            licenceId = tokens[1].Substring(0, tokens[1].Length - 1);
                           
                            
                        }
                       
                        

                    }
                }
            }
            catch (Exception e)
            {

            }
            return licenceId;

        }

        public async Task<string> RegisterUser(string licenceid, string mobileNumber, string name, string geo_location, string npid, string userid)
        {
            string text = "";
            string url = "http://mobileapi.work121.com/api/Login/VerifylicenceId?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&" +
                "associatepwd=mA121&licenceId=" + licenceid + "&ProviderName=&ProvideId=&MobileNumber=" + mobileNumber + "&EmailID=&Name=" + name + "&" +
                "geolocation=" + geo_location + "&selfiePath=&NPID=" + npid + "&UserId=" + userid + "&licenceFor=OPDApp";

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        text = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error:", e.Message);
            }
            return text;

        }

        ISharedPreferences prefs;

        public async Task<string> GetServiceMethod(Context context, string methodName, string jsonData)
        {

            prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            init(context);

            if (jsonData.Equals(""))
            {
                jsonData = "w3ZUSfhMYXujjSK28/4kfw==";
            }
            //else
            //{
            //    encry_jsonData = await OnCryptoAsync(jsonData, "Encryption");
            //}


            string url = "http://mobileapi.work121.com/api/Hosted/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" +
                licenceId + "&UserId=" + UserId + "&methodname=" + methodName + "&jsonData=" + jsonData + "&AppDateTime=" + AppDateTime;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                            //try
                            // {
                            // List<AilmentModel> responseModel2 = JsonConvert.DeserializeObject<List<AilmentModel>>(decrypted_Text);
                            // jsonDoc = await Task.Run(() => JsonArray.Parse(decrypted_Text));
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                            // }
                            //  catch (Exception e)
                            //  {
                            //     return null;
                            //  }

                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public void init(Context context)
        {
            prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            licenceId = prefs.GetString("LicenceId", "");
            UserId = prefs.GetString("DesignationId", "");
            AppDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            geolocation = geo.GetGeoLocation(context);

        }

        public async Task<JsonValue> PostServiceMethod(Context context, string methodName, string jsonData)
        {
            JsonValue jsonDoc;
            init(context);
            string url = "http://mobileapi.work121.com/api/Hosted/PostMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" +
                licenceId + "&UserId=" + UserId + "&methodname=" + methodName + "&jsonData=" + jsonData + "&AppDateTime=" + AppDateTime + "&GeoLocation=" + geolocation;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                           
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                           

                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }


        //public async Task<JsonValue> getAilmentList(string licenceId)
        //{

        //    JsonValue jsonDoc = null;
        //    string url = "http://mobileapi.work121.com/api/Hosted/GetAilmentMaster/8B280FFF-BFDD-4F62-8D46-08BA937DB981/mA121/"+licenceId+"";

        //    request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
        //    request.ContentType = "application/json";
        //    request.Method = "GET";
        //    request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
        //    request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
        //    try
        //    {
        //        using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
        //        {
        //            using (Stream stream = response.GetResponseStream())
        //            {
        //                StreamReader reader = new StreamReader(stream);
        //                string text = reader.ReadToEnd();
        //                string decrypted_Text = await OnCryptoAsync(text, "Decryption");
        //                // Use this stream to build a JSON document object:
        //                jsonDoc = await Task.Run(() => JsonObject.Parse(decrypted_Text));
        //                Log.Error("Response: {0}", jsonDoc.ToString());



        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //    return jsonDoc;
        //}





        public async Task<JsonValue> getAttendanceList(string licenceId, string desigId, string npId)
        {

            JsonValue jsonDoc = null;
            string url = "http://mobileapi.work121.com/api/Attendance/GetNaturalPersonForAttendance?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId + "&designationid=" + desigId + "&npid=" + npId + "";
            //dynamic attendance = new dynamic();

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        // Use this stream to build a JSON document object:
                        jsonDoc = await Task.Run(() => JsonObject.Parse(decrypted_Text));
                        Log.Error("Response: {0}", jsonDoc.ToString());



                    }
                }
            }
            catch (Exception e)
            {
            }
            return jsonDoc;
        }


        public async Task<string> getNaturalpersondata(Context context, string jsondata, string methodName)
        {
            init(context);
            JsonValue jsonDoc = null;
            string decrypted_Text = null;
            string url = "http://mobileapi.work121.com/api/Snayu/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=" + methodName + "&jsonData=" + jsondata + "&AppDateTime=" + AppDateTime;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();

                        //if (responseModel.ResponseCode.Equals("Ok"))
                        //{
                        decrypted_Text = await OnCryptoAsync2(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(decrypted_Text);
                        //  jsonDoc = await Task.Run(() => JsonObject.Parse(decrypted_Text));
                        //    reader.Close();
                        //    stream.Close();
                        //    return decrypted_Text;

                        //}
                        //else
                        //{
                        //    return responseModel.ResponseCode;
                        //}
                        return responseModel.ResponseValue;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<JsonValue> postNaturalpersondata(Context context, string jsondata, string methodName)
        {
            init(context);
            JsonValue jsonDoc = null;
            string url = "http://mobileapi.work121.com/api/Snayu/PostMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=" + methodName + "&jsonData=" + jsondata + "&AppDateTime=" + AppDateTime;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        jsonDoc = await Task.Run(() => JsonObject.Parse(decrypted_Text));

                    }
                }
            }
            catch (Exception e)
            {
            }
            return jsonDoc;
        }


        public async Task<string> OnCryptoAsync(string textValue, string encDesc)
        {
            string text = "";
            if (encDesc.Equals("Encryption"))
            {
                text = textValue;
            }
            else
            {
                text = textValue.Substring(1, textValue.Length - 2);
            }
            string decryptedString = "";

            try
            {
                decryptedString = await cryptography.FunctionAsync(textValue, encDesc);
            }
            catch (Exception e)
            {
               
            }

            return decryptedString;
        }

        public async Task<string> OnCryptoAsync2(string textValue, string encDesc)
        {
            string text = "";
            if (encDesc.Equals("Encryption"))
            {
                text = textValue;
            }
            else
            {
                text = textValue.Substring(1, textValue.Length - 2);
            }
            string decryptedString = "";

            try
            {
                decryptedString = await cryptography.FunctionAsync(text, encDesc);
            }
            catch (Exception e)
            {
                // Debug.WriteLine("error", e.Message);
            }

            return decryptedString;
        }


        // Attendance Submission Service
        //public async Task<JsonValue> AttendanceSubmission(string licenceid, string npid, string employeeid, string attendanceType,
        //    string attendanceStatus, string geolocation, string photoPath, string videoPath, string audioPath, string appdateTime, string distance, string markingLatLng)
        //{
        //    JsonValue jsonDoc = null;
        //    string url = "http://mobileapi.work121.com/api/Attendance/SetNaturalPersonAttendance?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceid=" + licenceid + "&npid=" + npid + "&EmployeeID=" + employeeid + "&AttendanceType=" +
        //        "" + attendanceType + "&AttendanceStatus=" + attendanceStatus + "&Geolocation=" + geolocation + "&PhotoPath=" + photoPath + "&VideoPath=" + videoPath + "&AudioPath=" +
        //        "" + audioPath + "&AppDateTime=" + appdateTime + "&Distance=" + distance + "&MarkerLatLng=" + markingLatLng + "";

        //    request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
        //    request.ContentType = "application/json";
        //    request.Method = "GET";
        //    request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
        //    request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

        //    try
        //    {
        //        // Send the request to the server and wait for the response:
        //        using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
        //        {
        //            // Get a stream representation of the HTTP web response:
        //            using (Stream stream = response.GetResponseStream())
        //            {
        //                StreamReader reader = new StreamReader(stream);
        //                string text = reader.ReadToEnd();

        //                string check = "";
        //                string encryptedString = await OnCryptoAsync(check, "Encryption");

        //                string decrypted_Text = await OnCryptoAsync(text, "");
        //                // Use this stream to build a JSON document object:
        //                jsonDoc = await Task.Run(() => JsonObject.Parse(decrypted_Text));
        //                Debug.WriteLine("Response: {0}", jsonDoc.ToString());
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("Error:", e.Message);
        //    }
        //    return jsonDoc;
        //}

        //public async Task<string> SubmitCampDetail(string licenceId, string userId, string jsonData)
        //{
        //    string text = null;
        //    // string encJsonData = await OnCryptoAsync(jsonData, "Encryption");
        //    // string encJsonData2= encJsonData.Replace(" ", "+");
        //    string url = "http://mobileapi.work121.com/api/Hosted/SetCampDetails?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId + "&userId=" + userId + "&jsonData=" + jsonData;

        //    request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
        //    request.ContentType = "application/json";
        //    request.Method = "GET";
        //    request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
        //    request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
        //    try
        //    {
        //        // Send the request to the server and wait for the response:
        //        using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
        //        {
        //            // Get a stream representation of the HTTP web response:
        //            using (Stream stream = response.GetResponseStream())
        //            {
        //                StreamReader reader = new StreamReader(stream);
        //                text = reader.ReadToEnd();
        //                //string decrypted_Text = await OnCryptoAsync(text, "Decryption");
        //                //text2 = decrypted_Text;
        //                //  Debug.WriteLine("Response: {0}", text);

        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //Debug.WriteLine("Error:", e.Message);
        //    }
        //    return text;
        //}

        //public async Task<JsonValue> LocationType(string licenceId)
        //{
        //    JsonValue jsonDoc = null;
        //    string url = "http://mobileapi.work121.com/api/Hosted/GetLocationType?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId;

        //    request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
        //    request.ContentType = "application/json";
        //    request.Method = "GET";
        //    request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
        //    request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
        //    try
        //    {
        //        // Send the request to the server and wait for the response:
        //        using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
        //        {
        //            // Get a stream representation of the HTTP web response:
        //            using (Stream stream = response.GetResponseStream())
        //            {
        //                StreamReader reader = new StreamReader(stream);
        //                string text = reader.ReadToEnd();
        //                string decrypted_Text = await OnCryptoAsync(text, "Decryption");
        //                jsonDoc = await Task.Run(() => JsonObject.Parse(decrypted_Text));


        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //Debug.WriteLine("Error:", e.Message);
        //    }
        //    return jsonDoc;
        //}


        public async Task<JsonValue> GetAttendanceService(Context context, string methodName, string jsonData)
        {
            JsonValue jsonDoc;
            prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            string licenceId = prefs.GetString("LicenceId", "");
            string UserId = prefs.GetString("DesignationId", "");
            string AppDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (jsonData.Equals(""))
            {
                jsonData = "w3ZUSfhMYXujjSK28/4kfw==";
            }
            //else
            //{
            //    encry_jsonData = await OnCryptoAsync(jsonData, "Encryption");
            //}


            string url = "http://mobileapi.work121.com/api/Attendance/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" +
                licenceId + "&UserId=" + UserId + "&methodname=" + methodName + "&jsonData=" + jsonData + "&AppDateTime=" + AppDateTime;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                            //try
                            // {
                            // List<AilmentModel> responseModel2 = JsonConvert.DeserializeObject<List<AilmentModel>>(decrypted_Text);
                            // jsonDoc = await Task.Run(() => JsonArray.Parse(decrypted_Text));
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                            // }
                            //  catch (Exception e)
                            //  {
                            //     return null;
                            //  }

                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<JsonValue> PostAttendanceService(Context context, string methodName, string jsonData)
        {
            JsonValue jsonDoc;
            prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            string licenceId = prefs.GetString("LicenceId", "");
            string UserId = prefs.GetString("DesignationId", "");
            string AppDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // string encry_jsonData = await OnCryptoAsync(jsonData, "Encryption");
            string geolocation = geo.GetGeoLocation(context);

            string url = "http://mobileapi.work121.com/api/Attendance/PostMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" +
                licenceId + "&UserId=" + UserId + "&methodname=" + methodName + "&jsonData=" + jsonData + "&AppDateTime=" + AppDateTime + "&GeoLocation=" + geolocation;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                            //try
                            // {
                            // List<AilmentModel> responseModel2 = JsonConvert.DeserializeObject<List<AilmentModel>>(decrypted_Text);
                            // jsonDoc = await Task.Run(() => JsonArray.Parse(decrypted_Text));
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                            // }
                            //  catch (Exception e)
                            //  {
                            //     return null;
                            //  }

                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }


        public async Task<string> SendSMS(string licenceId, string message, string phone_no, string user_id)
        {
            //JsonValue jsonDoc = null;
            string text = null;
            string url = "http://mobileapi.work121.com/api/Login/SendSMS?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId + "&UserId=" + user_id + "&MobileNo=" + phone_no + "&SMS=" + message;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        text = reader.ReadToEnd();
                        //string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        //jsonDoc = await Task.Run(() => JsonObject.Parse(decrypted_Text));
                    }
                }
            }
            catch (Exception e)
            {
                //Debug.WriteLine("Error:", e.Message);
            }
            return text;
        }


    }
}