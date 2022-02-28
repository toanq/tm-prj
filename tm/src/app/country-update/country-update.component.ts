import { Location } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../data.service';
import { Country } from '../interfaces';

@Component({
  selector: 'app-country-update',
  templateUrl: './country-update.component.html',
  styleUrls: ['./country-update.component.css']
})
export class CountryUpdateComponent implements OnInit {

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private dataService: DataService,
    private router: Router
  ) { }

  countryName?: String;
  currCountry?: Country;
  currCountryId?: number;
  ngOnInit(): void {
    this.route.params.subscribe( params => {
      const id = params['id'];
      this.currCountryId = id;
      this.dataService.getCountry(id)
        .subscribe(country => this.countryName = country.name);
      this.dataService.setMenu(id); 
    })
  }

  updateCountry(){
    this.dataService.countries?.map((e)=>{
      if (e.id == this.currCountryId) {
        this.currCountry = e;
      }
    })

    if (this.currCountry && this.countryName){
      this.currCountry.name = this.countryName;
      this.dataService.updateCountry({...this.currCountry}).subscribe();
      this.router.navigate([`/`])
    }
  }

  cancelCountry(){
    this.location.back();
  }
}
