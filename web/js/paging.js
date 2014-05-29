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
