import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { AuthService } from "../../services/auth.service";
import { AlertService } from "../../services/alert.service";
import { User } from "./../../models/user";

@Component({
    selector: 'signup',
    templateUrl: './signup.component.html',
    styleUrls: ['./signup.component.css']
})
/** signup component*/
export class SignupComponent implements OnInit
{
    newUser: User = new User();
    /** Called by Angular after signup component initialized */
    ngOnInit(): void { }

    constructor(private authService: AuthService, private router: Router, private alert: AlertService) { }
    signup() {
        if (this.newUser == null) {
            return;
        }

        this.authService.signup(this.newUser)
            .subscribe((data: any) => {
                this.router.navigate([""]);
            },
            (err: any) => {
                console.log(err);
                this.alert.setError("Failed to create user");
            });
    }
}