import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { User } from "./../../models/user";
import { AuthService } from "./../../services/auth.service";


@Component({
    selector: 'signin',
    templateUrl: './signin.component.html',
    styleUrls: ['./signin.component.css']
})
/** signin component*/
export class SigninComponent implements OnInit
{
    user: User = new User();

    /** signin ctor */
    constructor(private authService: AuthService, private router: Router) { }

    /** Called by Angular after signin component initialized */
    ngOnInit(): void { }

    signin() {
        if (this.user == null || this.user.name == null || this.user.password == null) {
            return;
        }

        this.authService.signin(this.user.name, this.user.password)
            .subscribe((data: any) => {
                this.router.navigate([""]);
            },
            (err: any) => {
                console.log(err);
            });
    }
}