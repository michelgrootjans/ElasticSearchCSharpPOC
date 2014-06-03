namespace ElasticSearch.ConsoleApp.Models
{
    internal class Project : IIndexable
    {
        public string _id { get; set; }
        public string identificatie { get; set; }
        public string project_type { get; set; }
        public string omschrijving { get; set; }
        public string status { get; set; }
        public string gemeente { get; set; }
    }
}