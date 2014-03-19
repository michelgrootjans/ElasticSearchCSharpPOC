using System;
using System.Linq;
using PlainElastic.Net;

namespace ElasticSearch.POC.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new ElasticConnection("localhost");

            var user = new UserIndexer(connection);
            user.Index("Michel", "Grootjans");
            user.Index("Bill", "Gates");
            user.Index("Steve", "Jobs");
            user.Index("Barak", "Obama");
            user.Index("Michele", "Obama");

            var tweet = new TweetIndexer();
            tweet.Index("michelgrootjans", "Ik schrijf een elasticsearch POC");

            //query
            new MyIndex(connection).Query("michel*");

            Console.Write("Press the ANY key...");
            Console.ReadLine();
        }

        private static void Query(string queryString)
        {
        }

    }
}
