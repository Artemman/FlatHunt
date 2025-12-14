import { NgModule } from '@angular/core';
import { RouterModule, Routes, PreloadAllModules } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';

const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'flats',
    loadComponent: () => import('./flats/flats.component').then(m => m.FlatsComponent),
    canActivate: [AuthGuard]
  },
  {
    path: '',
    redirectTo: 'flats',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'flats'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
