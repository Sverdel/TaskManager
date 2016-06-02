(function () {
    'use strict';

    angular
        .module('taskApp')
        .controller('taskController', ['$scope', '$http', 'httpService', 'usersApi', 'resourceApi', 'tasksApi',
            function ($scope, $http, httpService, usersApi, resourceApi, tasksApi) {
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

                    return tasksApi.getAllTasks(baseAddress, $scope.userId)
                        .success(function (data, status, headers, config) {
                            $scope.taskList = data;
                        });
                }
                
                $scope.getTask = function (id) {
                    return tasksApi.getTask(baseAddress, $scope.userId, id)
                        .success(function (data, status, headers, config) {
                            $scope.currentTask = data;
                        });
                }

                $scope.getUser = function () {
                    return usersApi.login(baseAddress, $scope.userName, $scope.password)
                        .success(function (data, status, headers, config) {
                            $scope.userId = data.Id;
                            $scope.getTasks();
                        })
                        .error(function (data, status, headers, config) {
                            alert("Incorrect user name or password");
                        });

                }

                $scope.logout = function () {
                    $scope.userId = null;
                    $scope.userName = null;
                    $scope.password = null;
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

                $scope.createTask = function (id) {
                    return;
                };

                $scope.removeTask = function (id) {
                    return;
                };

                //fill dictionaties
                resourceApi.getStates(baseAddress)
                        .success(function (data) {
                            $scope.stateList = data;
                        });

                resourceApi.getPriorities(baseAddress)
                        .success(function (data) {
                            $scope.priorityList = data;
                        });

                //////test
                $scope.userId = 1;
                $scope.userName = 'test user';
                $scope.password = null;
                var url = "users/" + $scope.userId + "/tasks/all";
                return $http({url: baseAddress.concat(url), method: "GET"})
                    .success(function (data, status, headers, config) {
                        $scope.taskList = data;
                    });
                //////test
            }]);
})();
