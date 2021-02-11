using CodexRoyaleUpdater.Handlers.Interfaces;
using Newtonsoft.Json;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater
{
    public class CardsHandler : ICardsHandler
    {
        //provides HTTP access for both codex API and the official API
        Client client;

        //connection sting for the Codex API Controller that is being handled
        private string codexConnectionString = "Cards";

        //passes in client for use
        public CardsHandler(Client c)
        {
            client = c;
        }

        //retrieves all cards in the game from official API
        //Returns as a list of Card objects parsed via Newtonsoft
        public async Task<List<Card>> GetAllOfficialCards()
        {
            try
            {
                //connection string for official cards
                string connectionString = "v1/cards";

                //gets the cards in a response message variable
                var result = await client.officialAPI.GetAsync(connectionString);

                //If the api call was successful
                if (result.IsSuccessStatusCode)
                {
                    //puts the returned content to a string (of Json)
                    var content = await result.Content.ReadAsStringAsync();

                    // trim "{"items":" and the  "}"   from the end from the json call to make it consumable via Newtonsoft 
                    //10 for the begginning being removed, 2 for the last two characters
                    content = content.Substring(9, (content.Length - (9 + 1)));

                    
                    //returns the json as a list of Card objects via newtonsoft
                    return JsonConvert.DeserializeObject<List<Card>>(content); ;
                }
                return null;


            }
            catch { return null; }
            return null;
        }
        
        //returns all the cards in the Codex Api
        public async Task<List<Card>> GetAllCodexCards()
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
                    return JsonConvert.DeserializeObject<List<Card>>(content); ;
                }
                return null;


            }
            catch { return null; }
            return null;
        }


        public async Task AddCard(Card card)
        {
            try
            {
                //serializes Card object to Json
                string json = JsonConvert.SerializeObject(card);

                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //posts the Card to the Codex API
                var response = await client.codexAPI.PostAsync("Cards", data);
            }
            catch { }
        }


        public async Task DeleteCard(int id)
        {
            try
            {
                //posts the Card to the Codex API
                var response = await client.codexAPI.DeleteAsync("Cards/" + id);
            }
            catch { }

        }

        public async Task<Card> GetCodexCard(int id)
        {

            try
            {
                //puts the returned content to a string (of Json)
                var result = await client.codexAPI.GetAsync(codexConnectionString+'/'+id);

                //If the api call was successful
                if (result.IsSuccessStatusCode)
                {
                    //json content from the response message
                    var content = await result.Content.ReadAsStringAsync();

                    //removing the base layer from called Json++++++++++++++++++++++++++++++++++++++++++++++++++++++++- 
                    return JsonConvert.DeserializeObject<Card>(content); ;
                }
                return null;


            }
            catch { return null; }
            return null;
        }


        public async Task UpdateCard(Card card)
        {
            try
            {
                //serializes Clan object to Json
                string json = JsonConvert.SerializeObject(card);

                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //Puts the Clan to the Codex API
                var response = await client.codexAPI.PutAsync(codexConnectionString, data);
            }
            catch { }
        }

        public async Task UpdateCodex()
        {
            //test against official
            //add any that aren't in DB
            List<Card> officialCards = await GetAllOfficialCards();
            List<Card> codexCards = await GetAllCodexCards();

            //if successfully returned
            //NEED TO TEST AGAINST 0 in Codex
            if (officialCards != null && codexCards != null)
            {
                if (officialCards.Count > codexCards.Count)
                {
                    //cycles through all cards in the codex
                    codexCards.ForEach(codexCard =>
                    {
                        //finds card in official with matching Id
                        //I'm doing it this way because the returned official cards class instance won't be an exact match to the one in codex
                        Card cardToRemove = officialCards.Where(c => c.Id == codexCard.Id).FirstOrDefault();

                        //if card was properly located it removes it from the list of official cards
                        if (cardToRemove != null)
                            officialCards.Remove(cardToRemove);
                    });

                    //after all the codex cards have been removed
                    //adds all the remaining cards that aren't in the codex
                    for(int i = 0; i < officialCards.Count; i++)
                    {
                        await AddCard(officialCards[i]);
                    }

                }
            }




        }
    }
}
/*
        public CardsHandler()
        {

        }
        public async Task<List<Card>> GetAllCards(HttpClient client)
        {
            string connectString;
            if (client.BaseAddress.OriginalString == "http://localhost:52003/api/")
            {
                connectString = "Cards";
            }
            else connectString = "/v1/cards?";

            var result = await client.GetAsync(connectString);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                //Official API has different format for Cards than mine
                if (content.StartsWith("{\"items\":"))
                {
                    content = content.Substring(9, content.Length - 10);
                }
                return JsonConvert.DeserializeObject<List<Card>>(content);
            }
            return null;
        }
*/