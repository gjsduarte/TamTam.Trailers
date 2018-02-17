import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { CounterComponent, MovieComponent, NavMenuComponent, SearchComponent  } from './components';
import { MovieService, SearchService, VideoService } from "./services";
import { SafePipe } from "./pipes";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    CounterComponent,
    SearchComponent,
    MovieComponent,
    SafePipe
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: SearchComponent, pathMatch: 'full' },
      { path: 'movie/:id', component: MovieComponent },
      { path: 'counter', component: CounterComponent }
    ]),
    NgbModule.forRoot()
  ],
  providers: [MovieService, SearchService, VideoService],
  bootstrap: [AppComponent]
})
export class AppModule { }
