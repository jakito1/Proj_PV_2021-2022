using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    [Authorize(Roles = "client, nutritionist")]
    /// <summary>
    /// MealsController, derives from Controller
    /// </summary>
    public class MealsController : Controller
    ***REMOVED***
        private readonly string SessionKeyMeals;
        private readonly string SessionKeyPhoto;
        private readonly IPhotoManagement _photoManagement;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="photoManagement">Photo management interface</param>
        public MealsController(IPhotoManagement photoManagement)
        ***REMOVED***
            SessionKeyMeals = "_Meals";
            SessionKeyPhoto = "_Photo";
            _photoManagement = photoManagement;
    ***REMOVED***

        /// <summary>
        /// Renders a partial view for a list of meals.
        /// </summary>
        /// <returns>A PartialView result</returns>
        public IActionResult ShowMealsList()
        ***REMOVED***
            List<Meal>? meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
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
                Photo? oldPhoto = HttpContext.Session.Get<Photo>(SessionKeyPhoto);
                Photo? newPhoto = _photoManagement.UploadProfilePhoto(formFile);
                if ((oldPhoto is not null && newPhoto is not null) || (newPhoto is not null && oldPhoto is null))
                ***REMOVED***
                    meal.MealPhoto = newPhoto;
            ***REMOVED***
                else
                ***REMOVED***
                    meal.MealPhoto = oldPhoto;
            ***REMOVED***
                if (HttpContext.Session.Get<List<Meal>>(SessionKeyMeals) is null)
                ***REMOVED***
                    HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, new List<Meal>() ***REMOVED*** meal ***REMOVED***);
            ***REMOVED***
                else
                ***REMOVED***
                    meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                    if (meals is not null)
                    ***REMOVED***
                        meals.Add(meal);
                        HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, meals);
                ***REMOVED***
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
            List<Meal>? meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);

            if (meals is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            Meal meal = meals[id];
            meals.RemoveAt(id);
            HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, meals);
            if (meal.MealPhoto is not null)
            ***REMOVED***
                HttpContext.Session.Set<Photo>(SessionKeyPhoto, meal.MealPhoto);
        ***REMOVED***
            return PartialView("_CreateMealPartial", meal);
    ***REMOVED***

        /// <summary>
        /// Renders a partial view to delete a meal, given the meal id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A PartialView result</returns>
        public IActionResult DeleteMeal(int id)
        ***REMOVED***
            List<Meal>? meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
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
