$('#searchForm').on('submit', function (e) {
    e.preventDefault();
    $('#loading-status').show();
    $('#errorResult').hide();
    $('#searchResult').hide();
    var nip = $('#nip').val();
    $.ajax({
        type: 'GET',
        url: '/Home/GetSearchResult?nip=' + nip,
        dataType: 'html',
        success: function (response) {
            $('#searchResult').show();
            $('#errorResult').hide();
            $('#searchResult').html(response);
        },
        error: function () {
            $('#searchResult').hide();
            $('#errorResult').show();
        },
        complete: function () {
            $('#loading-status').hide();
        }
    });
});