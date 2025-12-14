import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable()
export class ApiUrlInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (request.url.includes('api/')
      && !request.url.startsWith('https://')) {
      request = this.replaceDomain(request);
    }

    return next.handle(request);
  }

  replaceDomain(request: HttpRequest<any>): HttpRequest<any> {
    const index = request.url.indexOf('api/');
    const url = request.url.substring(index);
    return request.clone({
      url: `https://${environment.apiBaseUrl}/${url}`
    });
  }
}
