import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';

import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { TimesheetComponent } from './pages/timesheet/timesheet.component';
import AuthGuard from './_guards/auth.guard';

export const AppRoutes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  }, 
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: AdminLayoutComponent,
    canActivate :[AuthGuard],
    children: [
        {
      path: '',
      loadChildren: () => import('./layouts/admin-layout/admin-layout.module').then(x => x.AdminLayoutModule)
  }]},
  {
    path: '**',
    redirectTo: ''
  }
]
