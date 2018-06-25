import { Component } from '@angular/core';
import { AlertService } from '@services/alert.service';

@Component({
  selector: 'alert-msg',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css']
})

export class AlertComponent {

    constructor(private alert: AlertService) { }
    
    clearAlert(message): void {
        this.alert.clearAlert(message);
    }

}
