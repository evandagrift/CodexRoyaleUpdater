using CodexRoyaleUpdater.Handlers.Interfaces;
using Newtonsoft.Json;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater
{
    public class ClansHandler : IClansHandler
    {
        //client class has access to both APIs
        Client client;

        //connection string for clans in the API
        private string codexConnectionString = "Clans";
        private string officialConnectionString = "/v1/clans/%23";


        //Constructor adds reference to HTTP Clients
        public ClansHandler(Client c)
        {
            client = c;
        }

        //gets clan data from the official api via their clan tag
        public async Task<Clan> GetOfficialClan(string tag)
        {

            if (tag != null)
            {
                try
                {
                    //connection string for clan in offical API
                    string connectionString = officialConnectionString + tag.Substring(1);

                    //fetches clan data
                    var result = await client.officialAPI.GetAsync(connectionString);

                    
                    if (result.IsSuccessStatusCode)
                    {
                        var content = await result.Content.ReadAsStringAsync();

                        //deseriealizes json into Clan object
                        Clan clan = JsonConvert.DeserializeObject<Clan>(content);

                        //sets location code to a format that the DB can consume
                        clan.LocationCode = clan.Location["countryCode"];

                        //update time in same format as official API
                        clan.UpdateTime = DateTime.UtcNow.ToString("yyyyMMddTHHmmss");

                        return clan;
                    }
                }
                catch { return null; }
            }
            return null;
        }

        //gets clan save instance via Id from Code API
        public async Task<Clan> GetClan(int id)
        {
            try
            {
                string connectionString = "Clans/" + id;

                //fetches Clan record with given Id
                var result = await client.codexAPI.GetAsync(connectionString);

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Clan>(content);
                }
            }
            catch { return null; }
            return null;
        }

        //returns all saved Clan instances  from Codex API
        public async Task<List<Clan>> GetAllClans()
        {
            try
            {
                //gets all clans in the Codex API
                var result = await client.codexAPI.GetAsync(codexConnectionString);
                Console.WriteLine();

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Clan>>(content);
                }
            }
            catch { return null; }
            return null;
        }

        //adds clan to Codex via Clan Object
        //this cannot have a set Id because it is auto generated
        public async Task AddClan(Clan clan)
        {
            try
            {
                //serializes Clan object to Json
                string json = JsonConvert.SerializeObject(clan);

                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //posts the Clan to the Codex API
                var response = await client.codexAPI.PostAsync("Clans", data);
            }
            catch { }
        }

        //Updates clan instance(It needs a set Id)
        public async Task UpdateClan(Clan clan)
        {
            try
            {

                //serializes Clan object to Json
                string json = JsonConvert.SerializeObject(clan);

                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //Puts the Clan to the Codex API
                var response = await client.codexAPI.PutAsync("Clans", data);
            }
            catch { }

        }

        public async Task AddClan(string tag)
        {
            //fetches current clan by tag
            Clan clan = await GetOfficialClan(tag);

            //saves clan to codex
            await AddClan(clan);

        }
        public async Task DeleteClan(int id)
        {
            try
            {
                //deletes clan in codex at given Id
                var response = await client.codexAPI.DeleteAsync("Clans/" + id);
            }
            catch { }
        }

    }
}
