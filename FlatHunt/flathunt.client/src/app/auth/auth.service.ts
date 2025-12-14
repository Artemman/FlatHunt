import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import CommonConstants from '../constants/common.constants';

export interface AccountInfo extends User {
  expireAt: Date;
}

export interface User {
  id: number;
  email: string;
  name: string;
  roles: string[];
}

export interface UserTokenPaiload {
  sub: string;
  email: string;
  name: string;
  exp: number;
  role: string[]
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiBase = '/api/account';
  readonly diffToRefreshMs = 1 * 60 * 1000;

  private currentUserSubject = new BehaviorSubject<AccountInfo | null>(this.getAccountInfo());
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  login(email: string, password: string): Observable<User> {
    const url = `${this.apiBase}/login`;
    return this.http.post<AuthResponse>(url, { email, password })
      .pipe(
        map(res => this.saveAuth(res)),
        catchError(err => throwError(() => err)),

      );
  }

  logout(redirectTo: string = '/login'): void {
    try {
      const url = `${this.apiBase}/logout`;
      const headers = new HttpHeaders().set('Authorization', `Bearer ${this.getAccessToken() || ''}`);
      this.http.post(url, {}, { headers }).subscribe({ error: () => { } });
    } catch { }

    localStorage.removeItem(CommonConstants.AuthorizationToken);
    this.currentUserSubject.next(null);
    this.router.navigateByUrl(redirectTo);
  }

  isAuthenticated(): boolean {
    return !!this.getAccessToken();
  }

  getAccessToken(): string | null {
    return localStorage.getItem(CommonConstants.AuthorizationToken);
  }

  refreshToken(): Observable<AuthResponse | null> {
    const url = `${this.apiBase}/refresh`;
    const token = this.getAccessToken();
    if (!token) return of(null);

    return this.http.post<AuthResponse>(url, { token }).pipe(
      tap(res => this.saveAuth(res)),
      catchError(() => of(null))
    );
  }

  private saveAuth(res: AuthResponse): User {

    localStorage.setItem(CommonConstants.AuthorizationToken, res.accessToken);
    localStorage.setItem(CommonConstants.RefreshToken, res.refreshToken);

    const user = this.parseClaims(res.accessToken);
    this.currentUserSubject.next(this.parseClaims(res.accessToken));
    return user;
  }

  private parseClaims(accessToken: string): AccountInfo {
    const userInfoTokenPart = accessToken.split('.')[1];
    const decodedUserInfo = atob(userInfoTokenPart);
    const tokenPaiload: UserTokenPaiload = JSON.parse(decodedUserInfo);
    
    const user: AccountInfo = {
      id : Number.parseInt(tokenPaiload.sub, 10),
      email: tokenPaiload.email,
      name: tokenPaiload.name,
      roles: tokenPaiload.role,
      expireAt: new Date(tokenPaiload.exp * 1000)
    };

    localStorage.setItem(CommonConstants.RefreshTokenExpiresAt, user.expireAt.getTime().toString());
    return user;
  }

  private getAccountInfo(): AccountInfo | null {
    const token = localStorage.getItem(CommonConstants.AuthorizationToken);
    if (!token) {
      return null;
    }

    return this.parseClaims(token);
  }

   isAccessTokenExpired(): boolean {
    const user = this.getAccountInfo();

    if (!user) {
      return false;
    }

    const now = new Date();
    const timeLeftMs = user.expireAt.getTime() - now.getTime();

    return timeLeftMs <= this.diffToRefreshMs;
  }

  isRefreshTokenExpired(): boolean {
     const expireAt = this.getRefreshTokenExpiresAt();

    if (!expireAt) {
      return false;
    }

    const now = new Date();
    const timeLeftMs = expireAt.getTime() - now.getTime();

    return timeLeftMs <= this.diffToRefreshMs;
  }

  private getRefreshTokenExpiresAt() {
     const expireAt = localStorage.getItem(CommonConstants.RefreshTokenExpiresAt);
    return expireAt
        ? new Date(Number(expireAt))
        : null;
  }
}
