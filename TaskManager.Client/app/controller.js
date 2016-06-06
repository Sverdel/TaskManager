﻿(function () {
    'use strict';

    angular
        .module('taskApp')
        .controller('taskController', ['$scope', '$http', 'httpService', 'userService', 'resourceService', 'taskService', 'hubService', 'backendServerUrl', '$uibModal',
            function ($scope, $http, httpService, userService, resourceService, taskService, hubService, backendServerUrl, $uibModal) {
                $scope.listHeight = window.innerHeight;

                httpService.init(backendServerUrl);

                //inner functions
                function openDialog(message, isConfirm, yesCallback, noCallback) {

                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: isConfirm ? 'confirm.html' : 'message.html',
                        controller: 'ModalInstanceCtrl',
                        size: 'sm',
                        resolve: {
                            message: function () { return message; }
                        }
                    });

                    modalInstance.result.then(function () {
                        if (yesCallback) {
                            yesCallback()
                        }
                    },
                    function () {
                        if (noCallback) {
                            noCallback()
                        }
                    });
                };

                function getTaskInner(id) {
                    return taskService.getTask($scope.user, id)
                        .success(function (data, status, headers, config) {
                            $scope.currentTask = data;
                            $scope.shadowCopy = angular.copy(data);
                        });
                }

                // signalr hub events
                var taskHub = hubService(hubService.defaultServer, 'taskHub');

                taskHub.on('createTask', function (data) {
                    $scope.taskList.push({ id: data.id, name: data.name });
                });

                taskHub.on('deleteTask', function (data) {
                    $scope.taskList = $scope.taskList.filter(function (e) { return e.id !== data.id });

                    if ($scope.currentTask.id == data.id) { // alert
                        openDialog("The task has been removed on remote host", false, function () {
                            $scope.currentTask = null;
                            $scope.shadowCopy = null;
                        });
                    }
                });

                taskHub.on('editTask', function (data) {
                    if ($scope.currentTask.id == data.id) {
                        openDialog("The task has been updated on remote host", false, function () {
                            $scope.currentTask = data;
                            $scope.shadowCopy = angular.copy(data);
                        });
                    }

                    var task = $scope.taskList.filter(function (e) { return e.id == data.id })[0];
                    task.name = data.name;
                });

                // task functions
                $scope.getTaskList = function () {
                    if ($scope.user.id == null) { return; }

                    return taskService.getAllTasks($scope.user)
                        .success(function (data, status, headers, config) {
                            $scope.taskList = data;
                        });
                }

                $scope.getTask = function (id) {
                    if ($scope.currentTask != null && $scope.currentTask.id == id) {
                        return;
                    }

                    if ($scope.currentTask != null && $scope.changed == true) {
                        openDialog("You will lose all unsaved changes! Continue leave task?", true, function () {
                            getTaskInner(id);
                        })
                    }
                    else {
                        getTaskInner(id)
                    }
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
                    $scope.alertMessage = null;
                    $scope.currentTask = {
                        priorityId: 1, stateId: 1, userId: $scope.user.id
                    };
                };

                $scope.saveTask = function () {
                    $scope.alertMessage = null;
                    if ($scope.currentTask.createDateTime == null) {
                        taskService.createTask($scope.user, $scope.currentTask)
                            .success(function (data, status, headers, config) {
                                $scope.currentTask = data;
                                $scope.shadowCopy = angular.copy(data);
                                $scope.changed = false;
                                $scope.taskList.push({
                                    id: data.id, name: data.name
                                });
                            })
                            .error(function (data, status, headers, config) {
                                $scope.alertMessage = "Failed to create task: " + data.message;
                            });
                    }
                    else {
                        taskService.editTask($scope.user, $scope.currentTask)
                            .success(function (data, status, headers, config) {
                                $scope.shadowCopy = angular.copy($scope.currentTask);
                                $scope.changed = false;

                                var task = $scope.taskList.filter(function (e) { return e.id == $scope.currentTask.id })[0];
                                task.name = $scope.currentTask.name;
                            })
                            .error(function (data, status, headers, config) {
                                $scope.alertMessage = "Failed to edit task: " + data.message;
                            });
                    }
                };

                $scope.removeTask = function () {
                    $scope.alertMessage = null;
                    openDialog("Are you sure you want to delete this task?", true, function () {
                        taskService.deleteTask($scope.user, $scope.currentTask.id)
                            .success(function (data, status, headers, config) {
                                $scope.currentTask = null;
                                $scope.shadowCopy = null;
                                $scope.taskList = $scope.taskList.filter(function (e) { return e.id !== data.id });
                            })
                            .error(function (data, status, headers, config) {
                                $scope.alertMessage = "Failed to remove task: " + data.message;
                            });
                    });
                };

                $scope.cancelChanges = function () {
                    $scope.alertMessage = null;
                    openDialog("You will lose all unsaved changes! Confirm action?", true, function () {
                        $scope.currentTask = angular.copy($scope.shadowCopy);
                    });
                };

                $scope.$watch('currentTask', function () {
                    if ($scope.currentTask == null) { return }

                    $scope.changed = JSON.stringify($scope.currentTask) != JSON.stringify($scope.shadowCopy);

                }, true);

                // auth function
                $scope.login = function () {
                    $scope.alertMessage = null;
                    $scope.signUp = false;
                    if ($scope.user.name == null || $scope.user.password == null) {
                        return;
                    }
                    return userService.login($scope.user.name, $scope.user.password)
                        .success(function (data, status, headers, config) {
                            $scope.user = data;
                            $scope.getTaskList();
                            taskHub.invoke("userLogin", $scope.user);
                        })
                        .error(function (data, status, headers, config) {
                            $scope.alertMessage = "Login failed: " + data.message;
                        });
                }

                $scope.signup = function () {
                    $scope.alertMessage = null;
                    if ($scope.newUser.name == null || $scope.newUser.password == null) {
                        return;
                    }
                    return userService.register($scope.newUser.name, $scope.newUser.password)
                        .success(function (data, status, headers, config) {
                            $scope.user = data;
                            taskHub.invoke("userLogin", $scope.user);
                            $scope.signUp = false;
                            $scope.taskList = [];
                            $scope.newUser = {};
                        })
                        .error(function (data, status, headers, config) {
                            $scope.alertMessage = "Error while register user: " + data.message;
                        });
                }

                $scope.logout = function () {
                    taskHub.invoke("userLogout", $scope.user);
                    $scope.user = null;
                    $scope.currentTask = null;
                    $scope.shadowCopy = null;
                    $scope.taskList = [];
                }

                //fill dictionaties
                resourceService.getStates()
                        .success(function (data) {
                            $scope.stateList = data;
                        });

                resourceService.getPriorities()
                        .success(function (data) {
                            $scope.priorityList = data;
                        });
            }]);
})();
