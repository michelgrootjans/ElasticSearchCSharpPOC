$(document).ready(function(){

  var client = new $.es.Client({
    hosts: ['192.168.1.106:9200'],
//    log: 'trace'
  });

  ping(client);
  $('#search').change(function(){
    search(client, $(this).val());
  });

});

function search(client, q){
  var results = $('<ul/>');
  var facets  = $('<ul/>');

  $('#search_results').html('').append(results);
  $('#facets').html('').append(facets);

  client.search(
  {
    body: {
            query: {
              query_string: {
                query: q
              }
            },
            facets: {
              tags: {
                terms: {
                  field: '_type',
                  size: 100
                }
              }
            }
          }
  }).then(function(body) {
    body.hits.hits.forEach(function(item){
      results.append(display_result(item));
    });
    body.facets.tags.terms.forEach(function(facet){
      facets.append(display_facet(facet));
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

function display_result(item){
  var transform = { 'tag': 'li' };
  if(item._type == 'user') { transform.html = '${FirstName} ${LastName} (${UserName})'; }
  if(item._type == 'tweet') { transform.html = "${UserName} tweeted: '${Text}'" }
  return json2html.transform(item._source, transform)
}

function display_facet(facet){
  var transform = { 'tag': 'li', 'html': '${term} (${count})'};
  return json2html.transform(facet, transform)
}

