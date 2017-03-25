System.register(["@angular/core", "./../service/task.service", "./../service/auth.service", "./../service/resource.service"], function (exports_1, context_1) {
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
    var core_1, task_service_1, auth_service_1, resource_service_1, TaskListComponent;
    return {
        setters: [
            function (core_1_1) {
                core_1 = core_1_1;
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
            TaskListComponent = class TaskListComponent {
                constructor(taskService, authService, resourceService) {
                    this.taskService = taskService;
                    this.authService = authService;
                    this.resourceService = resourceService;
                    this.taskList = null;
                    this.currentTaskId = null;
                }
                ngOnInit() {
                    if (this.authService.user != null) {
                        this.taskService.getAllTasks(this.authService.user.id, this.authService.user.token)
                            .subscribe(response => {
                            this.taskList = response.json();
                        });
                    }
                }
                removeTask() {
                    this.taskService.deleteTask(this.authService.user.id, this.authService.user.token, this.currentTaskId)
                        .subscribe(response => {
                        this.taskList = this.taskList.filter(function (e) {
                            return e.id !== response.json().id;
                        });
                        this.currentTaskId = null;
                    });
                }
                getTask(id) {
                    this.currentTaskId = id;
                }
                createTask() {
                    this.currentTaskId = -1;
                }
            };
            TaskListComponent = __decorate([
                core_1.Component({
                    selector: "tasklist",
                    templateUrl: './tasklist.component.html'
                }),
                __metadata("design:paramtypes", [task_service_1.TaskService,
                    auth_service_1.AuthService,
                    resource_service_1.ResourceService])
            ], TaskListComponent);
            exports_1("TaskListComponent", TaskListComponent);
        }
    };
});
