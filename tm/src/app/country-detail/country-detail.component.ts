import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../data.service';
import { Club, Menu } from '../interfaces';

@Component({
  selector: 'app-country-detail',
  templateUrl: './country-detail.component.html',
  styleUrls: ['./country-detail.component.css']
})
export class CountryDetailComponent implements OnInit {

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private dataService: DataService,
    private router: Router
  ) { }

  pgTitle?: String;
  clubs?: Club[];
  countryId?: number;

  ngOnInit(): void {
    this.route.params.subscribe( params => {
      const id = params['id'];
      this.countryId = id;
      this.dataService.getClubsByCountryId(id)
        .subscribe(clubs => this.clubs = clubs);
      this.dataService.getCountry(id)
        .subscribe(country => this.pgTitle = country.name);
      this.dataService.setMenu(id);
    })
  }

  addCountry(){
    this.router.navigate([`/country/add`])
  }

  updateCountry(){
    this.router.navigate([`/country/update/${this.countryId}`]);
  }

  deleteCountry(){
    if (this.countryId){
      this.dataService.deleteCountry(this.countryId).subscribe();
      const newList= this.dataService.countries?.filter(e => e.id != this.countryId)
      if (newList && this.dataService.countries) {
        for (let ii = 0; ii< newList.length; ii++){
          this.dataService.countries[ii] = newList[ii];
        }
        this.dataService.countries.length = newList.length;
      }
      this.router.navigate([`/`])
    }
  }
}
