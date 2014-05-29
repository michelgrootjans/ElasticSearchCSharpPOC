function display_highlights(item){
  try{
    var result = $('<div/>').addClass('highlights');
    var highlights = item.highlight;

    for(var highlight_name in highlights){
      if(highlights.hasOwnProperty(highlight_name)){
        result.append(display_highlight(highlight_name, highlights[highlight_name]));
      }
    }
    
    return result;
  }
  catch(e){
    return '';
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
