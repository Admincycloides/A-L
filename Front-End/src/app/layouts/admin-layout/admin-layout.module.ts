import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';

import { AdminLayoutRoutes } from './admin-layout.routing';
import { TimesheetComponent }       from '../../pages/timesheet/timesheet.component';
import { ReviewtimesheetComponent }       from '../../pages/reviewtimesheet/reviewtimesheet.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown'




import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule } from '@angular/common/http';
import { NgxPaginationModule } from 'ngx-pagination';
import { ReviewlinkComponent } from 'app/pages/reviewtimesheet/reviewlink/reviewlink.component';
import { ReportsComponent } from 'app/pages/reports/reports.component';
import { httpInterceptProviders } from 'app/_interceptors';



@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(AdminLayoutRoutes),
    FormsModule,
    NgbModule,
    HttpClientModule,
    NgxPaginationModule,
    ReactiveFormsModule,
    NgMultiSelectDropDownModule.forRoot()
  ],
  declarations: [
    TimesheetComponent,
    ReviewtimesheetComponent,
    ReviewlinkComponent,
    ReportsComponent,  
  ],
  providers: [
    httpInterceptProviders
  ],
})

export class AdminLayoutModule {}
