import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { AuthService } from "./../../services/auth.service";
import { AlertService } from "../../services/alert.service";

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    title = "Task Manager";

    constructor(private router: Router, private authService: AuthService, private alert: AlertService) {
    }

    isAuthorized(): boolean {
        return this.authService.isAuthorized();
    }

    signout(): boolean {
        this.authService.signout()
            .subscribe((result: boolean) => {
            if (result) {
                this.router.navigate([""]);
            }
            },
            (err: any) => {
                console.log(err);
                this.alert.setError("Error on signout");
            });

        return false;
    }

    signin(provider: string): boolean {
        this.authService.signinExt(provider)
            .subscribe((result: boolean) => {
            if (result) {
                this.router.navigate([""]);
            }
            },
            (err: any) => {
                console.log(err);
                this.alert.setError("Error on signin with external service");
            });

        return false;
    }
}
