import { Inject, Injectable } from "@angular/core";

import { HttpClient } from '@angular/common/http';

@Injectable()
export class MovieService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  public async search(query: string): Promise<Movie[]> {
    if (!query.trim()) {
      // if not search term, return empty movie array.
      return [];
    }

    return this.http.get<Movie[]>(`${this.baseUrl}api/Movies/Search`, { params: { query: query } })
      .toPromise();
  }

  public get(id: string): Promise<Movie> {
    return this.http.get<Movie>(`${this.baseUrl}api/Movies/${id}`).toPromise();
  }
}
