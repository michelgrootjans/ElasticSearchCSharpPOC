using System;
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
        public void Index(string tweeter, string text)
        {
            var connection = new ElasticConnection("localhost");
            var tweet = new Tweet { UserName = tweeter, Text = text };
            var command = Commands.Index(index: "twitter", type: "tweet", id: tweet._id);
            var user = new JsonNetSerializer().ToJson(tweet);
            var response = connection.Put(command, user);
            Console.WriteLine(response);
        }
    }
}