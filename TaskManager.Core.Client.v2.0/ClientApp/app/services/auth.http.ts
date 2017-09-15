import { Injectable } from "@angular/core";
import { Http, Headers } from '@angular/http';
import { Environment } from "./../environments/environment";
import { PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';


@Injectable()
export class AuthHttp {
    authKey: string = "auth";
   
    constructor(@Inject(PLATFORM_ID) private platformId: Object, private http: Http, private env: Environment) {
    }

    get(url: any, opts = {}) {
        this.configureAuth(opts);
        return this.http.get(this.env.apiUrl + url, opts);
    }

    post(url: any, data: any, opts = {}) {
        this.configureAuth(opts);
        return this.http.post(this.env.apiUrl + url, data, opts);
    }

    put(url: any, data: any, opts = {}) {
        this.configureAuth(opts);
        return this.http.put(this.env.apiUrl + url, data, opts);
    }

    delete(url: any, opts = {}) {
        this.configureAuth(opts);
        return this.http.delete(this.env.apiUrl + url, opts);
    }

    configureAuth(opts: any) {
        if (isPlatformServer(this.platformId)) {
            return;
        }

        var i = localStorage.getItem(this.authKey);
        if (i != null) {
            var auth = JSON.parse(i);
            if (auth.accessToken != null) {
                if (opts.headers == null) {
                    opts.headers = new Headers();
                }
                opts.headers.set("Authorization", `Bearer ${auth.accessToken}`);
            }
        }
    }

    isLoggedIn(): boolean {
        if (isPlatformServer(this.platformId)) {
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
}