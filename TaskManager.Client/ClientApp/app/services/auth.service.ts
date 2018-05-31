import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http"
import { PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';

import { Observable } from "rxjs";
import { map, catchError } from "rxjs/operators";

import { TokenService } from './token.service';
import { Environment } from "./../environments/environment"
import { User } from "../models/user";

@Injectable()
export class AuthService {
    authKey: string = "auth";
    userName: string = "TestUser";
    user?: User = new User();

    constructor(private http: HttpClient, @Inject(PLATFORM_ID) private platformId: Object, private env: Environment, private tokenService: TokenService) {
        this.user = this.tokenService.getUser();
    }

    // Returns TRUE if the user is logged in, FALSE otherwise.
    isAuthorized(): boolean {
        if (this.isServer()) {
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
        var url = this.env.apiUrl + "account/signin"; // JwtProvider's LoginPath
        var data = {
            name: username,
            password: password
        };

        return this.http.post<User>(url, data)
            .pipe(map((response: any) => {
                this.tokenService.setUser(response);
                return true;
            }));
    }

    signinExt(provider: string): any {
        var url = this.env.apiUrl + "account/signinExt/" + provider; // JwtProvider's LoginPath
        var data = {};

        return this.http.post<User>(url, data)
            .pipe(map((response: any) => {
                this.tokenService.setUser(response);
                return true;
            }));
    }

    signout(): any {
        return this.http.post<User>(this.env.apiUrl + "account/signout", null)
            .pipe(
                map((response: any) => {
                this.tokenService.setUser(undefined);
                    return true;
                }),
                catchError((err: any) => {
                    return Observable.throw(err);
                })
            );
    }

    signup(user: User) {
        return this.http.post<User>(this.env.apiUrl + "account/signup", user)
            .pipe(
                catchError((err: any) => Observable.throw(err)
            ));
    }

    
    

    isServer(): boolean {
        return isPlatformServer(this.platformId);
    }
}