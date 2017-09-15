import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { AuthHttp } from "./auth.http"

@Injectable()
export class AuthService {
    authKey: string = "auth";
    userName: string = "TestUser";

    constructor(private http: AuthHttp) {
    }

    // Returns TRUE if the user is logged in, FALSE otherwise.
    isLoggedIn(): boolean {
        return this.http.isLoggedIn();
    }


}