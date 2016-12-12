import {ModuleWithProviders} from "@angular/core";
import {Routes, RouterModule} from "@angular/router";
import {HomeComponent} from "./component/home.component";
//import {AboutComponent} from "./about.component";
//import {LoginComponent} from "./login.component";
import {PageNotFoundComponent} from "./component/page-not-found.component";
//import {ItemDetailEditComponent} from "./item-detail-edit.component";
//import {ItemDetailViewComponent} from "./item-detail-view.component";
import {SignUpComponent} from "./component/signup.component";
import {SignInComponent} from "./component/signin.component";

const appRoutes: Routes = [
    {
        path: "",
        component: HomeComponent
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