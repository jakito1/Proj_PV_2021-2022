using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
{
    public class Machine
    {
        public int MachineId { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string? MachineName { get; set; }
        public string? MachineDescription { get; set; }
        public List<Exercise>? MachineExercises { get; set; }
        public ExerciseMuscles? MachineMuscles { get; set; }
        public Photo? MachineProfilePhoto { get; set; }
        public MachineType? MachineType { get; set; }
        public string? MachineQRCodeUri { get; set; }
    }

    public enum MachineType
    {
        [Display(Name = "Sentado")]
        SEATED
    }
}
