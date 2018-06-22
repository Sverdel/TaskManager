import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { forkJoin } from "rxjs";

import { AuthService } from "./auth.service";
import { SignalRService } from "./signalr.service";
import { Environment } from "./../environments/environment"
import { ExchangeRate } from "./../models/exchange-rate"

@Injectable()
export class ExchangeRateService {
    private exchangeUrl: string;
    rates: ExchangeRate[] = new Array();
    constructor(private http: HttpClient,
        private authService: AuthService,
        private signalRService: SignalRService) {
        this.exchangeUrl = Environment.apiUrl + "rate/";
        this.getRates();

        this.signalRService.exchangeRateChanged.subscribe((rate: ExchangeRate) => {
            if (!this.rates)
                this.rates = new Array();

            if (rate) {
                var i = this.rates.findIndex((r: ExchangeRate) => r != null && r.currency == rate.currency);
                if (i >= 0)
                    this.rates[i] = rate;
                else
                    this.rates.push(rate);
            }
        });
    }

    getRates(): void {
        if (!this.authService.isAuthorized()) {
            return undefined;
        }

        let usd = this.http.get<ExchangeRate>(this.exchangeUrl + "USD");
        let eur = this.http.get<ExchangeRate>(this.exchangeUrl + "EUR");
        
        forkJoin([usd, eur]).subscribe(results => {
            results.forEach(data => {
                let rate = <ExchangeRate>data;
                if (rate)
                    this.rates.push(rate)
            });
        });
    }
}
