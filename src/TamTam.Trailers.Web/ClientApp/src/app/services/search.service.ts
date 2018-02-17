import { Injectable } from "@angular/core";

import { MovieService } from "./movie.service";

@Injectable()
export class SearchService {

  public query: string;

  constructor(private service: MovieService) {
    this.query = "Star Wars";
  }

  public async search(): Promise<Movie[]> {
    return this.service.search(this.query);
  }

}
