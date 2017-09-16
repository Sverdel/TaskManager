import { Component, OnInit } from '@angular/core';
import { AuthService } from "../../services/auth.service";
import { Router } from "@angular/router";
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
    
    constructor(public authService: AuthService, public router: Router) { }
    signup() {
        if (this.newUser == null) {
            return;
        }

        this.authService.signup(this.newUser)
            .subscribe((data: any) => {
                if (this.newUser == null || this.newUser.name == null || this.newUser.password == null) {
                    return;
                }

                this.authService.signin(this.newUser.name, this.newUser.password)
                    .subscribe((data: any) => {
                        this.router.navigate([""]);
                    },
                    (err: any) => {
                        console.log(err);
                    });
            },
            (err: any) => {
                console.log(err);
            });
    }
}