import type { IFormFile } from './microsoft/asp-net-core/http/models';
import type { FileResult } from './microsoft/asp-net-core/mvc/models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ProfilePictureService {
  apiName = 'Default';

  get = () =>
    this.restService.request<any, FileResult>({
      method: 'GET',
      url: '/api/app/profile-picture',
    },
    { apiName: this.apiName });

  upload = (file: IFormFile) =>
    this.restService.request<any, string>({
      method: 'POST',
      responseType: 'text',
      url: '/api/app/profile-picture/upload',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
