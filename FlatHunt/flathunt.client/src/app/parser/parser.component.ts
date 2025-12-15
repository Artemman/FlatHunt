import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Subject, takeUntil, finalize, catchError, of } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { FlatSyncService, FlatSourceType, FlatSyncFilter } from '../services/flat-sync.service';
import { RouterModule } from '@angular/router';
import { MatDividerModule } from '@angular/material/divider';
import { CityDto, CitiesService } from '../services/cities.service';

@Component({
  selector: 'app-parser',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDividerModule
  ],
  templateUrl: './parser.component.html',
  styleUrls: ['./parser.component.scss']
})
export class ParserComponent implements OnInit, OnDestroy {

  form: FormGroup<any>;

  running = false;
  private destroy$ = new Subject<void>();

  lastCount: number | null = null;
  lastRun: Date | null = null;

  cities: CityDto[] = [];
  citiesLoading = false;

  readonly sources = [
    { value: FlatSourceType.Lun.toString(), label: 'Lun' },
    { value: FlatSourceType.FlatFy.toString(), label: 'FlatFy' }
  ];

  ngOnInit(): void {
    this.loadCities();
  }

  constructor(
    private fb: FormBuilder,
    private syncService: FlatSyncService,
    private snackBar: MatSnackBar,
    private citiesService: CitiesService
  ) {
    this.form = this.fb.group({
      source: ['Lun'], // Lun | FlatFy
      cityId: [null],
      roomCount: [1],
      createdFrom: [new Date()],
      createdTo: [null]
    });
  }

  startParsing(): void {
    if (this.running) return;

    this.form.markAllAsTouched();

    const raw = this.form.value;
    const filter: FlatSyncFilter = {
      roomCount: raw.roomCount ?? 1,
      cityId: raw.cityId ?? undefined,
      page: 1,
      pageSize: 100
    };

    if (raw.createdFrom) {
      filter.createdFrom = (raw.createdFrom as Date).toISOString();
    }
    if (raw.createdTo) {
      filter.createdTo = (raw.createdTo as Date).toISOString();
    }

    if (raw.source) {
      filter.flatSourceType = Number(raw.source) as FlatSourceType;
    }

    this.running = true;

    this.snackBar.open('Parsing started…', undefined, { duration: 2500 });

    this.syncService.syncFlats(filter)
      .pipe(
        takeUntil(this.destroy$),
        catchError((err) => {
          const message = err?.message || (err?.error?.message) || 'Unknown error';
          this.snackBar.open(`Parsing failed: ${message}`, 'Close', { duration: 7000 });
          return of(null);
        }),
        finalize(() => {
          this.running = false;
        })
      )
      .subscribe((count) => {
        if (count == null) return;
        this.lastCount = count;
        this.lastRun = new Date();
        this.snackBar.open(`Parsing completed. ${count} items.`, 'View', { duration: 5000 });
      });
  }

  clear(): void {
    this.form.reset({
      source: 'Lun',
      cityId: null,
      roomCount: 1,
      createdFrom: null,
      createdTo: null
    });
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
          // keep UI friendly if cities cannot be loaded
          this.snackBar.open('Failed to load cities', 'Close', { duration: 4000 });
          return of([]);
        }),
        finalize(() => (this.citiesLoading = false))
      )
      .subscribe((c) => {
        this.cities = c || [];
      });
  }
}
