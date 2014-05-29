function display_result(item){
  var result = $('<div/>').addClass('search-result');
  result.append(display_summary(item));
  result.append(display_highlights(item));
  return result;
}

function display_summary(item){
  var transform = { 'tag': 'div', 'class': 'summary', 'html': get_transform(item) };
  return json2html.transform(item._source, transform)
}

function get_transform(item){
  switch(item._type){
    case 'book':
      return '<b>${book.name}</b> by ${book.author.first_name} ${book.author.last_name}';
    case 'song':
      return '<b>${song.name}</b> by ${song.artist} - Album: ${song.album}';  
    case 'status':
      return '<b>${user.name}</b> tweeted:';  
    case 'page':
      return '${title}';
    case 'project':
      return "${identificatie}: '${omschrijving}' -  score: " + item._score ;
    default:
      return "don't know how to display: " + item._type;  
  }
}


