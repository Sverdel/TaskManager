(function () {
    'use strict';

    angular
        .module('taskApp')
        .factory('hubService', ['$rootScope', 'backendServerUrl', 
      function ($rootScope, backendServerUrl) {

          function backendFactory(serverUrl, hubName) {
              var connection = $.hubConnection(backendServerUrl);
              var proxy = connection.createHubProxy(hubName);

              connection.start().done(function () { });

              return {
                  on: function (eventName, callback) {
                      proxy.on(eventName, function (result) {
                          $rootScope.$apply(function () {
                              if (callback) {
                                  callback(result);
                              }
                          });
                      });
                  },
                  invoke: function (methodName, data, callback) {
                      proxy.invoke(methodName, data)
                      .done(function (result) {
                          $rootScope.$apply(function () {
                              if (callback) {
                                  callback(result);
                              }
                          });
                      });
                  }
              };
          };

          return backendFactory;
      }])
})();