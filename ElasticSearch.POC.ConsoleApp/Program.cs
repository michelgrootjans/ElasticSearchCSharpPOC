using System;
using System.Linq;
using System.Threading;
using PlainElastic.Net;

namespace ElasticSearch.POC.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new ElasticConnection("localhost");
            ResetIndex(connection);

            var user = new UserIndexer(connection);
            user.Index("Michel", "Grootjans");
            user.Index("Bill", "Gates");
            user.Index("Steve", "Jobs");
            user.Index("Barak", "Obama");
            user.Index("Michele", "Obama");

            var tweet = new TweetIndexer(connection);
            tweet.Index("michelgrootjans", "Ik schrijf een elasticsearch POC");

            Sleep(3);

            new QueryExecutor(connection).Query("michel*");

            Console.Write("Press the ANY key...");
            Console.ReadLine();
        }

        private static void ResetIndex(ElasticConnection connection)
        {

        }

        private static void Sleep(int numberOfSeconds)
        {
            Console.Write("Let ES do its indexing ");
            for (int second = 0; second < numberOfSeconds; second++)
            {
                Thread.Sleep(1000);
                Console.Write(".");
            }
            Console.WriteLine();
        }

    }
}
