$(function () {
  var includes = $('[data-include]')
  $.each(includes, function () {
    var file = 'includes/' + $(this).data('include') + '.html'
    $(this).load(file)
  })
})


$('.first-option').hide();

$(function () {
  $('[data-toggle="tooltip"]').tooltip();
})