using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests
{
    public class PhaseOneTest
    {
        private readonly PhaseOneService _phaseOneService;
        private readonly DeckInitializerService _deckInitializerService;
        private readonly List<Player> _players;
        private readonly List<PropertyCard> _propertyCards;

        public PhaseOneTest()
        {
            _deckInitializerService = new DeckInitializerService();
            _players = _deckInitializerService.GetPlayers();
            _propertyCards = _deckInitializerService.GetPropertyCards();
            _phaseOneService = new PhaseOneService(_players, _propertyCards);
        }

        [Fact]
        public void SpreadCardsTest()
        {
            List<PropertyCard> sprededProeprtyCards = _phaseOneService.SpreadPropertyCards();
            Assert.Equal(4, sprededProeprtyCards.Count);
            Assert.Equal(4, sprededProeprtyCards.Distinct().Count());
        }

        [Fact]
        public void DontSpreadRedundantCards()
        {
            List<PropertyCard> spreadedCards = _phaseOneService.SpreadPropertyCards();
            spreadedCards.AddRange(_phaseOneService.SpreadPropertyCards());
            spreadedCards.AddRange(_phaseOneService.SpreadPropertyCards());
            spreadedCards.AddRange(_phaseOneService.SpreadPropertyCards());
            spreadedCards.AddRange(_phaseOneService.SpreadPropertyCards());
            spreadedCards.AddRange(_phaseOneService.SpreadPropertyCards());
            spreadedCards.AddRange(_phaseOneService.SpreadPropertyCards());

            Assert.Equal(spreadedCards.Count(), spreadedCards
                .Select(c => c.Value).Distinct().Count());
        }

        [Fact]
        public void GivenBetOneThenRaise()
        {
            _phaseOneService.Bet(_players.First(), 1);
        }
        
        [Fact]
        public void GivenPlayer2ThenDontLetRaise()
        {
            try
            {
                _phaseOneService.Bet(_players[1], 1);
                Assert.True(false);
            }
            catch (WrongTurnException)
            {
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void Given2BetsThenTurnsBeOk()
        {
            _phaseOneService.Bet(_players[0], 1);
            _phaseOneService.Bet(_players[1], 2);
            _phaseOneService.Bet(_players[2], 3);
            _phaseOneService.Bet(_players[3], 4);
            _phaseOneService.Bet(_players[0], 5);
            _phaseOneService.Bet(_players[1], 6);
            _phaseOneService.Bet(_players[2], 7);
            Assert.True(true);
        }

        [Fact]
        public void GivenBet1ThenMoneyDecrease()
        {
            _phaseOneService.Bet(_players[0], 1);
            Assert.Equal(14, _players[0].Money);
        }

        [Fact]
        public void Given16BetMoneyThenThrowException()
        {
            try
            {
                _phaseOneService.Bet(_players[0], 16);
                Assert.False(true);
            }
            catch (NotEnoughMoneyException)
            {
                Assert.True(true);
            }
            catch
            {
                Assert.False(true);
            }
        }

        [Fact]
        public void Given2BetsOnelessThenPreviousBetThenThrowException()
        {
            Assert.Throws<BetIsLesserThanLastBetException>(delegate ()
             {
                 _phaseOneService.Bet(_players[0], 1);
                 _phaseOneService.Bet(_players[1], 1);
             });
        }

        [Fact]
        public void GivenPassThenGetMinPropertyAndHalfMoneyBackWithOddNumber()
        {
            var propertyCards = _phaseOneService.SpreadPropertyCards();
            PropertyCard[] propertyCardsArray = new PropertyCard[4];
            propertyCards.CopyTo(propertyCardsArray);
            _phaseOneService.Bet(_players[0], 1);
            _phaseOneService.Bet(_players[1], 2);
            _phaseOneService.Bet(_players[2], 3);
            _phaseOneService.Bet(_players[3], 4);
            _phaseOneService.Pass(_players[0].ID);

            Assert.Equal(14, _players[0].Money);
            Assert.Contains(propertyCardsArray.Single(c => c.Value == propertyCardsArray.Min(z => z.Value)), _players[0].properties);
        }

        [Fact]
        public void GivenPassThenGiveBackHalfMoneyWithEvenNumber()
        {
            var propertyCards = _phaseOneService.SpreadPropertyCards();
            PropertyCard[] propertyCardsArray = new PropertyCard[4];
            propertyCards.CopyTo(propertyCardsArray);
            _phaseOneService.Bet(_players[0], 2);
            _phaseOneService.Bet(_players[1], 3);
            _phaseOneService.Bet(_players[2], 4);
            _phaseOneService.Bet(_players[3], 5);
            _phaseOneService.Pass(_players[0].ID);

            Assert.Equal(14, _players[0].Money);
            Assert.Contains(propertyCardsArray.Single(c => c.Value == propertyCardsArray.Min(z => z.Value)), _players[0].properties);
        }

        [Fact]
        public void PassOutOfTurn()
        {
            Assert.Throws<WrongTurnException>(delegate
            {
                _phaseOneService.SpreadPropertyCards();
                _phaseOneService.Pass(_players[0].ID);
                _phaseOneService.Pass(_players[0].ID);
            });
        }

        [Fact]
        public void Pass5TimesThenThrowNoActiveRoundsException()
        {
            Assert.Throws<NoActiveRoundException>(delegate
            {
                _phaseOneService.SpreadPropertyCards();
                _phaseOneService.Pass(_players[0].ID);
                _phaseOneService.Pass(_players[1].ID);
                _phaseOneService.Pass(_players[2].ID);
                _phaseOneService.Pass(_players[3].ID);
                _phaseOneService.Pass(_players[0].ID);
            });
        }
    }
}
