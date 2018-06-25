import { EventEmitter, Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { Task } from "@models/task";
import { ExchangeRate } from "@models/exchange-rate";
import { HttpClient } from "@angular/common/http"
import { Environment } from "@environments/environment";

@Injectable()
export class SignalRService {

    taskChanged = new EventEmitter<Task>();
    taskAdded = new EventEmitter<Task>();
    taskDeleted = new EventEmitter<Task>();
    exchangeRateChanged = new EventEmitter<ExchangeRate>();

    private _taskHubConnection: HubConnection;
    private _exchangeHubConnection: HubConnection;

    constructor() {

        this._taskHubConnection = new HubConnectionBuilder()
            .withUrl(Environment.baseUrl + 'task')
            .build();

        this._exchangeHubConnection = new HubConnectionBuilder()
            .withUrl(Environment.baseUrl + 'exchange')
            .build();

        this.registerOnServerEvents();

        this.startConnection();
    }

    private startConnection(): void {

        this._taskHubConnection.start()
            .then(() => {
                console.log('Task hub connection started');
            })
            .catch(err => {
                console.log(`Error while establishing connection: ${err}`);
            });

        this._exchangeHubConnection.start()
            .then(() => {
                console.log('Exchange hub connection started');
            })
            .catch(err => {
                console.log(`Error while establishing connection: ${err}`);
            });
    }

    private registerOnServerEvents(): void {

        this._taskHubConnection.on('createTask', (data: any) => {
            if (this.taskAdded.observers.length > 0)
                this.taskAdded.emit(data);
        });

        this._taskHubConnection.on('editTask', (data: any) => {
            if (this.taskChanged.observers.length > 0)
                this.taskChanged.emit(data);
        });

        this._taskHubConnection.on('deleteTask', (data: any) => {
            if (this.taskDeleted.observers.length > 0)
                this.taskDeleted.emit(data);
        });

        this._exchangeHubConnection.on('rateChanged', (data: any) => {
            if (this.exchangeRateChanged.observers.length > 0)
                this.exchangeRateChanged.emit(data);
        });
    }
}
