using Models;
using Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class DeckTests
    {
        private readonly DeckInitializerService _deckInitializerService;

        public DeckTests()
        {
            _deckInitializerService = new DeckInitializerService();
        }

        [Fact]
        public void PropertyCardsShouldBe28Cards()
        {
            List<PropertyCard> propertyCards = _deckInitializerService.GetPropertyCards();
            Assert.Equal(28, propertyCards.Count);
        }

        [Fact]
        public void PropertyCardsShouldHaveUniqueValues()
        {
            List<PropertyCard> propertyCards = _deckInitializerService.GetPropertyCards();
            Assert.Equal(propertyCards.Select(c => c.Value).Distinct().Count(), propertyCards.Count);
            Assert.Equal(propertyCards.Select(c => c.Name).Distinct().Count(), propertyCards.Count);
        }

        [Fact]
        public void ChequeCardsCountShouldBe28Cards()
        {
            List<ChequeCard> chequeCards = _deckInitializerService.GetChequeCards();
            Assert.Equal(28, chequeCards.Count);
        }

        [Fact]
        public void ChequeCardsShouldBeUnique()
        {
            List<ChequeCard> chequeCards = _deckInitializerService.GetChequeCards();
            Assert.Equal(14, chequeCards.Select(c => c.Value).Distinct().Count());
        }

        [Fact]
        public void ChequeCardsShouldNotContain1()
        {
            List<ChequeCard> chequeCards = _deckInitializerService.GetChequeCards();
            Assert.DoesNotContain(chequeCards, c => c.Value == 1);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        [InlineData(15)]
        public void ChequeCardsShouldContain2OfAnyNumber(int number)
        {
            List<ChequeCard> chequeCards = _deckInitializerService.GetChequeCards();
            Assert.Equal(2, chequeCards.Where(c => c.Value == number).Count());
        }


        [Fact]
        public void ThereShouldBe4Players()
        {
            List<Player> players = _deckInitializerService.GetPlayers();
            Assert.Equal(4, players.Count());
        }

        [Fact]
        public void PlayersShouldHave15Money()
        {
            List<Player> players = _deckInitializerService.GetPlayers();
            Assert.DoesNotContain(players, c => c.Money != 15);
        }
    }
}
