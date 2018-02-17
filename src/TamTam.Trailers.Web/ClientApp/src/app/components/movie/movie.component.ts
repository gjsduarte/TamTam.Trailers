import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";

import { MovieService, VideoService } from "../../services";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-movie',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.scss']
})
export class MovieComponent implements OnInit {
  public movie: Movie;
  public videos: Video[];

  constructor(private route: ActivatedRoute,
              private movieService: MovieService,
              private videoService: VideoService,
              private modalService: NgbModal) {
  }

  public async ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.movie = await this.movieService.get(id);
    this.videos = await this.videoService.get(this.movie.imdbId);
  }

  public open(content) {
    this.modalService.open(content, { size: "lg" });
  }
}
