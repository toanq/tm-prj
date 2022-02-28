import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../data.service';
import { Club, Country, Player } from '../interfaces';

@Component({
  selector: 'app-player-update',
  templateUrl: './player-update.component.html',
  styleUrls: ['./player-update.component.css']
})
export class PlayerUpdateComponent implements OnInit {

  constructor(
    private router: Router,
    private location: Location,
    private route: ActivatedRoute,
    private dataService: DataService,
  ) { }

  title?: String;
  clubs?: Club[];
  countries?: Country[];
  currPlayer?: Player;

  ngOnInit(): void {
    this.route.params.subscribe( params => {
      const id = params['id'];
      this.dataService.getPlayer(id)
        .subscribe(player => {
          this.currPlayer = player;
          if (!this.currPlayer) return;
          this.dataService.getCountries()
            .subscribe(countries => this.countries = countries);
          this.dataService.getClubs()
            .subscribe(clubs => this.clubs = clubs);
          this.dataService.setMenu(player.nation, player.clubId, player.id);
        });
      }
    )
  }

  updatePlayer(): void {
    if (this.currPlayer){
      this.dataService.updatePlayer(this.currPlayer).subscribe();
      this.router.navigate([`/club/${this.currPlayer.clubId}`])
    }
  }

  cancelPlayer(): void {
    this.location.back();
  }

}
