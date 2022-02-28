import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClubDetailComponent } from './club-detail/club-detail.component';
import { PlayerDetailComponent } from './player-detail/player-detail.component';
import { SearchResultComponent } from './search-result/search-result.component';
import { CountryDetailComponent } from './country-detail/country-detail.component';
import { PlayerUpdateComponent } from './player-update/player-update.component';
import { PlayerAddComponent } from './player-add/player-add.component';
import { CountryAddComponent } from './country-add/country-add.component';
import { CountryUpdateComponent } from './country-update/country-update.component';
import { ClubAddComponent } from './club-add/club-add.component';
import { ClubUpdateComponent } from './club-update/club-update.component';

const routes: Routes = [
  {path: 'search/:q', component: SearchResultComponent},
  {path: 'country/add', component: CountryAddComponent},
  {path: 'country/update/:id', component: CountryUpdateComponent},
  {path: 'country/:id', component: CountryDetailComponent},
  {path: 'club/add', component: ClubAddComponent},
  {path: 'club/update/:id', component: ClubUpdateComponent},
  {path: 'club/:id', component: ClubDetailComponent},
  {path: 'player/add', component: PlayerAddComponent},
  {path: 'player/update/:id', component: PlayerUpdateComponent},
  {path: 'player/:id', component: PlayerDetailComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
