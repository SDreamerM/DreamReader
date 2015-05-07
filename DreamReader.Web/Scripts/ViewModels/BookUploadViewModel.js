function BookUploadViewModel() {
    var self = this;

    this.uploaded = ko.observable(false);
    this.validationMessage = ko.observable('');
}