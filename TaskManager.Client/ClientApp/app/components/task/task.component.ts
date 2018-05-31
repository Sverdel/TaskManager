import { Component, NgZone, Input, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Task } from "./../../models/task";
import { State } from "./../../models/state";
import { Priority } from "./../../models/priority";
import { TaskService } from "./../../services/task.service";
import { AuthService } from "./../../services/auth.service";
import { ResourceService } from "./../../services/resource.service";
import { SignalRService } from "../../services/signalr.service";

@Component({
    selector: 'task',
    templateUrl: './task.component.html',
    styleUrls: ['./task.component.css']
})
/** task component*/
export class TaskComponent implements OnInit {
    changed: boolean = false;
    currentTask?: Task;
    shadowCopy?: Task = undefined;
    @Input() taskList!: Task[];
    @Input() currentTaskId?: number;
    localTaskId?: number = undefined;


    /** task ctor */
    constructor(
        private taskService: TaskService,
        private authService: AuthService,
        private resourceService: ResourceService,
        private signalRService: SignalRService) {

    }

    /** Called by Angular after task component initialized */
    ngOnInit(): void {
        this.signalRService.taskChanged.subscribe((task: Task) => {
            if (task && this.currentTaskId == task.id) {
                this.currentTask = task;
                this.shadowCopy = task;
            }
        });
    }

    ngDoCheck() {
        if (!this.resourceService.isInitialized()) {
            this.resourceService.init();
        }

        if (this.currentTaskId == undefined) {
            this.currentTask = undefined;
            this.shadowCopy = undefined;
            return;
        }

        if (this.currentTaskId != this.localTaskId) {
            this.localTaskId = this.currentTaskId;

            if (this.currentTaskId == -1) {
                if (this.authService.user) {
                    this.currentTask = new Task(this.authService.user.id);
                    this.shadowCopy = new Task(this.authService.user.id);
                }
            }
            else if (this.currentTaskId) {
                this.taskService.getTask(this.currentTaskId)
                    .subscribe((response: Task) => {
                        this.currentTask = response;
                        this.shadowCopy = response;
                    });
            }
        }

        this.changed = JSON.stringify(this.currentTask) != JSON.stringify(this.shadowCopy)
    }

    saveTask() {
        if (!this.currentTask) {
            return;
        }

        if (this.currentTaskId == -1) {
            this.taskService.createTask(this.currentTask)
                .subscribe((response: Task) => {

                    this.currentTask = response;
                    this.shadowCopy = response;
                    this.changed = false;
                    if (this.currentTask) {
                        this.currentTaskId = this.currentTask.id;
                        this.localTaskId = this.currentTask.id;
                        if (this.taskList.some((x: Task) => x.id == this.currentTaskId)) {
                            return;
                        }

                        this.taskList.push(this.currentTask);
                    }
                });
        } else {
            this.taskService.editTask(this.currentTask)
                .subscribe((response: Task) => {
                    if (this.currentTaskId) {
                        this.taskService.getTask(this.currentTaskId)
                            .subscribe((response: any) => {
                                this.currentTask = response;
                                this.shadowCopy = response;
                            });
                    }
                });
        }
    }

    cancelChanges() {
        if (this.currentTaskId == -1) {
            this.currentTaskId = this.localTaskId = undefined;
            this.currentTask = this.shadowCopy = undefined;
        } else {
            this.currentTask = JSON.parse(JSON.stringify(this.shadowCopy));
        }
    }

    setState(stateId: number) {
        if (this.currentTask) {
            this.currentTask.stateId = stateId;
        }
    }

    setPriority(priorityId: number) {
        if (this.currentTask) {
            this.currentTask.priorityId = priorityId;
        }
    }
}