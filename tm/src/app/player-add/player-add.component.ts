import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../data.service';
import { Club, Country, Player } from '../interfaces';

@Component({
  selector: 'app-player-add',
  templateUrl: './player-add.component.html',
  styleUrls: ['./player-add.component.css']
})
export class PlayerAddComponent implements OnInit {

  constructor(
    private router: Router,
    private location: Location,
    private route: ActivatedRoute,
    private dataService: DataService,
  ) { }

  title?: String;
  clubs?: Club[];
  countries?: Country[];
  currPlayer: Player = {} as Player;
  
  ngOnInit(): void {
    this.dataService.getCountries()
      .subscribe(countries => this.countries = countries);
    this.dataService.getClubs()
      .subscribe(clubs => this.clubs = clubs);
  }

  addPlayer(): void {
    if (this.currPlayer.name 
      && this.currPlayer.clubId 
      && this.currPlayer.nation 
      && this.currPlayer.height
      && this.currPlayer.position){
      this.dataService.addPlayer(this.currPlayer).subscribe(player => this.dataService.players?.push(player));
      this.router.navigate([`club/${this.currPlayer.clubId}`])
    }
  }

  cancelPlayer(): void {
    this.location.back();
  }
}
