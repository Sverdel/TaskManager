import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http"
import { PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';

import { Observable } from "rxjs";
import { map, catchError } from "rxjs/operators";

import { TokenService } from '@services/token.service';
import { Environment } from "@environments/environment"
import { User } from "@models/user";
import { Credentials } from "@models/credentials";

@Injectable()
export class AuthService {
    user?: User = new User();

    constructor(private http: HttpClient, @Inject(PLATFORM_ID) private platformId: Object, private tokenService: TokenService) {
        this.user = this.tokenService.getUser();
    }

    isAuthorized(): boolean {
        if (this.isServer()) {
            return false;
        }

        var user = this.tokenService.getUser();
        if (!user) return false;

        return true;
    }

    signin(user: Credentials): Observable<boolean> {
        var url = Environment.apiUrl + "account/signin"; // JwtProvider's LoginPath

        return this.http.post<Credentials>(url, user)
            .pipe(map((response: User) => {
                this.user = response;
                this.tokenService.setUser(response);
                return true;
            }));
    }

    signinExt(provider: string): Observable<boolean> {
        var url = Environment.apiUrl + "account/signinExt/" + provider; // JwtProvider's LoginPath
        var data = {};

        return this.http.post<User>(url, data)
            .pipe(map((response: User) => {
                this.user = response;
                this.tokenService.setUser(response);
                return true;
            }));
    }

    signout(): Observable<boolean> {
        return this.http.post(Environment.apiUrl + "account/signout", null)
            .pipe(
                map((response: any) => {
                    this.tokenService.setUser(undefined);
                    return true;
                })
            );
    }

    signup(user: Credentials): Observable<boolean> {
        return this.http.post<Credentials>(Environment.apiUrl + "account/signup", user)
            .pipe(map((response: User) => {
                this.user = response;
                this.tokenService.setUser(response);
                return true;
        }));
    }

    isServer(): boolean {
        return isPlatformServer(this.platformId);
    }
}