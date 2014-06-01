function execute_search(client){
  var q =  escapeHtml($.querystring('q'));
  if(q=="null") return;

  $('#search-results .title').html('Searching ...');

  var perPage = parseInt($.querystring('per_page') || 10);
  var pageNum = parseInt($.querystring('page') || 0);

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
            filter: { and: getFilter() },
            facets: {
              "project_type": { 
                terms: { field: 'project_type', all_terms: true, order: 'count' }
              },
              "gemeente": { 
                terms: { field: 'gemeente', order: 'count' }
              },
              "status": {
                terms: { field: 'status', order: 'count' }
              },
              "category": {
                terms: { field: 'category', order: 'count' }
              }
            },
            highlight: {
              fields: {
                "omschrijving": {},
                "text": {}
              }
            }
          }
  })
  .then(function(body) {
    console.log(body);
    $('#search-results .title').html('Search Results (' + body.hits.total + ')');
    body.hits.hits.forEach(function(item){
      $('#search-results').append(display_result(item));
    });
    $('#facets').append(display_facets(body.facets));

    display_paging(body.hits.total, pageNum, perPage);
  }, function(error) {
    notify('error', error.message);
  });
}
