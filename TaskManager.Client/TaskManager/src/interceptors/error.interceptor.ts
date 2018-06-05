import { Injectable } from '@angular/core';
import { Router } from "@angular/router";
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from "rxjs";
import { tap } from 'rxjs/operators';
import { AlertService } from "./../services/alert.service";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private alert: AlertService) { }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        return next.handle(request).pipe(tap(
            (event: HttpEvent<any>) => { },
            (err: any) => {
                if (err instanceof HttpErrorResponse) {
                    this.alert.setError(err.message + ':' + JSON.stringify(err.error));
                }
            }));
    }
}