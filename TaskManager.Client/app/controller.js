(function () {
    'use strict';

    angular
        .module('taskApp')
        .controller('taskController', ['$scope', '$http', 'httpService', 'userService', 'resourceService', 'taskService', 'hubService', 'backendServerUrl',
            function ($scope, $http, httpService, userService, resourceService, taskService, hubService, backendServerUrl) {
                $scope.listHeight = window.innerHeight;

                httpService.init(backendServerUrl);

                var taskHub = hubService(hubService.defaultServer, 'taskHub');

                taskHub.on('createTask', function (data) {
                    $scope.taskList.push({ id: data.id, name: data.name });
                });

                taskHub.on('deleteTask', function (data) {
                    $scope.taskList = $scope.taskList.filter(function (e) { return e.id !== data.id });

                    if ($scope.currentTask.id == data.id) { // alert
                        $scope.currentTask = null;
                        $scope.shadowCopy = null;
                    }
                });
                
                taskHub.on('editTask', function (data) {
                    if ($scope.currentTask.id == data.id) {// alert
                        $scope.currentTask = data;
                        $scope.shadowCopy = angular.copy(data);
                    }
                });
               
                $scope.getTasks = function () {
                    if ($scope.user.id == null) { return; }

                    return taskService.getAllTasks($scope.user.id)
                        .success(function (data, status, headers, config) {
                            $scope.taskList = data;
                        });
                }
                
                $scope.getTask = function (id) {
                    return taskService.getTask($scope.user.id, id)
                        .success(function (data, status, headers, config) {
                            $scope.currentTask = data;
                            $scope.shadowCopy = angular.copy(data);
                        });
                }

                $scope.getUser = function () {
                    $scope.alertMessage = null;
                    if ($scope.user.name == null || $scope.user.password == null) {
                        return;
                    }
                    return userService.login($scope.user.name, $scope.user.password)
                        .success(function (data, status, headers, config) {
                            $scope.user = data;
                            $scope.getTasks();
                            taskHub.invoke("userLogin", $scope.user);

                        })
                        .error(function (data, status, headers, config) {
                            $scope.alertMessage = "Incorrect user name or password";
                        });

                }

                $scope.logout = function () {
                    taskHub.invoke("userLogout", $scope.user);
                    $scope.user = null;
                    $scope.currentTask = null;
                    $scope.shadowCopy = null;
                    $scope.taskList = null;
                }


                $scope.setState = function (id) {
                    if ($scope.currentTask != null) {
                        $scope.currentTask.stateId = id;
                    }
                };

                $scope.setPriority = function (id) {
                    if ($scope.currentTask != null) {
                        $scope.currentTask.priorityId = id;
                    }
                };

                $scope.createTask = function () {
                    $scope.currentTask = {
                        priorityId: 0, stateId: 0, Userid: $scope.user.id
                    };
                };

                $scope.saveTask = function () {
                    if ($scope.currentTask.createDateTime == null) {
                        taskService.createTask($scope.user.id, $scope.currentTask)
                            .success(function(data, status, headers, config) {
                                $scope.currentTask = data;
                            });
                    }
                    else {
                        taskService.editTask($scope.user.id, $scope.currentTask);
                    }

                    $scope.shadowCopy = angular.copy($scope.currentTask);
                    $scope.changed = false;
                };

                $scope.removeTask = function () {
                    taskService.deleteTask($scope.user.id, $scope.currentTask.id);
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

                ////////test
                //$scope.user = { id: 1, name: 'test user', password: null }
                //var url = "/tasks/" + $scope.user.id;
                //return $http({ url: backendServerUrl.concat(url), method: "GET" })
                //    .success(function (data, status, headers, config) {
                //        $scope.taskList = data;
                //    });
                ////////test
            }]);
})();
