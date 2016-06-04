(function () {
    'use strict';

    angular
        .module('taskApp')
        .service('taskService', ['httpService',
        function (httpService) {

            this.getAllTasks = function (userId) {
                return httpService.send("/tasks/" + userId, "GET");
            }

            this.getTask = function (userId, taskId) {
                return httpService.send("/tasks/" + userId + "/" + taskId, "GET");
            }

            this.createTask = function (userId, task) {
                return httpService.send("/tasks/" + userId, "POST", task);
            }

            this.editTask = function (userId, task) {
                return httpService.send("/tasks/" + userId + "/" + task.Id, "PUT", task);
            }

            this.deleteTask = function (userId, taskId) {
                return httpService.send("/tasks/" + userId + "/" + taskId, "DELETE");
            }

        }]);
})();