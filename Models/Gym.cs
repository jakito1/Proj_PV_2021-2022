using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
***REMOVED***
    public class Gym
    ***REMOVED***
        [DisplayName("ID do Ginásio")]
        public int GymId ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? GymName ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Foto de Perfil")]
        public Photo? GymProfilePhoto ***REMOVED*** get; set; ***REMOVED***

        public UserAccountModel UserAccountModel ***REMOVED*** get; set; ***REMOVED***

        public List<Client>? Clients ***REMOVED*** get; set; ***REMOVED***
        public List<Nutritionist>? Nutritionists ***REMOVED*** get; set; ***REMOVED***
        public List<Trainer>? Trainers ***REMOVED*** get; set; ***REMOVED***
        public List<Machine>? Machines ***REMOVED*** get; set; ***REMOVED***

***REMOVED***
***REMOVED***
