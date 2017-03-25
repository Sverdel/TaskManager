import { Injectable, EventEmitter } from "@angular/core";
import { Http, Headers, Response, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { Task } from "./../model/task";
import { AuthHttp } from "./../service/auth.http";

//import {AuthHttp} from "./auth.http";
//import { User } from "./../model/user";

@Injectable()
export class TaskService {
    

    private baseUrl = "http://localhost:8000/api/tasks/"
    constructor(private http: AuthHttp) { }

    getAllTasks(userId: string, token: string): any  {
        return this.http.get(this.baseUrl + userId + "/" + token);
    }

    getTask(userId: string, token: string, taskId: number): any  {
        return this.http.get(this.baseUrl + userId + "/" + token + "/" + taskId);
    }

    createTask(userId: string, token: string, task: Task): any  {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this.http.post(this.baseUrl + userId + "/" + token, JSON.stringify(task), options)
    }

    editTask(userId: string, token: string, task: Task): any  {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this.http.put(this.baseUrl + userId + "/" + token, JSON.stringify(task), options)
    }

    deleteTask(userId: string, token: string, taskId: number): any {
        return this.http.delete(this.baseUrl + userId + "/" + token + "/" + taskId);
    }
}