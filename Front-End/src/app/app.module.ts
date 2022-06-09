import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ToastrModule } from "ngx-toastr";
import { FormsModule } from '@angular/forms';

import { SidebarModule } from './sidebar/sidebar.module';
import { FooterModule } from './shared/footer/footer.module';
import { NavbarModule} from './shared/navbar/navbar.module';
import { FixedPluginModule} from './shared/fixedplugin/fixedplugin.module';

import { AppComponent } from './app.component';
import { AppRoutes } from './app.routing';

import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { LogComponent } from './pages/log/log.component';
import { LoginComponent } from './auth/login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from "@angular/common/http";
import { ReviewtimesheetComponent } from './pages/reviewtimesheet/reviewtimesheet.component';
import { ReviewlinkComponent } from './pages/reviewtimesheet/reviewlink/reviewlink.component';
import { ProjectsComponent } from "./pages/projects/projects.component";
import { ActivitiesComponent } from "./pages/projects/activities/activities.component";



@NgModule({
  declarations: [
    AppComponent,
    AdminLayoutComponent,
    LogComponent,
    LoginComponent,
    ProjectsComponent,
    ActivitiesComponent
  ],
  imports: [
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(AppRoutes,{
      useHash: true
    }),
    SidebarModule,
    NavbarModule,
    ToastrModule.forRoot(),
    FooterModule,
    FixedPluginModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
