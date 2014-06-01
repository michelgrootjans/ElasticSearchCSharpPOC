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

            IndexVmswData(connection);
            while (true)
            {
                var result = QueryData(connection);
                PrintResult(result);
            }
        }

        private static void IndexVmswData(ElasticConnection connection)
        {
            var indexer = new Indexer(connection, "prisma");
            indexer.Reset();
//            indexer.InitializeWith(new ProjectMapper());
            var projecten = new DataAccessLayer().GetVmswProjecten();
            var start = DateTime.Now;
            indexer.Index(projecten);
            indexer.Flush();
            Console.WriteLine("Done indexing {0} records - took {1}ms", projecten.Count(), (DateTime.Now - start).TotalMilliseconds);
        }

        private static string QueryData(ElasticConnection connection)
        {
            Console.WriteLine("************************************************");
            Console.WriteLine("What do you want to query? (type 'exit' to exit)");
            Console.WriteLine("************************************************");

            var queryExecutor = new QueryExecutor(connection, "prisma");
            return queryExecutor.Query(Console.ReadLine());
        }

        private static void PrintResult(string result)
        {
            Console.WriteLine("Result:");
            Console.WriteLine("*******");
            Console.WriteLine(result);
        }
    }
}
