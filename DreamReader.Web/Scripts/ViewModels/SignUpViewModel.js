function SignUpViewModel(dreamReaderViewModel) {
    var self = this;

    this.email = ko.observable('').extend({ required: true, email: true });
    this.password = ko.observable('').extend({ required: true });
    this.confirmPassword = ko.observable('').extend({ required: true, equal: self.password });;

    this.validationMessage = ko.observable('');

    this.errors = ko.validation.group(self);

    this.showEmailValidation = ko.observable(false);
    this.showPasswordValidation = ko.observable(false);
    this.showConfirmPasswordValidation = ko.observable(false);
    this.email.subscribe(function () {
        self.showEmailValidation(true);
        self.validationMessage('');
    });
    this.password.subscribe(function () {
        self.showPasswordValidation(true);
        self.validationMessage('');
    });
    this.confirmPassword.subscribe(function () {
        self.showConfirmPasswordValidation(true);
        self.validationMessage('');
    });

    this.signingUp = ko.observable(false);

    this.signUp = function () {
        self.email.valueHasMutated();
        self.password.valueHasMutated();
        self.confirmPassword.valueHasMutated();

        if (self.errors().length === 0) {
            self.signingUp(true);
            dreamReaderViewModel.readonly(true);

            $.ajax({
                url: window.signUpUrl,
                type: 'POST',
                data: {
                    Email: self.email(),
                    Password: self.password(),
                    ConfirmPassword: self.confirmPassword(),
                    __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
                }
            }).done(function (response) {
                if (response.result) {
                    $('#sign-up-modal').modal('hide');
                    dreamReaderViewModel.reloadBooks();
                    dreamReaderViewModel.readonly(false);
                    dreamReaderViewModel.isAuthenticated(true);
                } else {
                    self.signingUp(false);
                    dreamReaderViewModel.readonly(false);
                    self.validationMessage(response.message);
                }
            });
        }
    }
}