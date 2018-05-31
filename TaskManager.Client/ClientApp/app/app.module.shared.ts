import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { JwtInterceptor } from './interceptors/jwt.interceptor';

import { AppComponent } from './components/app/app.component';
import { SigninComponent } from './components/signin/signin.component';
import { SignupComponent } from './components/signup/signup.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { TasklistComponent } from './components/tasklist/tasklist.component';
import { TaskComponent } from './components/task/task.component';

import { Environment } from './environments/environment';
import { AuthService } from './services/auth.service';
import { ResourceService } from './services/resource.service';
import { TaskService } from './services/task.service';
import { SignalRService } from './services/signalr.service';
import { TokenService } from './services/token.service';

import { FromDictionaryPipe } from './pipes/fromDictionary.pipe';

@NgModule({
    declarations: [
        AppComponent,
        SigninComponent,
        SignupComponent,
        PageNotFoundComponent,
        TasklistComponent,
        TaskComponent,

        FromDictionaryPipe
    ],
    providers: [
        Environment,
        AuthService,
        ResourceService,
        TaskService,
        SignalRService,
        TokenService,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: TokenInterceptor,
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: JwtInterceptor,
            multi: true
        }
    ],
    imports: [
        CommonModule,
        HttpClientModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', component: TasklistComponent },
            { path: "signup", component: SignupComponent },
            { path: "signin", component: SigninComponent },
            { path: "**", component: PageNotFoundComponent }
        ])
    ]
})
export class AppModuleShared {
}
