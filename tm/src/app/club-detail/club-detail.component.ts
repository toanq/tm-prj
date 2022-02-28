import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../data.service';
import { Club, Player } from '../interfaces';

@Component({
  selector: 'app-club-detail',
  templateUrl: './club-detail.component.html',
  styleUrls: ['./club-detail.component.css']
})
export class ClubDetailComponent implements OnInit {

  constructor(
    private router: Router,
    private location: Location,
    private route: ActivatedRoute,
    private dataService: DataService,
  ) { }

  pgTitle?: String;
  players?: Player[];
  clubId?: Number;

  ngOnInit(): void {
    this.route.params.subscribe( params => {
      const id = params['id'];
      this.clubId = id;
      this.dataService.getPlayersByClubId(id)
        .subscribe(players => this.players = players);
      this.dataService.getClub(id)
        .subscribe(club => {
          this.pgTitle = club.name;
          this.dataService.setMenu(club.countryId, id);
        });
    })
  }

  addClub(){
    this.router.navigate([`club/add/`]);
  }
  updateClub(){
    this.router.navigate([`club/update/${this.clubId}`]);
  }
  deleteClub(){
    if (this.clubId){
      this.dataService.deleteClub(this.clubId).subscribe();
      const newList= this.dataService.clubs?.filter(e => e.id != this.clubId)
      if (newList && this.dataService.clubs) {
        for (let ii = 0; ii< newList.length; ii++){
          this.dataService.clubs[ii] = newList[ii];
        }
        this.dataService.clubs.length = newList.length;
      }
      this.router.navigate([`/`])
    }
  }
}
