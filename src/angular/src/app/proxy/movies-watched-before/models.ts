import type { EntityDto } from '@abp/ng.core';

export interface CreateUpdateWatchedBeforeDto {
  movieId: number;
  userId?: string;
}

export interface WatchedBeforeDto extends EntityDto<string> {
  movieId: number;
  userId?: string;
}
