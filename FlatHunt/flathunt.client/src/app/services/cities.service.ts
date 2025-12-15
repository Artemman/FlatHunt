import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CityDto {
  id: number;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class CitiesService {
  private readonly api = '/api/cities';

  constructor(private http: HttpClient) {}

  getCities(): Observable<CityDto[]> {
    return this.http.get<CityDto[]>(this.api);
  }
}