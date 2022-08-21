import { AuthService, PagedResultDto, ConfigStateService, CurrentUserDto } from '@abp/ng.core';
import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { PostDto, PostService } from '@proxy/posts';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {

  
  maxCount = 5
  skipCount = 0;
  isLoading = false;
  noData = false;
  noMoreData = false;
  currentUser: CurrentUserDto;
  post = { items: [], totalCount: 0 } as PagedResultDto<PostDto>;
  
  @HostListener('window:scroll', ['$event'])
  onScroll($event) {

    if (this.noMoreData || this.isLoading) return;
    
    let offset = document.documentElement.scrollTop + window.innerHeight;
    let height = document.documentElement.offsetHeight;

    if (offset >= height) {
      this.loadPosts(this.currentUser);
    }

  }


  get hasLoggedIn(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  constructor(
    private oAuthService: OAuthService,
    private authService: AuthService,
    private postService: PostService,
    private configStateService: ConfigStateService) { }

  ngOnInit(): void {
    this.currentUser = this.configStateService.getOne('currentUser') as CurrentUserDto;
    this.loadPosts(this.currentUser);
  }

  login() {
    this.authService.navigateToLogin();
  }

  loadPosts(currentUser: CurrentUserDto) {
    this.isLoading = true;
    
    this.postService.getFeed({ userId: currentUser.id, maxCount: this.maxCount, skipCount: this.skipCount }).subscribe(res => {
      this.post.items.push(...res.items);

      this.post.totalCount = res.totalCount
      this.skipCount += res.items.length;

      if (res.totalCount == 0) {
        this.noData = true

      } else if (res.items.length < this.maxCount) {

        this.noMoreData = true;
      }
      this.isLoading = false;
    });
  }

}


