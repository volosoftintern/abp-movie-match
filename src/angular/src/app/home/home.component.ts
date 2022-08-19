import { AuthService, PagedResultDto, ConfigStateService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { PostDto, PostService } from '@proxy/posts';
import { OAuthService } from 'angular-oauth2-oidc';
import { IdentityUserService, IdentityUserDto } from '@abp/ng.identity/proxy';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent{

  get hasLoggedIn(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  constructor(
    private oAuthService: OAuthService,
    private authService: AuthService) { }


  login() {
    this.authService.navigateToLogin();
  }
}


