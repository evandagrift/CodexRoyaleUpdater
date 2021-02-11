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
    public class DecksHandler : IDecksHandler
    {

        //provides HTTP access for both codex API and the official API
        Client client;

        //connection sting for the Codex API Controller that is being handled
        private string codexConnectionString = "Decks";

        //passes in client for use
        public DecksHandler(Client c)
        {
            client = c;
        }


        //returns all the decks in the Codex Api
        public async Task<List<Deck>> GetAllDecks()
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
                    return JsonConvert.DeserializeObject<List<Deck>>(content); ;
                }
                return null;


            }
            catch { return null; }
            return null;
        }

        public async Task<Deck> GetDeck(int id)
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
                    return JsonConvert.DeserializeObject<Deck>(content); ;
                }
                return null;


            }
            catch { return null; }
            return null;
        }

        public async Task<Deck> GetSetDeckId(Deck deck)
        {
            try
            {
                //serializes Card object to Json
                string json = JsonConvert.SerializeObject(deck);

                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //posts the Card to the Codex API
                var response = await client.codexAPI.PostAsync("Decks/GetDeckWithId", data);
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Deck>(content);
            }
            catch { }
            return null;
        }

        public async Task<int> GetDeckId(Deck deck)
        {
            //returns the Id of the given deck(creating a new one if it doesn't exist)
            return GetSetDeckId(deck).Result.Id;
        }

        public async Task DeleteDeck(int id)
        {
            try
            {
                //posts the Deck to the Codex API
                var response = await client.codexAPI.DeleteAsync("Decks/" + id);
            }
            catch { }

        }


        public async Task UpdateDeck(Deck deck)
        {
            try
            {
                //serializes Clan object to Json
                string json = JsonConvert.SerializeObject(deck);

                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //Puts the Clan to the Codex API
                var response = await client.codexAPI.PutAsync(codexConnectionString, data);
            }
            catch { }
        }

        //gets players current deck from Clash Royale API
        public async Task<Deck> GetPlayerCurrentDeck(string tag)
        {

            if (tag != null)
            {
                //try in case connection fail
                try
                {
                    string connectionString = "/v1/players/%23" + tag.Substring(1);

                    //gets player data from official API
                    var result = await client.officialAPI.GetAsync(connectionString);

                    if (result.IsSuccessStatusCode)
                    {
                        var content = await result.Content.ReadAsStringAsync();

                        //deserializes returned JSON
                        Player player = JsonConvert.DeserializeObject<Player>(content);

                        //calls the set Deck function which will set all the CardIds for DB consumption
                        player.SetDeck();

                        //gets that deck or adds it if it doesn't exist
                        return await GetSetDeckId(player.Deck);
                    }
                }
                catch { return null; }
            }
            return null;
        }


    }
}
