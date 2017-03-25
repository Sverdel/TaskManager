System.register(["@angular/router", "./component/tasklist.component", "./component/page-not-found.component", "./component/signup.component", "./component/signin.component"], function (exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var router_1, tasklist_component_1, page_not_found_component_1, signup_component_1, signin_component_1, appRoutes, AppRoutingProviders, AppRouting;
    return {
        setters: [
            function (router_1_1) {
                router_1 = router_1_1;
            },
            function (tasklist_component_1_1) {
                tasklist_component_1 = tasklist_component_1_1;
            },
            function (page_not_found_component_1_1) {
                page_not_found_component_1 = page_not_found_component_1_1;
            },
            function (signup_component_1_1) {
                signup_component_1 = signup_component_1_1;
            },
            function (signin_component_1_1) {
                signin_component_1 = signin_component_1_1;
            }
        ],
        execute: function () {
            appRoutes = [
                {
                    path: "",
                    component: tasklist_component_1.TaskListComponent
                },
                {
                    path: "home",
                    redirectTo: ""
                },
                {
                    path: "signup",
                    component: signup_component_1.SignUpComponent
                },
                {
                    path: "signin",
                    component: signin_component_1.SignInComponent
                },
                {
                    path: '**',
                    component: page_not_found_component_1.PageNotFoundComponent
                }
            ];
            exports_1("AppRoutingProviders", AppRoutingProviders = []);
            exports_1("AppRouting", AppRouting = router_1.RouterModule.forRoot(appRoutes));
        }
    };
});
