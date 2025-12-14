import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthInterceptor implements HttpInterceptor {

   authEndpoints: string[] = [
    '/refresh',
    '/logout',
    '/login'
  ];

  constructor(
    private authService: AuthService
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.isAuthEndpoint(req.url)) {
      return next.handle(req);
    }

    if (!this.authService.isAccessTokenExpired()
     && !this.authService.isRefreshTokenExpired()) {
     const accessToken = this.authService.getAccessToken()!;
      req = this.addTokenHeader(req, accessToken);
      return next.handle(req);
    }

    //todo add refresh token
    return next.handle(req);
   
  }

  private isAuthEndpoint(url: string): boolean {
    return !!this.authEndpoints.find(x => x.includes(url));
  }

  private addTokenHeader(request: HttpRequest<any>, token: string): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }
}
