﻿function SignInViewModel(DreamReaderViewModel) {
    var self = this;

    this.email = ko.observable('').extend({ required: true, email: true });
    this.password = ko.observable('').extend({ required: true });
    this.rememberMe = ko.observable(true);

    this.validationMessage = ko.observable('');

    this.errors = ko.validation.group(self);

    this.showValidation = ko.observable(false);
    this.email.subscribe(function () {
        self.showValidation(true);
        self.validationMessage('');
    });
    this.password.subscribe(function () {
        self.showValidation(true);
        self.validationMessage('');
    });

    this.signIn = function () {
        self.email.valueHasMutated();
        self.password.valueHasMutated();

        if (self.errors().length === 0) {
            $.ajax({
                url: window.signInUrl,
                type: 'POST',
                data: {
                    Email: self.email(),
                    Password: self.password(),
                    RememberMe: self.rememberMe(),
                    __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
                }
            }).done(function(response) {
                if (response.result) {
                    var model = response.data;
                    DreamReaderViewModel.isAuthenticated(true);
                } else {
                    self.validationMessage(response.message);
                }
            });
        }
    }
}