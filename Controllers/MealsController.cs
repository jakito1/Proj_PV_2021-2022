using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    public class MealsController : Controller
    {
        private readonly string SessionKeyMeals;
        private readonly IPhotoManagement _photoManagement;

        public MealsController(IPhotoManagement photoManagement)
        {
            SessionKeyMeals = "_Meals";
            _photoManagement = photoManagement;
        }

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
        public IActionResult GetCleanCreateMealPartial()
        {
            return PartialView("_CreateMealPartial");
        }
    }
}
