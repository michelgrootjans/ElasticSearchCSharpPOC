using System;
using JsonFx.Json;
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
                QueryData(connection);
            }
        }

        private static void QueryData(ElasticConnection connection)
        {
            Console.WriteLine("************************************************");
            Console.WriteLine("What do you want to query? (type 'exit' to exit)");
            Console.WriteLine("************************************************");

            var queryExecutor = new QueryExecutor(connection, "prisma");
            var result = queryExecutor.Query(Console.ReadLine());

            PrintResult(result);
            ParseResult(result);
        }

        private static void ParseResult(string result)
        {
            Console.WriteLine("Parsing:");
            Console.WriteLine("*******");
            var reader = new JsonReader();
            dynamic dynamicResult = reader.Read(result);
            Console.WriteLine("{0} results", dynamicResult.hits.total);
            foreach (var hit in dynamicResult.hits.hits)
            {
                Console.WriteLine("{0} - {1}", hit._source.Identificatie, hit._source.Omschrijving);
            }
        }

        private static void PrintResult(string result)
        {
            Console.WriteLine("Result:");
            Console.WriteLine("*******");
            Console.WriteLine(result);
        }

        private static void IndexVmswData(ElasticConnection connection)
        {
            var indexer = new Indexer(connection, "prisma");
            indexer.Reset();
            indexer.InitializeWith(new ProjectMapper());
            var projecten = new DataAccessLayer().GetVmswProjecten();
            indexer.Index(projecten);
            indexer.Flush();
        }
    }
}
