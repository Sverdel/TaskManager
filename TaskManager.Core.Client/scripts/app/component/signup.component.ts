import { Component, NgZone } from "@angular/core";
import { Router } from "@angular/router";
import { User } from "./../model/user";
import { AuthService } from "./../service/auth.service";

@Component({
    selector: "signup",
    templateUrl: './signup.component.html'
})
export class SignUpComponent {
    NewUser: User = new User();

    constructor(public authService: AuthService, public router: Router) { }
    signup() {
        if (this.NewUser == null) {
            return;
        }

        this.authService.add(this.NewUser)
            .subscribe((data) => {
                this.authService.login(this.NewUser.Name, this.NewUser.Password)
                    .subscribe((data) => { this.router.navigate([""]); },
                    (err) => { console.log(err); }
                    );
            },
            (err) => {
                console.log(err);
            });
    }
}
