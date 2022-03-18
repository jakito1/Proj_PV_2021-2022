﻿namespace NutriFitWeb.Models
{
    public class TrainingPlan
    {
        public int TrainingPlanId { get; set; }
        public string? TrainingPlanName { get; set; }
        public string? TrainingPlanDescription { get; set; }

        public List<Exercise>? Exercises { get; set; }
    }

}
