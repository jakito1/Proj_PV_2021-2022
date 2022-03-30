namespace NutriFitWeb.Models
{
    public class TrainingPlanRequest
    {
        public int TrainingPlanRequestId { get; set; }
        public string? TrainingPlanRequestDescription { get; set; }
        public DateTime? TrainingPlanDateRequested { get; set; }

        public TrainingPlan? TrainingPlan { get; set; }

        public Client? Client { get; set; }
    }
}
