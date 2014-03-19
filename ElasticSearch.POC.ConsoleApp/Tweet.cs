using PlainElastic.Net;
using PlainElastic.Net.Serialization;

namespace ElasticSearch.POC.ConsoleApp
{
    internal class Tweet
    {
        private static int count;
        public string _id { get; private set; }
        public string UserName { get; set; }
        public string Text { get; set; }

        public Tweet()
        {
            _id = (++count).ToString();
        }
    }

    internal class TweetIndexer
    {
        private readonly ElasticConnection connection;

        public TweetIndexer(ElasticConnection connection)
        {
            this.connection = connection;
        }

        public void Index(string tweeter, string text)
        {
            var tweet = new Tweet { UserName = tweeter, Text = text };
            var command = Commands.Index(index: "twitter", type: "tweet", id: tweet._id);
            var user = new JsonNetSerializer().ToJson(tweet);
            connection.Put(command, user);
        }
    }
}