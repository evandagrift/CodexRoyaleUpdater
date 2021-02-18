using CodexRoyaleUpdater;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CodexRoyaleTests
{
    public class CardsTests
    {
        //client includes httpclient links to the official API and the Codex API
        Client client;

        //cards handler has all the json parsing and HTTP Calls
        CardsHandler handler;

        //assigns a client(this might change w/ dependancy injection)\
        //builds handler from client
        public CardsTests()
        {
            client = new Client();
            handler = new CardsHandler(client);
        }
        

        [Fact]
        public async Task GetCardsOfficialTest()
        {
            //gets all the cards from the official API
            List<Card> cards = await handler.GetAllOfficialCards();

            //passes if cards were returned
            Assert.NotNull(cards);
        }

        [Fact]
        public async Task GetAllCodexCards()
        { 
            //fetches list of cards
            List<Card> cards = await handler.GetAllCodexCards();

            //if there are cards in the codex it will pass
            Assert.NotNull(cards);
        }

        [Fact]
        public async Task GetCodexCard()
        {
            //fetches list of cards to fetch a valCardId CardId
            //test will fail if getAllCodex isn't functioning
            List<Card> cards = await handler.GetAllCodexCards();

            //gets a card to fetch a valCardId CardId
            Card valCardIdCard = cards[0];

            //fetches card at an CardId that is known to exist
            Card fetchedCard = await handler.GetCodexCard(valCardIdCard.Id);

            //if a card is fetched it will pass
            Assert.NotNull(fetchedCard);
        }


        [Fact]
        public async Task DeleteCardTest()
        {
            //gets all cards in codex API
            List<Card> cardsCodex = await handler.GetAllCodexCards();

            //if successfully fetched cards frm codex
            if(cardsCodex != null)
            {
                //value to be tested against as well as making sure there are cards to delete
                int cardsBefore = cardsCodex.Count;

                //if there are cards one will be deleted
                if(cardsBefore > 0)
                {
                    //deletes the last card in the list
                    Card cardToDelete = cardsCodex[cardsCodex.Count - 1];

                    //delete is called via the card CardId
                    await handler.DeleteCard(cardToDelete.Id);

                    //gets all cards to test if update
                    cardsCodex = await handler.GetAllCodexCards();

                    //passes if a card was delted
                    Assert.Equal(cardsBefore - 1, cardsCodex.Count);


                }
                    else { Assert.False(true); }
            }
            else { Assert.False(true); }
        }

        [Fact]
        public async Task UpdateCardTest()
        {
            List<Card> cards = await handler.GetAllCodexCards();


            Card cardToUpdate = cards[0];

            //I'm testing directly with the DB using this to keep from buggering up the data
            string oldName = cardToUpdate.Name;

            //set updated name
            cardToUpdate.Name = "UPDATED";

            //send it to handler/codex api to be updated
            await handler.UpdateCard(cardToUpdate);

            //fetches the updated card (CardId will not have changed)
            Card updatedCard = await handler.GetCodexCard(cardToUpdate.Id);

            Assert.Equal("UPDATED", updatedCard.Name);

            //send it to handler/codex api to be updated
            //setting the name back to it's previous value
            updatedCard.Name = oldName;
            await handler.UpdateCard(updatedCard);
        }

        [Fact]
        public async Task UpdateCodexTest()
        {
            //gets all the cards from both APIs
            List<Card> cardsOfficial = await handler.GetAllOfficialCards();
            List<Card> cardsCodex = await handler.GetAllCodexCards();

            if(cardsCodex.Count > 0)
            {
                //deletes first card in the list
                await handler.DeleteCard(cardsCodex[0].Id);
                cardsCodex = await handler.GetAllCodexCards();
            }

            //tests to make sure that Codex isn't full
            Assert.True(cardsCodex.Count != cardsOfficial.Count);

            //runs the function to add any missing cards to Codex
            await handler.UpdateCodex();

            //calls a fresh list of cards in codex
            cardsCodex = await handler.GetAllCodexCards();

            //makes sure all cards are added
            Assert.Equal(cardsOfficial.Count, cardsCodex.Count);
        }

    }
}
