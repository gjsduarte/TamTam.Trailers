import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router'
  ;
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ShareModule } from '@ngx-share/core';

import { AppComponent } from './app.component';
import { MovieComponent, NavMenuComponent, SearchComponent, TrailerComponent } from './components';
import { MovieService, SearchService, VideoService } from "./services";
import { SafePipe } from "./pipes";

@NgModule({
  declarations: [
    AppComponent,
    SearchComponent,
    MovieComponent,
    NavMenuComponent,
    TrailerComponent,
    SafePipe
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: SearchComponent, pathMatch: 'full' },
      { path: 'movie/:id', component: MovieComponent }
    ]),
    NgbModule.forRoot(),
    ShareModule.forRoot()
  ],
  providers: [ MovieService, SearchService, VideoService ],
  bootstrap: [ AppComponent ],
  entryComponents: [ TrailerComponent ]
})
export class AppModule { }
