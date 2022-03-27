#nullable disable
using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class MealsController : Controller
    ***REMOVED***
        private readonly string SessionKeyMeals;

        public MealsController()
        ***REMOVED***
            SessionKeyMeals = "_Meals";
    ***REMOVED***

        public IActionResult ShowMealsList()
        ***REMOVED***
            List<Meal> meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
            if (meals is not null)
            ***REMOVED***
                meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                return PartialView("_ShowMealsPartial", meals);
        ***REMOVED***

            return PartialView("_ShowMealsPartial", new List<Meal>());
    ***REMOVED***

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void CreateMeal([Bind("MealId,MealName,MealDescription,MealCalorie,MealProtein,MealFat,MealCarbohydrate,MealDate,MealWeekDay,MealType,MealURL")] Meal meal)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                List<Meal> meals = new();
                if (HttpContext.Session.Get<List<Meal>>(SessionKeyMeals) is null)
                ***REMOVED***
                    HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, new List<Meal>() ***REMOVED*** meal ***REMOVED***);
                    meals.Add(meal);
            ***REMOVED***
                else
                ***REMOVED***
                    meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                    meals.Add(meal);
                    HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, meals);
            ***REMOVED***
        ***REMOVED***
    ***REMOVED***

        public IActionResult EditMeal(int id)
        ***REMOVED***

            List<Meal> meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);

            if (meals is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            Meal meal = meals[id];
            meals.RemoveAt(id);
            HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, meals);
            return PartialView("_CreateMealPartial", meal);

    ***REMOVED***

        public IActionResult DeleteMeal(int id)
        ***REMOVED***

            List<Meal> meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
            if (meals is not null)
            ***REMOVED***
                meals.RemoveAt(id);
                HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, meals);

                return PartialView("_ShowMealsPartial", meals);
        ***REMOVED***

            return PartialView("_ShowMealsPartial", new List<Exercise>());
    ***REMOVED***
        public IActionResult GetCleanCreateMealPartial()
        ***REMOVED***
            return PartialView("_CreateMealPartial");
    ***REMOVED***
***REMOVED***
***REMOVED***
