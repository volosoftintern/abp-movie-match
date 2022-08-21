import type { CreateUpdateWatchLaterDto, WatchLaterDto } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class WatchLaterService {
  apiName = 'Default';

  create = (input: CreateUpdateWatchLaterDto) =>
    this.restService.request<any, WatchLaterDto>({
      method: 'POST',
      url: '/api/app/watch-later',
      body: input,
    },
    { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/watch-later/${id}`,
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, WatchLaterDto>({
      method: 'GET',
      url: `/api/app/watch-later/${id}`,
    },
    { apiName: this.apiName });

  getCount = (id: string) =>
    this.restService.request<any, number>({
      method: 'GET',
      url: `/api/app/watch-later/${id}/count`,
    },
    { apiName: this.apiName });

  getList = (input: PagedAndSortedResultRequestDto) =>
    this.restService.request<any, PagedResultDto<WatchLaterDto>>({
      method: 'GET',
      url: '/api/app/watch-later',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName });

  update = (id: string, input: CreateUpdateWatchLaterDto) =>
    this.restService.request<any, WatchLaterDto>({
      method: 'PUT',
      url: `/api/app/watch-later/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
