function ping(client){
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
}

function notify(error_type, message){
  $('#notifications').attr('class', '')
                     .addClass(error_type)
                     .html(message)
};

function display(item){
  var transform = { 'tag': 'li' };
  if(item._type == 'user') { transform.html = '${FirstName} ${LastName} (${UserName})'; }
  if(item._type == 'tweet') { transform.html = "${UserName} tweeted: '${Text}'" }
  return json2html.transform(item._source, transform)
}

$(document).ready(function(){

  var client = new $.es.Client({
    hosts: ['192.168.1.106:9200'],
    log: 'trace'
  });

  ping(client);
  $('#search').change(function(){
    $('#search_results').html('');
    //simple_search(client, $(this).val(), $('#search_results'));
    advanced_search(client, $(this).val(), $('#search_results'));
  });

});

function advanced_search(client, q, output_element){
  var list = $('<ul/>');
  output_element.append(list);

  client.search(
  {
    index: 'twitter',
    body: {
            query: {
              query_string: {
                query: q
              }
            }
          }
  }).then(function(body) {
    body.hits.hits.forEach(function(item){
      list.append(display(item));
    });
  }, function(error) {
    notify('error', error.message);
  });
}


function simple_search(client, query, output_element){
  var list = $('<ul/>');
  output_element.append(list);
  client.search({q: query})
        .then(function(body){
          body.hits.hits.forEach(function(item){
            list.append(display(item));
          });
        }, function(error){
          notify('error', error.message);
        });
}
