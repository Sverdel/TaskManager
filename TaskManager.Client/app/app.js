(function () {
    'use strict';

    var app = angular.module('taskApp', [
        'ngRoute',
        'ui.bootstrap'
    ]);


    // date parsing
    var regexIso8601 = /^(\d{4}|\+\d{6})(?:-(\d{2})(?:-(\d{2})(?:T(\d{2}):(\d{2}):(\d{2})\.(\d{1,})(Z|([\-+])(\d{2}):(\d{2}))?)?)?)?$/;

    function convertDateStringsToDates(input) {
        if (typeof input !== "object") return input;

        for (var key in input) {
            if (!input.hasOwnProperty(key)) continue;

            var value = input[key];
            var match;
            if (typeof value === "string" && (match = value.match(regexIso8601))) {
                var milliseconds = Date.parse(match[0])
                if (!isNaN(milliseconds)) {
                    input[key] = new Date(milliseconds);
                }
            } else if (typeof value === "object") {
                convertDateStringsToDates(value);
            }
        }
    }

    app.config(["$httpProvider", function ($httpProvider) {
        $httpProvider.defaults.transformResponse.push(function (responseData) {
            convertDateStringsToDates(responseData);
            return responseData;
        });
    }]);

    app.value('backendServerUrl', 'http://localhost:8000/api');

})();