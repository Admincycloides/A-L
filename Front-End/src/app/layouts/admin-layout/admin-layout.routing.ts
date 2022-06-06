import { Routes } from '@angular/router';

import { DashboardComponent } from '../../pages/dashboard/dashboard.component';
import { UserComponent } from '../../pages/user/user.component';
import { TableComponent } from '../../pages/table/table.component';
import { TypographyComponent } from '../../pages/typography/typography.component';
import { IconsComponent } from '../../pages/icons/icons.component';
import { MapsComponent } from '../../pages/maps/maps.component';
import { NotificationsComponent } from '../../pages/notifications/notifications.component';
import { UpgradeComponent } from '../../pages/upgrade/upgrade.component';
import { TimesheetComponent } from '../../pages/timesheet/timesheet.component';
import { AddtimesheetComponent } from '../../pages/timesheet/addtimesheet/addtimesheet.component';
import { ProjectsComponent } from '../../pages/projects/projects.component';
import { ActivitiesComponent } from '../../pages/projects/activities/activities.component';
import { ReviewtimesheetComponent } from '../../pages/reviewtimesheet/reviewtimesheet.component';
import { ReviewlinkComponent } from '../../pages/reviewtimesheet/reviewlink/reviewlink.component';

export const AdminLayoutRoutes: Routes = [
    { path: 'dashboard',      component: DashboardComponent },
    { path: 'user',           component: UserComponent },
    { path: 'table',          component: TableComponent },
    { path: 'typography',     component: TypographyComponent },
    { path: 'icons',          component: IconsComponent },
    { path: 'maps',           component: MapsComponent },
    { path: 'notifications',  component: NotificationsComponent },
    { path: 'upgrade',        component: UpgradeComponent },
    { path: 'timesheet',      component: TimesheetComponent },
    { path: 'addtimesheet',      component: AddtimesheetComponent },
    { path: 'projects',      component: ProjectsComponent },
    { path: 'activities',      component: ActivitiesComponent },
    { path: 'activities',      component: ActivitiesComponent },
    { path: 'review',      component: ReviewtimesheetComponent},
    { path: 'reviewlink',      component: ReviewlinkComponent},
    ]

