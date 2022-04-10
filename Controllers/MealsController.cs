using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    /// <summary>
    /// MealsController, derives from Controller
    /// </summary>
    public class MealsController : Controller
    ***REMOVED***
        private readonly string SessionKeyMeals;
        private readonly IPhotoManagement _photoManagement;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="photoManagement">Photo management interface</param>
        public MealsController(IPhotoManagement photoManagement)
        ***REMOVED***
            SessionKeyMeals = "_Meals";
            _photoManagement = photoManagement;
    ***REMOVED***

        /// <summary>
        /// Renders a partial view for a list of meals.
        /// </summary>
        /// <returns>A PartialView result</returns>
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

        /// <summary>
        /// HTTP POST action on the API to Create a meal.
        /// </summary>
        /// <param name="meal"></param>
        /// <param name="formFile"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void CreateMeal([Bind("MealId,MealName,MealDescription,MealCalorie,MealProtein,MealFat,MealCarbohydrate,MealDate,MealWeekDay,MealType,MealURL")] Meal meal,
            IFormFile? formFile)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                List<Meal>? meals;
                meal.MealPhoto = _photoManagement.UploadProfilePhoto(formFile);
                if (HttpContext.Session.Get<List<Meal>>(SessionKeyMeals) is null)
                ***REMOVED***
                    HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, new List<Meal>() ***REMOVED*** meal ***REMOVED***);
            ***REMOVED***
                else
                ***REMOVED***
                    meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                    meals.Add(meal);
                    HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, meals);
            ***REMOVED***
        ***REMOVED***
    ***REMOVED***

        /// <summary>
        /// Renders a partial view to edit a meal, given the meal's id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A PartialView result</returns>
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

        /// <summary>
        /// Renders a partial view to delete a meal, given the meal id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A PartialView result</returns>
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

        /// <summary>
        /// Renders a partial view to create a a Meal.
        /// </summary>
        /// <returns>A PartialView result</returns>
        public IActionResult GetCleanCreateMealPartial()
        ***REMOVED***
            return PartialView("_CreateMealPartial");
    ***REMOVED***
***REMOVED***
***REMOVED***
