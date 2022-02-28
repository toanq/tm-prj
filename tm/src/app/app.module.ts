import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule} from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SearchBarComponent } from './search-bar/search-bar.component';
import { CountryDetailComponent } from './country-detail/country-detail.component';
import { ClubDetailComponent } from './club-detail/club-detail.component';
import { PlayerDetailComponent } from './player-detail/player-detail.component';
import { SearchResultComponent } from './search-result/search-result.component';
import { PlayerAddComponent } from './player-add/player-add.component';
import { CountryAddComponent } from './country-add/country-add.component';
import { ClubAddComponent } from './club-add/club-add.component';
import { ClubUpdateComponent } from './club-update/club-update.component';
import { CountryUpdateComponent } from './country-update/country-update.component';
import { PlayerUpdateComponent } from './player-update/player-update.component';

//import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';
//import { InMemoryDataService } from './in-memory-data.service';

@NgModule({
  declarations: [
    AppComponent,
    SearchBarComponent,
    CountryDetailComponent,
    ClubDetailComponent,
    PlayerDetailComponent,
    SearchResultComponent,
    PlayerAddComponent,
    CountryAddComponent,
    ClubAddComponent,
    ClubUpdateComponent,
    CountryUpdateComponent,
    PlayerUpdateComponent,
  ],
  imports: [
    FormsModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    //HttpClientInMemoryWebApiModule.forRoot(InMemoryDataService, { dataEncapsulation: false })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
