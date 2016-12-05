(function () {
    'use strict';

    angular
        .module('taskApp')
        .service('authService', ['$http',
        function authService($http) {
            var baseAddress = null;
            var authKey = "auth";
            this.init = function (address) {
                baseAddress = address;
            };

            this.login = function (username, password) {
                var url = "/connect/token"; // JwtProvider's LoginPath
                var data = {
                    username: username,
                    password: password,
                    client_id: "TaskManager",
                    // required when signing up with username/password
                    grant_type: "password",
                    // space-separated list of scopes for which the token is issued
                    scope: "offline_access profile email"
                };
                return $http.post(baseAddress.concat(url), this.toUrlEncodedString(data), { headers: { "Content-Type": "application/x-www-form-urlencoded" } })
                            .success(response => {
                                //var auth = response.json();
                                //console.log("The following auth JSON object has been received:");
                                //console.log(auth);
                                
                                this.setAuth(response.auth);
                                return response.user;
                            });
            }
            this.logout = function () {
                this.setAuth(null);
                return false;
            }

            // Converts a Json object to urlencoded format
            this.toUrlEncodedString = function (data) {
                var body = "";
                for (var key in data) {
                    if (body.length) {
                        body += "&";
                    }
                    body += key + "=";
                    body += encodeURIComponent(data[key]);
                }
                return body;
            }
            // Persist auth into localStorage or removes it if a NULL argument is given
            this.setAuth = function (auth) {
                if (auth) {
                    localStorage.setItem(authKey, JSON.stringify(auth));
                }
                else {
                    localStorage.removeItem(authKey);
                }
                return true;
            }

            // Retrieves the auth JSON object (or NULL if none)
            this.getAuth = function () {
                var i = localStorage.getItem(authKey);
                if (i) {
                    return JSON.parse(i);
                }
                else {
                    return null;
                }
            }

            // Returns TRUE if the user is logged in, FALSE otherwise.
            this.isLoggedIn = function () {
                return localStorage.getItem(authKey) != null;
            }
        }]);
})();