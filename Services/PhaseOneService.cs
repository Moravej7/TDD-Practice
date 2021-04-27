using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class PhaseOneService : IPhaseOneService
    {
        private readonly List<Player> _players;
        private readonly List<PropertyCard> _propertyCards;
        private readonly Random _random;
        private readonly List<PropertyCard> _playedCards;
        private List<PropertyCard> _thisRoundPropertyCards;
        private int _turnPlayerId;
        private int _maxBet;

        public PhaseOneService(List<Player> players, List<PropertyCard> propertyCards)
        {
            _players = players;
            _random = new Random();
            _propertyCards = propertyCards.OrderBy(c => _random.Next()).ToList();
            _playedCards = new List<PropertyCard>();
            _turnPlayerId = _players.First().ID;
        }

        public List<PropertyCard> SpreadPropertyCards()
        {
            _thisRoundPropertyCards = _propertyCards
                .Take(4).ToList();
            _playedCards.AddRange(_thisRoundPropertyCards);
            _propertyCards.RemoveRange(0, 4);
            _maxBet = 0;
            return _thisRoundPropertyCards;
        }

        public void Bet(Player player, int betValue)
        {
            if (_turnPlayerId != player.ID)
                throw new WrongTurnException();

            _turnPlayerId = NextPlayerTurn(_turnPlayerId);

            if (player.Money < betValue)
                throw new NotEnoughMoneyException();

            if (_maxBet >= betValue)
                throw new BetIsLesserThanLastBetException();

            if (_maxBet < betValue)
                _maxBet = betValue;

            player.Money -= betValue;
            player.Bet += betValue;
        }

        public void Pass(int playerId)
        {
            if (_thisRoundPropertyCards.Count() == 0)
                throw new NoActiveRoundException();

            if (_turnPlayerId != playerId)
                throw new WrongTurnException();

            _turnPlayerId = NextPlayerTurn(playerId);

            _players[playerId].HasPassed = true;
            _players[playerId].Money += _players[playerId].Bet / 2;
            _players[playerId].properties.Add(_thisRoundPropertyCards.Single(c => c.Value == _thisRoundPropertyCards.Min(x => x.Value)));
            _thisRoundPropertyCards.Remove(_thisRoundPropertyCards.Single(c => c.Value == _thisRoundPropertyCards.Min(x => x.Value)));
        }

        private int NextPlayerTurn(int currentPlayerId)
        {
            int index = currentPlayerId + 1;
            index %= _players.Count;

            if (_players[index].HasPassed == true)
                return NextPlayerTurn(index);

            return index;
        }
    }
}
