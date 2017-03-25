import { Injectable, EventEmitter } from "@angular/core";
import { Http, Headers, Response, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
//import {AuthHttp} from "./auth.http";
import { User } from "./../model/user";

@Injectable()
export class AuthService {
    authKey = "auth";
    public user: User = new User();

    constructor(private http: Http) {
        this.user = this.getUser();
    }

    login(username: string, password: string): any {
        var url = "http://localhost:8000/api/connect/token"; // JwtProvider's LoginPath
        var data = {
            username: username,
            password: password,
            client_id: "TaskManager",
            // required when signing up with username/password
            grant_type: "password",
            // space-separated list of scopes for which the token is issued
            scope: "offline_access profile email"
        };

       return this.http.post(
            url,
            this.toUrlEncodedString(data),
            new RequestOptions({
                headers: new Headers({
                    "Content-Type": "application/x-www-form-urlencoded"
                })
            }))
            .map(response => {
                this.setUser(response.json());
                return true;
            });
    }

    logout(): any {
        return this.http.post("http://localhost:8000/api/users/logout", null)
            .map(response => {
                this.setUser(null);
                return true;
            })
            .catch(err => {
                return Observable.throw(err);
            });
    }

    // Converts a Json object to urlencoded format
    toUrlEncodedString(data: any) {
        var body = "";
        for (var key in data) {
            if (body.length) {
                body += "&";
            }
            body += key + "=";
            body += encodeURIComponent(data[key]);
        }
        return body;
    }
    // Persist auth into localStorage or removes it if a NULL argument is  given
    setUser(user: User): boolean {
        if (user) {
            localStorage.setItem(this.authKey, JSON.stringify(user));
        }
        else {
            localStorage.removeItem(this.authKey);
        }
        this.user = user;

        return true;
    }
    // Retrieves the auth JSON object (or NULL if none)
    getUser(): any {
        var i = localStorage.getItem(this.authKey);
        if (i) {
            return JSON.parse(i);
        }
        else {
            return null;
        }
    }
    // Returns TRUE if the user is logged in, FALSE otherwise.
    isLoggedIn(): boolean {
        if (localStorage.getItem(this.authKey) == null)
            return false;

        var auth = JSON.parse(localStorage.getItem(this.authKey));
        var authDate = new Date(auth.tokenExpireDate);
        var current = new Date();
        if (authDate < current) {
            localStorage.removeItem(this.authKey);
            return false;
        }

        return true;
    }

    //get() {
    //    return this.http.get("api/Accounts")
    //               .map(response => response.json());
    //}

    add(user: User) {
        return this.http.post("http://localhost:8000/api/users", JSON.stringify(user),
            new RequestOptions({
                headers: new Headers({ "Content-Type": "application/json" })
            }))
            .map(response => response.json());
    }

    //update(user: User) {
    //    return this.http.put( "api/Accounts", JSON.stringify(user),
    //            new RequestOptions({
    //                headers: new Headers({ "Content-Type": "application/json" })
    //        }))
    //    .map(response => response.json());
    //}
}