import { Component, OnInit } from '@angular/core';
import { TaskService } from "../../services/task.service";
import { AuthService } from "../../services/auth.service";
import { SignalRService } from "../../services/signalr.service";
import { ResourceService } from "../../services/resource.service";
import { Task } from "./../../models/task";
import { AlertService } from "./../../services/alert.service";

@Component({
    selector: 'tasklist',
    templateUrl: './tasklist.component.html',
    styleUrls: ['./tasklist.component.css']
})
/** tasklist component*/
export class TasklistComponent implements OnInit
{
    taskList!: Task[];
    currentTaskId?: number = undefined;

    /** tasklist ctor */
    constructor(public taskService: TaskService,
        public authService: AuthService,
        public resourceService: ResourceService,
        private signalRService: SignalRService,
        private alert: AlertService) { }

    /** Called by Angular after tasklist component initialized */
    ngOnInit(): void {
        if (this.authService.user != null) {
            this.taskService.getAllTasks(this.authService.user.id)
                .subscribe((response: Task[]) => {
                    this.taskList = response;
                }, (err: any) => {
                    console.log(err);
                    this.alert.setError("Error on getting tasks. Can't get task list");
                });
        }

        this.subscribeToEvents();
    }

    removeTask() {
        if (!this.currentTaskId) {
            return;
        }

        this.taskService.deleteTask(this.currentTaskId)
            .subscribe((response: Task) => {
                this.taskList = this.taskList.filter(function (e) {
                    return e.id !== response.id;
                });

                this.currentTaskId = undefined;
            }, (err: any) => {
                console.log(err);
                this.alert.setError("Error on deleting tasks. Task wasn't deleted");
            });
    }

    getTask(id: number) {
        this.currentTaskId = id;
    }

    createTask() {
        this.currentTaskId = -1;
    }

    private subscribeToEvents(): void {
        this.signalRService.taskAdded.subscribe((task: Task) => {
            if (!this.taskList)
                this.taskList = Task[0];

            if (task && !this.taskList.some((e: Task) => e.id == task.id)) {
                this.taskList.push(task)
            }

            if (this.currentTaskId == -1)
                this.currentTaskId = task.id;
        });

        this.signalRService.taskChanged.subscribe((task: Task) => {
            if (!this.taskList)
                this.taskList = Task[0];

            if (task && this.currentTaskId == task.id) {
                var i = this.taskList.findIndex(x => x.id == task.id);
                this.taskList[i] = task;
            }
        });

        this.signalRService.taskDeleted.subscribe((task: Task) => {
            if (!this.taskList)
                this.taskList = Task[0];

            if (task) {
                this.taskList = this.taskList.filter(x => x.id != task.id);
                if (this.currentTaskId == task.id) {
                    this.currentTaskId = undefined;
                }
            }
        });
    }
}