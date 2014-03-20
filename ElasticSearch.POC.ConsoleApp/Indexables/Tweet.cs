namespace ElasticSearch.POC.ConsoleApp.Indexables
{
    internal class Tweet : IIndexable
    {
        private static int count;
        public string _id { get; private set; }
        public string UserName { get; set; }
        public string Text { get; set; }

        public Tweet()
        {
            _id = (++count).ToString();
        }
    }
}