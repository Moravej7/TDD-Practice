using System.Collections.Generic;
using Models;

namespace Services
{
    public interface IPhaseOneService
    {
        void Bet(Player player, int betValue);
        void Pass(int playerId);
        List<PropertyCard> SpreadPropertyCards();
    }
}