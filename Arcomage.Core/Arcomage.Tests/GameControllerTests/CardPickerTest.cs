using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Players;

namespace Arcomage.Tests.GameControllerTests
{
    class CardPickerTest : ICardPicker
    {
        public List<ICardObserver> Observers { get; private set; }
        public CardPickerTest()
        {
            Observers = new List<ICardObserver>();
        }


        public void AddObserver(ICardObserver observer)
        {
            Observers.Add(observer);
        }

        public void RemoveObserver(ICardObserver observer)
        {
            Observers.Remove(observer);
        }

        public void NotifyObservers(Card card)
        {
            foreach (var observer in Observers)
            {
                observer.Update(card);
            }
        }
    }
}
