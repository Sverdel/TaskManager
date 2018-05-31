import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { State } from "./../models/state";
import { Priority } from "./../models/priority";
import { HttpClient } from "@angular/common/http";
import { AuthService } from "./auth.service";
import { map } from "rxjs/operators"
import { Environment } from "./../environments/environment"

@Injectable()
export class ResourceService {
    public states?: State[] = undefined;
    public priorities?: Priority[] = undefined;
    private stateUrl: string;
    private priorityUrl: string;

    constructor(private http: HttpClient, private authService: AuthService, env: Environment) {
        this.stateUrl = env.apiUrl + "states/";
        this.priorityUrl = env.apiUrl + "priorities/";
        this.init();
        
    }

    init() {
        if (!this.authService.isAuthorized()) {
            return;
        }

        this.http.get<State[]>(this.stateUrl)
            .subscribe((states: State[]) => {
                this.states = states;
            });

        this.http.get<Priority[]>(this.priorityUrl)
            .subscribe((priorities: Priority[]) => {
                this.priorities = priorities;
            });
    }

    isInitialized() {
        return this.states != null && this.priorities != null;
    }
}