import { CoreModule } from '@abp/ng.core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgModule } from '@angular/core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { TimeDifferPipe } from './timeDiffer/time-differ.pipe';
import { PostComponent } from "./post/post.component";
@NgModule({
  declarations: [
    TimeDifferPipe,
    PostComponent
  ],
  imports: [
    CoreModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
  ],
  exports: [
    CoreModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    TimeDifferPipe,
    PostComponent
  ],
  providers: []
})
export class SharedModule {}
