namespace NutriFitWeb.Models
{
    public class Picture
    {
        public int PictureId { get; set; }
        public byte[] Bytes { get; set; }
        public string? Description { get; set; }
        public string FileExtension { get; set; }
        public double Size { get; set; }

        public List<Exercise>? Exercises { get; set; }
    }
}
