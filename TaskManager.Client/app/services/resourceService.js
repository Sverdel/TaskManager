(function () {
    'use strict';

    angular
        .module('taskApp')
        .service('resourceApi', ['httpService',
        function (httpService) {

            this.getStates = function () {
                return httpService.send("resources/states", "GET");
            }

            this.getPriorities = function () {
                return httpService.send("resources/priorities", "GET");
            }

        }]);
})();