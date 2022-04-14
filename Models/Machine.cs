using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    /// <summary>
    /// Machine class
    /// </summary>
    public class Machine
    ***REMOVED***
        /// <summary>
        /// Gets and Sets the machine id.
        /// </summary>
        public int MachineId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the machine name.
        /// </summary>
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string? MachineName ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the machine description.
        /// </summary>
        public string? MachineDescription ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the machine's exercise list.
        /// </summary>
        public List<Exercise>? MachineExercises ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the machine's picture.
        /// </summary>
        public Photo? MachineProfilePhoto ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the machine's type.
        /// </summary>
        [Column(TypeName = "nvarchar(24)")]
        public MachineType? MachineType ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the machine's QR code uri address.
        /// </summary>
        public string? MachineQRCodeUri ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the gym that the machine belongs to.
        /// </summary>
        public Gym? MachineGym ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the exercise associated with the machine.
        /// </summary>
        [NotMapped]
        public Exercise? MachineExercise ***REMOVED*** get; set; ***REMOVED***
***REMOVED***

    /// <summary>
    /// Machine type of: SEATED
    /// </summary>
    public enum MachineType
    ***REMOVED***
        [Display(Name = "Sentado")]
        SEATED
***REMOVED***
***REMOVED***
