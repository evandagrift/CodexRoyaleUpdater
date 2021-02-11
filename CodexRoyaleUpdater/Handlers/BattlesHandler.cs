using CodexRoyaleUpdater.Handlers.Interfaces;
using Newtonsoft.Json;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater.Handlers
{
    public class BattlesHandler : IBattlesHandler
    {

        //provides HTTP access for both codex API and the official API
        Client client;

        //connection sting for the Codex API Controller that is being handled
        private string codexConnectionString = "Battles";
        private string officialConnectionString = "/v1/players/%23";


        //passes in client for use
        public BattlesHandler(Client c)
        {
            client = c;
        }


        public async Task<List<Battle>> GetOfficialPlayerBattles(string tag)
        {
            //connection string to fetch player battles with given Tag
            string connectionString =  officialConnectionString + tag.Substring(1) + "/battlelog/";

            //calls the official API
            var result = await client.officialAPI.GetAsync(connectionString);

            //if the call is a success it returns the List of Battles
                if (result.IsSuccessStatusCode)
                {
                //content to json string once recieved and parsed
                    var content = await result.Content.ReadAsStringAsync();

                //deserielizes the json to list of battles
                    var battles = JsonConvert.DeserializeObject<List<Battle>>(content);
                    
                    //cleans up the time string, the official API includes a non functioning TimeZone offset to their datetime string
                    battles.ForEach(b =>
                    {
                        b.BattleTime = b.BattleTime.Substring(0, 15);
                    });

                //returns fetched list of battles
                    return battles;
                }
                return null;
        }

        public async Task<Battle> GetSetBattleId(Battle battle)
        {
            try
            {
                //serializes Card object to Json
                string json = JsonConvert.SerializeObject(battle);

                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //posts the Battle to the Codex API
                var response = await client.codexAPI.PostAsync("Battles/getbattlewithid", data);
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Battle>(content);
            }
            catch { }
            return null;
        }


        //returns all the Battles in the Codex Api
        public async Task<List<Battle>> GetAllBattles()
        {
            try
            {
                //puts the returned content to a string (of Json)
                var result = await client.codexAPI.GetAsync(codexConnectionString);

                //If the api call was successful
                if (result.IsSuccessStatusCode)
                {
                    //json content from the response message
                    var content = await result.Content.ReadAsStringAsync();

                    //removing the base layer from called Json++++++++++++++++++++++++++++++++++++++++++++++++++++++++- 
                    return JsonConvert.DeserializeObject<List<Battle>>(content); ;
                }
                return null;


            }
            catch { return null; }
            return null;
        }

        public async Task<Battle> GetBattleById(int id)
        {

            try
            {
                //puts the returned content to a string (of Json)
                var result = await client.codexAPI.GetAsync(codexConnectionString + '/' + id);

                //If the api call was successful
                if (result.IsSuccessStatusCode)
                {
                    //json content from the response message
                    var content = await result.Content.ReadAsStringAsync();

                    //removing the base layer from called Json++++++++++++++++++++++++++++++++++++++++++++++++++++++++- 
                    return JsonConvert.DeserializeObject<Battle>(content); ;
                }
                return null;


            }
            catch { return null; }
            return null;
        }


        public async Task AddBattle(Battle battle)
        {
            try
            {
                //serializes Card object to Json
                string json = JsonConvert.SerializeObject(battle);

                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //posts the Battle to the Codex API
                await client.codexAPI.PostAsync("Battles/addbattle", data);
            }
            catch { }
        }
        public async Task<int> AddBattles(List<Battle> battles)
        {
            try
            {
                //serializes Card object to Json
                string json = JsonConvert.SerializeObject(battles);

                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //posts the Battle to the Codex API
                var response = await client.codexAPI.PostAsync("Battles/addbattles", data);
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<int>(content);
            }
            catch { }
            return 0;
        }


        public async Task DeleteBattle(int id)
        {
            try
            {
                //posts the Battle to the Codex API
                var response = await client.codexAPI.DeleteAsync("Battles/" + id);
            }
            catch { }

        }


        public async Task UpdateBattle(Battle battle)
        {
            try
            {
                //serializes Clan object to Json
                string json = JsonConvert.SerializeObject(battle);

                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //Puts the Clan to the Codex API
                var response = await client.codexAPI.PutAsync(codexConnectionString, data);
            }
            catch { }
        }






        /*
public async Task<Battle> GetPlayerCurrentBattle(string tag)
{

   if (tag != null)
   {
       try
       {
           string connectionString = "/v1/players/%23" + tag.Substring(1);

           var result = await client.officialAPI.GetAsync(connectionString);

           if (result.IsSuccessStatusCode)
           {
               var content = await result.Content.ReadAsStringAsync();
               Player player = JsonConvert.DeserializeObject<Player>(content);
               player.SetBattle();
               return await GetSetBattleId(player.Battle);
           }
       }
       catch { return null; }
   }
   return null;
}
*/
    }
}
