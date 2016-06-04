(function () {
    'use strict';

    angular
        .module('taskApp')
        .controller('taskController', ['$scope', '$http', 'httpService', 'userService', 'resourceService', 'taskService',
            function ($scope, $http, httpService, userService, resourceService, taskService) {
                $scope.listHeight = window.innerHeight;

                var baseAddress = "http://localhost:8000/api/";
                httpService.init(baseAddress);

               
                $scope.getTasks = function () {
                    if ($scope.user.Id == null) { return; }

                    return taskService.getAllTasks($scope.user.Id)
                        .success(function (data, status, headers, config) {
                            $scope.taskList = data;
                        });
                }
                
                $scope.getTask = function (id) {
                    return taskService.getTask($scope.user.Id, id)
                        .success(function (data, status, headers, config) {
                            $scope.currentTask = data;
                            $scope.shadowCopy = angular.copy(data);
                        });
                }

                $scope.getUser = function () {
                    $scope.alertMessage = null;
                    if ($scope.user.Name == null || $scope.user.Password == null) {
                        return;
                    }
                    return userService.login($scope.user.Name, $scope.user.Password)
                        .success(function (data, status, headers, config) {
                            $scope.user = data;
                            $scope.getTasks();
                        })
                        .error(function (data, status, headers, config) {
                            $scope.alertMessage = "Incorrect user name or password";
                        });

                }

                $scope.logout = function () {
                    $scope.user = null;
                    $scope.currentTask = null;
                    $scope.shadowCopy = null;
                    $scope.taskList = null;
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

                $scope.createTask = function () {
                    $scope.currentTask = {
                        PriorityId: 0, StateId: 0, UserId: $scope.user.Id
                    };
                };

                $scope.saveTask = function () {
                    if ($scope.currentTask.CreateDateTime == null) {
                        taskService.createTask($scope.user.Id, $scope.currentTask)
                            .success(function(data, status, headers, config) {
                                $scope.currentTask = data;
                                $scope.taskList.push({ Id: data.Id, Name: data.Name});
                            });
                    }
                    else {
                        taskService.editTask($scope.user.Id, $scope.currentTask);
                    }

                    $scope.shadowCopy = angular.copy($scope.currentTask);
                    $scope.changed = false;
                };

                $scope.removeTask = function () {
                    taskService.deleteTask($scope.user.Id, $scope.currentTask.Id)
                    .success(function (data) {
                        $scope.taskList = $scope.taskList.filter(function (e) { return e.Id !== data.Id });
                        $scope.currentTask = null;
                        $scope.shadowCopy = null;
                    });
                };

                $scope.cancelChanges = function () {
                    $scope.currentTask = angular.copy($scope.shadowCopy);
                };

                $scope.$watch('currentTask', function () {
                    if ($scope.currentTask == null) { return }

                    $scope.changed = JSON.stringify($scope.currentTask) != JSON.stringify($scope.shadowCopy);

                }, true);

                

                //fill dictionaties
                resourceService.getStates()
                        .success(function (data) {
                            $scope.stateList = data;
                        });

                $scope.priorityList = resourceService.getPriorities()
                        .success(function (data) {
                            $scope.priorityList = data;
                        });

                //////test
                $scope.user = { Id: 1, Name: 'test user', Password: null }
                var url = "users/" + $scope.user.Id + "/tasks/all";
                return $http({url: baseAddress.concat(url), method: "GET"})
                    .success(function (data, status, headers, config) {
                        $scope.taskList = data;
                    });
                //////test
            }]);
})();
