(function () {
    'use strict';

    var app = angular.module('taskApp', [
        'ngRoute',
        'ui.bootstrap'
    ]);

    app.value('backendServerUrl', 'http://localhost:8000/api');


    app.controller('ModalInstanceCtrl', function ($scope, $uibModalInstance, message) {

        $scope.message = message;
        
        $scope.ok = function () {
            $uibModalInstance.close();
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    });
})();