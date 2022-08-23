import { Component, Input, OnInit } from '@angular/core';
import { PostDto } from '@proxy/posts';
import { imagePath } from 'src/environments/environment';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent implements OnInit {
  
  imgPath=imagePath;
  @Input() post:PostDto;
  
  constructor() { }

  ngOnInit(): void {
  }

}
