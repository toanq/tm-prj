import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../data.service';
import { Player } from '../interfaces';

@Component({
  selector: 'app-player-detail',
  templateUrl: './player-detail.component.html',
  styleUrls: ['./player-detail.component.css']
})
export class PlayerDetailComponent implements OnInit {

  constructor(
    private router: Router,
    private location: Location,
    private route: ActivatedRoute,
    private dataService: DataService,
  ) { }

  playerId?: Number;
  player?: Player;
  clubName?: String;
  countryName?: String;

  ngOnInit(): void {
    this.route.params.subscribe( params => {
      const id = params['id'];
      this.playerId = id;
      this.dataService.getPlayer(id)
        .subscribe(player => {
          this.player = player;
          if (!this.player) return;
          this.dataService.setMenu(player.nation, player.clubId, player.id);
          this.dataService.getClub(this.player.clubId)
            .subscribe(club => {
              this.clubName = club.name;
            })
          this.dataService.getCountry(this.player.nation)
            .subscribe(country => this.countryName = country.name)
        });
      }
    )
  }
  deletePlayer(): void{
    if (this.playerId){
      this.dataService.deletePlayer(this.playerId).subscribe();
      this.router.navigate([`/club/${this.player?.clubId}`])
    }
  }
  updatePlayer(): void{
    this.router.navigate([`/player/update/${this.playerId}`])
  }
  addPlayer(): void{
    this.router.navigate([`/player/add`])
  }

}
