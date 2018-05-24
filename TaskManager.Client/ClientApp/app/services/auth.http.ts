import { Injectable } from "@angular/core";
import { Http, Headers, RequestOptionsArgs } from '@angular/http';
import { Environment } from "./../environments/environment";
import { PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';
import { User } from "../models/user";


@Injectable()
export class AuthHttp {
    authKey: string = "auth";
   
    constructor(@Inject(PLATFORM_ID) private platformId: Object, private http: Http, private env: Environment) {
    }

    get(url: any, opts: RequestOptionsArgs = {}) {
        this.configureAuth(opts);
        return this.http.get(this.env.apiUrl + url, opts);
    }

    post(url: any, data: any, opts: RequestOptionsArgs = {}) {
        this.configureAuth(opts);
        return this.http.post(this.env.apiUrl + url, data, opts);
    }

    put(url: any, data: any, opts: RequestOptionsArgs = {}) {
        this.configureAuth(opts);
        return this.http.put(this.env.apiUrl + url, data, opts);
    }

    delete(url: any, opts: RequestOptionsArgs = {}) {
        this.configureAuth(opts);
        return this.http.delete(this.env.apiUrl + url, opts);
    }

    isServer(): boolean
    {
        return isPlatformServer(this.platformId);
    }

    private configureAuth(opts: RequestOptionsArgs) {
        if (this.isServer()) {
            return;
        }

        if (opts.headers == null) {
            opts.headers = new Headers();
        }

        opts.headers.append("Access-Control-Allow-Origin", "*");
        opts.headers.append("Access-Control-Allow-Methods", "GET, POST, OPTIONS, PUT, PATCH, DELETE");
        opts.headers.append("Access-Control-Allow-Headers", "X-Requested-With,content-type");
        opts.headers.append("Access-Control-Allow-Credentials", "true");

        var token = localStorage.getItem(this.authKey);
        if (token != null) {
            var auth = JSON.parse(token);
            if (auth.accessToken != null) {
                opts.headers.append("Authorization", `Bearer ${auth.accessToken}`);
            }
        }
    }
}