(function () {
    'use strict';

    angular
        .module('taskApp')
        .service('userService', ['httpService',
        function (httpService) {

            this.login = function (userName, password) {
                return httpService.send("/users/" + userName + "/" + password, "GET");
            }

            this.register = function (user) {
                return httpService.send("/users", "POST", user );
            }

            this.edit = function (user) {
                return httpService.send("/users/" + user.Id, "PUT", user );
            }

            this.remove = function (userId) {
                return httpService.send("/users/" + userId, "DELTE");
            }

        }]);
})();