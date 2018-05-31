import { Injectable } from "@angular/core";
import { Http, Headers, RequestOptionsArgs } from '@angular/http';
import { PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';
import { User } from "../models/user";
import { Environment } from "../environments/environment"


@Injectable()
export class AuthHttp {
    authKey: string = "auth";
   
    constructor(@Inject(PLATFORM_ID) private platformId: Object, private http: Http) {
    }

    get(url: any, opts: RequestOptionsArgs = {}) {
        this.configureAuth(opts);
      return this.http.get(Environment.apiUrl + url, opts);
    }

    post(url: any, data: any, opts: RequestOptionsArgs = {}) {
        this.configureAuth(opts);
        return this.http.post(Environment.apiUrl + url, data, opts);
    }

    put(url: any, data: any, opts: RequestOptionsArgs = {}) {
        this.configureAuth(opts);
        return this.http.put(Environment.apiUrl + url, data, opts);
    }

    delete(url: any, opts: RequestOptionsArgs = {}) {
        this.configureAuth(opts);
        return this.http.delete(Environment.apiUrl + url, opts);
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
