import { Injectable } from '@angular/core';

@Injectable()
export class AlertService {

    // make list of errors 
    constructor() { }
    messages: string[];

    clearAlert(message: string): void {
        this.messages = this.messages.filter(x => x != message);
    }

    clearAll(): void {
        this.messages = undefined;
    }

    setError(message: string): void {
        if (!this.messages)
            this.messages = [];

        this.messages.push(message);
    }
}
