import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'page-not-found',
    templateUrl: './page-not-found.component.html',
    styleUrls: ['./page-not-found.component.css']
})
/** page-not-found component*/
export class PageNotFoundComponent implements OnInit
{
    title = "Page not Found";

    /** page-not-found ctor */
    constructor() { }

    /** Called by Angular after page-not-found component initialized */
    ngOnInit(): void { }
}