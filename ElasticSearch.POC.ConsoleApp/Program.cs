using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
            IndexData(indexer);
            IndexVmswData(indexer);
            while (true)
            {
                Console.WriteLine("************************************************");
                Console.WriteLine("What do you want to query? (type 'exit' to exit)");
                Console.WriteLine("************************************************");

                queryExecutor.Query(Console.ReadLine());
            }
        }

        private static IEnumerable<Project> GetVmswProjecten()
        {
            using (var connection = CreatePrismaConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                                        select *
                                        from VMSW_PO_Projecten
                                        ";
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    var resultList = new List<Project>();
                    while (reader.Read())
                    {
                        var project = new Project
                        {
                            _id = reader.GetGuidValue("Id").ToString(),
                            Identificatie = reader.GetStringValue("identificatie"),
                            ProjectType = reader.GetStringValue("projecttype"),
                            Omschrijving = reader.GetStringValue("omschrijving"),
                            HuidigeProgrammatiefase = reader.GetIntValue("programmatiefase")
                        };
                        resultList.Add(project);
                    }
                    return resultList;
                }
            }
        }
        private static IDbConnection CreatePrismaConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["PrismaConnectionString"].ConnectionString;
            return new SqlConnection(connectionString);
        }


        private static void IndexVmswData(Indexer indexer)
        {
            var projecten = GetVmswProjecten();
            indexer.Index(projecten);
        }

        private static void IndexData(Indexer indexer)
        {
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
        public string Identificatie { get; set; }
        public string ProjectType { get; set; }
        public int HuidigeProgrammatiefase { get; set; }
        public string Omschrijving { get; set; }
    }

    public static class IDataReaderExtensions
    {
        public static Guid GetGuidValue(this IDataReader reader, string key)
        {
            var value = reader[key];
            if (value is DBNull) return Guid.Empty;
            return (Guid)value;
        }

//        public static short GetShortValue(this IDataReader reader, string key)
//        {
//            var value = reader[key];
//            if (value is DBNull) return 0;
//            return (short)value;
//        }

        public static int GetIntValue(this IDataReader reader, string key)
        {
            var value = reader[key];
            if (value is DBNull) return 0;
            return (int)value;
        }

//        public static string GetDateValue(this IDataReader reader, string key, string format = "dd/MM/yyyy")
//        {
//            var value = reader[key];
//            if (value is DBNull) return "";

//            return ((DateTime)value).ToString(format);
//        }

        public static string GetStringValue(this IDataReader reader, string key)
        {
            var value = reader[key];
            if (value is DBNull) return "";
            return (string)value;
        }

    }
}
