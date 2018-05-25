import { Injectable, EventEmitter } from "@angular/core";
import { Http, Headers, Response, RequestOptions } from "@angular/http";
import { Observable } from "rxjs";
import { Task } from "./../models/task";
import { AuthHttp } from "./auth.http";
import { HttpParams, HttpHeaders } from '@angular/common/http';

@Injectable()
export class TaskService {
    private baseUrl = "tasks/"
    constructor(private http: AuthHttp) { }

    getAllTasks(userId: string): any {
        return this.http.get(this.baseUrl + "list/" + userId);
    }

    getTask(taskId: number): any {
        return this.http.get(this.baseUrl + taskId);
    }

    createTask(task: Task): any {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this.http.post(this.baseUrl, JSON.stringify(task), options);
    }

    editTask(task: Task): any {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this.http.put(this.baseUrl + task.id, JSON.stringify(task), options);
    }

    deleteTask(taskId: number): any {
        return this.http.delete(this.baseUrl + taskId);
    }
}