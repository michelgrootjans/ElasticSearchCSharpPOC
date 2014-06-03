using System.Collections.Generic;
using ElasticSearch.ConsoleApp.Models;
using PlainElastic.Net;
using PlainElastic.Net.Serialization;

namespace ElasticSearch.ConsoleApp.EsAccess
{
    internal class Indexer
    {
        private readonly IElasticConnection connection;
        private readonly string indexName;
        private readonly JsonNetSerializer serializer;

        public Indexer(IElasticConnection connection, string indexName)
        {
            this.connection = connection;
            this.indexName = indexName;
            serializer = new JsonNetSerializer();
        }

        public Indexer Reset()
        {
            try
            {
                connection.Delete(Commands.Delete(index: indexName));
            }
            catch { }
            return this;
        }

        public Indexer Index(IIndexable indexable)
        {
            var command = Commands.Index(index: indexName, type: indexable.GetType().Name, id: indexable._id);
            var data = serializer.ToJson(indexable);
            connection.Put(command, data);
            return this;
        }

        public Indexer Index<T>(IEnumerable<T> indexables) where T : IIndexable
        {
            var command = Commands.Bulk(indexName, typeof (T).Name);
            var bulkJson =
                new BulkBuilder(serializer)
                   .BuildCollection(indexables,
                        (builder, data) => builder.Index(data: data, id: data._id)
            );
            connection.Put(command, bulkJson);
            return this;
        }

        public Indexer Flush()
        {
            var flushCommand = Commands.Flush(index: indexName);
            connection.Post(flushCommand);
            return this;
        }

        public Indexer InitializeWith(params ITypeMapper[] mappers)
        {
            connection.Put(new IndexCommand(index: indexName));
            foreach (var mapper in mappers)
                connection.Put(new PutMappingCommand(index: indexName, type: mapper.TypeName), mapper.Mapping);
            return this;
        }
    }
}