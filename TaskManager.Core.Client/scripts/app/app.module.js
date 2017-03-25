System.register(["@angular/core", "@angular/platform-browser", "@angular/http", "@angular/router", "@angular/forms", "rxjs/Rx", "./component/app.component", "./component/signin.component", "./component/signup.component", "./component/tasklist.component", "./component/task.component", "./component/page-not-found.component", "./app.routing", "./service/auth.service", "./service/task.service", "./service/resource.service", "./service/auth.http", "./pipe/fromDictionary.pipe"], function (exports_1, context_1) {
    "use strict";
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __moduleName = context_1 && context_1.id;
    var core_1, platform_browser_1, http_1, router_1, forms_1, app_component_1, signin_component_1, signup_component_1, tasklist_component_1, task_component_1, page_not_found_component_1, app_routing_1, auth_service_1, task_service_1, resource_service_1, auth_http_1, fromDictionary_pipe_1, AppModule;
    return {
        setters: [
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (platform_browser_1_1) {
                platform_browser_1 = platform_browser_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
            },
            function (forms_1_1) {
                forms_1 = forms_1_1;
            },
            function (_1) {
            },
            function (app_component_1_1) {
                app_component_1 = app_component_1_1;
            },
            function (signin_component_1_1) {
                signin_component_1 = signin_component_1_1;
            },
            function (signup_component_1_1) {
                signup_component_1 = signup_component_1_1;
            },
            function (tasklist_component_1_1) {
                tasklist_component_1 = tasklist_component_1_1;
            },
            function (task_component_1_1) {
                task_component_1 = task_component_1_1;
            },
            function (page_not_found_component_1_1) {
                page_not_found_component_1 = page_not_found_component_1_1;
            },
            function (app_routing_1_1) {
                app_routing_1 = app_routing_1_1;
            },
            function (auth_service_1_1) {
                auth_service_1 = auth_service_1_1;
            },
            function (task_service_1_1) {
                task_service_1 = task_service_1_1;
            },
            function (resource_service_1_1) {
                resource_service_1 = resource_service_1_1;
            },
            function (auth_http_1_1) {
                auth_http_1 = auth_http_1_1;
            },
            function (fromDictionary_pipe_1_1) {
                fromDictionary_pipe_1 = fromDictionary_pipe_1_1;
            }
        ],
        execute: function () {
            AppModule = class AppModule {
            };
            AppModule = __decorate([
                core_1.NgModule({
                    // directives, components, and pipes
                    declarations: [
                        app_component_1.AppComponent,
                        signin_component_1.SignInComponent,
                        signup_component_1.SignUpComponent,
                        tasklist_component_1.TaskListComponent,
                        task_component_1.TaskComponent,
                        page_not_found_component_1.PageNotFoundComponent,
                        fromDictionary_pipe_1.FromDictionaryPipe
                    ],
                    // modules
                    imports: [
                        platform_browser_1.BrowserModule,
                        http_1.HttpModule,
                        forms_1.FormsModule,
                        forms_1.ReactiveFormsModule,
                        router_1.RouterModule,
                        app_routing_1.AppRouting,
                    ],
                    // providers
                    providers: [
                        auth_http_1.AuthHttp,
                        auth_service_1.AuthService,
                        task_service_1.TaskService,
                        resource_service_1.ResourceService,
                    ],
                    bootstrap: [
                        app_component_1.AppComponent
                    ]
                })
            ], AppModule);
            exports_1("AppModule", AppModule);
        }
    };
});
