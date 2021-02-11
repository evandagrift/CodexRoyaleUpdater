using Newtonsoft.Json;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater.Handlers
{
    public class PlayersHandler : IPlayersHandler
    {
        //client class has access to both APIs
        Client client;

        //connection string for players in the API
        private string connectionString = "Players";

        //Constructor adds reference to HTTP Clients
        public PlayersHandler(Client c)
        {
            client = c;
        }

        //Player Last seen is fetched from a Clan Api Call
        public async Task<string> GetLastSeen(string playerTag, string clanTag)
        {
            //handler to fetch clan
            ClansHandler clansHandler = new ClansHandler(client);

            //fetch clan to get data from
            Clan clan = await clansHandler.GetOfficialClan(clanTag);

            //clan members are a list of players, we grab the player with matching tag
            Player player = clan.MemberList.Where(p => p.Tag == playerTag).FirstOrDefault();

            //return when last seen, Substringed because returned time has a timezone offset but all players are returned with an offset of 0
            return player.LastSeen.Substring(0,15);
        }


        //gets player data from the official api via their player tag
        public async Task<Player> GetOfficialPlayer(string tag)
        {
            //teams handler to get/set teamId
            TeamsHandler teamsHandler = new TeamsHandler(client);
            //decks handler to get set deckId
            DecksHandler decksHandler = new DecksHandler(client);

            //try in case we get connection errors`
                try
                {
                //
                    string connectionString = "/v1/players/%23" + tag.Substring(1);

                    var result = await client.officialAPI.GetAsync(connectionString);

                    if (result.IsSuccessStatusCode)
                    {
                        var content = await result.Content.ReadAsStringAsync();
                        Player player = JsonConvert.DeserializeObject<Player>(content);

                        player.TeamId = await teamsHandler.GetSetTeamId(player);

                        player.Deck = new Deck(player.CurrentDeck);
                        Deck deck = await decksHandler.GetSetDeckId(player.Deck);
                        player.CurrentDeckId = deck.Id;
                        player.CurrentFavouriteCardId = player.CurrentFavouriteCard.Id;
                        player.UpdateTime = DateTime.UtcNow.ToString("yyyyMMddTHHmmss");
                        player.ClanTag = player.Clan.Tag;
                        player.LastSeen = await GetLastSeen(player.Tag, player.ClanTag);
                        player.CardsDiscovered = player.Cards.Count;
                        return player;
                    }
                }
                catch { return null; }
            return null;
        }

        //gets player save instance via Id from Code API
        public async Task<Player> GetCodexPlayer(int id)
        {
                try
                {
                    string connectionString = "Players/" + id;

                    var result = await client.codexAPI.GetAsync(connectionString);

                    if (result.IsSuccessStatusCode)
                    {
                        var content = await result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<Player>(content);
                    }
                }
                catch { return null; }
            return null;
        }

        //returns all saved Player instances  from Codex API
        public async Task<List<Player>> GetAllCodexPlayers()
        {
            List<Player> players = new List<Player>();
            try
            {
                var result = await client.codexAPI.GetAsync(connectionString);

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Player>>(content);
                }
            }
            catch { return null; }
            return null;
        }
        
        //adds player to Codex via Player Object
        //this cannot have a set Id because it is auto generated
        public async Task AddPlayer(Player player)
        {
            try
            {
                //serializes Player object to Json
                string json = JsonConvert.SerializeObject(player);
                
                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //posts the Player to the Codex API
                var response = await client.codexAPI.PostAsync("Players", data);
            }
            catch { }
        }

        //Updates player instance(It needs a set Id)
        public async Task UpdatePlayer(Player player)
        {
            try
            {

                //serializes Player object to Json
                string json = JsonConvert.SerializeObject(player);

                //creates the string content to be sent in the HTTP Post
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //Puts the Player to the Codex API
                var response = await client.codexAPI.PutAsync("Players", data);
            }
            catch { }

        }

        public async Task AddPlayer(string tag)
        {
            //fetches current player by tag
            Player player = await GetOfficialPlayer(tag);

            //saves player to codex
            await AddPlayer(player);

        }
        public async Task DeletePlayer(int id)
        {
            try
            {
                //deletes player in codex at given Id
                var response = await client.codexAPI.DeleteAsync("Players/" + id);
            }
            catch { }
        }

    }
}
