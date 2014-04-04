using System;
using JsonFx.Json;
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

        public string Query(string queryString)
        {
            var query = new QueryBuilder<object>()
                .Query(q => q
                    .QueryString(qs => qs
                        .Fields("_all")
                        .Query(queryString)))
                .Facets(facets => facets
                    .Terms(t => t.FacetName("Projecttypes").Field("ProjectType"))
                    .Terms(t => t.FacetName("Programmatie").Field("HuidigeProgrammatiefase"))
                    .Terms(t => t.FacetName("Status").Field("Status"))
                    )
                .BuildBeautified();
            Console.WriteLine("Query:");
            Console.WriteLine("******");
            Console.WriteLine(query);
            Console.WriteLine();

            var result = connection.Post(Commands.Search(index_name), query).Result.BeautifyJson();


            return result;
        }
    }
}