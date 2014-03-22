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
            var div = $('<ul/>');
            div.html(display(item));
            $('#search_results').append(div);
          });
        }, function(error){
          notify('error', error.message);
        })
  });

});


