import { AuthService, PagedResultDto, ConfigStateService, CurrentUserDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { PostDto, PostService } from '@proxy/posts';
import { OAuthService } from 'angular-oauth2-oidc';
import { IdentityUserService } from '@abp/ng.identity/proxy';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  post = { items: [], totalCount: 0 } as PagedResultDto<PostDto>;
  
  get hasLoggedIn(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  constructor(
    private oAuthService: OAuthService,
    private authService: AuthService, private postService: PostService,
    private identityService: IdentityUserService, private configStateService: ConfigStateService) { }

  ngOnInit(): void {
    const currentUser = this.configStateService.getOne('currentUser') as CurrentUserDto;
    this.postService.getFeed({ userId: currentUser.id, maxCount: 10, skipCount: 0 }).subscribe(res => this.post = res);

  }

  login() {
    this.authService.navigateToLogin();
  }

  
}


