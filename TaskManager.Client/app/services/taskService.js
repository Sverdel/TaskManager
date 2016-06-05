(function () {
    'use strict';

    angular
        .module('taskApp')
        .service('taskService', ['httpService',
        function (httpService) {

            this.getAllTasks = function (user) {
                return httpService.send("/tasks/" + user.id + "/" + user.token, "GET");
            }

            this.getTask = function (user, taskId) {
                return httpService.send("/tasks/" + user.id + "/" + user.token + "/" + taskId, "GET");
            }

            this.createTask = function (user, task) {
                return httpService.send("/tasks/" + user.id + "/" + user.token, "POST", task);
            }

            this.editTask = function (user, task) {
                return httpService.send("/tasks/" + user.id + "/" + user.token + "/" + task.id, "PUT", task);
            }

            this.deleteTask = function (user, taskId) {
                return httpService.send("/tasks/" + user.id + "/" + user.token + "/" + taskId, "DELETE");
            }

        }]);
})();