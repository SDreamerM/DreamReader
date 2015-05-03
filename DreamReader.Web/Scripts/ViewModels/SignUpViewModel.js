function SignUpViewModel() {
    var self = this;

    this.email = ko.observable('').extend({ required: true, email: true });
    this.password = ko.observable('').extend({ required: true });
    this.confirmPassword = ko.observable('').extend({ required: true, equal: self.password });;

    this.errors = ko.validation.group(self);

    this.showValidation = ko.observable(false);
    this.email.subscribe(function () {
        self.showValidation(true);
    });
    this.password.subscribe(function () {
        self.showValidation(true);
    });
    this.confirmPassword.subscribe(function () {
        self.showValidation(true);
    });

    this.signUp = function () {
        self.email.valueHasMutated();
        self.password.valueHasMutated();
        self.confirmPassword.valueHasMutated();

        if (self.errors().length === 0) {
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

            });
        }
    }
}