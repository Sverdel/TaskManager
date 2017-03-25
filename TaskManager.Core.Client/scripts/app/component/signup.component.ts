import { Component, NgZone } from "@angular/core";
import { Router } from "@angular/router";
import { User } from "./../model/user";
import { AuthService } from "./../service/auth.service";

@Component({
    selector: "signup",
    templateUrl: './signup.component.html'
})
export class SignUpComponent {
    newUser: User = new User();

    constructor(public authService: AuthService, public router: Router) { }
    signup() {
        if (this.newUser == null) {
            return;
        }

        this.authService.add(this.newUser)
            .subscribe((data) => {
                this.authService.login(this.newUser.name, this.newUser.password)
                    .subscribe((data) => { this.router.navigate([""]); },
                    (err) => { console.log(err); }
                    );
            },
            (err) => {
                console.log(err);
            });
    }
}
