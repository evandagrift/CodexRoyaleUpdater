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


        public static string officialBearerToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiIsImtpZCI6IjI4YTMxOGY3LTAwMDAtYTFlYi03ZmExLTJjNzQzM2M2Y2NhNSJ9.eyJpc3MiOiJzdXBlcmNlbGwiLCJhdWQiOiJzdXBlcmNlbGw6Z2FtZWFwaSIsImp0aSI6ImQ2MjlmZWI0LWU4ZWUtNDAwMi1hNWNhLWY5Y2MxZTYyOGNiMiIsImlhdCI6MTYwNjAzNzc3Nywic3ViIjoiZGV2ZWxvcGVyLzNhNjMxNDdlLWM0MDItNjA0YS1lN2YzLWU0ODc3MDNhOWE2YyIsInNjb3BlcyI6WyJyb3lhbGUiXSwibGltaXRzIjpbeyJ0aWVyIjoiZGV2ZWxvcGVyL3NpbHZlciIsInR5cGUiOiJ0aHJvdHRsaW5nIn0seyJjaWRycyI6WyI3MS4yMzYuMTM4LjE2MSJdLCJ0eXBlIjoiY2xpZW50In1dfQ.upmGRqrABJn6H8cOx0TW5gdwil9_6aZ4vwJHvPqZEfT39Q1RjtNfhEhUKz4SPZYz8sUFgQr8ehlRs1QKBa9-sw";
        public static string codexBearerToken = "071aa839-8e03-4aa1-966e-fe547284f2e9";

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
            codexAPI.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", codexBearerToken);

        }
    }
}
