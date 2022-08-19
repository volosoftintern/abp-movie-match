import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { IActionResult } from '../../../../../microsoft/asp-net-core/mvc/models';

@Injectable({
  providedIn: 'root',
})
export class CmsKitPublicGlobalResourcesService {
  apiName = 'Default';

  getGlobalScript = () =>
    this.restService.request<any, IActionResult>({
      method: 'GET',
      url: '/cms-kit/global-resources/script',
    },
    { apiName: this.apiName });

  getGlobalStyle = () =>
    this.restService.request<any, IActionResult>({
      method: 'GET',
      url: '/cms-kit/global-resources/style',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
