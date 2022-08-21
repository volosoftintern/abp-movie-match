import type { BlobDto, GetBlobRequestDto, SaveBlobInputDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class FileService {
  apiName = 'Default';

  getBlob = (input: GetBlobRequestDto) =>
    this.restService.request<any, BlobDto>({
      method: 'GET',
      url: '/api/app/file/blob',
      params: { name: input.name },
    },
    { apiName: this.apiName });

  saveBlob = (input: SaveBlobInputDto) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/file/save-blob',
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
