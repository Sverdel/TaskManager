import { Component, NgZone } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "./../service/auth.service";
import { ResourceService } from "./../service/resource.service";

@Component({
    selector: "taskmanager",
    templateUrl: './app.component.html'
})
export class AppComponent {
    title = "Task Manager";
    alertMessage: string = null;

    constructor(public router: Router,
        public authService: AuthService) {
    }

    isActive(data: any[]): boolean {
        return this.router.isActive(this.router.createUrlTree(data), true);
    }

    logout(): boolean {
        this.authService.logout().subscribe(result => {
            if (result) {
                this.router.navigate([""]);
            }
        });
        return false;
    }
}
