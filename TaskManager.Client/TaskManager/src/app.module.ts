import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './components/app/app.component';
import { SigninComponent } from './components/signin/signin.component';
import { SignupComponent } from './components/signup/signup.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { TasklistComponent } from './components/tasklist/tasklist.component';
import { TaskComponent } from './components/task/task.component';

import { Environment } from './environments/environment';
import { AuthService } from './services/auth.service';
import { AuthHttp } from './services/auth.http';
import { ResourceService } from './services/resource.service';
import { TaskService } from './services/task.service';
import { SignalRService } from './services/signalr.service';

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
    AuthService,
    AuthHttp,
    ResourceService,
    TaskService,
    SignalRService,
    { provide: 'BASE_URL', useFactory: getBaseUrl }
  ],
  imports: [
    CommonModule,
    BrowserModule,
    HttpModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: TasklistComponent },
      { path: "signup", component: SignupComponent },
      { path: "signin", component: SigninComponent },
      { path: "**", component: PageNotFoundComponent }
    ])
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}
