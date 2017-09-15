import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { SigninComponent } from './components/signin/signin.component';
import { SignupComponent } from './components/signup/signup.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';

import { Environment } from './environments/environment';
import { AuthService } from './services/auth.service';
import { AuthHttp } from './services/auth.http';


@NgModule({
    declarations: [
        AppComponent,
        SigninComponent,
        SignupComponent,
        PageNotFoundComponent
    ],
    providers: [
        Environment,
        AuthService,
        AuthHttp
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', component: PageNotFoundComponent },
            { path: "signup", component: SignupComponent },
            { path: "signin", component: SigninComponent },

        //    { path: '', redirectTo: 'home', pathMatch: 'full' },
        //    { path: 'home', component: HomeComponent },
        //    { path: 'counter', component: CounterComponent },
        //    { path: 'fetch-data', component: FetchDataComponent },
        //    { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}
