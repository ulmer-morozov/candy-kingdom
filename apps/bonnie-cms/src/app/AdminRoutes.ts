import { Route } from '@angular/router';
import { ForecastComponent } from './forecast.component';
import { AdminHomeComponent } from './admin-home/admin-home.component';

export const ADMIN_ROUTES: Route[] = [
  {
    path: '',
    pathMatch: 'full',
    component: AdminHomeComponent
  },
  {
    path: 'forecast',
    component: ForecastComponent
  },
];
