(function () {
    'use strict';

    angular
        .module('taskApp')
        .service('httpService', ["$http",

    function ($http) {
        this.send = function(baseAddress, action, method, params) {
            return $http({
                url: baseAddress.concat(action),
                method: method,
                data: params
            })
        };
    }])
})();