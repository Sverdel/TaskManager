import {Component, NgZone} from "@angular/core";
import {Router} from "@angular/router";
import {User} from "./../model/user";

//import {AuthService} from "./auth.service";
@Component({
    selector: "signin",
    templateUrl: './signin.component.html'
})
export class SignInComponent {
    User: User = new User();
    constructor() {}
    signin() {}
}
