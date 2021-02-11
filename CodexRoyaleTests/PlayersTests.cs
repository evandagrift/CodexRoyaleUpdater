using CodexRoyaleUpdater;
using CodexRoyaleUpdater.Handlers;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CodexRoyaleTests
{
    public class PlayersTests
    {
        //handler that makes API Calls
        PlayersHandler handler;

        //player data for testing
        static string elodinTag = "#29PGJURQL";
        static string randomTag = "#29UV0VJ8J";


        public PlayersTests()
        {
            //creates client and passes it to the new Handler
            Client client = new Client();
            handler = new PlayersHandler(client);
        }

        [Fact]
        public async Task GetPlayerOfficialTest()
        {
            //gets Player from the offial Clash Royale API and Casts it to player type because returns object
            Player player = await handler.GetOfficialPlayer(randomTag);
            Player elodin = await handler.GetOfficialPlayer(elodinTag);

            //this is my player tag so I know the user name will stay static
            Assert.Equal("GreenGo", player.Name);
        }
        [Fact]
        public async Task AddPlayerTest()
        {
            //list of all Codex Players to get count
            List<Player> players = await handler.GetAllCodexPlayers();
            int playerCount = players.Count;

            //gets a player instance from the official API
            Player playerToAdd = await handler.GetOfficialPlayer(randomTag);

            //adds the fethced player to the Codex API
            await handler.AddPlayer(playerToAdd);

            //fetches new list of players and gets count
            players = await handler.GetAllCodexPlayers();
            int newPlayerCount = players.Count;

            //if one player was added test passes
            Assert.Equal(newPlayerCount, playerCount + 1);

            //gets a player instance from the official API
            playerToAdd = await handler.GetOfficialPlayer(elodinTag);

            //adds the fethced player to the Codex API
            await handler.AddPlayer(playerToAdd);
        }

        [Fact]
        public async Task GetAllPlayersCodexTest()
        {
            //gets all players from Codex API
            List<Player> players = await handler.GetAllCodexPlayers();

            //if the handler fails to fetch it will return null
            Assert.NotNull(players);
        }

        [Fact]
        public async Task GetPlayerCodexTest()
        {
            //gets list of all player to make sure there is a valid player at ID
            List<Player> players = await handler.GetAllCodexPlayers();
            
            //gets player from my API w/ Id
            Player player = await handler.GetCodexPlayer(players[0].Id);

            Assert.NotNull(player);
        }




        [Fact]
        public async Task UpdatePlayerTest()
        {
            //fetches all player in Codex
            List<Player> players = await handler.GetAllCodexPlayers();

            //Gets the last Player in the list of all Players
            Player playerToUpdate = players[players.Count - 1];

            string playerOldName = playerToUpdate.Name;

            //updates fetched player
            playerToUpdate.Name = "UPDATED";
            await handler.UpdatePlayer(playerToUpdate);

            //fetches the updated player from Codex API
            Player updatedPlayer = await handler.GetCodexPlayer(playerToUpdate.Id);

            //if fetched player's name is updated test succeeds
            Assert.Equal("UPDATED", updatedPlayer.Name);


            //updates fetched player
            playerToUpdate.Name = playerOldName;
            await handler.UpdatePlayer(playerToUpdate);
        }

        [Fact]
        public async Task AddPlayerByTagTaskTest()
        {
            //gets all players to get count before adding
            List<Player> players = await handler.GetAllCodexPlayers();
            int playerCount = players.Count;

            //adds player to codex db via their tag
            await handler.AddPlayer(elodinTag);

            //gets updated count of players
            players = await handler.GetAllCodexPlayers();
            int newPlayerCount = players.Count;

            //if one player was added the test passes
            Assert.Equal(newPlayerCount, playerCount + 1);
        }


        [Fact]
        public async Task DeletePlayerTest()
        {
            //gets all cards in codex API
            List<Player> playersCodex = await handler.GetAllCodexPlayers();

            //if successfully fetched players frm codex
            if (playersCodex != null)
            {
                //value to be tested against as well as making sure there are players to delete
                int playersBefore = playersCodex.Count;

                //if there are players one will be deleted
                if (playersBefore > 0)
                {
                    //deletes the last player in the list
                    Player playerToDelete = playersCodex[playersCodex.Count - 1];

                    //delete is called via the player Id
                    await handler.DeletePlayer(playerToDelete.Id);

                    //gets all players to test if update
                    playersCodex = await handler.GetAllCodexPlayers();

                    //passes if a player was delted
                    Assert.Equal(playersBefore - 1, playersCodex.Count);


                }
                else { Assert.False(true); }
            }//if codex fails to fetch, or there are no players in the Codex the test fails
            else { Assert.False(true); }
        }
    }
}