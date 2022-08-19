import type { MovieDto } from '../movies/models';
import type { IdentityUserDto } from '../volo/abp/identity/models';

export interface PostDto {
  comment?: string;
  movieId: number;
  userId?: string;
  rate: number;
  movie: MovieDto;
  user: IdentityUserDto;
  creationTime?: string;
}

export interface PostFeedDto {
  userId?: string;
  maxCount: number;
  skipCount: number;
}
