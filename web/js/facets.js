function display_facets(facets){
  var result = $('<ul/>').addClass('side-nav');
  for(var facet_name in facets){
    if(facets.hasOwnProperty(facet_name)){
      result.append(display_facet(facet_name, facets[facet_name]));
    }
  }
  return result;
}

function display_facet(facet_name, facet)
{
  if(facet.terms.length == 0) return "";

  var result = new Array();

  result.push(display_facet_header(facet_name));
  facet.terms.forEach(function(f){
    result.push(display_facet_item(f, facet_name));
  });

  result.push($('<li/>').addClass('divider'));

  return result;
}

function display_facet_header(facet_name){
  var link = $('<a/>').attr('href', url_without(facet_name))
                      .html(facet_name);
  var item =  $('<li/>').addClass('facet')
                        .addClass(facet_name)
                        .addClass('heading')
                        .html(link);
  if($.querystring(facet_name) == null)
    item.addClass('current');

  return item;
}

function display_facet_item(facet, type){
  var item = $('<li/>');
  var transform = { 'tag': 'a', 'html': '${term} (${count})'};
  var link = $(json2html.transform(facet, transform)).attr('href', url_with(type, escapeHtml(facet.term)));
  if(facet.term == $.querystring(type))
    item.addClass('current');
  return item.addClass('facet-value')
             .addClass(type)
             .append(link);
}
