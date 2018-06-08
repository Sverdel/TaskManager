import { Component, OnInit } from '@angular/core';
import { ExchangeRateService } from './../../services/exchange-rate.service'
import { ExchangeRate } from "./../../models/exchange-rate"

@Component({
    selector: 'exchange-rate',
    templateUrl: './exchange-rate.component.html',
    styleUrls: ['./exchange-rate.component.css']
})
export class ExchangeRateComponent implements OnInit {
    constructor(private rateService: ExchangeRateService) { }

    ngOnInit() {
    }

}
