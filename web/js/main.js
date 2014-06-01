$(document).ready(function(){
  prepare_search();
  
  var client = new $.es.Client({
    hosts: ['localhost:9200'],
    log: 'trace'
  });

  ping(client);
  execute_search(client);
});

function prepare_search(){
  $('#q').val($.querystring('q'));
  $('#q').focus();
  $('#search_button').click(function(){
    $('#search_form').submit();
  });
}
