import { Component, OnInit } from '@angular/core';


export interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
}

export const ROUTES: RouteInfo[] = [
    // { path: 'dashboard',     title: 'Dashboard',         icon:'nc-bank',       class: '' },
    // { path: 'icons',         title: 'Icons',             icon:'nc-diamond',    class: '' },
    // { path: 'maps',          title: 'Maps',              icon:'nc-pin-3',      class: '' },
    // { path: 'notifications', title: 'Notifications',     icon:'nc-bell-55',    class: '' },
    // { path: 'user',          title: 'User Profile',      icon:'nc-single-02',  class: '' },
    // { path: 'table',         title: 'Table List',        icon:'nc-tile-56',    class: '' },
    // { path: 'typography',    title: 'Typography',        icon:'nc-caps-small', class: '' },
    { path: 'timesheet',    title: 'Timesheet Management',  icon:'nc-watch-time', class: 'm-item' },
    { path: 'projects',    title: 'Projects',  icon:'nc-settings', class: 'm-item' },
    { path: 'log',    title: 'Log (Audit Trail)',  icon:'nc-paper', class: 'm-item' },
    { path: 'review',    title: 'Review Timesheet',  icon:'nc-paper', class: 'm-item review-ico' },
    { path: 'reports',    title: 'View Reports',  icon:'nc-paper', class: 'm-item report-ico' },
];

@Component({
    moduleId: module.id,
    selector: 'sidebar-cmp',
    templateUrl: 'sidebar.component.html',
})

export class SidebarComponent implements OnInit {
    public menuItems: any[];
    ngOnInit() {
        this.menuItems = ROUTES.filter(menuItem => menuItem);
    }
}
