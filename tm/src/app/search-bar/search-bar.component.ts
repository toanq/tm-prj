import { Location } from '@angular/common';
import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import { DataService } from '../data.service';
import { Club, Country, Menu, Player } from '../interfaces';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent implements OnInit, AfterViewInit {

  searchText?: String;
  currRoute?: String;
  menu: Menu = {
    country: -1,
    club: -1,
    player: -1,
    countries: [],
    clubs: [],
    players: []
  }
  @ViewChild('searchBar') eSearchBar!: ElementRef;
  
  constructor(
    private router: Router,
    private dataService: DataService,
    private route: ActivatedRoute,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.getCountries();
    this.dataService.menu = this.menu;
  }
  ngAfterViewInit(): void { 
    this.eSearchBar.nativeElement.focus(); 
  }
  keyPress(event: KeyboardEvent): void {
    if (event.code == "Enter") {
      this.gotoSearch();
    };
  }
  getCountries() {
    this.dataService.getCountries()
      .subscribe(countries => this.dataService.countries = this.menu.countries = countries);
  }
  getClubs() {
    this.dataService.getClubsByCountryId(this.menu.country)
      .subscribe(clubs => this.dataService.clubs = this.menu.clubs = clubs);
  }
  getPlayers() {
    this.dataService.getPlayersByClubId(this.menu.club)
      .subscribe(players => this.dataService.players = this.menu.players = players);
  }
  countryChange(){
    this.getClubs();
    this.menu.club = -1;
    this.menu.player = -1;
  }
  clubChange(){
    this.getPlayers();
    this.menu.player = -1;
  }
  playerChange(){
    
  }
  gotoCountryDetails(id: Number){
    this.menu.club = -1;
    this.menu.player = -1;
    this.router.navigate([`country/${id}`])
  }
  gotoClubDetails(id: Number){
    this.menu.player = -1;
    this.router.navigate([`club/${id}`])
  }
  gotoPlayerDetails(id: Number){
    this.router.navigate([`player/${id}`])
  }
  gotoSearch(){
    if (!this.searchText) return;
    this.router.navigate([`search/${this.searchText}`])
  }
}
