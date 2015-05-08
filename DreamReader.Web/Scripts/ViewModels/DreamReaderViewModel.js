function DreamReaderViewModel() {
    var self = this;

    this.loading = ko.observable(true);
    this.readonly = ko.observable(false);
    this.profileImageUrl = ko.observable('');
    this.isAuthenticated = ko.observable(false);

    this.selectedBook = ko.observable(null);
    this.books = ko.observableArray([]);

    this.signUpViewModel = ko.observable(new SignUpViewModel(self));
    this.signInViewModel = ko.observable(new SignInViewModel(self));
    this.profileViewModel = ko.observable(new ProfileViewModel(self));
    this.bookUploadViewModel = ko.observable(new BookUploadViewModel(self));

    this.init = function () {
        $.get(window.getProfileUrl).done(function (response) {
            if (response.result) {
                var model = response.data;
                self.isAuthenticated(model.IsAuthenticated);
                self.profileImageUrl(model.ProfileImageUrl);
                if (self.isAuthenticated()) {
                    self.reloadBooks().done(function (response) {
                        self.loading(false);
                    });
                } else {
                    self.loading(false);
                }
            }
        });
    }

    this.logout = function () {
        $.ajax({
            url: window.logOffUrl,
            type: 'POST',
            data: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
            }
        }).done(function (response) {
            if (response.result) {
                window.location.reload();
            }
        });
    }

    this.reloadBooks = function () {
        return $.get(window.getBooksUrl).done(function(response) {
            if (response.result) {
                var books = response.data;
                for (var i = 0; i < books.length; i++) {
                    var book = books[i];
                    var bookViewModel = new BookViewModel(self);
                    bookViewModel.id(book.Id);
                    bookViewModel.title(book.Title);
                    bookViewModel.annotation(book.Annotation);
                    self.books.push(bookViewModel);
                }
            }
        });
    }

    this.showProfile = function() {
        $.ajax({
            url: window.getProfileUrl,
            type: 'GET',
            data: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
            }
        }).done(function(response) {
            if (response.result) {
                var model = response.data;
                self.profileViewModel().profileImageUrl(model.ProfileImageUrl);
                $('#profile-modal').modal('show');
            }
        });
    }
}