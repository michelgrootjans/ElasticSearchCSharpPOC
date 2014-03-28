namespace ElasticSearch.POC.ConsoleApp
{
    internal class Project : IIndexable
    {
        public string _id { get; set; }
        public string Identificatie { get; set; }
        public string ProjectType { get; set; }
        public int HuidigeProgrammatiefase { get; set; }
        public string Omschrijving { get; set; }
        public string Status { get; set; }
    }
}