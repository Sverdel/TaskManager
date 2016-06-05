(function () {
    'use strict';

    var app = angular.module('taskApp', [
        'ngRoute',
        'ui.bootstrap'
    ]);

    app.value('backendServerUrl', 'http://localhost:8000/api');

})();