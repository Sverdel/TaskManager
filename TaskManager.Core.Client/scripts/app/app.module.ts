///<reference path="../../typings/index.d.ts"/>
import {NgModule} from "@angular/core";
import {BrowserModule} from "@angular/platform-browser";
import {HttpModule} from "@angular/http";
import {RouterModule} from "@angular/router";
import {FormsModule, ReactiveFormsModule} from "@angular/forms"
import "rxjs/Rx";

import {AppComponent} from "./component/app.component";
import {SignInComponent} from "./component/signin.component";
import {SignUpComponent} from "./component/signup.component";
import {TaskListComponent} from "./component/tasklist.component";
import {TaskComponent} from "./component/task.component";
import {PageNotFoundComponent} from "./component/page-not-found.component";
import {HomeComponent} from "./component/home.component";

import {AppRouting} from "./app.routing";
//import {ItemDetailEditComponent} from "./item-detail-edit.component";
//import {ItemDetailViewComponent} from "./item-detail-view.component";
//import {ItemListComponent} from "./item-list.component";
//import {LoginComponent} from "./login.component";
//import {ItemService} from "./item.service";
//import {AuthService} from "./auth.service";
//import {AuthHttp} from "./auth.http";
//import {UserEditComponent} from "./user-edit.component";

@NgModule({
    // directives, components, and pipes
    declarations: [
        //AboutComponent,
        AppComponent,
        SignInComponent,
        SignUpComponent,
        TaskListComponent,
        TaskComponent,
        PageNotFoundComponent,
        HomeComponent,
        //LoginComponent,
        //UserEditComponent
    ],
    // modules
    imports: [
        BrowserModule,
        HttpModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        AppRouting
    ],
    // providers
    providers: [
        //ItemService,
        //AuthService,
        //AuthHttp
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }