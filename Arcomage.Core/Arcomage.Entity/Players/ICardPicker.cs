using Arcomage.Entity.Cards;

namespace Arcomage.Entity.Players
{
    public interface ICardPicker
    {
        void AddObserver(ICardObserver observer);
        void RemoveObserver(ICardObserver observer);
        void NotifyObservers(Card card);
    }
}
