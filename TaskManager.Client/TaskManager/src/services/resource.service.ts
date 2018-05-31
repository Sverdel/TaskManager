import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { State } from "./../models/state";
import { Priority } from "./../models/priority";
import { AuthHttp } from "./auth.http";
import { AuthService } from "./auth.service";
import { map } from "rxjs/operators"

@Injectable()
export class ResourceService {
    public states?: State[] = undefined;
    public priorities?: Priority[] = undefined;
    private stateUrl = "states/"
    private priorityUrl = "priorities/"

    constructor(private http: AuthHttp, private authService: AuthService) {
        this.init();
    }

    init() {
        if (!this.authService.isAuthorized()) {
            return;
        }

        this.getStates()
            .subscribe((states: State[]) => {
                this.states = states;
            });

        this.getPriorities()
            .subscribe((priorities: Priority[]) => {
                this.priorities = priorities;
            });
    }

    isInitialized() {
        return this.states != null && this.priorities != null;
    }

    private getStates(): Observable<State[]> {
        return this.http.get(this.stateUrl)
            .pipe(map((response: any) => {
                return response.json();
            }));
    }

    private getPriorities(): Observable<Priority[]> {
        return this.http.get(this.priorityUrl)
            .pipe(map((response: any) => {
                return response.json();
            }));
    }
}