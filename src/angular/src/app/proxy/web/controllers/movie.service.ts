import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { IActionResult } from '../../microsoft/asp-net-core/mvc/models';

@Injectable({
  providedIn: 'root',
})
export class MovieService {
  apiName = 'Default';

  counters = () =>
    this.restService.request<any, IActionResult>({
      method: 'GET',
      url: '/Widgets/Counters',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
