import { Component, OnInit } from '@angular/core';

import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';

import { SearchService } from "../../services";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html'
})
export class SearchComponent implements OnInit {
  public movies: Movie[];
  public query$ = new Subject<string>();
  public loading = false;

  constructor(private service: SearchService) {
    this.query$
      .debounceTime(500)
      .distinctUntilChanged()
      .subscribe(() => this.getMovies());
  }

  async ngOnInit() {
    await this.getMovies();
  }

  private async getMovies() {
    this.loading = true;
    this.movies = await this.service.search();
    this.loading = false;
  }
}
