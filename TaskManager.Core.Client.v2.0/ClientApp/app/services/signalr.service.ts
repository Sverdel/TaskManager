import { Environment } from "./../environments/environment";
import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection } from '@aspnet/signalr-client';
import { Task } from "./../models/task";
import { AuthHttp } from "./auth.http"

@Injectable()
export class SignalRService {

    taskChanged = new EventEmitter<Task>();
    taskAdded = new EventEmitter<Task>();
    taskDeleted = new EventEmitter<Task>();
    
    private _hubConnection: HubConnection;

    constructor(private env: Environment) {

        this._hubConnection = new HubConnection(env.baseUrl + 'task');
      
        this.registerOnServerEvents();

        this.startConnection();
    }

    private startConnection(): void {

        this._hubConnection.start()
            .then(() => {
                console.log('Hub connection started');
            })
            .catch(err => {
                console.log('Error while establishing connection')
            });
    }

    private registerOnServerEvents(): void {

        this._hubConnection.on('createTask', (data: any) => {
            this.taskAdded.emit(data);
        });

        this._hubConnection.on('editTask', (data: any) => {
            this.taskChanged.emit(data);
        });

        this._hubConnection.on('deleteTask', (data: any) => {
            this.taskDeleted.emit(data);
        });
    }
}
