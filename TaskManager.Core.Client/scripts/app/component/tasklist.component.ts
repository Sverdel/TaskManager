import { Component, NgZone } from "@angular/core";
import { Router } from "@angular/router";
import { Task } from "./../model/task";
import { TaskService } from "./../service/task.service";
import { AuthService } from "./../service/auth.service";
import { ResourceService } from "./../service/resource.service";

//import {AuthService} from "./auth.service";
@Component({
    selector: "tasklist",
    templateUrl: './tasklist.component.html'
})
export class TaskListComponent {
    taskList: Task[] = null;
    currentTaskId: number = null;

    constructor(public taskService: TaskService,
        public authService: AuthService,
        public resourceService: ResourceService) { }

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

    getTask(id: number) {
        this.currentTaskId = id;
    }

    createTask() {
        this.currentTaskId = -1;
    }
}
