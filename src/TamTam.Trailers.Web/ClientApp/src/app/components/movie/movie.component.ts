import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";

import { NgbModal } from "@ng-bootstrap/ng-bootstrap";

import { MovieService, VideoService } from "../../services";
import { TrailerComponent } from "../trailer/trailer.component";

@Component({
  selector: 'app-movie',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.scss']
})
export class MovieComponent implements OnInit {
  public movie: Movie;
  public videos: Video[];

  constructor(private route: ActivatedRoute,
              private modalService: NgbModal,
              private movieService: MovieService,
              private videoService: VideoService) {
  }

  public async ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.movie = await this.movieService.get(id);
    this.videos = await this.videoService.get(this.movie.imdbId);
  }

  public openVideo(video: Video) {
    const modal = this.modalService.open(TrailerComponent, { size: "lg" });
    modal.componentInstance.video = video;
  }
}
