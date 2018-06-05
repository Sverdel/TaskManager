import { Injectable } from "@angular/core";
import { User } from "../models/user";

@Injectable()
export class TokenService {
    authKey: string = "auth";

    constructor() {
    }

    setUser(user?: User): boolean {
        if (user) {
            localStorage.setItem(this.authKey, JSON.stringify(user));
        }
        else {
            localStorage.removeItem(this.authKey);
        }

        return true;
    }

    getUser(): User | undefined {
        var userStr = localStorage.getItem(this.authKey);
        if (!userStr) {
            return undefined;
        }

        var user = JSON.parse(userStr);
        var authDate = new Date(user.tokenExpireDate);
        var current = new Date();

        if (authDate < current) {
            this.setUser(undefined);
            return undefined
        }

        return user;
    }

    getToken(): string | undefined {
        var user = this.getUser();
        if (user != null) {
            return user.accessToken;
        }

        return undefined;
    }
}