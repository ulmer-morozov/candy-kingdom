import { Route } from '@angular/router';
import { AdminHomeComponent } from './admin-home/admin-home.component';
import { AdminSettingsComponent } from './admin-settings/admin-settings.component';

export const ADMIN_ROUTES: Route[] = [
  {
    path: '',
    pathMatch: 'full',
    component: AdminHomeComponent,
  },
  {
    path: 'settings',
    pathMatch: 'full',
    component: AdminSettingsComponent,
  }
];
