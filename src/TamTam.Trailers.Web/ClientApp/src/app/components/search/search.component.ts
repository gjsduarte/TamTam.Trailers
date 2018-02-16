import { Component, OnInit } from '@angular/core';

import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';

import { MovieService } from "../../services";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html'
})
export class SearchComponent implements OnInit {
  public movies: Movie[];
  public query$ = new Subject<string>();
  public loading = false;

  constructor(private service: MovieService) {
    this.query$
      .debounceTime(500)
      .distinctUntilChanged()
      .subscribe(query => this.getMovies(query));
  }

  async ngOnInit() {
    await this.getMovies();
  }

  private async getMovies(query: string = "Star Wars") {
    this.loading = true;
    this.movies = await this.service.search(query);
    this.loading = false;
  }
}
