using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    /// <summary>
    /// Machine class
    /// </summary>
    public class Machine
    {
        /// <summary>
        /// Gets and Sets the machine id.
        /// </summary>
        public int MachineId { get; set; }
        /// <summary>
        /// Gets and Sets the machine name.
        /// </summary>
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? MachineName { get; set; }
        /// <summary>
        /// Gets and Sets the machine description.
        /// </summary>
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? MachineDescription { get; set; }
        /// <summary>
        /// Gets and Sets the machine's exercise list.
        /// </summary>
        public List<Exercise>? MachineExercises { get; set; }
        /// <summary>
        /// Gets and Sets the machine's picture.
        /// </summary>
        public Photo? MachineProfilePhoto { get; set; }
        /// <summary>
        /// Gets and Sets the machine's type.
        /// </summary>
        [Column(TypeName = "nvarchar(24)")]
        public MachineType? MachineType { get; set; }
        /// <summary>
        /// Gets and Sets the machine's QR code uri address.
        /// </summary>
        public string? MachineQRCodeUri { get; set; }

        /// <summary>
        /// Gets and Sets the gym that the machine belongs to.
        /// </summary>
        public Gym? MachineGym { get; set; }

        /// <summary>
        /// Gets and Sets the exercise associated with the machine.
        /// </summary>
        [NotMapped]
        public Exercise? MachineExercise { get; set; }
    }

    /// <summary>
    /// Machine type of: SEATED
    /// </summary>
    public enum MachineType
    {
        [Display(Name = "Sentado")]
        SEATED
    }
}
