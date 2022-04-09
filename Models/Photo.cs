using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    public class Photo
    ***REMOVED***
        public int PhotoId ***REMOVED*** get; set; ***REMOVED***
        public string? PhotoTitle ***REMOVED*** get; set; ***REMOVED***
        public byte[]? PhotoData ***REMOVED*** get; set; ***REMOVED***

        [NotMapped]
        public string? PhotoUrl ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
