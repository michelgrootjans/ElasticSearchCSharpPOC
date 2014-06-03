using System;
using System.Linq;
using ElasticSearch.ConsoleApp.DbAccess;
using ElasticSearch.ConsoleApp.EsAccess;
using ElasticSearch.ConsoleApp.Models;
using PlainElastic.Net;

namespace ElasticSearch.ConsoleApp
{
    public static class Program
    {
        static void Main()
        {
            var connection = new ElasticConnection("localhost");

            IndexData(connection);
            while (true)
            {
                var result = QueryData(connection);
                PrintResult(result);
            }
        }

        private static void IndexData(ElasticConnection connection)
        {
            var indexer = new Indexer(connection, "projects")
                                .Reset()
                                .InitializeWith(new ProjectMapper());
            var projecten = new DataAccessLayer().GetProjecten();
            var start = DateTime.Now;
            indexer.Index(new Tweet { UserName = "michelgrootjans", Text = "Learning about elasticsearch" })
                   .Index(new User { FirstName = "Michel", LastName = "Grootjans" })
                   .Index(projecten)
                   .Flush();
            Console.WriteLine("Done indexing {0} records - took {1}ms", projecten.Count(), (DateTime.Now - start).TotalMilliseconds);
        }

        private static string QueryData(ElasticConnection connection)
        {
            Console.WriteLine("**************************");
            Console.WriteLine("What do you want to query?");
            Console.WriteLine("**************************");

            var queryExecutor = new QueryExecutor(connection, "projects");
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
