function ping(client){
  client.ping({ requestTimeout: 2000 }, function(error){
    if (error){
      notify('warning', 'elasticsearch cluster is down');
    } else {
      // notify('success', 'elasticsearch cluster is up');
    }
  });
}

function url_with(param, value){
  var current_location = window.location.href;
  return updateQueryStringParameter(current_location, param, value);
}

function notify(error_type, message){
  $('#notifications').attr('class', '')
                     .addClass('alert-box')
                     .addClass(error_type)
                     .html(message)
};

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
