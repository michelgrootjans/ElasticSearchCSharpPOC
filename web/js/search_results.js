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
  if(item._type == 'book') { transform.html = '<b>${book.name}</b> by ${book.author.first_name} ${book.author.last_name}' }
  if(item._type == 'song') { transform.html = '<b>${song.name}</b> by ${song.artist} - Album: ${song.album}' }
  if(item._type == 'page') { transform.html = '${title}' }
  if(item._type == 'project') { 
    transform.html = "${identificatie}: '${omschrijving}' -  score: " + score
  }
  return json2html.transform(item._source, transform)
}


