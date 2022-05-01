using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class Machine
    {
        public int MachineId { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? MachineName { get; set; }
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? MachineDescription { get; set; }
        public List<Exercise>? MachineExercises { get; set; }
        [RegularExpression(@"(.*\.)(jpe?g|gif)$", ErrorMessage = "Apenas imagens são permitidas.")]
        public Photo? MachineProfilePhoto { get; set; }
        [Column(TypeName = "nvarchar(24)")]
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
