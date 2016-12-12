import {Component, NgZone} from "@angular/core";
import {Router} from "@angular/router";
import {Task} from "./../model/task";
import {State} from "./../model/state";
import {Priority} from "./../model/priority";

//import {AuthService} from "./auth.service";
@Component({
    selector: "task",
    templateUrl: './task.component.html'
})
export class TaskComponent {
    States : State[] = null;
    Priorities: Priority[] = null;
    CurrentTask: Task = null;
    Changed : boolean = false;
    constructor() {}

    saveTask() {}
    cancelChanges() {}
    setState(stateId : number) {}
    setPriority(priorityIs : number) {}
    
}
