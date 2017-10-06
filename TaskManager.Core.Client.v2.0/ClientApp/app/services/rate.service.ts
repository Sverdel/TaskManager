import { Injectable } from "@angular/core";
import { AuthHttp } from "./auth.http";

@Injectable()
export class RateService {
    private baseUrl = "rate/"
    constructor(private http: AuthHttp) { }

    getExchangeRate(currency: string): any {
        return this.http.get(this.baseUrl + currency);
    }
}