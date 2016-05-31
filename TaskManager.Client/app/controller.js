(function () {
    'use strict';

    angular
        .module('taskApp')
        .controller('taskController', ['$scope', '$http',
            function ($scope, $http) {
                $scope.listHeight = window.innerHeight;

                var baseAddress = "http://localhost:8000/api/";

                function httpSend(action, method, params) {
                    return $http({
                        url: baseAddress.concat(action),
                        method: method,
                        data: params
                        })
                };

                httpSend("users/1/tasks", "GET")
                        .success(function (data, status, headers, config) {
                            $scope.taskList = data;
                        });
                
                $scope.getTask = function (id) {
                    return httpSend("users/1/tasks/" + id, "GET")
                        .success(function (data, status, headers, config) {
                            $scope.currentTask = data;
                        });
                }

                //fill dictionaties
                httpSend("resources/states", "GET")
                        .success(function (data, status, headers, config) {
                            $scope.stateList = data;
                        });

                httpSend("resources/priorities", "GET")
                        .success(function (data, status, headers, config) {
                            $scope.priorityList = data;
                        });
            }]);
})();
