using System.ComponentModel;

namespace NutriFitWeb.Models
***REMOVED***
    public class Gym
    ***REMOVED***
        [DisplayName("ID do Ginásio")]
        public int GymId ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Nome")]
        public string? GymName ***REMOVED*** get; set; ***REMOVED***

        public UserAccountModel? UserAccountModel ***REMOVED*** get; set; ***REMOVED***

        public List<Client>? Clients ***REMOVED*** get; set; ***REMOVED***
        public List<Nutritionist>? Nutritionists ***REMOVED*** get; set; ***REMOVED***
        public List<Trainer>? Trainers ***REMOVED*** get; set; ***REMOVED***

***REMOVED***
***REMOVED***
