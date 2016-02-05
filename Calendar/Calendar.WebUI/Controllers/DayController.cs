using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using Calendar.Domain.Abstract;
using Calendar.Domain.Entities;
using Calendar.WebUI.Models;

namespace Calendar.WebUI.Controllers
{
    [Authorize]
    public class DayController : Controller
    {
        private IDayRepository repositoryDay;
        private ICardRepository repositoryCard;
        private const string _mesajEroare = "A aparut o eroare neprevazuta in site.";

        public DayController(IDayRepository dayRepository,
            ICardRepository cardRepository)
        {
            this.repositoryDay = dayRepository;
            this.repositoryCard = cardRepository;
        }

        public ViewResult ListDays()
        {           
            try
            {
                DayViewModel model = new DayViewModel();
                if (repositoryDay.Days.Any())
                {
                    model.lstDays = repositoryDay.Days.ToList();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                return View("Eroare",
                    new EroareViewModel { MesajEroare = _mesajEroare });
            }
        }

        public ViewResult AddDay()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDay(int month, int day)
        {
            repositoryDay.SaveDay(new Day
            {
                d_Date = new DateTime(2015, month, day),
                d_ID = 0
            });

            return RedirectToAction("ListDays", "Day");
        }

        public ViewResult EditDay(int id)
        {
            Day date = repositoryDay.Days.First(d => d.d_ID == id);
            List<Card> listCards = repositoryCard.Cards
                .Where(c => c.c_DayID == id)
                .OrderBy(c => c.c_StartHour)
                .ThenBy(c => c.c_Index)
                .ToList();

            EditDayModel model = new EditDayModel
            {
                Date = date,
                lstCards = listCards
            };

            //aflu ce carduri sunt in conflict ca timp
            List<int> lstConflictingCards = new List<int>();
            foreach (Card card1 in model.lstCards)
            {
                foreach (Card card2 in model.lstCards)
                {
                    if (card1.c_ID != card2.c_ID)
                    {
                        if (//daca cardul 1 incepe in timpul cardului 2
                            (card1.c_StartHour > card2.c_StartHour && 
                            card1.c_StartHour < 
                            card2.c_StartHour.AddMinutes(card2.c_Minutes)) 
                            || 
                            //daca cardul 1 se termina in timpul cardului 2
                            (card1.c_StartHour.AddMinutes(card1.c_Minutes) > 
                            card2.c_StartHour &&
                            card1.c_StartHour.AddMinutes(card1.c_Minutes) <
                            card2.c_StartHour.AddMinutes(card2.c_Minutes)) ||
                            //daca cardurile au acelasi timp
                            (card1.c_StartHour == card2.c_StartHour &&
                            card1.c_StartHour.AddMinutes(card1.c_Minutes) ==
                            card2.c_StartHour.AddMinutes(card2.c_Minutes)))
                        {
                            lstConflictingCards.Add(card1.c_ID);
                            lstConflictingCards.Add(card2.c_ID);
                        }
                    }
                }
            }
            ViewBag.lstConflictingCards = lstConflictingCards.Distinct().ToList(); ;

            ViewBag.Date = date.d_Date;
            return View(model);
        }

        public ActionResult DeleteDay(int id)
        {
            Day date = repositoryDay.Days.First(d => d.d_ID == id);
            repositoryDay.DeleteDay(date);

            return RedirectToAction("ListDays", "Day");
        }

        public ViewResult AddCard(int id)
        {
            Day day = repositoryDay.Days.First(d => d.d_ID == id);

            CardModel model = new CardModel
            {
                DayId = id,
                StartHour = new DateTime(2015, day.d_Date.Month,day.d_Date.Day)
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddCard(CardModel model)
        {
            Day day = repositoryDay.Days.First(d => d.d_ID == model.DayId);
            DateTime genericDate = 
                new DateTime(2015, day.d_Date.Month, day.d_Date.Day, 0, 0, 0);

            DateTime previousEndHour = new DateTime();
            List<Card> lstCardsAfter = new List<Card>();
            if (model.StartHour != genericDate)//pun dupa ora
            {
                //gaseste primul card cu ora mai mare ca el
                lstCardsAfter = repositoryCard.Cards
                    .Where(c => c.c_DayID == model.DayId)
                    .Where(c => c.c_StartHour >= model.StartHour)
                    .OrderBy(c => c.c_StartHour)
                    .ToList();

                previousEndHour =
                    model.StartHour.AddMinutes(model.Minutes);

            }
            else if (model.Index != 0)//pun dupa index
            {
                //gaseste primul card cu index mai mare ca el
                lstCardsAfter = repositoryCard.Cards
                    .Where(c => c.c_DayID == model.DayId)
                    .Where(c => c.c_Index >= model.Index)
                    .OrderBy(c => c.c_StartHour)
                    .ToList();

                //gaseste primul card cu index mai mic ca el
                Card cardBefore = repositoryCard.Cards
                    .Where(c => c.c_DayID == model.DayId)
                    .Where(c => c.c_Index < model.Index)
                    .OrderByDescending(c => c.c_StartHour)
                    .First();

                model.StartHour = 
                    cardBefore.c_StartHour.AddMinutes(cardBefore.c_Minutes);

                previousEndHour = model.StartHour.AddMinutes(model.Minutes);
            }

            if (lstCardsAfter.Count > 0)
            {
                model.Index = lstCardsAfter.First().c_Index;

                // la toate cardurile de mai tarziu schimba-le indexul si ora
                int currentIndex = model.Index + 1;

                foreach (Card cardAfter in lstCardsAfter)
                {
                    cardAfter.c_Index = currentIndex;

                    if (cardAfter.c_FixedHour == false) // care nu sunt fixe
                    {
                        if (previousEndHour >= cardAfter.c_StartHour)
                        {
                            cardAfter.c_StartHour = previousEndHour;
                        }
                        previousEndHour =
                            cardAfter.c_StartHour.AddMinutes(cardAfter.c_Minutes);
                    }

                    repositoryCard.SaveCard(cardAfter);

                    currentIndex = currentIndex + 1;
                }
            }
            else
            {
                //daca pun un index f mare
                if (model.Index > repositoryCard.Cards
                    .Count(c => c.c_DayID == model.DayId)+1)
                {
                    model.Index = repositoryCard.Cards
                        .Count(c => c.c_DayID == model.DayId);
                }
            }

            //in ce situatie nu adaug ?
            repositoryCard.SaveCard(new Card
            {
                c_DayID = model.DayId,
                c_Name = model.Name,
                c_Minutes = model.Minutes,
                c_StartHour = model.StartHour,
                c_Index = model.Index
            });

            return RedirectToAction("ListDays", "Day");
        }

        public ViewResult UpdateCard(int id)
        {
            Card card = repositoryCard.Cards.First(c => c.c_ID == id);
            CardModel model = new CardModel
            {
                cID = card.c_ID,
                DayId = card.c_DayID,
                Index = card.c_Index,
                Name = card.c_Name,
                Minutes = card.c_Minutes,
                StartHour = card.c_StartHour,
                Fixed = card.c_FixedHour,
                Done = card.c_Done
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateCard(CardModel model)
        {
            Card card = new Card
            {
                c_ID = model.cID,
                c_DayID = model.DayId,
                c_Index = model.Index,
                c_Name = model.Name,
                c_Minutes = model.Minutes,
                c_StartHour = model.StartHour,
                c_FixedHour = model.Fixed,
                c_Done = model.Done
            };
            repositoryCard.UpdateCard(card);

            //sa updatez indecsii pt ca nu ii corectez altfel
            List<Card> lstCards = repositoryCard.Cards
                    .Where(c => c.c_DayID == model.DayId)
                    .OrderBy(c => c.c_StartHour)
                    .ToList();
            for(int i = 0; i < lstCards.Count; i++)
            {
                lstCards[i].c_Index = i;
                repositoryCard.UpdateCard(lstCards[i]);
            }


            return RedirectToAction("ListDays", "Day");
        }

        public ActionResult DeleteCard(int id)
        {
            Card card = repositoryCard.Cards.First(c => c.c_ID == id);
            repositoryCard.DeleteCard(card);

            //modify indexes of cards afterwards
            List<Card> lstCardsAfter = repositoryCard.Cards
                .Where(c => c.c_DayID == card.c_DayID)
                .Where(c => c.c_Index > card.c_Index)
                .ToList();
            foreach (Card cardAfter in lstCardsAfter)
            {
                cardAfter.c_Index = cardAfter.c_Index - 1;
                repositoryCard.SaveCard(cardAfter);
            }

            return RedirectToAction("ListDays", "Day");
        }

        public ViewResult Features()
        {
            return View();
        }
    }
}
