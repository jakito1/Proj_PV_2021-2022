using System.ComponentModel;

namespace NutriFitWeb.Models
***REMOVED***
    /// <summary>
    /// Gym class
    /// </summary>
    public class Gym
    ***REMOVED***
        /// <summary>
        /// Gets and Sets the gym id.
        /// Display name = ID do Ginásio
        /// </summary>
        [DisplayName("ID do Ginásio")]
        public int GymId ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the gym name.
        /// Display name = Nome
        /// </summary>
        [DisplayName("Nome")]
        public string? GymName ***REMOVED*** get; set; ***REMOVED***
        
        /// <summary>
        /// Gets and Sets the gym profile picture.
        /// Display name = Foto de Perfil
        /// </summary>
        [DisplayName("Foto de Perfil")]
        public Photo? GymProfilePhoto ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the gym user account model.
        /// </summary>
        public UserAccountModel? UserAccountModel ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the gym client list.
        /// </summary>
        public List<Client>? Clients ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the gym nutritionist list.
        /// </summary>
        public List<Nutritionist>? Nutritionists ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the gym trainer list.
        /// </summary>
        public List<Trainer>? Trainers ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the gym machines list.
        /// </summary>
        public List<Machine>? Machines ***REMOVED*** get; set; ***REMOVED***

***REMOVED***
***REMOVED***
