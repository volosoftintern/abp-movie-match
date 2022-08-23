import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { IActionResult } from '../microsoft/asp-net-core/mvc/models';

@Injectable({
  providedIn: 'root',
})
export class FileService {
  apiName = 'Default';

  download = (fileName: string) =>
    this.restService.request<any, IActionResult>({
      method: 'GET',
      url: `/download/${fileName}`,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
