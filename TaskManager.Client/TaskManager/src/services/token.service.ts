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
        var i = localStorage.getItem(this.authKey);
        if (i) {
            return JSON.parse(i);
        }
        else {
            return undefined;
        }
    }

    getToken(): string | undefined {
        var user = this.getUser();
        if (user != null) {
            return user.accessToken;
        }

        return undefined;
    }
}