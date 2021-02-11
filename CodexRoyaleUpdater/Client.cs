using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CodexRoyaleUpdater
{
    public class Client
    {
        public static string officialAPIConnectionString = "https://api.clashroyale.com/";
        public static string codexAPIConnectionString = "http://localhost:52003/api/";


        public static string officialBearerToken = "BEARER_TOKEN";
        public static string codexBearerToekn = "BEARER_TOKEN";

        public HttpClient codexAPI;
        public HttpClient officialAPI;

        public Client()
        {

            officialAPI = new HttpClient();
            officialAPI.BaseAddress = new Uri(officialAPIConnectionString);
            officialAPI.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", officialBearerToken);

            codexAPI = new HttpClient();
            codexAPI.BaseAddress = new Uri(codexAPIConnectionString);
            //pull from appsettings
            codexAPI.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", codexBearerToekn);

        }
    }
}
