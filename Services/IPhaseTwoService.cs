using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public interface IPhaseTwoService
    {
        void ChoosePropertyCard(Dictionary<int, PropertyCard> playedCards);
        List<ChequeCard> SpreadCards();
    }
}
