using System;
using ElasticSearch.POC.ConsoleApp.Indexables;
using PlainElastic.Net;

namespace ElasticSearch.POC.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new ElasticConnection("localhost");
            var indexer = new Indexer(connection, "prisma");
            var queryExecutor = new QueryExecutor(connection, "prisma");

            indexer.Reset();
            indexer.InitializeWith(new ProjectMapper().Mapping());
            IndexVmswData(indexer);

            indexer.Flush();

//            while (true)
//            {
//                Console.WriteLine("************************************************");
//                Console.WriteLine("What do you want to query? (type 'exit' to exit)");
//                Console.WriteLine("************************************************");
//
//                queryExecutor.Query(Console.ReadLine());
//            }
        }

        private static void IndexDummyData(Indexer indexer)
        {
            indexer.Index(new User {FirstName = "Michel", LastName = "Grootjans"});
            indexer.Index(new Tweet {UserName = "michelgrootjans", Text = "Ik schrijf een elasticsearch POC"});

            indexer.Index(new User {FirstName = "Bill", LastName = "Gates"});
            indexer.Index(new Tweet {UserName = "billgates", Text = "Damn you steve, your phone is more succesful than mine"});

            indexer.Index(new User {FirstName = "Steve", LastName = "Jobs"});
            indexer.Index(new Tweet { UserName = "stevejobs", Text = "Famous last words: wait 'till you see the iPhone 7" });

            indexer.Index(new User {FirstName = "Barak", LastName = "Obama"});
            indexer.Index(new User {FirstName = "Michele", LastName = "Obama"});
        }

        private static void IndexVmswData(Indexer indexer)
        {
            var projecten = new DataAccessLayer().GetVmswProjecten();
            indexer.Index(projecten);
        }
    }
}
