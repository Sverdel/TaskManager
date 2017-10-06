import { Component, OnInit } from '@angular/core';
import { RateService } from '../../services/rate.service';

@Component({
    selector: 'rate',
    templateUrl: './rate.component.html',
    styleUrls: ['./rate.component.css']
})
/** rate component*/
export class RateComponent implements OnInit
{
    usd: number;
    eur: number;

    /** rate ctor */
    constructor(private rateService: RateService) { }

    /** Called by Angular after rate component initialized */
    ngOnInit(): void {
        this.rateService.getExchangeRate("usd")
            .subscribe((rate: any) => {
                this.usd = rate.json()
            });
        this.rateService.getExchangeRate("eur")
            .subscribe((rate: any) => {
                this.eur = rate.json()
            });
    }


}