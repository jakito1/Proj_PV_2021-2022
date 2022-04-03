using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
***REMOVED***
    public class Machine
    ***REMOVED***
        public int MachineId ***REMOVED*** get; set; ***REMOVED***

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string? MachineName ***REMOVED*** get; set; ***REMOVED***
        public string? MachineDescription ***REMOVED*** get; set; ***REMOVED***
        public List<Exercise>? MachineExercises ***REMOVED*** get; set; ***REMOVED***
        public Photo? MachineProfilePhoto ***REMOVED*** get; set; ***REMOVED***
        public MachineType? MachineType ***REMOVED*** get; set; ***REMOVED***
        public string? MachineQRCodeUri ***REMOVED*** get; set; ***REMOVED***
***REMOVED***

    public enum MachineType
    ***REMOVED***
        [Display(Name = "Sentado")]
        SEATED
***REMOVED***
***REMOVED***
