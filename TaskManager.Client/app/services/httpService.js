(function () {
    'use strict';

    angular
        .module('taskApp')
        .service('httpService', ["$http",

    function ($http) {
        var baseAddress = null;

        this.init = function (address) {
            baseAddress = address;
        };

        this.send = function(action, method, params) {
            return $http({
                url: baseAddress.concat(action),
                method: method,
                data: params
            })
        };
    }])
})();