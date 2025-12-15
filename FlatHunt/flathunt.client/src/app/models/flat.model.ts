export interface FlatDto {
  id: number;
  title?: string | null;
  address?: string | null;
  city?: number | null;
  rooms: number;
  area?: number | null;
  price?: number | null;
  currency?: string | null;
  isActive: boolean;
  createdAt?: string | null;
  externalId?: string | null;
}

export interface FlatFilterRequest {
  page?: number;
  pageSize?: number;
  cityId?: number | null;
  rooms?: number | null;
  minPrice?: number | null;
  maxPrice?: number | null;
  minArea?: number | null;
  maxArea?: number | null;
  search?: string | null;
  sortBy?: string | null;
  sortDir?: 'asc' | 'desc' | null;
}

export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
