using System;
using PlainElastic.Net;
using PlainElastic.Net.Serialization;

namespace ElasticSearch.POC.ConsoleApp
{
    internal class User
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

    internal class UserIndexer
    {
        private readonly IElasticConnection connection;
        private readonly JsonNetSerializer serializer;

        public UserIndexer(IElasticConnection connection)
        {
            this.connection = connection;
            serializer = new JsonNetSerializer();
        }

        public void Index(string firstName, string lastName)
        {
            IndexUser(firstName, lastName);
        }

        private void IndexUser(string firstName, string lastName)
        {
            var random = new Random();
            var user = new User { FirstName = firstName, LastName = lastName };
            if (random.Next(10) > 5)
                WriteUser_WithOwnObject(user);
            else
                WriteUser_WithPlainJson(user);
        }

        private void WriteUser_WithPlainJson(User user)
        {
            var command = string.Format("http://localhost:9200/twitter/user/{0}", user._id);
            var serializedUser = string.Format("{{\"FirstName\": \"{1}\",\"LastName\": \"{2}\",\"UserName\": \"{3}\"}}", user._id, user.FirstName, user.LastName, user.UserName);
            Console.WriteLine(connection.Put(command, serializedUser));
        }

        private void WriteUser_WithOwnObject(User user)
        {
            var command = Commands.Index(index: "twitter", type: "user", id: user._id);
            var serializedUser = serializer.ToJson(user);
            Console.WriteLine(connection.Put(command, serializedUser));
        }
    }

}