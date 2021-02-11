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
    public class TeamsTests
    {
        //handler that makes API Calls
        TeamsHandler handler;


        public TeamsTests()
        {
            //creates client and passes it to the new Handler
            Client client = new Client();
            handler = new TeamsHandler(client);
        }

        [Fact]
        public async void GetSetTeamIdTest()
        {
            Team teamToGetSet = new Team() { Tag = "#TEST", Name = "TEST", TeamName = "TEST" };
            Team returnedTeam = await handler.GetSetTeamId(teamToGetSet);
            Assert.NotNull(returnedTeam);
            Assert.True(returnedTeam.TeamId > 0);
        }

        [Fact]
        public async void GetTeamById()
        {
            // makes sure there is a team in the DB            
            Team teamToGetSet = new Team() { Tag = "#TEST", Name = "TEST", TeamName = "TEST" };

            Team returnedTeam = await handler.GetSetTeamId(teamToGetSet);
            Team team = await handler.GetTeam(returnedTeam.TeamId);

            Assert.NotNull(team);

            //passes the team without and Id and get's the Id as an int
            int fetchedId = await handler.GetTeamId(teamToGetSet);

            //Ids start at 1
            Assert.True(fetchedId > 0);

        }
        [Fact]
        public async void GetAllTeamsTest()
        {
            // makes sure there is a team in the DB
            Team teamToGetSet = new Team() { Tag = "#29PGJURQL", Name = "Elodin", TeamName = "Elodin1v1" };
            Team returnedTeam = await handler.GetSetTeamId(teamToGetSet);

            //fetches all teams
            List<Team> teams = await handler.GetTeams();

            //if fails to fetch will be null
            Assert.NotNull(teams);
        }

        [Fact]
        public async void DeleteTeamTest()
        {
            // makes sure there is a team in the DB            
            Team teamToGetSet = new Team() { Tag = "#TEST", Name = "TEST", TeamName = "TEST" };

            Team returnedTeam = await handler.GetSetTeamId(teamToGetSet);
            List<Team> teams = await handler.GetTeams();

            //value to be tested against as well as making sure there are teams to delete
            int teamsBefore = teams.Count;

            //if there are teams one will be deleted
            if (teamsBefore > 0)
            {
                //deletes the last team in the list
                Team teamToDelete = teams[teams.Count - 1];

                //delete is called via the team Id
                await handler.DeleteTeam(teamToDelete.TeamId);

                //gets all teams to test if update
                teams = await handler.GetTeams();

                //passes if a team was delted
                Assert.Equal(teamsBefore - 1, teams.Count);
            }
            else { Assert.False(true); }
        }

        [Fact]
        public async void UpdateTeamTest()
        {
            // makes sure there is a team in the DB
            Team teamToGetSet = new Team() { Tag = "#29PGJURQL", Name = "Elodin", TeamName = "Elodin1v1" };
            Team returnedTeam = await handler.GetSetTeamId(teamToGetSet);

            //list of all teams to get a valid Id
            List<Team> teams = await handler.GetTeams();

            //substantiates the first instance in list
            Team teamToUpdate = teams[0];

            //I'm testing directly with the DB using this to keep from buggering up the data
            string oldTeamName = teamToUpdate.TeamName;

            //set updated name
            teamToUpdate.TeamName = "UPDATED";

            //send it to handler/codex api to be updated
            await handler.UpdateTeam(teamToUpdate);

            //fetches the updated team (Id will not have changed)
            Team updatedteam = await handler.GetTeam(teamToUpdate.TeamId);

            Assert.Equal("UPDATED", updatedteam.TeamName);

            //send it to handler/codex api to be updated
            //setting the name back to it's previous value
            updatedteam.TeamName = oldTeamName;
            await handler.UpdateTeam(updatedteam);
        }



    }
}
