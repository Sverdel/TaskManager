(function () {
    'use strict';

    angular
        .module('taskApp')
        .controller('taskController', ['$scope', '$http',
            function ($scope, $http) {
                var baseAddress = "http://localhost:8000/api/";

                function httpPost(action, params) {
                    return $http.get(
                        baseAddress.concat(action),
                        params
                    )
                };

                $scope.getAllTask = function () {
                    return $http.get(baseAddress.concat("users/1/tasks"))
                        .success(function(data, status, headers, config) {
                            $scope.taskList = data;
                        })
                }
            }]);
})();
