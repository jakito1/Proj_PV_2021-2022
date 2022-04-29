using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    [Authorize(Roles = "client, nutritionist")]
    public class MealsController : Controller
    {
        private readonly string SessionKeyMeals;
        private readonly string SessionKeyPhoto;
        private readonly IPhotoManagement _photoManagement;

        public MealsController(IPhotoManagement photoManagement)
        {
            SessionKeyMeals = "_Meals";
            SessionKeyPhoto = "_Photo";
            _photoManagement = photoManagement;
        }

        public IActionResult ShowMealsList()
        {
            List<Meal>? meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
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
                Photo? oldPhoto = HttpContext.Session.Get<Photo>(SessionKeyPhoto);
                Photo? newPhoto = _photoManagement.UploadProfilePhoto(formFile);
                if ((oldPhoto is not null && newPhoto is not null) || (newPhoto is not null && oldPhoto is null))
                {
                    meal.MealPhoto = newPhoto;
                }
                else
                {
                    meal.MealPhoto = oldPhoto;
                }
                if (HttpContext.Session.Get<List<Meal>>(SessionKeyMeals) is null)
                {
                    HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, new List<Meal>() { meal });
                }
                else
                {
                    meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                    if (meals is not null)
                    {
                        meals.Add(meal);
                        HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, meals);
                    }
                }
            }
        }

        public IActionResult EditMeal(int id)
        {
            List<Meal>? meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);

            if (meals is null)
            {
                return NotFound();
            }

            Meal meal = meals[id];
            meals.RemoveAt(id);
            HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, meals);
            if (meal.MealPhoto is not null)
            {
                HttpContext.Session.Set<Photo>(SessionKeyPhoto, meal.MealPhoto);
            }
            return PartialView("_CreateMealPartial", meal);
        }

        public IActionResult DeleteMeal(int id)
        {
            List<Meal>? meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
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
