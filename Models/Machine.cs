using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class Machine
    {
        public int MachineId { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string? MachineName { get; set; }
        public string? MachineDescription { get; set; }
        public List<Exercise>? MachineExercises { get; set; }
        public Photo? MachineProfilePhoto { get; set; }
        public MachineType? MachineType { get; set; }
        public string? MachineQRCodeUri { get; set; }

        public Gym? MachineGym { get; set; }

        [NotMapped]
        public Exercise? MachineExercise { get; set; }
    }

    public enum MachineType
    {
        [Display(Name = "Sentado")]
        SEATED
    }
}
