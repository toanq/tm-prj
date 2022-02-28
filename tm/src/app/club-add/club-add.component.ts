import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../data.service';
import { Club, Country } from '../interfaces';
@Component({
  selector: 'app-club-add',
  templateUrl: './club-add.component.html',
  styleUrls: ['./club-add.component.css']
})
export class ClubAddComponent implements OnInit {

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private dataService: DataService,
    private router: Router  
  ) { }

  clubName?: String;
  countries?: Country[];
  currCountryId?: number;
  ngOnInit(): void {
    this.dataService.getCountries().subscribe( countries => {
      this.countries = countries;
    })
  }

  addClub(){
    if (this.clubName && this.currCountryId){
      const newClub = {
        name: this.clubName,
        countryId: this.currCountryId
      } as Club;
      this.dataService.addClub(newClub).subscribe(club => this.dataService.clubs?.push(club));
      this.router.navigate([`/`])
    }
  }

  cancelClub(){
    this.location.back();
  }
}
