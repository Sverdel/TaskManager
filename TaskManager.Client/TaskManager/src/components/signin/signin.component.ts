import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { Credentials } from "@models/credentials";
import { AuthService } from "@services/auth.service";

@Component({
    selector: 'signin',
    templateUrl: './signin.component.html',
    styleUrls: ['./signin.component.css']
})
export class SigninComponent 
{
    user: Credentials = new Credentials();

    constructor(private authService: AuthService, private router: Router) { }


    signin() : void {
        if (this.user == null || this.user.name == null || this.user.password == null) {
            return;
        }

        this.authService.signin(this.user)
            .subscribe((data: any) => {
                this.router.navigate([""]);
            });
    }
}