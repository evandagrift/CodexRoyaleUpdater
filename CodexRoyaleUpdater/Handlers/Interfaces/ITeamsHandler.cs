using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater.Handlers.Interfaces
{
    public interface ITeamsHandler
    {
        //Read
        Task<List<Team>> GetTeams();
        Task<Team> GetTeam(int id);
        //Read + create
        //any Team that doesn't have an Id will find the Id or create a Team and return new teams Id
        Task<Team> GetSetTeamId(Team team);
        Task<int> GetSetTeamId(Player player);
        Task<int> GetTeamId(Team team);

        //Update
        Task UpdateTeam(Team team);

        //Delete
        Task DeleteTeam(int id);
    }
}
