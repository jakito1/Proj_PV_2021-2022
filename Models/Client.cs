using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    public class Client
    ***REMOVED***

        [DisplayName("ID do Cliente")]
        public int ClientId ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Primeiro Nome")]
        public string? ClientFirstName ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Último Nome")]
        public string? ClientLastName ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Idade")]
        public int? ClientAge ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Peso")]
        public double? Weight ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Altura")]
        public double? Height ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Ginásio")]
        public Gym? Gym ***REMOVED*** get; set; ***REMOVED***

        public UserAccountModel? UserAccountModel ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
