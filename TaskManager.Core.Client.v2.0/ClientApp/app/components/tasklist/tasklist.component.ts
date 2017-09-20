import { Component, OnInit } from '@angular/core';
import { TaskService } from "../../services/task.service";
import { AuthService } from "../../services/auth.service";
import { ResourceService } from "../../services/resource.service";
import {Task} from "./../../models/task";

@Component({
    selector: 'tasklist',
    templateUrl: './tasklist.component.html',
    styleUrls: ['./tasklist.component.css']
})
/** tasklist component*/
export class TasklistComponent implements OnInit
{
    taskList: Task[];
    currentTaskId?: number = undefined;

    /** tasklist ctor */
    constructor(public taskService: TaskService,
        public authService: AuthService,
        public resourceService: ResourceService) { }

    /** Called by Angular after tasklist component initialized */
    ngOnInit(): void {
        if (this.authService.user != null) {
            this.taskService.getAllTasks(this.authService.user.id)
                .subscribe((response: any) => {
                    this.taskList = response.json();
                });
        }
    }

    removeTask() {
        if (!this.currentTaskId) {
            return;
        }

        this.taskService.deleteTask(this.currentTaskId)
            .subscribe((response: any) => {
                this.taskList = this.taskList.filter(function (e) {
                    return e.id !== response.json().id;
                });

                this.currentTaskId = undefined;
            });
    }

    getTask(id: number) {
        this.currentTaskId = id;
    }

    createTask() {
        this.currentTaskId = -1;
    }
}