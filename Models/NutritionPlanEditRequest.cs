﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class NutritionPlanEditRequest
    {
        public int NutritionPlanEditRequestId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanEditRequestDescription { get; set; }
        public DateTime? NutritionPlanEditRequestDate { get; set; }

        public int? NutritionPlanId { get; set; }

        [ForeignKey("NutritionPlanId")]
        public NutritionPlan? NutritionPlan { get; set; }

        public bool NutritionPlanEditRequestDone { get; set; } = false;

        public Client? Client { get; set; }
    }
}
