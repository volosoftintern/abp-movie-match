import { Component, Input, OnInit } from '@angular/core';
import { PostDto } from '@proxy/posts';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent implements OnInit {
  imgPath = "https://image.tmdb.org/t/p/original/";
  @Input() post:PostDto;

  constructor() { }

  ngOnInit(): void {
  }

}
