using System.Linq;
using Calendar.Domain.Entities;

namespace Calendar.Domain.Abstract
{
    public interface ICardRepository 
    {
        IQueryable<Card> Cards { get; }

        int SaveCard(Card ev);

        void DeleteCard(Card ev);

        void UpdateCard(Card ev);
    }
}
