import { Location } from '@angular/common';
import { Component, OnInit, ɵclearResolutionOfComponentResourcesQueue } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../data.service';
import { Club, Country } from '../interfaces';

@Component({
  selector: 'app-club-update',
  templateUrl: './club-update.component.html',
  styleUrls: ['./club-update.component.css']
})
export class ClubUpdateComponent implements OnInit {

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private dataService: DataService,
    private router: Router
  ) { }

  countries?: Country[];
  currClub: Club = {id: 0, name: "", countryId: 0};
  currClubId?: number;
  ngOnInit(): void {
    this.dataService.getCountries().subscribe( countries => {
      this.countries = countries;
    })

    this.route.params.subscribe( params => {
      const id = params['id'];
      this.currClubId = id;
      this.dataService.getClub(id)
        .subscribe(club => {
          this.currClub = club
          this.dataService.setMenu(club.countryId, id);
        });
    })

  }

  updateClub(){
    if (this.currClub){
      this.dataService.updateClub(this.currClub).subscribe();
      this.router.navigate([`/country/${this.currClub.countryId}`])
    }
  }

  cancelClub(){
    this.location.back();
  }

}
