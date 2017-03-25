System.register(["@angular/core", "@angular/router", "./../model/user", "./../service/auth.service"], function (exports_1, context_1) {
    "use strict";
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var __moduleName = context_1 && context_1.id;
    var core_1, router_1, user_1, auth_service_1, SignUpComponent;
    return {
        setters: [
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
            },
            function (user_1_1) {
                user_1 = user_1_1;
            },
            function (auth_service_1_1) {
                auth_service_1 = auth_service_1_1;
            }
        ],
        execute: function () {
            SignUpComponent = class SignUpComponent {
                constructor(authService, router) {
                    this.authService = authService;
                    this.router = router;
                    this.newUser = new user_1.User();
                }
                signup() {
                    if (this.newUser == null) {
                        return;
                    }
                    this.authService.add(this.newUser)
                        .subscribe((data) => {
                        this.authService.login(this.newUser.name, this.newUser.password)
                            .subscribe((data) => { this.router.navigate([""]); }, (err) => { console.log(err); });
                    }, (err) => {
                        console.log(err);
                    });
                }
            };
            SignUpComponent = __decorate([
                core_1.Component({
                    selector: "signup",
                    templateUrl: './signup.component.html'
                }),
                __metadata("design:paramtypes", [auth_service_1.AuthService, router_1.Router])
            ], SignUpComponent);
            exports_1("SignUpComponent", SignUpComponent);
        }
    };
});
