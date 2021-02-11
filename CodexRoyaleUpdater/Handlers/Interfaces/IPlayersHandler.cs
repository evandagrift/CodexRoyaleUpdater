using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater.Handlers
{
    public interface IPlayersHandler
    {
        //Read
        public Task<Player> GetOfficialPlayer(string tag);
        public Task<Player> GetCodexPlayer(int id);
        public Task<List<Player>> GetAllCodexPlayers();
        
        //Create
        public Task AddPlayer(Player player);
        public Task AddPlayer(string tag);

        //Update
        public Task UpdatePlayer(Player player);

        //Delete
        public Task DeletePlayer(int id);
    }
}
