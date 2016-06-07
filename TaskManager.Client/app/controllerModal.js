(function () {
    'use strict';

    angular
       .module('taskApp')
       .controller('ModalInstanceCtrl', function ($scope, $uibModalInstance, message) {

        $scope.message = message;

        $scope.ok = function () {
            $uibModalInstance.close();
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    });
})();
