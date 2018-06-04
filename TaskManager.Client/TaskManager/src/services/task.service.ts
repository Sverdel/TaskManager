import { Injectable, EventEmitter } from "@angular/core";
import { Observable } from "rxjs";
import { Task } from "./../models/task";
import { HttpClient } from "@angular/common/http"
import { HttpParams, HttpHeaders } from "@angular/common/http";
import { Environment } from "./../environments/environment"



@Injectable()
export class TaskService {
    private baseUrl: string;
    private httpOptions = {
        headers: new HttpHeaders({'Content-Type': 'application/json'})
    };

    constructor(private http: HttpClient) {
        this.baseUrl = Environment.apiUrl + "tasks/";
    }

    getAllTasks(userId: string): any {
        return this.http.get<Task[]>(this.baseUrl + "list/" + userId);
    }

    getTask(taskId: number): any {
        return this.http.get<Task>(this.baseUrl + taskId);
    }

    createTask(task: Task): any {
        return this.http.post<Task>(this.baseUrl, task, this.httpOptions);
    }

    editTask(task: Task): any {
        return this.http.put<Task>(this.baseUrl + task.id, task, this.httpOptions);
    }

    deleteTask(taskId: number): any {
        return this.http.delete<Task>(this.baseUrl + taskId);
    }
}