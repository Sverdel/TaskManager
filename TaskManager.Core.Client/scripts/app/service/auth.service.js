System.register(["@angular/core", "@angular/http", "rxjs/Observable", "./../model/user"], function (exports_1, context_1) {
    "use strict";
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var __moduleName = context_1 && context_1.id;
    var core_1, http_1, Observable_1, user_1, AuthService;
    return {
        setters: [
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (Observable_1_1) {
                Observable_1 = Observable_1_1;
            },
            function (user_1_1) {
                user_1 = user_1_1;
            }
        ],
        execute: function () {
            AuthService = class AuthService {
                constructor(http) {
                    this.http = http;
                    this.authKey = "auth";
                    this.user = new user_1.User();
                    this.user = this.getUser();
                }
                login(username, password) {
                    var url = "http://localhost:8000/api/connect/token"; // JwtProvider's LoginPath
                    var data = {
                        username: username,
                        password: password,
                        client_id: "TaskManager",
                        // required when signing up with username/password
                        grant_type: "password",
                        // space-separated list of scopes for which the token is issued
                        scope: "offline_access profile email"
                    };
                    return this.http.post(url, this.toUrlEncodedString(data), new http_1.RequestOptions({
                        headers: new http_1.Headers({
                            "Content-Type": "application/x-www-form-urlencoded"
                        })
                    }))
                        .map(response => {
                        this.setUser(response.json());
                        return true;
                    });
                }
                logout() {
                    return this.http.post("http://localhost:8000/api/users/logout", null)
                        .map(response => {
                        this.setUser(null);
                        return true;
                    })
                        .catch(err => {
                        return Observable_1.Observable.throw(err);
                    });
                }
                // Converts a Json object to urlencoded format
                toUrlEncodedString(data) {
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
                // Persist auth into localStorage or removes it if a NULL argument is  given
                setUser(user) {
                    if (user) {
                        localStorage.setItem(this.authKey, JSON.stringify(user));
                    }
                    else {
                        localStorage.removeItem(this.authKey);
                    }
                    this.user = user;
                    return true;
                }
                // Retrieves the auth JSON object (or NULL if none)
                getUser() {
                    var i = localStorage.getItem(this.authKey);
                    if (i) {
                        return JSON.parse(i);
                    }
                    else {
                        return null;
                    }
                }
                // Returns TRUE if the user is logged in, FALSE otherwise.
                isLoggedIn() {
                    if (localStorage.getItem(this.authKey) == null)
                        return false;
                    var auth = JSON.parse(localStorage.getItem(this.authKey));
                    var authDate = new Date(auth.tokenExpireDate);
                    var current = new Date();
                    if (authDate < current) {
                        localStorage.removeItem(this.authKey);
                        return false;
                    }
                    return true;
                }
                //get() {
                //    return this.http.get("api/Accounts")
                //               .map(response => response.json());
                //}
                add(user) {
                    return this.http.post("http://localhost:8000/api/users", JSON.stringify(user), new http_1.RequestOptions({
                        headers: new http_1.Headers({ "Content-Type": "application/json" })
                    }))
                        .map(response => response.json());
                }
            };
            AuthService = __decorate([
                core_1.Injectable(),
                __metadata("design:paramtypes", [http_1.Http])
            ], AuthService);
            exports_1("AuthService", AuthService);
        }
    };
});
