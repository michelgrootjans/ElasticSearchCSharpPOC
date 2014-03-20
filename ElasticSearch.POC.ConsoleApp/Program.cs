using System;
using System.Threading;
using PlainElastic.Net;

namespace ElasticSearch.POC.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new ElasticConnection("localhost");
            ResetIndex(connection, "twitter");

            var user = new UserIndexer(connection);
            user.Index("Michel", "Grootjans");
            user.Index("Bill", "Gates");
            user.Index("Steve", "Jobs");
            user.Index("Barak", "Obama");
            user.Index("Michele", "Obamma");

            var tweetIndexer = new TweetIndexer(connection);
            tweetIndexer.Index("michelgrootjans", "Ik schrijf een elasticsearch POC");

            Flush(connection, "twitter");

            new QueryExecutor(connection).Query("michel*");

            Console.Write("Press the ANY key...");
            Console.ReadLine();
        }

        private static void ResetIndex(IElasticConnection connection, string index)
        {
            try
            {
                var deleteCommand = Commands.Delete(index: index);
                connection.Delete(deleteCommand);
            }
            catch {}
        }

        private static void Flush(IElasticConnection connection, string index)
        {
            var flushCommand = Commands.Flush(index: index);
            connection.Post(flushCommand);
        }

    }
}
