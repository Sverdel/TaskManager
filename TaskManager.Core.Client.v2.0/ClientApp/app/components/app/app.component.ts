import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { AuthService } from "./../../services/auth.service";

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    title = "Task Manager";

    constructor(public router: Router, private authService: AuthService) {
    }

}
