import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export enum FlatSourceType {
  Lun = 1,
  FlatFy = 2
}

export interface FlatSyncFilter {
  roomCount?: number;
  cityId?: number;
  createdFrom?: string; // ISO string
  createdTo?: string;   
  page?: number;
  pageSize?: number;
  flatSourceType?: FlatSourceType;
}

@Injectable({
  providedIn: 'root'
})
export class FlatSyncService {
  private readonly api = '/api/flat-sync'; 

  constructor(private http: HttpClient) {}

  syncFlats(filter: FlatSyncFilter): Observable<number> {
    return this.http.post<number>(this.api, filter);
  }
}
