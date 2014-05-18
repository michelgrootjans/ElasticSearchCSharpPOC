// *******
// results
// *******
function display_result(item){
  var result = $('<div/>').addClass('search-result');
  result.append(display_summary(item));
  result.append(display_highlights(item));
  return result;
}

function display_summary(item){
  var transform = { 'tag': 'div', 'class': 'summary' };
  var score = item._score;
  if(item._type == 'user') { transform.html = '${FirstName} ${LastName} (${UserName}) - score: ' + score }
  if(item._type == 'status') { transform.html = '<b>${user.name}</b> tweeted:' }
  if(item._type == 'page') { transform.html = '${title}' }
  if(item._type == 'project') { 
    transform.html = "${identificatie}: '${omschrijving}' -  score: " + score
  }
  return json2html.transform(item._source, transform)
}

function display_highlights(item){
  try{
    var result = $('<div/>').addClass('highlights');
    var highlights = item.highlight
  for(var highlight_name in highlights){
    if(highlights.hasOwnProperty(highlight_name)){
      result.append(display_highlight(highlight_name, highlights[highlight_name]));
    }
  }
    return result;
  }
  catch(e){
    return "";
  }
}
function display_highlight(highlight_name, highlight)
{
  var result = $('<div/>').addClass('highlighted').addClass(highlight_name);
  highlight.forEach(function(h){
    result.append(
      $('<div/>').addClass('highlight')
                 .append('...')
                 .append(h)
                 .append('...')
    );
  });
  return result;
}

// ******
// paging
// ******

function display_paging(total_number_of_records, current_page_number, per_page){
  // not the cleanest code, but good enough for a demo
  var number_of_pages = Math.ceil(total_number_of_records / per_page);

  var first_page = current_page_number - 4;
  var last_page = current_page_number + 4;
  if(first_page < 0){
    first_page = 0;
  }
  if(last_page >= number_of_pages){
    last_page = number_of_pages-1;
  }

  if(first_page != 0)
    $('#paging').append($('<li/>').addClass('unavailable').html('&hellip;'));

  for(var page_number=first_page; page_number <= last_page; page_number++){
    var li = $('<li/>')
    var link = $("<a/>").addClass('page').html(page_number + 1).attr('href', url_with('page', page_number));

    if(page_number == current_page_number)
      li.addClass('current');

    $('#paging').append(li.append(link));
  }

  if(last_page != number_of_pages-1)
    $('#paging').append($('<li/>').html('&hellip;'));
}

// ******
// facets
// ******
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
  //var result = $('<div/>').addClass(facet_name).addClass('facet');
  result.push(
    $('<li/>').addClass('facet')
              .addClass(facet_name)
              .addClass('heading')
              .html(facet_name)
  );
  facet.terms.forEach(function(f){
    result.push(display_facet_item(f, facet_name));
  });

  result.push($('<li/>').addClass('divider'));

  return result;
}

function display_facet_item(facet, type){
  var item = $('<li/>');
  var transform = { 'tag': 'a', 'html': '${term} (${count})'};
  var link = $(json2html.transform(facet, transform)).attr('href', url_with(type, escapeHtml(facet.term)));
  if(facet.term == $.querystring(type))
    item.addClass('active');
  return item.addClass('facet-value')
             .addClass(type)
             .append(link);
}
