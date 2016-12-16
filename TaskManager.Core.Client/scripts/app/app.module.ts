///<reference path="../../typings/index.d.ts"/>
import { NgModule,  } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { HttpModule } from "@angular/http";
import { RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import "rxjs/Rx";

import { AppComponent } from "./component/app.component";
import { SignInComponent } from "./component/signin.component";
import { SignUpComponent } from "./component/signup.component";
import { TaskListComponent } from "./component/tasklist.component";
import { TaskComponent } from "./component/task.component";
import { PageNotFoundComponent } from "./component/page-not-found.component";
import { HomeComponent } from "./component/home.component";

import { AppRouting } from "./app.routing";

import { AuthService } from "./service/auth.service";

@NgModule({
    // directives, components, and pipes
    declarations: [
        AppComponent,
        SignInComponent,
        SignUpComponent,
        TaskListComponent,
        TaskComponent,
        PageNotFoundComponent,
        HomeComponent
    ],
    // modules
    imports: [
        BrowserModule,
        HttpModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        AppRouting,

    ],
    // providers
    providers: [
        AuthService,
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }