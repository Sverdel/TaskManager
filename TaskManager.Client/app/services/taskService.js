(function () {
    'use strict';

    angular
        .module('taskApp')
        .service('taskService', ['httpService',
        function (httpService) {

            this.getAllTask = function (userId) {
                return httpService.send("users/" + userId + "/tasks/all", "GET");
            }

            this.getTask = function (userId, taskId) {
                return httpService.send("users/" + userId + "/tasks/" + taskId, "GET");
            }

            this.createTask = function (userId, task) {
                return httpService.send("users/" + userId + "/tasks", "POST", task );
            }

            this.editTask = function (userId, task) {
                return httpService.send("users/" + userId + "/tasks/" + task.Id, "PUT", task);
            }

            this.deleteTask = function (userId, taskId) {
                return httpService.send("users/" + userId + "/tasks/" + taskId, "DELETE");
            }

        }]);
})();