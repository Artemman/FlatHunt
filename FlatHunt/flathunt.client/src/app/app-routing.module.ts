import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Example minimal routes using standalone component lazy load.
// Adjust import paths if you placed the component elsewhere.
export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./login.component').then(m => m.LoginComponent) },
  //{ path: 'home', loadComponent: () => import('./home.component').then(m => m.HomeComponent) }, // replace with your home component
  { path: '', redirectTo: 'home', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
