import type { EntityDto } from '@abp/ng.core';

export interface CreateUpdateWatchLaterDto {
  movieId: number;
  userId?: string;
}

export interface WatchLaterDto extends EntityDto<string> {
  userId?: string;
  movieId: number;
}
