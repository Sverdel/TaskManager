import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common'

@Component({
    selector: 'clock',
    templateUrl: './clock.component.html',
    styleUrls: ['./clock.component.css']
})
export class ClockComponent implements OnInit {
    currentdate: Date;
    constructor() { }

    ngOnInit() {
        this.getTime();
    }

    private getTime(): void {
        setInterval(() => {
            this.currentdate = new Date();
        }, 1000);
    }
}
