namespace ElasticSearch.POC.ConsoleApp
{
    internal class User : IIndexable
    {
        private static int count;
        public string _id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName
        {
            get { return FirstName.ToLower() + LastName.ToLower(); }
        }

        public User()
        {
            _id = (++count).ToString();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }
    }

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