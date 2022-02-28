import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Club, Country, Menu, Player } from './interfaces';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(
    private http: HttpClient
  ) {}

  private countriesUrl = "api/countries"; 
  private clubsUrl = "api/clubs"; 
  private playersUrl = "api/players"; 

  public countries?: Country[];
  public clubs?: Club[];
  public players?: Player[];
  public menu?: Menu;

  setMenu(country?: number, club?: number, player?: number) {
    if (this.menu){
      if (country) {
        this.menu.country = country;
        this.getClubsByCountryId(country)
          .subscribe(clubs => this.menu && (this.menu.clubs = clubs))
      }
      if (club) {
        this.menu.club = club
        this.getPlayersByClubId(club)
          .subscribe(players => this.menu && (this.menu.players = players))
      }
      if (player) {
        this.menu.player = player
      };
    }
  }

  getCountries(): Observable<Country[]> {
    return this.http.get<Country[]>(this.countriesUrl);
  }

  getClubs(): Observable<Club[]> {
    return this.http.get<Club[]>(this.clubsUrl);
  }

  getPlayers(): Observable<Player[]> {
    return this.http.get<Player[]>(this.playersUrl);
  }

  getCountry(id: number): Observable<Country> {
    const url =`${this.countriesUrl}/${id}`
    return this.http.get<Country>(url);
  }

  getClub(id: number): Observable<Club> {
    const url =`${this.clubsUrl}/${id}`
    return this.http.get<Club>(url);
  }

  getPlayer(id: number): Observable<Player> {
    const url =`${this.playersUrl}/${id}`
    return this.http.get<Player>(url);
  }

  getClubsByCountryId(id: Number): Observable<Club[]> {
    const url = `${this.clubsUrl}?countryId=${id}`;
    return this.http.get<Club[]>(url);
  }

  getPlayersByClubId(id: Number): Observable<Player[]> {
    const url = `${this.playersUrl}?clubId=${id}`;
    return this.http.get<Player[]>(url) ;
  }

  deleteCountry(id: Number): Observable<Country> {
    const url = `${this.countriesUrl}/${id}`
    return this.http.delete<Country>(url);
  }

  deleteClub(id: Number): Observable<Club> {
    const url = `${this.clubsUrl}/${id}`
    return this.http.delete<Club>(url);
  }

  deletePlayer(id: Number): Observable<Player> {
    const url = `${this.playersUrl}/${id}`
    return this.http.delete<Player>(url);
  }

  addCountry(country: Country): Observable<Country> {
    const url = `${this.countriesUrl}`
    return this.http.post<Country>(url, country);
  }
  addClub(club: Club): Observable<Club> {
    const url = `${this.clubsUrl}`
    return this.http.post<Club>(url, club);
  }
  addPlayer(player: Player): Observable<Player> {
    const url = `${this.playersUrl}`
    return this.http.post<Player>(url, player);
  }

  updateCountry(country: Country): Observable<Country> {
    const url = `${this.countriesUrl}/${country.id}`
    return this.http.put<Country>(url, country);
  }
  updateClub(club: Club): Observable<Club> {
    const url = `${this.countriesUrl}/${club.id}`
    return this.http.put<Club>(url, club);
  }
  updatePlayer(player: Player): Observable<Player> {
    const url = `${this.countriesUrl}/${player.id}`
    return this.http.put<Player>(url, player);
  }
  searchPlayers(name: String): Observable<Player[]> {
    if (!name.trim()) {
      return of([]);
    }
    const url = `${this.playersUrl}?name=${name}`;
    return this.http.get<Player[]>(url);
  }

  searchClubs(name: String): Observable<Club[]> {
    if (!name.trim()) {
      return of([]);
    }
    const url = `${this.clubsUrl}?name=${name}`;
    return this.http.get<Club[]>(url);
  }
}
