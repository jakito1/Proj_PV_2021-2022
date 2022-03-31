﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    public class NutritionPlan
    ***REMOVED***
        public int NutritionPlanId ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanName ***REMOVED*** get; set; ***REMOVED***
        public string? NutritionPlanDescription ***REMOVED*** get; set; ***REMOVED***

        public List<Meal>? Meals ***REMOVED*** get; set; ***REMOVED***
        public Nutritionist? Nutritionist ***REMOVED*** get; set; ***REMOVED***
        public Client? Client ***REMOVED*** get; set; ***REMOVED***

        /*
        public int? NutritionPlanRequestId ***REMOVED*** get; set; ***REMOVED***

        [ForeignKey("NutritionPlanRequestId")]
        public NutritionPlanNewRequest? NutritionPlanRequest ***REMOVED*** get; set; ***REMOVED****/

        [NotMapped]
        [Remote(action: "VerifyClientEmail", controller: "NutritionPlans")]
        public string? ClientEmail ***REMOVED*** get; set; ***REMOVED***

        [NotMapped]
        public Meal? Meal ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
