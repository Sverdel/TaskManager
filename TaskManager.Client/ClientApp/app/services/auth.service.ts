import { Injectable } from "@angular/core";
import { Http, RequestOptions, Headers } from "@angular/http";
import { AuthHttp } from "./auth.http"
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";
import { User } from "../models/user";

@Injectable()
export class AuthService {
    authKey: string = "auth";
    userName: string = "TestUser";
    user?: User = new User();

    constructor(private http: AuthHttp) {
        this.user = this.getUser();
    }

    // Returns TRUE if the user is logged in, FALSE otherwise.
    isAuthorized(): boolean {
        if (this.http.isServer()) {
            return false;
        }

        if (localStorage.getItem(this.authKey) == null)
            return false;

        var item = localStorage.getItem(this.authKey);
        if (item) {
            var auth = JSON.parse(item);
            var authDate = new Date(auth.tokenExpireDate);
            var current = new Date();
            if (authDate < current) {
                localStorage.removeItem(this.authKey);
                return false;
            }
        }

        return true;
    }

    signin(username: string, password: string): any {
        var url = "account/signin"; // JwtProvider's LoginPath
        var data = {
            name: username,
            password: password
        };

        return this.http.post(url, data)
            .map(response => {
                this.setUser(response.json());
                return true;
            });
    }

    signinExt(provider: string): any {
        var url = "account/signinExt/" + provider; // JwtProvider's LoginPath
        var data = {};

        return this.http.post(url, data)
            .map(response => {
                this.setUser(response.json());
                return true;
            });
    }

    signout(): any {
        return this.http.post("account/signout", null)
            .map(response => {
                this.setUser(undefined);
                return true;
            })
            .catch((err: any) => {
                return Observable.throw(err);
            });
    }

    signup(user: User) {
        return this.http.post("account/signup", user)
            .map(response => response.json())
            .catch((err: any) => {
                return Observable.throw(err);
            });;
    }

    
    // Persist auth into localStorage or removes it if a NULL argument is  given
    private setUser(user?: User): boolean {
        if (this.http.isServer()) {
            return false;
        }

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
    private getUser(): any {
        if (this.http.isServer()) {
            return;
        }

        var i = localStorage.getItem(this.authKey);
        if (i) {
            return JSON.parse(i);
        }
        else {
            return null;
        }
    }
}