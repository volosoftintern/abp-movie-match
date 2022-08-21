import type { CreateUpdateWatchedBeforeDto, WatchedBeforeDto } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { IdentityUserDto } from '../volo/abp/identity/models';

@Injectable({
  providedIn: 'root',
})
export class WatchedBeforeService {
  apiName = 'Default';

  create = (input: CreateUpdateWatchedBeforeDto) =>
    this.restService.request<any, WatchedBeforeDto>({
      method: 'POST',
      url: '/api/app/watched-before',
      body: input,
    },
    { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/watched-before/${id}`,
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, WatchedBeforeDto>({
      method: 'GET',
      url: `/api/app/watched-before/${id}`,
    },
    { apiName: this.apiName });

  getCount = (id: string) =>
    this.restService.request<any, number>({
      method: 'GET',
      url: `/api/app/watched-before/${id}/count`,
    },
    { apiName: this.apiName });

  getList = (input: PagedAndSortedResultRequestDto) =>
    this.restService.request<any, PagedResultDto<WatchedBeforeDto>>({
      method: 'GET',
      url: '/api/app/watched-before',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName });

  listOfUsersByMovieId = (movieId: number) =>
    this.restService.request<any, IdentityUserDto[]>({
      method: 'POST',
      url: `/api/app/watched-before/list-of-users/${movieId}`,
    },
    { apiName: this.apiName });

  update = (id: string, input: CreateUpdateWatchedBeforeDto) =>
    this.restService.request<any, WatchedBeforeDto>({
      method: 'PUT',
      url: `/api/app/watched-before/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
