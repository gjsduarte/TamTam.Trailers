import { Component, Inject, Input, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from "@angular/router";
import {MovieService} from "../../services";

@Component({
  selector: 'app-movie',
  templateUrl: './movie.component.html'
})
export class MovieComponent implements OnInit {
  @Input() movie: Movie;

  constructor(private route: ActivatedRoute, private service: MovieService) {

  }

  async ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.movie = await this.service.get(id);
  }
}
