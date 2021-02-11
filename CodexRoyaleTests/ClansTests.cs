using CodexRoyaleUpdater;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CodexRoyaleTests
{
    public class ClansTests
    {
        ClansHandler handler;
        static string clanTag = "#8CYPL8R";
        string randomClanTag = "#9QGPC82Y0";
        public ClansTests()
        {
            Client client = new Client();
            handler = new ClansHandler(client);
        }

        [Fact]
        public async Task GetClanOfficialTest()
        {
            //gets Clan from the offial Clash Royale API and Casts it to clan type because returns object
            Clan clan = await handler.GetOfficialClan(clanTag);

            //this is my clan tag so I know the user name will stay static
            Assert.NotNull(clan);
        }
        
        [Fact]
        public async Task AddClanTest()
        {
            //Gets cards in codex
            List<Clan> clans = await handler.GetAllClans();
           // if a clan was successfully fetched
            if(clans != null)
            {
                //count of clans before adding
                int clansCount = clans.Count;

                //fetches clan from official api to be added
                Clan clanToAdd = await handler.GetOfficialClan(clanTag);

                //adds clan instance to codex
                await handler.AddClan(clanToAdd);

                //fetches all clans saved in codex
                clans = await handler.GetAllClans();

                //count of clans after adding
                int newClanCount = clans.Count;

                //tests if one was added
                Assert.Equal(newClanCount, clansCount + 1);
            }
        }
        [Fact]
        public async Task GetClanCodexTest()
        {
            //gets clan from my API
            List<Clan> clans = await handler.GetAllClans();

            //fetches clan by Id valid Id is sourced from get all
            Clan fetchedClan = await handler.GetClan(clans[0].Id);

            //if a clan was properly fetched tests true
            Assert.NotNull(fetchedClan);
        }


        [Fact]
        public async Task GetAllClansCodexTest()
        {
            //gets Clan from my API
            List<Clan> clans = await handler.GetAllClans();

            //if successfully returned will not be null
            Assert.NotNull(clans);
        }


        [Fact]
        public async Task UpdateClanTest()
        {
            //gets all clans in codex
            List<Clan> clans = await handler.GetAllClans();

            //creates an instance of the last clan in the list
            Clan clanToUpdate = clans[clans.Count - 1];

            //updates the clans name
            clanToUpdate.Name = "UPDATED";

            //calls the update function updating codexAPI
            await handler.UpdateClan(clanToUpdate);

            //fetches the updated clan from codex API
            Clan updatedClan = await handler.GetClan(clanToUpdate.Id);

            //if the name is updated passes true
            Assert.Equal("UPDATED", updatedClan.Name);
        }

        [Fact]
        public async Task AddClanByTagTaskTest()
        {
            //fetches all clans to get an initial count
            List<Clan> clans = await handler.GetAllClans();
            int clanCount = clans.Count;

            //adds clan to codex db via their tag
            //add clan can be overloaded with Tag or Clan<T>
            await handler.AddClan(clanTag);
            
            //gets a fresh list of all clans
            clans = await handler.GetAllClans();

            //gets the new count of clans
            int newClanCount = clans.Count;

            //tests if one clan was added
            Assert.Equal(newClanCount, clanCount + 1);
        }

        [Fact]
        public async Task DeleteClanTest()
        {
            //gets all clans to get an inital count
            List<Clan> allClans = await handler.GetAllClans();
            int clanCount = allClans.Count;

            //deletes via Id the last clan in the list
            await handler.DeleteClan(allClans[clanCount - 1].Id);

            //gets updated list of clans and gets count
            allClans = await handler.GetAllClans();
            int newClanCount = allClans.Count;

            //test if one clan was removed
            Assert.Equal(newClanCount, clanCount - 1);

        }

    }
}
