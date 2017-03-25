System.register(["@angular/core", "./../model/task", "./../service/task.service", "./../service/auth.service", "./../service/resource.service"], function (exports_1, context_1) {
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
    var core_1, task_1, task_service_1, auth_service_1, resource_service_1, TaskComponent;
    return {
        setters: [
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (task_1_1) {
                task_1 = task_1_1;
            },
            function (task_service_1_1) {
                task_service_1 = task_service_1_1;
            },
            function (auth_service_1_1) {
                auth_service_1 = auth_service_1_1;
            },
            function (resource_service_1_1) {
                resource_service_1 = resource_service_1_1;
            }
        ],
        execute: function () {
            TaskComponent = class TaskComponent {
                constructor(taskService, authService, resourceService) {
                    this.taskService = taskService;
                    this.authService = authService;
                    this.resourceService = resourceService;
                    this.changed = false;
                    this.currentTask = null;
                    this.shadowCopy = null;
                    this.localTaskId = null;
                }
                ngDoCheck() {
                    if (!this.resourceService.isInitialized()) {
                        this.resourceService.init();
                    }
                    if (this.currentTaskId != this.localTaskId) {
                        this.localTaskId = this.currentTaskId;
                        if (this.currentTaskId == -1) {
                            this.currentTask = new task_1.Task();
                            this.shadowCopy = new task_1.Task();
                        }
                        else {
                            this.taskService.getTask(this.authService.user.id, this.authService.user.token, this.currentTaskId)
                                .subscribe(response => {
                                this.currentTask = response.json();
                                this.shadowCopy = response.json();
                            });
                        }
                    }
                    this.changed = JSON.stringify(this.currentTask) != JSON.stringify(this.shadowCopy);
                }
                saveTask() {
                    if (this.currentTaskId == -1) {
                        this.taskService.createTask(this.authService.user.id, this.authService.user.token, this.currentTask)
                            .subscribe(response => {
                            this.currentTask = response.json();
                            this.shadowCopy = response.json();
                            this.changed = false;
                            this.currentTaskId = this.currentTask.id;
                            this.localTaskId = this.currentTask.id;
                            this.taskList.push(this.currentTask);
                        });
                    }
                    else {
                        this.taskService.editTask(this.authService.user.id, this.authService.user.token, this.currentTask)
                            .subscribe(response => {
                            this.taskService.getTask(this.authService.user.id, this.authService.user.token, this.currentTaskId)
                                .subscribe(response => {
                                this.currentTask = response.json();
                                this.shadowCopy = response.json();
                            });
                        });
                    }
                }
                cancelChanges() {
                    if (this.currentTaskId == -1) {
                        this.currentTaskId = this.localTaskId = null;
                        this.currentTask = this.shadowCopy = null;
                    }
                    else {
                        this.currentTask = JSON.parse(JSON.stringify(this.shadowCopy));
                    }
                }
                setState(stateId) {
                    this.currentTask.stateId = stateId;
                }
                setPriority(priorityId) {
                    this.currentTask.priorityId = priorityId;
                }
            };
            __decorate([
                core_1.Input(),
                __metadata("design:type", Array)
            ], TaskComponent.prototype, "taskList", void 0);
            __decorate([
                core_1.Input(),
                __metadata("design:type", Number)
            ], TaskComponent.prototype, "currentTaskId", void 0);
            TaskComponent = __decorate([
                core_1.Component({
                    selector: "task",
                    templateUrl: './task.component.html'
                }),
                __metadata("design:paramtypes", [task_service_1.TaskService,
                    auth_service_1.AuthService,
                    resource_service_1.ResourceService])
            ], TaskComponent);
            exports_1("TaskComponent", TaskComponent);
        }
    };
});
