import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { forkJoin } from "rxjs";

import { AuthService } from "./auth.service";
import { Environment } from "./../environments/environment"
import { ExchangeRate } from "./../models/exchange-rate"

@Injectable()
export class ExchangeRateService {
    private exchangeUrl: string;
    rates: ExchangeRate[];
    constructor(private http: HttpClient, private authService: AuthService) {
        this.exchangeUrl = Environment.apiUrl + "rate/";
        this.getRates();
    }

    getRates(): void {
        if (!this.authService.isAuthorized()) {
            return undefined;
        }

        let usd = this.http.get<ExchangeRate>(this.exchangeUrl + "USD");
        let eur = this.http.get<ExchangeRate>(this.exchangeUrl + "EUR");
        
        forkJoin([usd, eur]).subscribe(results => {
            this.rates = <ExchangeRate[]><any>results;
        });
    }
}
