import { Route, mapToCanActivate } from '@angular/router';
import { AuthGuard } from './guard';

export const APP_Routes: Route[] = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./face.component')
  },
  {
    path: 'signin',
    loadComponent: () => import('./signin.component')
  },
  {
    path: 'register',
    loadComponent: () => import('./register.component')
  },
  {
    path: 'admin',
    loadComponent: () => import('./admin.component'),
    loadChildren: () => import('./AdminRoutes').then(x => x.ADMIN_ROUTES),
    canActivateChild: mapToCanActivate([AuthGuard]),
  }
];


