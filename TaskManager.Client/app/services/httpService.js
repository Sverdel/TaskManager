(function () {
    'use strict';

    angular
        .module('taskApp')
        .service('httpService', ["$http",

    function ($http) {
        var baseAddress = null;
        var authKey = "auth";

        this.init = function (address) {
            baseAddress = address;
        };

        this.send = function (action, method, params, headers) {
            return $http({
                url: baseAddress.concat(action),
                method: method,
                data: params,
                headers: this.getAuthHeader()
            })
        };

        this.getAuthHeader = function() {
            var i = localStorage.getItem(authKey);
            if (i != null) {
                var auth = JSON.parse(i);

                if (auth.access_token != null) {

                    return { "Authorization": "Bearer " + auth.access_token };
                }
            }

            return null;
        }
    }])
})();