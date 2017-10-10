using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GuessWhat.Web.Data;
using GuessWhat.Web.Models.Home;

namespace GuessWhat.Web.Controllers
{
    public class HomeController : Controller
    {
        public const string Unknown = "UNKNOWN";

        private static Dictionary<string, List<Item>> animalsWithDetails = new Dictionary<string, List<Item>>();
        private static int questionCounter;
        
        private readonly GuessWhatContext db = new GuessWhatContext();

        public ActionResult Index()
        {
            animalsWithDetails =
                db.Animals
                .Where(a => a.AnimalDetails.Any())
                .OrderBy(a => a.Name)
                .ToDictionary(
                    a => a.Name,
                    a =>
                        a.AnimalDetails
                        .Select(ad =>
                            new Item
                            {
                                Value = ad.Detail,
                                WasShown = false
                            })
                        .ToList());

            questionCounter = 0; // Reset counter
            
            var viewModel =
                new IndexViewModel
                {
                    AllAnimalNames = animalsWithDetails.Select(x => x.Key)
                };

            return View(viewModel);
        }

        [OutputCache(Duration = 0)]
        public ActionResult Question(string value = "", bool answer = false)
        {
            if (!animalsWithDetails.Any())
            {
                RedirectToAction("Index", new { Controller = "Home" });
            }

            ModelState.Clear();
            ViewBag.QuestionCount = ++questionCounter;
            
            var resultAnimal = string.Empty;
            
            if (value != string.Empty)
            {
                resultAnimal = Filter(value, answer);
            }

            if (!string.IsNullOrWhiteSpace(resultAnimal))
            {
                // Redirect to Result page.
                return RedirectToAction("Result", new { result = resultAnimal });
            }

            var viewModel = PrepareQuestionViewModel(value);

            if (viewModel.Value == Unknown)
            {
                return RedirectToAction("Result", new { result = Unknown });
            }

            return View(viewModel);
        }

        public ActionResult Result(string result)
        {
            return View((object)result);
        }

        public ActionResult ResetQuestions()
        {
            foreach (var item in animalsWithDetails.SelectMany(x => x.Value))
            {
                item.WasShown = false;
            }

            return RedirectToAction("Question");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Details";

            return View();
        }

        private static string Filter(string value, bool answer)
        {
            var targetAnimals = animalsWithDetails.Where(x => x.Value.Any(detail => detail.Value == value)).ToList();

            if (!targetAnimals.Any())
            {
                return string.Empty;
            }

            foreach (var animal in targetAnimals)
            {
                if (answer)
                {
                    var item = animal.Value.Single(i => i.Value == value);
                    animal.Value.Remove(item);
                }
                else
                {
                    var item = animal.Value.Single(i => i.Value == value);
                    item.WasShown = true;
                    
                    continue;
                }

                if (animal.Value.Any())
                {
                    continue;
                }

                // Remove animal from dictionary as no more details exist for this animal.
                animalsWithDetails.Remove(animal.Key);

                return animal.Key;
            }

            return string.Empty;
        }

        private static QuestionViewModel PrepareQuestionViewModel(string value)
        {
            var remainingQuestions =
                animalsWithDetails
                .SelectMany(kvp => kvp.Value)
                .Where(i => !i.WasShown)
                .Select(i => i.Value)
                .ToArray();

            if (!remainingQuestions.Any())
            {
                return new QuestionViewModel { Value = Unknown, Answer = false };  
            }

            var r = new Random();
            var rndInt = r.Next(0, remainingQuestions.Count());

            return new QuestionViewModel { Value = remainingQuestions[rndInt], Answer = false };
        }

        public class Item
        {
            public string Value { get; set; }

            public bool WasShown { get; set; }
        }
    }
}