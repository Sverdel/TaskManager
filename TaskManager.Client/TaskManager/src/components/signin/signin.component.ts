﻿import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { User } from "./../../models/user";
import { AuthService } from "./../../services/auth.service";
import { AlertService } from "./../../services/alert.service";

@Component({
    selector: 'signin',
    templateUrl: './signin.component.html',
    styleUrls: ['./signin.component.css']
})
export class SigninComponent 
{
    user: User = new User();

    constructor(private authService: AuthService, private router: Router, private alert: AlertService) { }


    signin() : void {
        if (this.user == null || this.user.userName == null || this.user.password == null) {
            return;
        }

        this.authService.signin(this.user.userName, this.user.password)
            .subscribe((data: any) => {
                this.router.navigate([""]);
            },
            (err: any) => {
                console.log(err);
                this.alert.setError("Incorrect user name or password");
            });
    }
}