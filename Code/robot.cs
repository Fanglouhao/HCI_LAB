/**
 * Turing Robot
 */

using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Net;
using System.Text;
using MiniJSON;

namespace HCI {
    
    /* Turing Robot */
    public class Robot {
        private static string url = "http://openapi.tuling123.com/openapi/api/v2";
        private static string apiKey = "71921271889046f3ae8c7f320ccd4803";
        private static string userId = "54749110";

        /* status and response */
        private bool status; // is ok ?
        private string msg;
        private string resp;

        public Robot() {
            reset();
        }

        private void reset() {
            status  = true;
            msg     = "";
            resp    = "";
        }

        public void accept(string question) {
            reset();

            string json_string = new JsonBuilder(question).getString();
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            var byteData = Encoding.UTF8.GetBytes(json_string);
            var length = byteData.Length;
            request.ContentLength = length;
            var writer = request.GetRequestStream();
            writer.Write(byteData, 0, length);
            writer.Close();

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
            var dic = Json.Deserialize(responseString) as Dictionary<string, object>;
            var results = dic["results"];
            var first_json = ((List<object>)results)[0];
            var values = ((Dictionary<string, object>)first_json)["values"];
            resp = (string)((Dictionary<string, object>)values)["text"];
        }

        public bool ok() {
            return status;
        }

        public string getErrorMessage() {
            return msg;
        }

        public string getResponse() {
            return resp;
        }

    }

    class JsonBuilder {
        private static string apiKey = "71921271889046f3ae8c7f320ccd4803";
        private static string userId = "54749110";

        private string json_string;

        public JsonBuilder(string text) {
            Dictionary<string, object> json = new Dictionary<string, object>();
            Dictionary<string, object>  perception = new Dictionary<string, object>(),
                                            inputText = new Dictionary<string, object>(),
                                        userInfo = new Dictionary<string, object>();
            inputText["text"] = (object)text;
            perception["inputText"] = (object)inputText;
            userInfo["apiKey"] = (object)apiKey;
            userInfo["userId"] = (object)userId;
            
            json["reqType"] = 0; // text
            json["perception"] = perception;
            json["userInfo"] = userInfo;

            json_string = Json.Serialize(json);
        }

        public string getString() {
            return json_string;
        }

    }

}