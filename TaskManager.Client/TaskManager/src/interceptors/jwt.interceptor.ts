import { Injectable } from '@angular/core';
import { Router } from "@angular/router";
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from "rxjs";
import { tap } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(private router: Router) { }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        return next.handle(request).pipe(tap(
            (event: HttpEvent<any>) => { },
            (err: any) => {
            if (err instanceof HttpErrorResponse) {
                if (this.router.url !== '/signin' && err.status === 401) {
                    this.router.navigate(["signin"]);
                }
            }
        }));
    }
}