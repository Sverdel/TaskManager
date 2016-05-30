(function () {
    'use strict';

    angular
        .module('taskApp')
        .controller('taskController', ['$scope', '$http',
            function ($scope, $http) {
                var baseAddress = "http://localhost:800/api/";

                function httpPost(action, params) {
                    return $http.post(
                        baseAddress.concat(action),
                        params
                    );
                };

                $scope.getAllTask = function () {
                    return httpPost("users/1/tasks")
                        .success(function (data) {
                            $scope.taskList = JSON.parse(data);
                        })

                }
            }]);
})();
