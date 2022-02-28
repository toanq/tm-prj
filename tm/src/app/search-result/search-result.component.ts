import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../data.service';
import { Club, Player } from '../interfaces';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchResultComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private dataService: DataService,
  ) { }
  players?: Player[];
  clubs?:Club[];

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const term = params['q'];
      this.searchPlayers(term);
      this.searchClubs(term);
    })
  }

  searchPlayers(term: String): void {
    this.dataService.searchPlayers(term)
      .subscribe(players => this.players = players);
  }

  searchClubs(term: String): void {
    this.dataService.searchClubs(term)
      .subscribe(clubs => this.clubs = clubs);
  }
}
