using Arcomage.Entity.Cards;

namespace Arcomage.Entity.Players
{
    public interface ICardObserver
    {
        void Update(Card card);
    }
}
