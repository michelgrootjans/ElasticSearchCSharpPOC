$(document).ready(function(){

  var client = new $.es.Client({
    hosts: ['192.168.1.109:9200'],
    log: 'trace'
  });

  ping(client);
  execute_search(client);

});

function execute_search(client){
  var q =  escapeHtml($.querystring('q'));
  var perPage = $.querystring('per_page') || 20;
  var pageNum = $.querystring('page') || 0;

  var results = $('<ul/>');
  var facets_element  = $('<ul/>');
  var search_resluts_title = $('<h3/>').append('Search Results');

  $('#q').val($.querystring('q'));
  $('#search_results').html(search_resluts_title).append(results);
  $('#facets').html('').append(facets_element);
  $('#paging').html('');

  client.search(
  {
    size: perPage,
    from: (pageNum - 0) * perPage,
    body: {
            query: {
              query_string: {
                query: q
              }
            },
            facets: {
              'projecttypes': { terms: { field: 'ProjectType' } },
              'programmatie': { terms: { field: 'Programmatie', order: "count" } },
              'status': { terms: { field: 'Status', order: "count" } },
            }
          }
  })
  .then(function(body) {
    console.log('displaying ' + body.hits.total + ' results');
    console.log(body);
    search_resluts_title.append(' (' + body.hits.total + ')')
    body.hits.hits.forEach(function(item){
      results.append(display_result(item));
    });
    facets_element.append($('<h3/>').html('Projecttypes'));
    body.facets.projecttypes.terms.forEach(function(facet){
      facets_element.append(display_facet(facet));
    });
    facets_element.append($('<h3/>').html('Programmatie'));
    body.facets.programmatie.terms.forEach(function(facet){
      facets_element.append(display_facet(facet));
    });
    facets_element.append($('<h3/>').html('Status'));
    body.facets.status.terms.forEach(function(facet){
      facets_element.append(display_facet(facet));
    });

    display_paging(body.hits.total, pageNum, perPage);
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
      requestTimeout: 2000,
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

function display_paging(total_number_of_records, page_num, per_page){
  var number_of_pages = Math.ceil(total_number_of_records / per_page);
  console.log("page " + page_num + "/" + number_of_pages + "(" + total_number_of_records + " records)");

  for(var page_number=0; page_number < number_of_pages; page_number++){
    var link = $("<a/>").addClass('page').html(page_number).attr('href', url_with('page', page_number));
    $('#paging').append(link);
  }
}

function url_with(param, value){
  var current_location = window.location;
  var current_href = current_location.href.replace(current_location.search, '');
  return window.location.href + "&" + param + "=" + value;
}

function notify(error_type, message){
  $('#notifications').attr('class', '')
                     .addClass(error_type)
                     .html(message)
};

function display_result(item){
  var transform = { 'tag': 'li' };
  var score = item._score;
  if(item._type == 'user') { transform.html = '${FirstName} ${LastName} (${UserName}) - score: ' + score; }
  if(item._type == 'tweet') { transform.html = "${UserName} tweeted: '${Text}' -  score: " + score }
  if(item._type == 'project') { transform.html = "${Identificatie}: '${Omschrijving}' -  score: " + score }
  return json2html.transform(item._source, transform)
}

function display_facet(facet){
  var transform = { 'tag': 'li', 'html': '${term} (${count})'};
  return json2html.transform(facet, transform)
}

var entityMap = {
  "&": "&amp;",
  "<": "&lt;",
  ">": "&gt;",
  '"': '&quot;',
  "'": '&#39;',
  "/": '&#x2F;'
};

function escapeHtml(string) {
  return String(string).replace(/[&<>"'\/]/g, function (s) {
    return entityMap[s];
  });
}
