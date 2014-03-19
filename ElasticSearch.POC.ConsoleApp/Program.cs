using System;
using System.Linq;
using PlainElastic.Net;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Serialization;

namespace ElasticSearch.POC.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Write some data
            WriteWithPlainJson("23", "Bill", "Gates");
            WriteWithOwnObject("24", "Michel", "Grootjans");

            //query
            Query("Michel");

            Console.Write("Press the ANY key...");
            Console.ReadLine();
        }

        private static void Query(string queryString)
        {
            var query = new QueryBuilder<User>()
                .Query(q => q
                    .Bool(b => b
                        .Must(m => m
                            .QueryString(qs => qs
                                .Fields(tweet => tweet.FirstName, tweet => tweet.LastName).Query(queryString)
                            )))).BuildBeautified();

            var connection = new ElasticConnection("localhost");
            var serializer = new JsonNetSerializer();

            var result = connection.Post(Commands.Search("twitter", "user"), query);
            var user = serializer.ToSearchResult<User>(result).Documents.First();

            Console.WriteLine("Found [{0}]", user);
        }

        private static void WriteWithPlainJson(string id, string firstName, string lastName)
        {
            var connection = new ElasticConnection("localhost");
            var command = string.Format("http://localhost:9200/twitter/user/{0}", id);
            var tweet = string.Format("{{{0}: {1},{2}: {3}}}", Enquote("FirstName"), Enquote(firstName), Enquote("LastName"), Enquote(lastName));
            var response = connection.Put(command, tweet);
            Console.WriteLine(response);
        }

        private static void WriteWithOwnObject(string id, string firstName, string lastName)
        {
            var connection = new ElasticConnection("localhost");
            var command = Commands.Index(index: "twitter", type: "user", id: id);
            var tweet = new User {FirstName = firstName, LastName = lastName};
            var data = new JsonNetSerializer().ToJson(tweet);
            string response = connection.Put(command, data);

            var indexResult = new JsonNetSerializer().ToIndexResult(response);
            if (indexResult.ok)
            {
                Console.WriteLine("OK");
            }
        }

        private static string Enquote(string txt)
        {
            return string.Format("\"{0}\"", txt);
        }
    }

    internal class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }
    }
}
