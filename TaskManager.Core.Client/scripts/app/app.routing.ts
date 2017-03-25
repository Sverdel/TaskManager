import { ModuleWithProviders } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { TaskListComponent } from "./component/tasklist.component";
import { PageNotFoundComponent } from "./component/page-not-found.component";
import { SignUpComponent } from "./component/signup.component";
import { SignInComponent } from "./component/signin.component";

const appRoutes: Routes = [
    {
        path: "",
        component: TaskListComponent
    },
    {
        path: "home",
        redirectTo: ""
    },
    {
        path: "signup",
        component: SignUpComponent
    },
    {
        path: "signin",
        component: SignInComponent
    },
    {
        path: '**',
        component: PageNotFoundComponent
    }
];
export const AppRoutingProviders: any[] = [];
export const AppRouting: ModuleWithProviders = RouterModule.forRoot(appRoutes);