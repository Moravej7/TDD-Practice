using System.Collections.Generic;
using Models;

namespace Services
{
    public interface IDeckInitializerService
    {
        List<ChequeCard> GetChequeCards();
        List<Player> GetPlayers();
        List<PropertyCard> GetPropertyCards();
    }
}