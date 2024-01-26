import { ActivatedRouteSnapshot, Route, mapToCanActivate } from '@angular/router';
import { AuthGuard } from './guard';
import { inject } from '@angular/core';
import { DataService } from './data.service';

export const APP_Routes: Route[] = [
  {
    path: 'signin',
    loadComponent: () => import('./signin.component'),
  },
  {
    path: 'register',
    loadComponent: () => import('./register.component'),
  },
  {
    path: 'admin',
    loadComponent: () => import('./admin.component'),
    loadChildren: () => import('./AdminRoutes').then((x) => x.ADMIN_ROUTES),
    canActivateChild: mapToCanActivate([AuthGuard]),
  },
  {
    path: '**',
    loadComponent: () => import('./face.component'),
    providers: [DataService],
    resolve: {
      page: (route: ActivatedRouteSnapshot) => inject(DataService).getPage(route.url.join('/')),
      faceView: () => inject(DataService).getView('face')
    }
  },
];

