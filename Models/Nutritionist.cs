using System.ComponentModel;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
***REMOVED***
    public class Nutritionist
    ***REMOVED***
        [DisplayName("ID do Nutricionista")]
        public int NutritionistId ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Primeiro Nome")]
        public string? NutritionistFirstName ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Último Nome")]
        public string? NutritionistLastName ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Ginásio")]
        public Gym? Gym ***REMOVED*** get; set; ***REMOVED***

        public UserAccountModel? UserAccountModel ***REMOVED*** get; set; ***REMOVED***

        [JsonIgnore]
        public List<Client>? Clients ***REMOVED*** get; set; ***REMOVED***
        public List<NutritionPlan>? NutritionPlans ***REMOVED*** get; set; ***REMOVED***

***REMOVED***
***REMOVED***
