import type { CreateMovieDto, MovieDetailDto, MovieDto, PersonDto } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MovieService {
  apiName = 'Default';

  any = (id: number) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: `/api/app/movie/${id}/any`,
    },
    { apiName: this.apiName });

  create = (input: CreateMovieDto) =>
    this.restService.request<any, MovieDto>({
      method: 'POST',
      url: '/api/app/movie',
      body: input,
    },
    { apiName: this.apiName });

  deleteMoviesWatchLater = (id: number) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/movie/${id}/movies-watch-later`,
    },
    { apiName: this.apiName });

  deleteMoviesWatchedBefore = (id: number) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/movie/${id}/movies-watched-before`,
    },
    { apiName: this.apiName });

  get = (id: number) =>
    this.restService.request<any, MovieDetailDto>({
      method: 'GET',
      url: `/api/app/movie/${id}`,
    },
    { apiName: this.apiName });

  getFromDb = (id: number) =>
    this.restService.request<any, MovieDto>({
      method: 'GET',
      url: `/api/app/movie/${id}/from-db`,
    },
    { apiName: this.apiName });

  getMovie = (id: number) =>
    this.restService.request<any, MovieDto>({
      method: 'GET',
      url: `/api/app/movie/${id}/movie`,
    },
    { apiName: this.apiName });

  getPerson = (personId: number, isDirector?: boolean) =>
    this.restService.request<any, PersonDto>({
      method: 'GET',
      url: `/api/app/movie/person/${personId}`,
      params: { isDirector },
    },
    { apiName: this.apiName });

  getWatchLaterList = (input: PagedAndSortedResultRequestDto) =>
    this.restService.request<any, PagedResultDto<MovieDto>>({
      method: 'GET',
      url: '/api/app/movie/watch-later-list',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName });

  getWatchedBeforeList = (input: PagedAndSortedResultRequestDto) =>
    this.restService.request<any, PagedResultDto<MovieDto>>({
      method: 'GET',
      url: '/api/app/movie/watched-before-list',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
