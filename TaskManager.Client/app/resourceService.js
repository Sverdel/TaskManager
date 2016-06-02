(function () {
    'use strict';

    angular
        .module('taskApp')
        .service('resourceApi', ['httpService',
        function (httpService) {

            this.getStates = function (baseAddress) {
                return httpService.send(baseAddress, "resources/states", "GET");
            }

            this.getPriorities = function (baseAddress) {
                return httpService.send(baseAddress, "resources/priorities", "GET");
            }

        }]);
})();