import type { SearchMovieDto, SearchResponseDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { MovieDto } from '../movies/models';

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  apiName = 'Default';

  getMoviesByInput = (input: SearchMovieDto) =>
    this.restService.request<any, SearchResponseDto<MovieDto>>({
      method: 'GET',
      url: '/api/app/search/movies',
      params: { name: input.name, currentPage: input.currentPage },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
