using System.ComponentModel;

namespace NutriFitWeb.Models
***REMOVED***
    public class Trainer
    ***REMOVED***
        [DisplayName("ID do Treinador")]
        public int TrainerId ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Primeiro Nome")]
        public string? TrainerFirstName ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Último Nome")]
        public string? TrainerLastName ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Ginásio")]
        public Gym? Gym ***REMOVED*** get; set; ***REMOVED***

        public UserAccountModel? UserAccountModel ***REMOVED*** get; set; ***REMOVED***

        public List<Client>? Clients ***REMOVED*** get; set; ***REMOVED***

***REMOVED***
***REMOVED***
