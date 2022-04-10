using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    /// <summary>
    /// MealsController, derives from Controller
    /// </summary>
    public class MealsController : Controller
    {
        private readonly string SessionKeyMeals;
        private readonly IPhotoManagement _photoManagement;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="photoManagement">Photo management interface</param>
        public MealsController(IPhotoManagement photoManagement)
        {
            SessionKeyMeals = "_Meals";
            _photoManagement = photoManagement;
        }

        /// <summary>
        /// Renders a partial view for a list of meals.
        /// </summary>
        /// <returns>A PartialView result</returns>
        public IActionResult ShowMealsList()
        {
            List<Meal> meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
            if (meals is not null)
            {
                meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                return PartialView("_ShowMealsPartial", meals);
            }

            return PartialView("_ShowMealsPartial", new List<Meal>());
        }

        /// <summary>
        /// HTTP POST action on the API to Create a meal.
        /// </summary>
        /// <param name="meal"></param>
        /// <param name="formFile"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void CreateMeal([Bind("MealId,MealName,MealDescription,MealCalorie,MealProtein,MealFat,MealCarbohydrate,MealDate,MealWeekDay,MealType,MealURL")] Meal meal,
            IFormFile? formFile)
        {
            if (ModelState.IsValid)
            {
                List<Meal>? meals;
                meal.MealPhoto = _photoManagement.UploadProfilePhoto(formFile);
                if (HttpContext.Session.Get<List<Meal>>(SessionKeyMeals) is null)
                {
                    HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, new List<Meal>() { meal });
                }
                else
                {
                    meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                    meals.Add(meal);
                    HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, meals);
                }
            }
        }

        /// <summary>
        /// Renders a partial view to edit a meal, given the meal's id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A PartialView result</returns>
        public IActionResult EditMeal(int id)
        {

            List<Meal> meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);

            if (meals is null)
            {
                return NotFound();
            }

            Meal meal = meals[id];
            meals.RemoveAt(id);
            HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, meals);
            return PartialView("_CreateMealPartial", meal);

        }

        /// <summary>
        /// Renders a partial view to delete a meal, given the meal id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A PartialView result</returns>
        public IActionResult DeleteMeal(int id)
        {

            List<Meal> meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
            if (meals is not null)
            {
                meals.RemoveAt(id);
                HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, meals);

                return PartialView("_ShowMealsPartial", meals);
            }

            return PartialView("_ShowMealsPartial", new List<Exercise>());
        }

        /// <summary>
        /// Renders a partial view to create a a Meal.
        /// </summary>
        /// <returns>A PartialView result</returns>
        public IActionResult GetCleanCreateMealPartial()
        {
            return PartialView("_CreateMealPartial");
        }
    }
}
