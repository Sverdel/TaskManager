import {Component, NgZone} from "@angular/core";
import {Router} from "@angular/router";
import {User} from "./../model/user";

//import {AuthService} from "./auth.service";
@Component({
    selector: "signup",
    templateUrl: './signup.component.html'
})
export class SignUpComponent {
    NewUser: User = new User();

    constructor() {}
    signup() {}
}
