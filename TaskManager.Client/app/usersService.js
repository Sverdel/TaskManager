(function () {
    'use strict';

    angular
        .module('taskApp')
        .service('usersApi', ['httpService',
        function (httpService) {

            this.login = function (baseAddress, userName, password) {
                return httpService.send(baseAddress, "users/" + userName + "/" + password, "GET");
            }

            this.register = function (baseAddress, user) {
                return httpService.send(baseAddress, "users", "POST", { user: user });
            }

            this.edit = function (baseAddress, user) {
                return httpService.send(baseAddress, "users/" + user.Id, "PUT", { user: user });
            }

            this.remove = function (baseAddress, userId) {
                return httpService.send(baseAddress, "users/" + userId, "DELTE");
            }

        }]);
})();