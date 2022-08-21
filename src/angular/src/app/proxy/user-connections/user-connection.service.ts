import type { FollowerDto } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { IFormFile } from '../microsoft/asp-net-core/http/models';
import type { GetUsersFollowInfo, UserInformationDto } from '../models';
import type { GetIdentityUsersInput, IdentityUserDto } from '../volo/abp/identity/models';

@Injectable({
  providedIn: 'root',
})
export class UserConnectionService {
  apiName = 'Default';

  follow = (id: string, isActive: boolean) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/user-connection/${id}/follow`,
      params: { isActive },
    },
    { apiName: this.apiName });

  getFirst = () =>
    this.restService.request<any, string[]>({
      method: 'GET',
      url: '/api/app/user-connection/first',
    },
    { apiName: this.apiName });

  getFollowers = (input: GetUsersFollowInfo) =>
    this.restService.request<any, PagedResultDto<FollowerDto>>({
      method: 'GET',
      url: '/api/app/user-connection/followers',
      params: { username: input.username, filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });

  getFollowersCountByUserName = (userName: string) =>
    this.restService.request<any, number>({
      method: 'GET',
      url: '/api/app/user-connection/followers-count',
      params: { userName },
    },
    { apiName: this.apiName });

  getFollowing = (input: GetUsersFollowInfo) =>
    this.restService.request<any, PagedResultDto<FollowerDto>>({
      method: 'GET',
      url: '/api/app/user-connection/following',
      params: { username: input.username, filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });

  getFollowingCountByUserName = (userName: string) =>
    this.restService.request<any, number>({
      method: 'GET',
      url: '/api/app/user-connection/following-count',
      params: { userName },
    },
    { apiName: this.apiName });

  getList = (input: GetIdentityUsersInput) =>
    this.restService.request<any, PagedResultDto<IdentityUserDto>>({
      method: 'GET',
      url: '/api/app/user-connection',
      params: { filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });

  getPhoto = (userName: string) =>
    this.restService.request<any, string>({
      method: 'GET',
      responseType: 'text',
      url: '/api/app/user-connection/photo',
      params: { userName },
    },
    { apiName: this.apiName });

  getUserInfo = (username: string) =>
    this.restService.request<any, UserInformationDto>({
      method: 'GET',
      url: '/api/app/user-connection/user-info',
      params: { username },
    },
    { apiName: this.apiName });

  getisFollow = (userName: string) =>
    this.restService.request<any, boolean>({
      method: 'GET',
      url: '/api/app/user-connection/is-follow',
      params: { userName },
    },
    { apiName: this.apiName });

  setPhoto = (userName: string, name: string) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/user-connection/set-photo',
      params: { userName, name },
    },
    { apiName: this.apiName });

  setisFollow = (userName: string, isFollow: boolean) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/user-connection/setis-follow',
      params: { userName, isFollow },
    },
    { apiName: this.apiName });

  unFollow = (id: string, isActive: boolean) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/user-connection/${id}/un-follow`,
      params: { isActive },
    },
    { apiName: this.apiName });

  upload = (file: IFormFile) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/user-connection/upload',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
