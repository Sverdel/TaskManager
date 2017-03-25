import { Component, NgZone, Input } from "@angular/core";
import { Router } from "@angular/router";
import { Task } from "./../model/task";
import { State } from "./../model/state";
import { Priority } from "./../model/priority";
import { TaskService } from "./../service/task.service";
import { AuthService } from "./../service/auth.service";
import { ResourceService } from "./../service/resource.service";

@Component({
    selector: "task",
    templateUrl: './task.component.html'
})
export class TaskComponent {
    changed: boolean = false;
    currentTask: Task = null;
    shadowCopy: Task = null;
    @Input() taskList: Task[];
    @Input() currentTaskId: number;
    localTaskId: number = null;

    constructor(
        public taskService: TaskService,
        public authService: AuthService,
        public resourceService: ResourceService) {

    }

    ngDoCheck() {
        if (!this.resourceService.isInitialized()) {
            this.resourceService.init();
        }

        if (this.currentTaskId != this.localTaskId) {
            this.localTaskId = this.currentTaskId;

            if (this.currentTaskId == -1) {
                this.currentTask = new Task();
                this.shadowCopy = new Task();
            }
            else {
                this.taskService.getTask(this.authService.user.id, this.authService.user.token, this.currentTaskId)
                    .subscribe(response => {
                        this.currentTask = response.json();
                        this.shadowCopy = response.json();
                    });
            }
        }

        this.changed = JSON.stringify(this.currentTask) != JSON.stringify(this.shadowCopy)
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
        } else {
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
        } else {
            this.currentTask = JSON.parse(JSON.stringify(this.shadowCopy));
        }
    }

    setState(stateId: number) {
        this.currentTask.stateId = stateId;
    }

    setPriority(priorityId: number) {
        this.currentTask.priorityId = priorityId;
    }

}
