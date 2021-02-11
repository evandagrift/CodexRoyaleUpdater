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
    public class DecksTests
    {
        //client includes httpclient links to the official API and the Codex API
        Client client;

        //Decks handler has all the json parsing and HTTP Calls
        DecksHandler decksHandler;
        CardsHandler cardsHandler;

        //player data for testing
        static string elodinTag = "#29PGJURQL";

        //assigns a client(this might change w/ dependancy injection)\
        //builds handler from client
        public DecksTests()
        {
            //client for HTTP Access
            client = new Client();

            //handler being tested
            decksHandler = new DecksHandler(client);
            cardsHandler = new CardsHandler(client);
        }

        [Fact]
        public async Task GetSetDeckIdTest()
        {
            //gets my current deck and returns with Id so will create Deck in Codex if there isn't one
            Deck currentDeck = await decksHandler.GetPlayerCurrentDeck(elodinTag);
            //decks are added by getting their Id
            Assert.True(currentDeck.Id > 0);

            //creates a deck made up of random real cards fetched from the official API
            List<Card> cards = await cardsHandler.GetAllOfficialCards();
            Deck newDeck = new Deck()
            {
                Card1Id = cards[0].Id,
                Card2Id = cards[1].Id,
                Card3Id = cards[2].Id,
                Card4Id = cards[3].Id,
                Card5Id = cards[4].Id,
                Card6Id = cards[5].Id,
                Card7Id = cards[6].Id,
                Card8Id = cards[7].Id
            };

            //trys to get the deck's Id it is new so will create a new deck in the DB
            int newDeckId = await decksHandler.GetDeckId(newDeck);

            //if a valid Id is returned passes true
            Assert.True(newDeckId > 1);
        }

        [Fact]
        public async Task GetAllCodexDecks()
        {
            //fetches list of decks
            List<Deck> decks = await decksHandler.GetAllDecks();

            //if there are decks in the codex it will pass
            Assert.NotNull(decks);
        }

        [Fact]
        public async Task GetCodexDeck()
        {

            //if this deck isn't the Codex it will add it so we don't test an empty DB
            await decksHandler.GetPlayerCurrentDeck(elodinTag);

            //fetches list of Decks to fetch a valid Id
            //test will fail if getAllCodex isn't functioning
            List<Deck> decks = await decksHandler.GetAllDecks();

            //gets a Deck to fetch a valid Id
            Deck validDeck = decks[0];

            //fetches deck at an Id that is known to exist
            Deck fetchedDeck = await decksHandler.GetDeck(validDeck.Id);

            //if a Deck is fetched it will pass
            Assert.NotNull(fetchedDeck);
        }

        [Fact]
        public async Task DeleteDeckTest()
        {
            //if this deck isn't the Codex it will add it so we don't test an empty DB
            await decksHandler.GetPlayerCurrentDeck(elodinTag);

            //gets all Decks in codex API
            List<Deck> decksCodex = await decksHandler.GetAllDecks();

            
                //value to be tested against as well as making sure there are decks to delete
                int decksBefore = decksCodex.Count;

                //if there are decks one will be deleted
                if (decksBefore > 0)
                {
                    //deletes the last deck in the list
                    Deck deckToDelete = decksCodex[decksCodex.Count - 1];

                    //delete is called via the deck Id
                    await decksHandler.DeleteDeck(deckToDelete.Id);

                    //gets all decks to test if update
                    decksCodex = await decksHandler.GetAllDecks();

                    //passes if a Deck was delted
                    Assert.Equal(decksBefore - 1, decksCodex.Count);


                }
                else { Assert.False(true); }
        }



        [Fact]
        public async Task UpdateDeckTest()
        {
            //if this deck isn't the Codex it will add it so we don't test an empty DB
            await decksHandler.GetPlayerCurrentDeck(elodinTag);

            //gets all Decks in codex API
            List<Deck> decksCodex = await decksHandler.GetAllDecks();

            Deck deckToUpdate = decksCodex[0];

            //I'm testing directly with the DB using this to keep from buggering up the data
            int oldCardId = deckToUpdate.Card1Id;

            //set updated name
            deckToUpdate.Card1Id = 999;

            //send it to handler/codex api to be updated
            await decksHandler.UpdateDeck(deckToUpdate);

            //fetches the updated deck (Id will not have changed)
            Deck updatedDeck = await decksHandler.GetDeck(deckToUpdate.Id);

            Assert.Equal(999, updatedDeck.Card1Id);

            //send it to handler/codex api to be updated
            //setting the name back to it's previous value
            updatedDeck.Card1Id = oldCardId;
            await decksHandler.UpdateDeck(updatedDeck);
        }

    }
}
