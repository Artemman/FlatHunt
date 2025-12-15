import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { FlatsService } from '../services/flats.service';
import { FlatDto, FlatFilterRequest } from '../models/flat.model';
import { merge, Subject, debounceTime, distinctUntilChanged, switchMap, startWith, takeUntil, catchError, of, finalize } from 'rxjs';
import { CitiesService, CityDto } from '../services/cities.service';

@Component({
  selector: 'app-flats',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule
  ],
  templateUrl: './flats.component.html',
  styleUrls: ['./flats.component.scss']
})
export class FlatsComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  displayedColumns = ['title', 'address', 'city', 'rooms', 'area', 'price', 'createdAt', 'externalId'];
  data: FlatDto[] = [];
  totalCount = 0;
  pageSize = 20;
  pageIndex = 0;
  isLoading = false;
  error: string | null = null;
  cities: CityDto[] = [];
  citiesLoading = false;

  filterForm: FormGroup<any>;

  private refresh$ = new Subject<void>();
  private destroy$ = new Subject<void>();

  constructor(private fb: FormBuilder, private service: FlatsService, private citiesService: CitiesService) {
    this.filterForm = this.fb.group({
    search: [''],
    cityId: [null],
    rooms: [null],
    minPrice: [null],
    maxPrice: [null],
    minArea: [null],
    maxArea: [null],
    sortBy: ['createdAt'],
    sortDir: ['desc']
  });
  }

  ngOnInit(): void {
    this.filterForm.valueChanges
      .pipe(
        debounceTime(350),
        distinctUntilChanged((a, b) => JSON.stringify(a) === JSON.stringify(b)),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.pageIndex = 0;
        this.paginator?.firstPage?.();
        this.refresh$.next();
      });

      this.loadCities();
  }

  ngAfterViewInit(): void {
    const paginator$ = this.paginator.page.pipe(startWith({ pageIndex: this.pageIndex, pageSize: this.pageSize }));
    
    merge(this.refresh$, paginator$)
      .pipe(
        takeUntil(this.destroy$),
        switchMap((evt) => {
          this.isLoading = true;
          this.error = null;
          
          const pe = evt as PageEvent | undefined;
          if (pe?.pageIndex != null) {
            this.pageIndex = pe.pageIndex;
            this.pageSize = pe.pageSize;
          }
          
          const f = this.buildFilterRequest();

          return this.service.getFlats(f).pipe(
            catchError(err => {
              this.error = 'Failed to load flats. Please try again.';
              return of(null);
            }),
            finalize(() => {
              this.isLoading = false;
            })
          );
        })
      )
      .subscribe((res) => {
        if (!res) {
          this.data = [];
          this.totalCount = 0;
          return;
        }
        this.data = res.items;
        this.totalCount = res.totalCount;
      });
  }

  private buildFilterRequest(): FlatFilterRequest {
    const raw = this.filterForm.value;
    return {
      page: this.pageIndex + 1,
      pageSize: this.pageSize,
      cityId: raw.cityId ? Number(raw.cityId) : undefined,
      rooms: raw.rooms ? Number(raw.rooms) : undefined,
      minPrice: raw.minPrice != null ? Number(raw.minPrice) : undefined,
      maxPrice: raw.maxPrice != null ? Number(raw.maxPrice) : undefined,
      minArea: raw.minArea != null ? Number(raw.minArea) : undefined,
      maxArea: raw.maxArea != null ? Number(raw.maxArea) : undefined,
      search: raw.search?.trim() || undefined,
      sortBy: raw.sortBy || undefined,
      sortDir: raw.sortDir || undefined
    };
  }

  clearFilters(): void {
    this.filterForm.reset({
      search: '',
      cityId: null,
      rooms: null,
      minPrice: null,
      maxPrice: null,
      minArea: null,
      maxArea: null,
      sortBy: 'createdAt',
      sortDir: 'desc'
    });
  }

  retry(): void {
    this.refresh$.next();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadCities(): void {
    this.citiesLoading = true;
    this.citiesService.getCities()
      .pipe(
        takeUntil(this.destroy$),
        catchError(() => {
          return of([]);
        }),
        finalize(() => (this.citiesLoading = false))
      )
      .subscribe((c) => {
        this.cities = c || [];
      });
  } 
}
