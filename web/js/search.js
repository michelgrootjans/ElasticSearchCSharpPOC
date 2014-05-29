function execute_search(client){
  var q =  escapeHtml($.querystring('q'));
  if(q=="null") return;
  var perPage = parseInt($.querystring('per_page') || 10);
  var pageNum = parseInt($.querystring('page') || 0);

  var results = $('<div/>').addClass("search-results");
  var search_resluts_title = $('<h3/>').append("Searching ...");

  $('#search_results').html(search_resluts_title).append(results);

  client.search(
  {
    size: perPage,
    from: (pageNum - 0) * perPage,
    body: {
            query: {
              query_string: {
                query: q
              }
            }
          }
  })
  .then(function(body) {
    console.log(body);
    search_resluts_title.html('Search Results (' + body.hits.total + ')');
    body.hits.hits.forEach(function(item){
      results.append(display_result(item));
    });
    $('#facets').append(display_facets(body.facets));

    display_paging(body.hits.total, pageNum, perPage);
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
  getFacetFilter(result, 'category');
  getFacetFilter(result, 'user.name');
  result.push({term: { redirect: false }});
  if (result.length == 0)
    result.push({});
  return result;
}

function getFacetFilter(result, facet_field){
  var value = $.querystring(facet_field);
  if(value == null) return;

  var filter = { term: {}};
  filter.term[facet_field] = value
  result.push(filter);
}







