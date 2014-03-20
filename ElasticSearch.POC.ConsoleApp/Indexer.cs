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
            var user = new JsonNetSerializer().ToJson(indexable);
            connection.Put(command, user);
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
    }
}