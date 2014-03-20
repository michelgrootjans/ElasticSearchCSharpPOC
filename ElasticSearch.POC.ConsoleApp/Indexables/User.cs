namespace ElasticSearch.POC.ConsoleApp.Indexables
{
    internal class User : IIndexable
    {
        private static int count;
        public string _id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public User()
        {
            _id = (++count).ToString();
        }

        public string UserName
        {
            get { return FirstName.ToLower() + LastName.ToLower(); }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }
    }
}