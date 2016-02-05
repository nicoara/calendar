using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Calendar.Domain.Abstract;
using Calendar.Domain.Entities;

namespace Calendar.Domain.Concrete
{
    public class EFCardRepository : ICardRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Card> Cards
        {
            get { return context.Cards; }
        }

        public int SaveCard(Card card)
        {
            if (card.c_ID == 0)
            {
                context.Cards.Add(card);
                context.SaveChanges();
                return card.c_ID;
            }
            else
            {
                Card dbEntry = context.Cards.Find(card.c_ID);
                if (dbEntry != null)
                {
                    dbEntry.c_Index = card.c_Index;
                }
                context.SaveChanges();
                return dbEntry.c_ID;
            }
            
        }

        public void DeleteCard(Card card)
        {
            Card dbEntry = context.Cards.Find(card.c_ID);
            context.Cards.Remove(dbEntry);
            context.SaveChanges();
        }

        public void UpdateCard(Card card)
        {
            Card dbEntry = context.Cards.Find(card.c_ID);

            if (dbEntry != null)
            {
                dbEntry.c_Index = card.c_Index;
                dbEntry.c_Name = card.c_Name;
                dbEntry.c_Minutes = card.c_Minutes;
                dbEntry.c_StartHour = card.c_StartHour;
                dbEntry.c_FixedHour = card.c_FixedHour;
                dbEntry.c_Done = card.c_Done;
            }

            context.SaveChanges();
        }
    }
}
