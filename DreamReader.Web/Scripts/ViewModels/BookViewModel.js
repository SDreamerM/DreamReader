function BookViewModel(dreamReaderViewModel) {
    var self = this;

    this.id = ko.observable(0);
    this.title = ko.observable('');
    this.annotation = ko.observable('');
    this.sections = ko.observableArray([]);

    this.read = function () {
        dreamReaderViewModel.loading(true);

        $.get(window.getBookUrl, { bookId: self.id() }).done(function(response) {
            if (response.result) {
                var model = response.data;
                var sections = model.Sections;
                for (var i = 0; i < sections.length; i++) {
                    var sectionViewModel = new BookSectionViewModel(self);
                    sectionViewModel.id(sections[i].Id);

                    var rows = sections[i].Rows;
                    for (var j = 0; j < rows.length; j++) {
                        var sectionRowViewModel = new BookSectionRowViewModel(sectionViewModel);
                        sectionRowViewModel.id(rows[j].Id);
                        sectionRowViewModel.content(rows[j].Content);
                        sectionViewModel.rows.push(sectionRowViewModel);
                    }

                    self.sections.push(sectionViewModel);
                }

                $('#book-modal').modal('show');
                dreamReaderViewModel.loading(false);
                dreamReaderViewModel.selectedBook(self);

            }
        });
    }

    this.close = function () {
        $('#book-modal').modal('hide');
        dreamReaderViewModel.selectedBook(null);
    }
}