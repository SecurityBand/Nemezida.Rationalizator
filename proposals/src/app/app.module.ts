import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ProposalComponent } from './proposal/proposal.component';
import { HomeComponent } from './pages/home/home.component';
import { RatingsComponent } from './pages/ratings/ratings.component';
import { TagComponent } from './tag/tag.component';
import { TagsComponent } from './tags/tags.component';

@NgModule({
  declarations: [
    AppComponent,
    ProposalComponent,
    HomeComponent,
    RatingsComponent,
    TagComponent,
    TagsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
