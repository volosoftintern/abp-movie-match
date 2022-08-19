import { AuthService, PagedResultDto, ConfigStateService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { PostDto, PostService } from '@proxy/posts';
import { OAuthService } from 'angular-oauth2-oidc';
import { IdentityUserService, IdentityUserDto } from '@abp/ng.identity/proxy';
import { switchMap } from "rxjs/operators";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  post = { items: [], totalCount: 0 } as PagedResultDto<PostDto>;
  imgPath="https://image.tmdb.org/t/p/original/";
  get hasLoggedIn(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  constructor(
    private oAuthService: OAuthService,
    private authService: AuthService, private postService: PostService,
    private identityService: IdentityUserService, private configStateService: ConfigStateService) { }

  ngOnInit(): void {
    let currentUser=this.configStateService.getOne('currentUser');
    this.postService.getFeed({userId:currentUser.id,maxCount:10,skipCount:0}).subscribe(res=>this.post=res);
  
  }

  login() {
    this.authService.navigateToLogin();
  }

  getTimeDiffer(creationTime: number): string {

    const date = new Date(creationTime);
    const currentTime = new Date();
    const diff = currentTime.getTime() - date.getTime();
    const minute = 60 * 1000
    const hour = 60 * minute;
    const day = 24 * hour;
    const month = 30 * day;

    if (diff < hour) {
      return `${Math.floor(diff / minute)}m`;
    } else if (diff < day) {
      return `${Math.floor(diff / hour)}h`;
    } else if (diff < month) {
      return `${Math.floor(diff / day)}d`;
    }

    return date.toLocaleDateString();

  }
}


