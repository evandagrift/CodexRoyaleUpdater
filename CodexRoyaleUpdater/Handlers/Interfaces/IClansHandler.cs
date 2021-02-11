using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater.Handlers.Interfaces
{
    public interface IClansHandler
    {
        //Read
        public Task<Clan> GetOfficialClan(string tag);
        public Task<Clan> GetClan(int id);
        public Task<List<Clan>> GetAllClans();

        //Create
        public Task AddClan(Clan clan);
        public Task AddClan(string tag);

        //Update
        public Task UpdateClan(Clan clan);

        //Delete
        public Task DeleteClan(int id);
    }
}

