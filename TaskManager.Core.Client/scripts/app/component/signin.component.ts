import { Component, NgZone } from "@angular/core";
import { Router } from "@angular/router";
import { User } from "./../model/user";
import { AuthService } from "./../service/auth.service";

@Component({
    selector: "signin",
    templateUrl: './signin.component.html'
})
export class SignInComponent {
    User: User = new User();
    constructor(public authService: AuthService, public router: Router) { }

    signin() {
        if (this.User == null) {
            return;
        }

        this.authService.login(this.User.Name, this.User.Password)
            .subscribe((data) => {
                this.router.navigate([""]);
            },
            (err) => {
                console.log(err);
            });
    }
}
