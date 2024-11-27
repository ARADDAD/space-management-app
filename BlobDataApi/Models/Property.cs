namespace BlobDataApi.Models
{
    public class Property
    {
        public string PropertyName { get; set; }
        public List<Space> Spaces { get; set; }
        public List<string> Features { get; set; }
        public List<string> Highlights { get; set; }
        public List<string> Transportation { get; set; }
    }
}
