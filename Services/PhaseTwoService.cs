using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;

namespace Services
{
    public class PhaseTwoService : IPhaseTwoService
    {
        private readonly List<Player> _players;
        private readonly List<ChequeCard> _chequeCards;
        private List<ChequeCard> _thisRoundChequeCards;

        public PhaseTwoService(List<Player> players, List<ChequeCard> chequeCards)
        {
            _players = players;
            _chequeCards = chequeCards.OrderBy(c => 10).ToList();
        }

        public void ChoosePropertyCard(Dictionary<int, PropertyCard> playedCards)
        {
            playedCards = playedCards.OrderBy(c => c.Value.Value).ToDictionary(c => c.Key, c => c.Value);
            int i = 0;
            foreach (var playedCard in playedCards)
            {
                _players[playedCard.Key].ChequeCards.Add(_thisRoundChequeCards[i]);
                _players[playedCard.Key].properties.Remove(_players[playedCard.Key].properties.Single(c => c.Value == playedCard.Value.Value));
                i++;
            }
        }

        public List<ChequeCard> SpreadCards()
        {
            var cheques = _chequeCards.GetRange(0, 4);
            _thisRoundChequeCards = cheques.OrderBy(c => c.Value).ToList();
            _chequeCards.RemoveRange(0, 4);
            return cheques.ToList();
        }
    }
}
