using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using PlainElastic.Net;
using PlainElastic.Net.Serialization;

namespace ElasticSearch.POC.ConsoleApp
{
    internal class Indexer
    {
        private readonly IElasticConnection connection;
        private readonly string index_name;

        public Indexer(IElasticConnection connection, string indexName)
        {
            this.connection = connection;
            index_name = indexName;
        }

        public void Index(IIndexable indexable)
        {
            var command = Commands.Index(index: index_name, type: indexable.GetType().Name, id: indexable._id);
            var data = new JsonNetSerializer().ToJson(indexable);
            connection.Put(command, data);
        }

        public void Flush()
        {
            var flushCommand = Commands.Flush(index: index_name);
            connection.Post(flushCommand);
        }

        public void Reset()
        {
            try
            {
                var deleteCommand = Commands.Delete(index: index_name);
                connection.Delete(deleteCommand);
            }
            catch { }
        }

        public void Index<T>(IEnumerable<T> indexables) where T : IIndexable
        {
            var command = Commands.Bulk(index_name, typeof (T).Name);
            var serializer = new JsonNetSerializer();
            string bulkJson =
                new BulkBuilder(serializer)
                   .BuildCollection(indexables,
                        (builder, indexable) => builder.Index(data: indexable, id: indexable._id)
            );
            connection.Put(command, bulkJson);
        }
    }
}