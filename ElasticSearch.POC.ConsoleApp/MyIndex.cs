using System;
using PlainElastic.Net;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Serialization;

namespace ElasticSearch.POC.ConsoleApp
{
    internal class MyIndex
    {
        private readonly IElasticConnection connection;

        public MyIndex(IElasticConnection connection)
        {
            this.connection = connection;
        }

        public void Query(string queryString)
        {
            var query = new QueryBuilder<User>()
                .Query(q => q
                    .Bool(b => b
                        .Must(m => m
                            .QueryString(qs => qs
                                .Fields(user => user.FirstName, user => user.LastName, user => user.UserName)
                                .Query(queryString)
                            ))))
                .BuildBeautified();

            var serializer = new JsonNetSerializer();

            Console.WriteLine("Query: {0}", query);

            var result = connection.Post(Commands.Search("twitter"), query);
            var users = serializer.ToSearchResult<User>(result).Documents;

            foreach (var user in users)
                Console.WriteLine("Found [{0}]", user);
        }
    }
}