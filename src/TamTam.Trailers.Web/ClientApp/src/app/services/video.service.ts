﻿import { Inject, Injectable } from "@angular/core";

import { HttpClient } from '@angular/common/http';

@Injectable()
export class VideoService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  public get(id: string): Promise<Video[]> {
    return this.http.get<Video[]>(`${this.baseUrl}api/Videos/${id}`).toPromise();
  }

  public getImageUrl(video: Video): string {
    return `http://img.youtube.com/vi/${video.key}/default.jpg`
  }

  public getVideoUrl(video: Video): string {
    return `https://www.youtube.com/embed/${video.key}`
  }

}
