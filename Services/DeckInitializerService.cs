using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class DeckInitializerService : IDeckInitializerService
    {
        public List<PropertyCard> GetPropertyCards()
        {
            List<PropertyCard> properties = new List<PropertyCard>();
            for (int i = 1; i <= 28; i++)
            {
                PropertyCard propertyCard = new PropertyCard()
                {
                    Name = "Card " + i.ToString(),
                    Value = i
                };
                properties.Add(propertyCard);
            }
            return properties;
        }

        public List<ChequeCard> GetChequeCards()
        {
            List<ChequeCard> chequeCards = new List<ChequeCard>();
            for (int j = 0; j < 2; j++)
                for (int i = 0; i <= 15; i++)
                {
                    if (i != 1 && i != 2)
                    {
                        ChequeCard checkCard = new ChequeCard()
                        {
                            Value = i
                        };
                        chequeCards.Add(checkCard);
                    }
                }

            return chequeCards;
        }

        public List<Player> GetPlayers()
        {
            List<Player> players = new List<Player>();
            for (int i = 0; i < 4; i++)
            {
                Player player = new Player()
                {
                    ID = i,
                    Name = "P" + i.ToString(),
                    Money = 15,
                    properties = new List<PropertyCard>()
                };
                players.Add(player);
            }
            return players;
        }
    }
}
