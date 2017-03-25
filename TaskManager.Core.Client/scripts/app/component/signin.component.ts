import { Component, NgZone } from "@angular/core";
import { Router } from "@angular/router";
import { User } from "./../model/user";
import { AuthService } from "./../service/auth.service";

@Component({
    selector: "signin",
    templateUrl: './signin.component.html'
})
export class SignInComponent {
    user: User = new User();
    constructor(public authService: AuthService, public router: Router) { }

    signin() {
        if (this.user == null) {
            return;
        }

        this.authService.login(this.user.name, this.user.password)
            .subscribe((data) => {
                this.router.navigate([""]);
            },
            (err) => {
                console.log(err);
            });
    }
}
