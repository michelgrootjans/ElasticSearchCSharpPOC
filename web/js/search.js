function notify(error_type, message){
  $('#notifications').attr('class', '')
                     .addClass(error_type)
                     .html(message)
};

function display(item){
  if(item._type == 'user') { return display_user(item._source); }
  if(item._type == 'tweet') { return display_tweet(item._source); }
}

function display_user(user){
  return "user: " + user.UserName + "(" + user.FirstName + " " + user.LastName + ")";
}

function display_tweet(tweet){
  return tweet.UserName + " tweeted: " + tweet.Text;
}

$(document).ready(function(){

  var client = new $.es.Client({
    hosts: ['192.168.1.104:9200']
    ,log: 'trace'
  });

  client.ping({
      requestTimeout: 1000,
      hello: "elasticsearch!"
    }, function(error){
      if (error){
        notify('error', 'elasticsearch cluster is down');
      } else {
        notify('success', 'elasticsearch cluster is up');
      }
    }
  );

$('#search').change(function(){
  $('#search_results').html('');
  client.search({ q: $(this).val()})
        .then(function(body){
          body.hits.hits.forEach(function(item){
            var div = $('<div/>');
            div.html(display(item));
            $('#search_results').append(div);
          });
        }, function(error){
          notify('error', error.message);
        })
  });

});


