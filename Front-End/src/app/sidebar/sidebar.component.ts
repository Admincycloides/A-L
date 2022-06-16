import { Component, OnInit } from '@angular/core';


export interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
}

export const ROUTES: RouteInfo[] = [
    { path: 'timesheet',    title: 'Timesheet Management',  icon:'nc-watch-time', class: 'm-item' },
    { path: 'projects',    title: 'Projects',  icon:'nc-settings', class: 'm-item' },
    { path: 'log',    title: 'Log (Audit Trail)',  icon:'nc-paper', class: 'm-item' },
    { path: 'review',    title: 'Review Timesheet',  icon:'nc-tile-56', class: 'm-item review-ico' },
    { path: 'reports',    title: 'View Reports',  icon:'nc-single-copy-04', class: 'm-item report-ico' },
];

@Component({
    moduleId: module.id,
    selector: 'sidebar-cmp',
    templateUrl: 'sidebar.component.html',
})

export class SidebarComponent implements OnInit {
    public menuItems: any[];
    private items: any[];
    user :any
    ngOnInit() {
        this.user = JSON.parse(localStorage.getItem('user'));
        this.items = ROUTES.filter(menuItem => menuItem);
        if(this.user.supervisorFlag == "Y")
        {
            this.menuItems = this.items;
        }
        if(this.user.supervisorFlag == "N"){
           this.menuItems = this.items.filter((item)=>{
                return item.title != 'Review Timesheet' && item.title != 'View Reports' ;
           })
 
        }
        
    }
}
