using System;
using PlainElastic.Net;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Serialization;
using PlainElastic.Net.Utils;

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
                    .QueryString(qs => qs
                        .Fields(u => u.FirstName, u => u.LastName, u => u.UserName)
                        .Query(queryString)))
                .Facets(facets => facets
                    .Terms(t => t
                        .FacetName("TypeCount")
                        .Field("_type")
                        .Size(100)))
                .BuildBeautified();

            var serializer = new JsonNetSerializer();

            Console.WriteLine("Query: {0}", query);

            var result = connection.Post(Commands.Search("twitter"), query);
            var users = serializer.ToSearchResult<User>(result).Documents;

            Console.WriteLine(result.Result.BeautifyJson());

        }
    }
}