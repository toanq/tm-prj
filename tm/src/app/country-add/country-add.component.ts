import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../data.service';
import { Country } from '../interfaces';

@Component({
  selector: 'app-country-add',
  templateUrl: './country-add.component.html',
  styleUrls: ['./country-add.component.css']
})
export class CountryAddComponent implements OnInit {

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private dataService: DataService,
    private router: Router
  ) { }
  ngOnInit(): void {
  }

  addCountry(name: String){
    if (name.length == 0) return;
    this.dataService.addCountry({name} as Country).subscribe(country => this.dataService.countries?.push(country));
    this.router.navigate([`/`])
  }

  cancelCountry(){
    this.location.back();
  }

}
