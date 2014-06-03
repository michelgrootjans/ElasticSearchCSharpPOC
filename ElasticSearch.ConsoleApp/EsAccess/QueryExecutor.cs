using System;
using PlainElastic.Net;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Utils;

namespace ElasticSearch.ConsoleApp.EsAccess
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

        public string Query(string queryString)
        {
            var query = new QueryBuilder<object>()
                .Query(q => q
                    .QueryString(qs => qs
                        .Fields("_all")
                        .Query(queryString)))
                .Facets(facets => facets
                    .Terms(t => t.FacetName("Projecttypes").Field("project_type"))
                    .Terms(t => t.FacetName("Statussen").Field("status"))
                    )
                .BuildBeautified();
            Console.WriteLine("Query:");
            Console.WriteLine("******");
            Console.WriteLine(query);
            Console.WriteLine();

            return connection.Post(Commands.Search(index_name), query).Result.BeautifyJson();
        }
    }
}