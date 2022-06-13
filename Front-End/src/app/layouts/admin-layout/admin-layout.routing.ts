import { Routes } from '@angular/router';
import { TimesheetComponent } from '../../pages/timesheet/timesheet.component';
import { AddtimesheetComponent } from '../../pages/timesheet/addtimesheet/addtimesheet.component';
import { ProjectsComponent } from '../../pages/projects/projects.component';
import { ActivitiesComponent } from '../../pages/projects/activities/activities.component';
import { ReviewtimesheetComponent } from 'app/pages/reviewtimesheet/reviewtimesheet.component';
import { ReviewlinkComponent } from 'app/pages/reviewtimesheet/reviewlink/reviewlink.component';
import { ReportsComponent } from 'app/pages/reports/reports.component';


export const AdminLayoutRoutes: Routes = [
    { path: 'timesheet',      component: TimesheetComponent },
    { path: 'addtimesheet',      component: AddtimesheetComponent },
    { path: 'projects',      component: ProjectsComponent },
    { path: 'activities',      component: ActivitiesComponent },
    { path: 'activities',      component: ActivitiesComponent },
    { path: 'review',      component: ReviewtimesheetComponent},
    { path: 'reviewlink',      component: ReviewlinkComponent},
    { path: 'reports',      component: ReportsComponent},
    ]

