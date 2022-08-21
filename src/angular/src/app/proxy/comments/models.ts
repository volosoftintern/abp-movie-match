import type { CmsUserDto } from '../volo/cms-kit/public/comments/models';

export interface CommentDto {
  id?: string;
  entityType?: string;
  entityId?: string;
  text?: string;
  repliedCommentId?: string;
  creatorId?: string;
  creationTime?: string;
  author: CmsUserDto;
  replies: CommentDto[];
  concurrencyStamp?: string;
}

export interface CommentWithStarsDto {
  id?: string;
  entityType?: string;
  entityId?: string;
  text?: string;
  creatorId?: string;
  creationTime?: string;
  replies: CommentDto[];
  author: CmsUserDto;
  concurrencyStamp?: string;
  starsCount: number;
  pageNumber: number;
}
