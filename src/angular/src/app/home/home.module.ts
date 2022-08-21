import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { PostComponent } from './post/post.component';

@NgModule({
  declarations: [HomeComponent,PostComponent],
  imports: [SharedModule, HomeRoutingModule],
})
export class HomeModule {}
