using CodexRoyaleUpdater;
using CodexRoyaleUpdater.Handlers;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CodexRoyaleTests
{
    public class BattlesTests
    {

        //client includes httpclient links to the official API and the Codex API
        Client client;

        //Battles handler has all the json parsing and HTTP Calls
        BattlesHandler handler;

        //player data for testing
        static string elodinTag = "#29PGJURQL";

        //assigns a client(this might change w/ dependancy injection)\
        //builds handler from client
        public BattlesTests()
        {
            //client for HTTP Access
            client = new Client();

            //handler being tested
            handler = new BattlesHandler(client);
        }

        [Fact]
        public async void GetOfficialBattlesTest()
        {
            //fetches battles from codex
            List<Battle> recentBattles = await handler.GetOfficialPlayerBattles(elodinTag);

            //if fail to fetch will return null
            Assert.NotNull(recentBattles);
        }

        //tests get setting a battle based off
        [Fact]
        public async void GetSetBattleIdTest()
        {
            //gets a list of recent battlesfo
            List<Battle> recentBattles = await handler.GetOfficialPlayerBattles(elodinTag);

            //gets the most recent battle I've played so this will regularly update for testing
            Battle battleA = await handler.GetSetBattleId(recentBattles[recentBattles.Count - 1]);

            //BattleA will save a new battle to the db where battleB will fetch the already saved battle
            Battle battleB = await handler.GetSetBattleId(recentBattles[recentBattles.Count - 1]);

            //tests to make sure a battle is saved and has an Id
            //tests that it is returning the same while creating or reading
            Assert.True(battleA.BattleId > 0);
            Assert.True(battleA.BattleId == battleB.BattleId);
        }
        
        [Fact]
        public async void GetBattleByIdTest()
        {
            //gets all battles to retrieve valid Id
            List<Battle> allBattles = await handler.GetAllBattles();

            //fetches battle at valid Id from fetched list of battles
            Battle fetchedBattle = await handler.GetBattleById(allBattles[0].BattleId);

            //if a battle is successfully returned 
            Assert.NotNull(fetchedBattle);
        }

        [Fact]
        public async void AddBattleTest()
        {
            //gets a list of my recent battles for testing
            List<Battle> recentBattles = await handler.GetOfficialPlayerBattles(elodinTag);

            //gets the most recent battle I've played so this will regularly update for testing
            await handler.AddBattle(recentBattles[recentBattles.Count - 1]);

            //gets count before fetching to see if it was added before get set
            List<Battle> allBattles = await handler.GetAllBattles();
            int battleCount = allBattles.Count;

            Battle savedBattle = await handler.GetSetBattleId(recentBattles[recentBattles.Count - 1]);

            //gets new count of battles
            allBattles = await handler.GetAllBattles();
            int newBattleCount = allBattles.Count;

            //if the battle was successfully fetched will pass
            Assert.NotNull(savedBattle);

            //this will fail if the new battle was added via getset
            Assert.True(newBattleCount == battleCount);

        }
        [Fact]
        public async void AddBattlesTest()
        {
            //gets a list of my recent battles for testing
            List<Battle> recentBattles = await handler.GetOfficialPlayerBattles(elodinTag);
            //gets the most recent battle I've played so this will regularly update for testing set
            int battlesAdded = await handler.AddBattles(recentBattles);

            //gets count of all battles after adding new ones
            List<Battle> allSavedBattles = await handler.GetAllBattles();

            Assert.True(battlesAdded > 0);
        }

        [Fact]
        public async void UpdateBattleTest()
        {
            //gets all battles to fetch a valid one to update
            List<Battle> battles = await handler.GetAllBattles();
            Battle battleToUpdate = battles[0];

            //saves old name so we can re update and keep from changing the data
            string oldTeamName = battleToUpdate.Team1Name;

            //updates class
            battleToUpdate.Team1Name = "UPDATED";

            //sends it to be updated
            await handler.UpdateBattle(battleToUpdate);
            
            //fetches the updated battle via Id
            Battle updatedBattle = await handler.GetBattleById(battleToUpdate.BattleId);

            //if updated passes test
            Assert.True(updatedBattle.Team1Name == "UPDATED");

            //updates back to old name to keep from messing up any data
            battleToUpdate.Team1Name = oldTeamName;

            //sends it to be updated
            await handler.UpdateBattle(battleToUpdate);
        }

        [Fact]
        public async void DeleteBattleTest()
        {
            //fetches player battles
            List<Battle> officialBattles = await handler.GetOfficialPlayerBattles(elodinTag);

            //adds a battle to make sure there is one to delete
            await handler.AddBattle(officialBattles[1]);

            //gets all battles to get a count
            List<Battle> battles = await handler.GetAllBattles();

            int initialCount = battles.Count;


            Battle battleToBeDeleted = battles[battles.Count - 1];

            //sends an Id to be deleted
            await handler.DeleteBattle(battleToBeDeleted.BattleId);

            //gets an updated count
            battles = await handler.GetAllBattles();
            int newCount = battles.Count;

            //checks if a battle was deleted
            Assert.Equal(newCount, initialCount - 1);
        }



    }
}
