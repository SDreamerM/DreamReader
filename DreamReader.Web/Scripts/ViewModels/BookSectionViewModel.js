function BookSectionViewModel(bookViewModel) {
    var self = this;

    this.id = ko.observable(0);
    this.rows = ko.observableArray([]);
}