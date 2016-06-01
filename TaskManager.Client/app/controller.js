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

                $scope.getTasks = function () {
                    if ($scope.userId == null) { return; }

                    var url = "users/" + $scope.userId + "/tasks";
                    return httpSend(url, "GET")
                        .success(function (data, status, headers, config) {
                            $scope.taskList = data;
                        });
                }
                
                $scope.getTask = function (id) {
                    return httpSend("users/" + $scope.userId + "/tasks/" + id, "GET")
                        .success(function (data, status, headers, config) {
                            $scope.currentTask = data;
                        });
                }

                $scope.getUser = function () {
                    return httpSend("users/" + $scope.userName + "/" + $scope.password, "GET")
                        .success(function (data, status, headers, config) {
                            $scope.userId = data.Id;
                            $scope.getTasks();
                        });
                }

                $scope.setState = function (id) {
                    if ($scope.currentTask != null) {
                        $scope.currentTask.StateId = id;
                    }
                };

                $scope.setPriority = function (id) {
                    if ($scope.currentTask != null) {
                        $scope.currentTask.PriorityId = id;
                    }
                };

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
