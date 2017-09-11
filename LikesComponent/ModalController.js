(function () {
    "use strict";

    var app = angular.module('app');

    app.controller('modalController', modalController);

    modalController.$inject = ['$uibModal', '$scope', 'userService', 'alertService', '$window'];

    function modalController($uibModal, $scope, userService, alertService, $window) {

        var vm = this;
        vm.openModal = _openModal;
        vm.logout = _logout;

        function _openModal(val) {
            $scope.mode = val;

            $uibModal.open({
                templateUrl: "LoginRegistrationPasswordReset/LoginRegistrationPassword.html"
                , controller: "loginRegistrationController as vm"
                , scope: $scope
            }).result.catch(function (res) {});
        }

        function _logout() {
            userService.logout().then(logoutSuccess, logoutError);
            
            function logoutSuccess(response) {
                $window.location.href = '/htmlpage';
            }

            function logoutError() {
                alertService.error('Could not log out, please try again later', 'NETWORK ERROR OCCURED');
            }
        }
    }
})();