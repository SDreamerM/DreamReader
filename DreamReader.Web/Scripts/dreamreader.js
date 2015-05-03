$(document).ready(function () {
    ko.validation.init({
        insertMessages: false
    });

    var dreamReaderViewModel = new DreamReaderViewModel();
    dreamReaderViewModel.init();
    ko.applyBindings(dreamReaderViewModel);

    $('#file').fileupload({
        url: window.bookUploadUrl,
        dataType: 'json',
        change: function () {
            $('#progress .progress-bar').css('width', '0');
            dreamReaderViewModel.bookUploadViewModel().validationMessage('');
        },
        done: function (event, data) {
            var response = data.result;
            if (response.result) {
                dreamReaderViewModel.reloadBooks();
                $('#book-upload-modal').modal('hide');
            } else {
                dreamReaderViewModel.bookUploadViewModel().validationMessage(response.message);
            }
        },
        progressall: function (event, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#progress .progress-bar').css('width', progress + '%');
        }
    });

    $('#book-upload-modal').on('hidden.bs.modal', function () {
        $('#progress .progress-bar').css('width', '0');
        dreamReaderViewModel.bookUploadViewModel().validationMessage('');
    });
});