import { Injectable, EventEmitter } from "@angular/core";
import { Http, Headers, Response, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { State } from "./../model/state";
import { Priority } from "./../model/priority";
import { AuthHttp } from "./../service/auth.http";
import { AuthService } from "./../service/auth.service";


@Injectable()
export class ResourceService {
    public states: State[] = null;
    public priorities: Priority[] = null;
    private baseUrl = "http://localhost:8000/api/resources/"

    constructor(private http: AuthHttp, private authService: AuthService) {
        this.init();
    }

    init() {
        if (!this.authService.isLoggedIn()) {
            return;
        }

        this.getStates()
            .subscribe(states => {
                this.states = states;
            });

        this.getPriorities()
            .subscribe(priorities => {
                this.priorities = priorities;
            });
    }

    isInitialized() {
        return this.states != null && this.priorities != null;
    }

    getStates(): Observable<State[]> {
        return this.http.get("http://localhost:8000/api/resources/states")
            .map(response => {
                return response.json();
            });
    }

    getPriorities(): Observable<Priority[]> {
        return this.http.get("http://localhost:8000/api/resources/priorities")
            .map(response => {
                return response.json();
            });
    }
}