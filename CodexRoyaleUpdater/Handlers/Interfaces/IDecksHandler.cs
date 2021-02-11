using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodexRoyaleUpdater.Handlers.Interfaces
{
    public interface IDecksHandler
    {
        //Read
        public Task<Deck> GetDeck(int id);
        public Task<List<Deck>> GetAllDecks();

        //Read + Create
        //any deck with returned Id will be added if it doesn't exist 
        public Task<Deck> GetPlayerCurrentDeck(string tag);
        public Task<int> GetDeckId(Deck deck);
        public Task<Deck> GetSetDeckId(Deck deck);

        //Update
        public Task UpdateDeck(Deck Deck);

        //Delete
        public Task DeleteDeck(int id);
    }
}
