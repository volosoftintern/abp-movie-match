import type { PostDto, PostFeedDto } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PostService {
  apiName = 'Default';

  getFeed = (input: PostFeedDto) =>
    this.restService.request<any, PagedResultDto<PostDto>>({
      method: 'GET',
      url: '/api/app/post/feed',
      params: { userId: input.userId, maxCount: input.maxCount, skipCount: input.skipCount },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
