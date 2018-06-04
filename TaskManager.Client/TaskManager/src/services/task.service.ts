import { Injectable, EventEmitter } from "@angular/core";
import { Http, Headers, Response, RequestOptions } from "@angular/http";
import { Observable } from "rxjs";
import { Task } from "./../models/task";
import { HttpClient } from "@angular/common/http"
import { HttpParams, HttpHeaders } from "@angular/common/http";
import { Environment } from "./../environments/environment"

@Injectable()
export class TaskService {
    private baseUrl: string;
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
        return this.http.post<Task>(this.baseUrl, JSON.stringify(task));
    }

    editTask(task: Task): any {
        return this.http.put<Task>(this.baseUrl + task.id, JSON.stringify(task));
    }

    deleteTask(taskId: number): any {
        return this.http.delete<Task>(this.baseUrl + taskId);
    }
}