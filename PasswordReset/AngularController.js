(function () {
    "use strict";
    var app = angular.module('app');

    app.controller('forgotPasswordController', forgotPasswordController);
    forgotPasswordController.$inject = ['forgotPasswordService', '$window'];

    function forgotPasswordController(forgotPasswordService, $window) {
        var vm = this;
        var searchParams = new URLSearchParams(location.search);
        vm.forgotPassword = _forgotPasswordForm;
        vm.forgotPasswordData = {};
        // grabbing query string from url and saving as property in object to pass later as part of payload in angular service
        vm.forgotPasswordData.token = searchParams.get('token');
        vm.$onInit = _onInit;

        function _onInit() {
            forgotPasswordService.checkToken(vm.forgotPasswordData.token).then(_checkTokenSuccess, _checkTokenError);
        }

        function _checkTokenSuccess(response) {
            vm.show = true;
            vm.forgotPasswordData.id = response.data;
        }

        function _checkTokenError() {
            $window.location.href = '/Content/landingpage.html';
        }

        function _forgotPasswordForm(validForm) {
            if (validForm && vm.forgotPasswordData.password === vm.forgotConfirmPassword) {
                forgotPasswordService.changePassword(vm.forgotPasswordData).then(_forgotPasswordSuccess, _forgotPasswordError);
            }
        }

        function _forgotPasswordSuccess() {
            // saving pending message to session tab so that on page redirect alert service will trigger a popup to show 
            // that password change happened successfully
            sessionStorage.pendingSuccessMessage = JSON.stringify({ 'msg': 'Update password successful' });
            $window.location.href = '/Content/index.html#!/home';
        }

        function _forgotPasswordError(error) {
            vm.forgotPasswordError = error;
        }
    }
})();

