(function () {
    'use strict';

    angular
        .module('taskApp')
        .service('tasksApi', ['httpService',
        function (httpService) {

            this.getAllTask = function (userId) {
                return httpService.send("users/" + userId + "/tasks/all", "GET");
            }

            this.getTask = function (userId, taskId) {
                return httpService.send("users/" + userId + "/tasks/" + taskId, "GET");
            }

            this.createTask = function (userId, task) {
                return httpService.send("users/" + userId + "/tasks", "POST", { task: task });
            }

            this.editTask = function (userId, task) {
                return httpService.send("users/" + userId + "/tasks/" + taskId, "PUT", { task: task });
            }

            this.deleteTask = function (userId, taskId) {
                return httpService.send("users/" + userId + "/tasks/" + taskId, "DELTE");
            }

        }]);
})();