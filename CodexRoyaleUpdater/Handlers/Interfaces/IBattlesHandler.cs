using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater.Handlers.Interfaces
{
    public interface IBattlesHandler
    {
        //Read
        Task<List<Battle>> GetOfficialPlayerBattles(string tag);
        Task<Battle> GetSetBattleId(Battle battle);
        Task<List<Battle>> GetAllBattles();
        Task<Battle> GetBattleById(int id);

        //Read + Create
        //any Battle with returned Id will be added if it doesn't exist 
        Task AddBattle(Battle battle);
        Task<int> AddBattles(List<Battle> battles);

        //Update
        Task UpdateBattle(Battle battle);

        //Delete
        Task DeleteBattle(int id);
    }
}
