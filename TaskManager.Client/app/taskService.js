(function () {
    'use strict';

    angular
        .module('taskApp')
        .service('tasksApi', ['httpService',
        function (httpService) {

            this.getAllTask = function (baseAddress, userId) {
                return httpService.send(baseAddress, "users/" + userId + "/tasks/all", "GET");
            }

            this.getTask = function (baseAddress, userId, taskId) {
                return httpService.send(baseAddress, "users/" + userId + "/tasks/" + taskId, "GET");
            }

            this.createTask = function (baseAddress, userId, task) {
                return httpService.send(baseAddress, "users/" + userId + "/tasks", "POST", { task: task });
            }

            this.editTask = function (baseAddress, userId, task) {
                return httpService.send(baseAddress, "users/" + userId + "/tasks/" + taskId, "PUT", { task: task });
            }

            this.deleteTask = function (baseAddress, userId, taskId) {
                return httpService.send(baseAddress, "users/" + userId + "/tasks/" + taskId, "DELTE");
            }

        }]);
})();