import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http"
import { HttpHeaders } from "@angular/common/http";
import { Environment } from "@environments/environment"
import { Task } from "@models/task";

@Injectable()
export class TaskService {
    private baseUrl: string;
    private httpOptions = {
        headers: new HttpHeaders({'Content-Type': 'application/json'})
    };

    constructor(private http: HttpClient) {
        this.baseUrl = Environment.apiUrl + "tasks/";
    }

    getAllTasks(userId: string): Observable<Task[]> {
        return this.http.get<Task[]>(this.baseUrl + "list/" + userId);
    }

    getTask(taskId: number): Observable<Task> {
        return this.http.get<Task>(this.baseUrl + taskId);
    }

    createTask(task: Task): Observable<Task> {
        return this.http.post<Task>(this.baseUrl, task, this.httpOptions);
    }

    editTask(task: Task): Observable<Task> {
        return this.http.put<Task>(this.baseUrl + task.id, task, this.httpOptions);
    }

    deleteTask(taskId: number): Observable<Task> {
        return this.http.delete<Task>(this.baseUrl + taskId);
    }
}