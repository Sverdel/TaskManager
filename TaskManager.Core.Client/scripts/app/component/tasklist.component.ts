import {Component, NgZone} from "@angular/core";
import {Router} from "@angular/router";
import {Task} from "./../model/task";

//import {AuthService} from "./auth.service";
@Component({
    selector: "tasklist",
    templateUrl: './tasklist.component.html'
})
export class TaskListComponent {
    taskList: Task[] = null;
    currentTask: Task = null;
    constructor() {}
    
    ngOnInit() {}
    removeTask() {}
    getTask(id : number) {}
    createTask() {}
}
