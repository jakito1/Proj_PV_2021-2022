namespace NutriFitWeb.Models
{
    public class NutritionPlanRequest
    {
        public int NutritionPlanRequestId { get; set; }
        public string? NutritionPlanRequestDescription { get; set; }
        public DateTime? NutritionPlanDateRequested { get; set; }

        public NutritionPlan? NutritionPlan { get; set; }

        public Client? Client { get; set; }
    }
}
