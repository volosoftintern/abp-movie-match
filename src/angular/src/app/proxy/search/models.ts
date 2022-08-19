
export interface SearchMovieDto {
  name?: string;
  currentPage: number;
}

export interface SearchResponseDto<T> {
  page: number;
  totalResults: number;
  totalPages: number;
  results: T[];
}
