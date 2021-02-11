using NUnit.Framework;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

            string bearerToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiIsImtpZCI6IjI4YTMxOGY3LTAwMDAtYTFlYi03ZmExLTJjNzQzM2M2Y2NhNSJ9.eyJpc3MiOiJzdXBlcmNlbGwiLCJhdWQiOiJzdXBlcmNlbGw6Z2FtZWFwaSIsImp0aSI6ImQ2MjlmZWI0LWU4ZWUtNDAwMi1hNWNhLWY5Y2MxZTYyOGNiMiIsImlhdCI6MTYwNjAzNzc3Nywic3ViIjoiZGV2ZWxvcGVyLzNhNjMxNDdlLWM0MDItNjA0YS1lN2YzLWU0ODc3MDNhOWE2YyIsInNjb3BlcyI6WyJyb3lhbGUiXSwibGltaXRzIjpbeyJ0aWVyIjoiZGV2ZWxvcGVyL3NpbHZlciIsInR5cGUiOiJ0aHJvdHRsaW5nIn0seyJjaWRycyI6WyI3MS4yMzYuMTM4LjE2MSJdLCJ0eXBlIjoiY2xpZW50In1dfQ.upmGRqrABJn6H8cOx0TW5gdwil9_6aZ4vwJHvPqZEfT39Q1RjtNfhEhUKz4SPZYz8sUFgQr8ehlRs1QKBa9-sw";


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52003/api/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "3ba1e2ed-bc14-470b-b3da-bd911bbfa0b8");

            HttpClient officialAPI = new HttpClient();
            officialAPI.BaseAddress = new Uri("https://api.clashroyale.com/");
            officialAPI.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            ClansHandler cardsHandler = new CardsHandler();

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}