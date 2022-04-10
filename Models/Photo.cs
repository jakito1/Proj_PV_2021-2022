using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    /// <summary>
    /// Photo class
    /// </summary>
    public class Photo
    ***REMOVED***
        /// <summary>
        /// Gets and Sets the photo id.
        /// </summary>
        public int PhotoId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the photo title.
        /// </summary>
        public string? PhotoTitle ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the byte code of the image.
        /// </summary>
        public byte[]? PhotoData ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the photo url.
        /// </summary>
        [NotMapped]
        public string? PhotoUrl ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
