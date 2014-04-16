$(document).ready(function(){

  var client = new $.es.Client({
    hosts: ['localhost:9200'],
    log: 'trace'
  });

  ping(client);
  execute_search(client);
});

function ping(client){
  client.ping({ requestTimeout: 2000 }, function(error){
    if (error){
      notify('error', 'elasticsearch cluster is down');
    } else {
      notify('success', 'elasticsearch cluster is up');
    }
  });
}

function execute_search(client){
  var q =  escapeHtml($.querystring('q'));
  var perPage = $.querystring('per_page') || 10;
  var pageNum = $.querystring('page') || 0;

  var results = $('<div/>').addClass("search-results");
  var facets_element  = $('<div/>');
  var search_resluts_title = $('<h3/>').append('Search Results');

  $('#q').val($.querystring('q'));
  $('#search_results').html(search_resluts_title).append(results);
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
            // filter: { and: getFilter() },
            // facets: {
              // project_type: { 
                // terms: { field: 'project_type', all_terms: true, order: 'count' }
              // },
              // programmatie: { 
                // terms: { field: 'programmatie', order: 'count' }
              // },
              // status: {
                // terms: { field: 'status', order: 'count' }
              // }
            // },
            // highlight: {
              // fields: {"omschrijving": {}}
            // }
          }
  })
  .then(function(body) {
    console.log(body);
    search_resluts_title.append(' (' + body.hits.total + ')')
    body.hits.hits.forEach(function(item){
      results.append(display_result(item));
    });
    // $('#facets').html('').append(display_facets(body.facets));

    // display_paging(body.hits.total, pageNum, perPage);
  }, function(error) {
    notify('error', error.message);
  });
}

function getFilter(){
  //this is the place where selected facets will be filtered from the hits
  var result = [];
  getFacetFilter(result, 'project_type');
  getFacetFilter(result, 'programmatie');
  getFacetFilter(result, 'status');
  if (result.length == 0)
    result.push({});
  return result;
}

//unused method
function getFacetFilter(result, facet_field){
  var value = $.querystring(facet_field);
  if(value == null) return;

  var filter = { term: {}};
  filter.term[facet_field] = value
  result.push(filter);
}

function display_paging(total_number_of_records, page_num, per_page){
  var number_of_pages = Math.ceil(total_number_of_records / per_page);
  console.log("page " + page_num + "/" + number_of_pages + "(" + total_number_of_records + " records)");

  for(var page_number=0; page_number < number_of_pages; page_number++){
    var link = $("<a/>").addClass('page').html(page_number).attr('href', url_with('page', page_number));

    if(page_number == $.querystring('page'))
      link.addClass('current');

    $('#paging').append(link);
  }
}

function url_with(param, value){
  var current_location = window.location.href;
  return updateQueryStringParameter(current_location, param, value);
}

function notify(error_type, message){
  $('#notifications').attr('class', '')
                     .addClass(error_type)
                     .html(message)
};



function display_result(item){
  var transform = { 'tag': 'div', 'class': 'search-result' };
  var score = item._score;
  if(item._type == 'user') { transform.html = '${FirstName} ${LastName} (${UserName}) - score: ' + score }
  if(item._type == 'tweet') { transform.html = "${UserName} tweeted: '${Text}' -  score: " + score }
  if(item._type == 'project') { 
    transform.html = "${identificatie}: '${omschrijving}' -  score: " + score + '<div class="highlight">' + getHighlights(item) + '</div>' 
  }
  return json2html.transform(item._source, transform)
}

function getHighlights(item){
  try{
    return item.highlight.omschrijving[0];
  }
  catch(e){
    return "";
  }
}

function display_facets(facets){
  var result = $('<div/>');
  for(var facet_name in facets){
    if(facets.hasOwnProperty(facet_name)){
      result.append(display_facet(facet_name, facets[facet_name]));
    }
  }
  return result;
}

function display_facet(facet_name, facet)
{
  var result = $('<div/>').addClass('facet').addClass(facet_name);
  result.append($('<h3/>').html(facet_name));
  facet.terms.forEach(function(facet){
    result.append(display_facet_item(facet, facet_name));
  });
  return result;
}

function display_facet_item(facet, type){
  var result = $('<div/>');
  var transform = { 'tag': 'a', 'html': '${term} (${count})'};
  var link = $(json2html.transform(facet, transform)).attr('href', url_with(type, escapeHtml(facet.term)));
  if(facet.term == $.querystring(type))
    result.addClass('current');
  return result.addClass('facet-value').append(link);
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

function updateQueryStringParameter(uri, key, value) {
  var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
  var separator = uri.indexOf('?') !== -1 ? "&" : "?";
  if (uri.match(re)) {
    return uri.replace(re, '$1' + key + "=" + value + '$2');
  }
  else {
    return uri + separator + key + "=" + value;
  }
}
