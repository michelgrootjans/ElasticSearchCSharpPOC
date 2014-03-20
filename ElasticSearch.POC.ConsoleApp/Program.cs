using System;
using PlainElastic.Net;

namespace ElasticSearch.POC.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new ElasticConnection("localhost");
            ResetIndex(connection, "twitter");

            var indexer = new Indexer(connection, "twitter");
            indexer.Index(new User { FirstName = "Michel", LastName = "Grootjans" });
            indexer.Index(new User { FirstName = "Bill", LastName = "Gates" });
            indexer.Index(new User { FirstName = "Steve", LastName = "Jobs" });
            indexer.Index(new User { FirstName = "Barak", LastName = "Obama" });
            indexer.Index(new User { FirstName = "Michele", LastName = "Obamma" });
            indexer.Index(new Tweet { UserName = "michelgrootjans", Text = "Ik schrijf een elasticsearch POC" });

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
