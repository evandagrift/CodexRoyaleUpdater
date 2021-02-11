using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater.Handlers.Interfaces
{
    public interface ICardsHandler
    {
        //Read
        public Task<List<Card>> GetAllOfficialCards();
        public Task<Card> GetCodexCard(int id);
        public Task<List<Card>> GetAllCodexCards();

        //Create
        public Task AddCard(Card card);

        //Update
        public Task UpdateCard(Card card);
        public Task UpdateCodex();

        //Delete
        public Task DeleteCard(int id);
    }
}
