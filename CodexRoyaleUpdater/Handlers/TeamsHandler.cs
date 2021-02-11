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
    public class TeamsHandler : ITeamsHandler
    {

        //provides HTTP access for both codex API and the official API
        Client client;
        //connection sting for the Codex API Controller that is being handled
        private string codexConnectionString = "Teams";

        //passes in client for use
        public TeamsHandler(Client c)
        {
            client = c;
        }

        public async Task<List<Team>> GetTeams()
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
                    return JsonConvert.DeserializeObject<List<Team>>(content); ;
                }
                return null;
            }
            catch { return null; }
            return null;
        }
        public async Task<Team> GetTeam(int id)
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
                    return JsonConvert.DeserializeObject<Team>(content); ;
                }
                return null;


            }
            catch { return null; }
            return null;
        }

        public async Task<Team> GetSetTeamId(Team team)
        {

            try
            {
                //serializes Card object to Json
                string json = JsonConvert.SerializeObject(team);

                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //posts the Card to the Codex API
                var response = await client.codexAPI.PostAsync("Teams/getsetteamid", data);
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Team>(content);
            }
            catch { }
            return null;


            return team;
        }
        public async Task<int> GetSetTeamId(Player player)
        {
            Team team = new Team() { Name = player.Name, Tag = player.Tag, TwoVTwo = false };
            try
            {
                //serializes Card object to Json
                string json = JsonConvert.SerializeObject(team);

                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //posts the Card to the Codex API
                var response = await client.codexAPI.PostAsync("Teams/getsetteamid", data);
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Team>(content).TeamId;
            }
            catch { }
            return 0;
        }

        public async Task<int> GetTeamId(Team team)
        {
            return GetSetTeamId(team).Result.TeamId;
        }

        public async Task DeleteTeam(int id)
        {
            try
            {
                //posts the Deck to the Codex API
                var response = await client.codexAPI.DeleteAsync("Teams/" + id);
            }
            catch { }
        }

        public async Task UpdateTeam(Team team)
        {
            try
            {
                //serializes Team object to Json
                string json = JsonConvert.SerializeObject(team);

                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //Puts the Team to the Codex API
                var response = await client.codexAPI.PutAsync(codexConnectionString, data);
            }
            catch { }
        }

    }
}
