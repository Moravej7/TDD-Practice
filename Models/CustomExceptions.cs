using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class WrongTurnException : Exception
    {
        public WrongTurnException() : base()
        {
        }
    }
    public class NotEnoughMoneyException : Exception
    {
        public NotEnoughMoneyException() : base()
        {
        }
    }
    public class BetIsLesserThanLastBetException : Exception
    {
        public BetIsLesserThanLastBetException() : base()
        {
        }
    }
    public class NoActiveRoundException : Exception
    {
        public NoActiveRoundException() : base()
        {
        }
    }
}
