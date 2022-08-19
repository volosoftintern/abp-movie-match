import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CommentWithStarsDto } from '../comments/models';
import type { CreateUpdateRatingInput, RatingDto, RatingWithStarCountDto } from '../volo/cms-kit/public/ratings/models';

@Injectable({
  providedIn: 'root',
})
export class RatingPublicService {
  apiName = 'Default';

  create = (entityType: string, entityId: string, input: CreateUpdateRatingInput) =>
    this.restService.request<any, RatingDto>({
      method: 'POST',
      url: '/api/app/rating-public',
      params: { entityType, entityId },
      body: input,
    },
    { apiName: this.apiName });

  delete = (entityType: string, entityId: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/app/rating-public',
      params: { entityType, entityId },
    },
    { apiName: this.apiName });

  getCommentsWithRating = (entityType: string, entityId: string) =>
    this.restService.request<any, CommentWithStarsDto[]>({
      method: 'GET',
      url: `/api/app/rating-public/comments-with-rating/${entityId}`,
      params: { entityType },
    },
    { apiName: this.apiName });

  getGroupedStarCounts = (entityType: string, entityId: string) =>
    this.restService.request<any, RatingWithStarCountDto[]>({
      method: 'GET',
      url: `/api/app/rating-public/grouped-star-counts/${entityId}`,
      params: { entityType },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
