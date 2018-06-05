import { Injectable } from '@angular/core';

@Injectable()
export class AlertService {

    // make list of errors 
    constructor() { }
    message: string = undefined;

    clearAlert(): void {
        this.message = undefined;
    }

    setError(message: string): void {
        this.message = message;
    }

}
