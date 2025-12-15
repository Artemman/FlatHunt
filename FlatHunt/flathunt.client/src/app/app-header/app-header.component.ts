import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Observable, map } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatTabsModule,
    MatTooltipModule,
    MatDividerModule 
  ],
  templateUrl: './app-header.component.html',
  styleUrls: ['./app-header.component.scss']
})
export class AppHeaderComponent {
  // Observables to drive template (keeps role logic out of template)
  showFlats$: Observable<boolean>;
  showParser$: Observable<boolean>;
  userDisplayName$: Observable<string | null>;

  constructor(private auth: AuthService, private router: Router) {
    this.showFlats$ = this.auth.currentUser$.pipe(
      map(u => !!u && Array.isArray(u.roles) && (u.roles.includes('Admin') || u.roles.includes('Broker')))
    );

    this.showParser$ = this.auth.currentUser$.pipe(
      map(u => !!u && Array.isArray(u.roles) && u.roles.includes('Admin'))
    );

    this.userDisplayName$ = this.auth.currentUser$.pipe(
      map(u => {
        if (!u) return null;
        return (u.name && u.name.trim().length) ? u.name : u.email;
      })
    );
  }

  logout(): void {
    this.auth.logout('/login');
  }

  navigate(path: string): void {
    this.router.navigateByUrl(path);
  }
}
