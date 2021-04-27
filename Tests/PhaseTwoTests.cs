using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class PhaseTwoTests
    {
        private readonly IDeckInitializerService _deckInitializerService;
        private readonly IPhaseTwoService _phaseTwoService;
        private readonly List<Player> _players;
        private readonly List<ChequeCard> _chequeCards;

        public PhaseTwoTests()
        {
            _deckInitializerService = new DeckInitializerService();
            _players = MockPlayers();
            _chequeCards = _deckInitializerService.GetChequeCards().OrderBy(c => 10).ToList();
            _phaseTwoService = new PhaseTwoService(_players, _chequeCards);
        }

        private List<Player> MockPlayers()
        {
            List<Player> players = _deckInitializerService.GetPlayers();
            Random random = new Random();
            List<PropertyCard> propertyCards = _deckInitializerService.GetPropertyCards().OrderBy(c => 10).ToList();
            int i = 7;
            foreach (var item in players)
            {
                item.properties.AddRange(propertyCards.Take(7));
                propertyCards.RemoveRange(0, 7);
                i++;
            }
            return players;
        }

        [Fact]
        public void CheckSpreadCardsDontReturnRedundantCards()
        {
            List<ChequeCard> cheques = _phaseTwoService.SpreadCards();
            Assert.True(2 < cheques.Select(c => c.Value).Distinct().Count() && 4 >= cheques.Select(c => c.Value).Distinct().Count());
        }

        [Fact]
        public void CheckSpreadCardsDontReturnRedundantCardsInAllTimes()
        {
            List<ChequeCard> cheques = _phaseTwoService.SpreadCards();
            cheques.AddRange(_phaseTwoService.SpreadCards());
            cheques.AddRange(_phaseTwoService.SpreadCards());
            cheques.AddRange(_phaseTwoService.SpreadCards());
            cheques.AddRange(_phaseTwoService.SpreadCards());
            cheques.AddRange(_phaseTwoService.SpreadCards());
            cheques.AddRange(_phaseTwoService.SpreadCards());
            Assert.Equal(28, cheques.Count());
            Assert.Equal(14, cheques.Select(c => c.Value).Distinct().Count());
        }

        [Fact]
        public void GivenPlayersPropertyCardsThenGivePlayersChequeCards()
        {
            var cheques = _phaseTwoService.SpreadCards();
            _phaseTwoService.ChoosePropertyCard(new Dictionary<int, PropertyCard>()
            {
                { _players[0].ID, _players[0].properties[1] },
                { _players[1].ID, _players[1].properties[1] },
                { _players[2].ID, _players[2].properties[1] },
                { _players[3].ID, _players[3].properties[1] }
            });

            Assert.True(_players.All(c => c.ChequeCards.Count == 1));
        }


        [Fact]
        public void GivenPlayersPropertyCardsThenChosenPropertyCardBeDeleted()
        {
            var cheques = _phaseTwoService.SpreadCards();
            _phaseTwoService.ChoosePropertyCard(new Dictionary<int, PropertyCard>()
            {
                { _players[0].ID, _players[0].properties[1] },
                { _players[1].ID, _players[1].properties[1] },
                { _players[2].ID, _players[2].properties[1] },
                { _players[3].ID, _players[3].properties[1] }
            });

            Assert.True(_players.All(c => c.properties.Count == 6));
        }

        [Fact]
        public void GivenPlayersPropertyThenGiveRightChequeCards()
        {
            var cheques = _phaseTwoService.SpreadCards();
            var playedCards = new Dictionary<int, PropertyCard>()
            {
                { _players[0].ID, _players[0].properties[1] },
                { _players[1].ID, _players[1].properties[1] },
                { _players[2].ID, _players[2].properties[1] },
                { _players[3].ID, _players[3].properties[1] }
            };
            _phaseTwoService.ChoosePropertyCard(playedCards);
            cheques = cheques.OrderBy(c => c.Value).ToList();
            playedCards = playedCards.OrderBy(c => c.Value.Value).ToDictionary(c => c.Key, c => c.Value);

            bool AssertFlag = true;

            for (int i = 0; i < 4; i++)
            {
                if (cheques[i].Value != _players[playedCards.ElementAt(i).Key].ChequeCards[0].Value)
                {
                    AssertFlag = false;
                }
            }

            Assert.True(AssertFlag);
        }

    }
}
