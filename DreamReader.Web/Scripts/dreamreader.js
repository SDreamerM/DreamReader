$(document).ready(function () {
    ko.validation.init({
        insertMessages: false
    });

    $.connection.hub.start({ waitForPageLoad: true, transport: 'serverSentEvents' });
    $.connection.bookHub.client.bookSectionRowProcessed = function (message, processedPercentage) {
        $('#book-section-row-processed-progress .progress-bar span').html(message);
        $('#book-section-row-processed-progress .progress-bar').css('width', processedPercentage + '%');
    };

    $.connection.bookHub.client.bookSectionProcessed = function (message, processedPercentage) {
        $('#book-section-processed-progress .progress-bar span').html(message);
        $('#book-section-processed-progress .progress-bar').css('width', processedPercentage + '%');
    };

    var dreamReaderViewModel = new DreamReaderViewModel();
    dreamReaderViewModel.init();
    ko.applyBindings(dreamReaderViewModel);

    $('#file').fileupload({
        url: window.bookUploadUrl,
        dataType: 'json',
        start: function() {
            dreamReaderViewModel.readonly(true);
        },
        change: function () {
            $('#upload-progress .progress-bar span').html('');
            $('#upload-progress .progress-bar').css('width', '0');
            dreamReaderViewModel.bookUploadViewModel().validationMessage('');
        },
        done: function (event, data) {
            var response = data.result;
            if (response.result) {
                dreamReaderViewModel.reloadBooks();
                dreamReaderViewModel.readonly(false);
                $('#book-upload-modal').modal('hide');
            } else {
                dreamReaderViewModel.bookUploadViewModel().validationMessage(response.message);
            }
        },
        progressall: function (event, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);

            if (progress === 100) {
                setTimeout(function() {
                    dreamReaderViewModel.bookUploadViewModel().uploaded(true);
                }, 750);
            }

            $('#upload-progress .progress-bar span').html(progress + '%');
            $('#upload-progress .progress-bar').css('width', progress + '%');
        }
    });

    $('#profile-image-file').fileupload({
        url: window.profileImageUploadUrl,
        dataType: 'json',
        done: function(event, data) {
            var response = data.result;
            if (response.result) {
                dreamReaderViewModel.profileViewModel().profileImageUrl(response.data);
            } else {
                dreamReaderViewModel.bookUploadViewModel().validationMessage(response.message);
            }
        }
    });

    $('#book-upload-modal').on('hidden.bs.modal', function () {
        $('#progress .progress-bar').css('width', '0');
        dreamReaderViewModel.bookUploadViewModel().validationMessage('');
    });
});