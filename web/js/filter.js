function getFilter(){
  //this is the place where selected facets will be filtered from the hits
  var result = [];
  getFacetFilter(result, 'project_type');
  getFacetFilter(result, 'gemeente');
  getFacetFilter(result, 'status');
  getFacetFilter(result, 'category');
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
