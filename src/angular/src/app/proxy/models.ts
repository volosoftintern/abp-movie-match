import type { IRemoteStreamContent } from './volo/abp/content/models';
import type { GetIdentityUsersInput } from './volo/abp/identity/models';

export interface BlobDto {
  content: number[];
  name?: string;
}

export interface GetBlobRequestDto {
  name: string;
}

export interface SaveBlobInputDto {
  content: IRemoteStreamContent;
  name: string;
}

export interface GetUsersFollowInfo extends GetIdentityUsersInput {
  username?: string;
}

export interface UserInformationDto {
  followingCount: number;
  followersCount: number;
  path?: string;
  username?: string;
}
