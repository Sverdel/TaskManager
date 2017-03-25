System.register(["@angular/core", "@angular/http", "./../service/auth.http"], function (exports_1, context_1) {
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
    var core_1, http_1, auth_http_1, TaskService;
    return {
        setters: [
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (auth_http_1_1) {
                auth_http_1 = auth_http_1_1;
            }
        ],
        execute: function () {
            TaskService = class TaskService {
                constructor(http) {
                    this.http = http;
                    this.baseUrl = "http://localhost:8000/api/tasks/";
                }
                getAllTasks(userId, token) {
                    return this.http.get(this.baseUrl + userId + "/" + token);
                }
                getTask(userId, token, taskId) {
                    return this.http.get(this.baseUrl + userId + "/" + token + "/" + taskId);
                }
                createTask(userId, token, task) {
                    let headers = new http_1.Headers({ 'Content-Type': 'application/json' });
                    let options = new http_1.RequestOptions({ headers: headers });
                    return this.http.post(this.baseUrl + userId + "/" + token, JSON.stringify(task), options);
                }
                editTask(userId, token, task) {
                    let headers = new http_1.Headers({ 'Content-Type': 'application/json' });
                    let options = new http_1.RequestOptions({ headers: headers });
                    return this.http.put(this.baseUrl + userId + "/" + token, JSON.stringify(task), options);
                }
                deleteTask(userId, token, taskId) {
                    return this.http.delete(this.baseUrl + userId + "/" + token + "/" + taskId);
                }
            };
            TaskService = __decorate([
                core_1.Injectable(),
                __metadata("design:paramtypes", [auth_http_1.AuthHttp])
            ], TaskService);
            exports_1("TaskService", TaskService);
        }
    };
});
