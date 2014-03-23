using System;
using System.Collections;
using System.Collections.Generic;
using ElasticSearch.POC.ConsoleApp.Indexables;
using PlainElastic.Net;

namespace ElasticSearch.POC.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new ElasticConnection("localhost");
            var indexer = new Indexer(connection, "twitter");
            var queryExecutor = new QueryExecutor(connection, "twitter");

            InsertData(indexer);
            InsertVmswData(new Indexer(connection, "vmsw"));
            while (true)
            {
                Console.WriteLine("************************************************");
                Console.WriteLine("What do you want to query? (type 'exit' to exit)");
                Console.WriteLine("************************************************");

                queryExecutor.Query(Console.ReadLine());
            }
        }

        private static void InsertVmswData(Indexer indexer)
        {
            indexer.Reset();
            var projecten = GetVmswProjecten();
            indexer.Index(projecten);
        }

        private static IEnumerable<Project> GetVmswProjecten()
        {
            return new List<Project>
            {
                new Project{_id = Guid.NewGuid().ToString()},
                new Project{_id = Guid.NewGuid().ToString()},
                new Project{_id = Guid.NewGuid().ToString()},
                new Project{_id = Guid.NewGuid().ToString()}
            };
        }

        private static void InsertData(Indexer indexer)
        {
            indexer.Reset();

            indexer.Index(new User {FirstName = "Michel", LastName = "Grootjans"});
            indexer.Index(new Tweet {UserName = "michelgrootjans", Text = "Ik schrijf een elasticsearch POC"});

            indexer.Index(new User {FirstName = "Bill", LastName = "Gates"});
            indexer.Index(new Tweet {UserName = "billgates", Text = "Damn you steve, your phone is more succesful than mine"});

            indexer.Index(new User {FirstName = "Steve", LastName = "Jobs"});
            indexer.Index(new Tweet { UserName = "stevejobs", Text = "Famous last words: wait 'till you see the iPhone 7" });

            indexer.Index(new User {FirstName = "Barak", LastName = "Obama"});
            indexer.Index(new User {FirstName = "Michele", LastName = "Obama"});

            indexer.Flush();
        }
    }

    internal class Project : IIndexable
    {
        public string _id { get; set; }
    }
}
