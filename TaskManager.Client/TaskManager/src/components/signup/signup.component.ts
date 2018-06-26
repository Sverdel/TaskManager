import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { AuthService } from "@services/auth.service";
import { Credentials } from "@models/credentials";

@Component({
    selector: 'signup',
    templateUrl: './signup.component.html',
    styleUrls: ['./signup.component.css']
})
/** signup component*/
export class SignupComponent implements OnInit
{
    newUser: Credentials = new Credentials();
    /** Called by Angular after signup component initialized */
    ngOnInit(): void { }

    constructor(private authService: AuthService, private router: Router) { }
    signup() {
        if (this.newUser == null) {
            return;
        }

        this.authService.signup(this.newUser)
            .subscribe((data: any) => {
                this.router.navigate([""]);
            });
    }
}