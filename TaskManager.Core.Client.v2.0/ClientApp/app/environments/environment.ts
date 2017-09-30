import { Injectable } from "@angular/core";

@Injectable()
export class Environment {
    baseUrl: string = "http://localhost:54255/";
    apiUrl: string = this.baseUrl + "api/";
};
