﻿function SignInViewModel(dreamReaderViewModel) {
    var self = this;

    this.email = ko.observable('').extend({ required: true, email: true });
    this.password = ko.observable('').extend({ required: true });
    this.rememberMe = ko.observable(true);

    this.validationMessage = ko.observable('');

    this.errors = ko.validation.group(self);

    this.showEmailValidation = ko.observable(false);
    this.showPasswordValidation = ko.observable(false);
    this.email.subscribe(function () {
        self.showEmailValidation(true);
        self.validationMessage('');
    });
    this.password.subscribe(function () {
        self.showPasswordValidation(true);
        self.validationMessage('');
    });

    this.signingIn = ko.observable(false);

    this.signIn = function () {
        self.email.valueHasMutated();
        self.password.valueHasMutated();

        if (self.errors().length === 0) {
            self.signingIn(true);
            dreamReaderViewModel.readonly(true);

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
                    $.get(window.getProfileUrl).done(function(response) {
                        if (response.result) {
                            var model = response.data;
                            dreamReaderViewModel.isAuthenticated(model.IsAuthenticated);
                            dreamReaderViewModel.profileImageUrl(model.ProfileImageUrl);

                            dreamReaderViewModel.reloadBooks().done(function(response) {
                                if (response.result) {
                                    $('#sign-in-modal').modal('hide');
                                    dreamReaderViewModel.readonly(false);
                                }
                            });
                        }
                    });
                } else {
                    self.signingIn(false);
                    dreamReaderViewModel.readonly(false);
                    self.validationMessage(response.message);
                }
            });
        }
    }
}