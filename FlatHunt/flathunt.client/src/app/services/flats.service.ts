import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FlatDto, FlatFilterRequest, PagedResult } from '../models/flat.model';

@Injectable({
  providedIn: 'root'
})
export class FlatsService {
  private readonly api = '/api/flats';

  constructor(private http: HttpClient) {}

  getFlats(filter: FlatFilterRequest): Observable<PagedResult<FlatDto>> {
    let params = new HttpParams();

    if (filter.page != null) params = params.set('page', String(filter.page));
    if (filter.pageSize != null) params = params.set('pageSize', String(filter.pageSize));
    if (filter.cityId != null) params = params.set('cityId', String(filter.cityId));
    if (filter.rooms != null) params = params.set('rooms', String(filter.rooms));
    if (filter.minPrice != null) params = params.set('minPrice', String(filter.minPrice));
    if (filter.maxPrice != null) params = params.set('maxPrice', String(filter.maxPrice));
    if (filter.minArea != null) params = params.set('minArea', String(filter.minArea));
    if (filter.maxArea != null) params = params.set('maxArea', String(filter.maxArea));
    if (filter.search) params = params.set('search', filter.search);
    if (filter.sortBy) params = params.set('sortBy', filter.sortBy);
    if (filter.sortDir) params = params.set('sortDir', filter.sortDir);

    return this.http.get<PagedResult<FlatDto>>(this.api, { params });
  }
}
