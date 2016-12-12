import {Component, NgZone} from "@angular/core";
import {Router} from "@angular/router";
import {User} from "./../model/user";
//import {AlertModule} from 'ng2-bootstrap/ng2-bootstrap';

//import {AuthService} from "./auth.service";
@Component({
    selector: "taskmanager",
    templateUrl: './app.component.html'
})
export class AppComponent {
    title = "Task Manager";
    alertMessage: string = null;
    user: User = null;

    constructor(public router: Router) {}
    
    isActive(data: any[]): boolean {
        return this.router.isActive(this.router.createUrlTree(data), true);
    }

    logout(): boolean {
    // logs out the user, then redirects him to Welcome View.
        //this.authService.logout().subscribe(result => {
        //    if (result) {
        //        this.router.navigate([""]);
        //    }
        //});
        return false;
    }
}
