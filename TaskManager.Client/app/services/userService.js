(function () {
    'use strict';

    angular
        .module('taskApp')
        .service('userService', ['httpService', 'authService',
        function (httpService, authService) {

            this.login = function (userName, password) {
                return authService.login(userName, password);
            }

            this.logout = function () {
                return authService.logout();
            }

            this.register = function (userName, password) {
                return httpService.send("/users/" + userName + "/" + password, "POST")
                .success(data => {
                    authService.login(userName, password);
                });
            }

            this.edit = function (user) {
                return httpService.send("/users/" + user.Id, "PUT", user );
            }

            this.remove = function (userId) {
                return httpService.send("/users/" + userId, "DELTE");
            }

        }]);
})();