using System;
using ElasticSearch.POC.ConsoleApp.Indexables;
using PlainElastic.Net;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Utils;

namespace ElasticSearch.POC.ConsoleApp
{
    internal class QueryExecutor
    {
        private readonly IElasticConnection connection;
        private readonly string index_name;

        public QueryExecutor(IElasticConnection connection, string indexName)
        {
            this.connection = connection;
            index_name = indexName;
        }

        public void Query(string queryString)
        {
            var query = new QueryBuilder<User>()
                .Query(q => q
                    .QueryString(qs => qs
                        .Fields("_all")
                        .Query(queryString)))
                .Facets(facets => facets
                    .Terms(t => t
                        .FacetName("TypeCount")
                        .Field("_type")
                        .Size(100)))
                .BuildBeautified();

            var result = connection.Post(Commands.Search(index_name), query);
            Console.WriteLine(result.Result.BeautifyJson());

        }
    }
}