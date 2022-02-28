import { HttpClientTestingModule } from '@angular/common/http/testing';
import { inject, TestBed, waitForAsync } from '@angular/core/testing';
import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';

import { DataService } from './data.service';
import { InMemoryDataService } from './in-memory-data.service';

describe('DataService', () => {
  let service: DataService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        HttpClientInMemoryWebApiModule.forRoot(
          InMemoryDataService, { dataEncapsulation: false }
        )
      ]
    });
    service = TestBed.inject(DataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('retrieves all the countries', (done: DoneFn) => {
    service.getCountries().subscribe( result => {
      expect(result.length).toBeGreaterThan(0);
      done();
    })
  })

  it('retrieves all the clubs', (done: DoneFn) => {
    service.getClubs().subscribe( result => {
      expect(result.length).toBeGreaterThan(0);
      done();
    })
  })

  it('retrieves all the players', (done: DoneFn) => {
    service.getPlayers().subscribe( result => {
      expect(result.length).toBeGreaterThan(0);
      done();
    })
  })

  it('retrieves a country', (done: DoneFn) => {
    service.getCountry(100).subscribe( result => {
      expect(result.id).toEqual(100);
      done();
    })
  })

  it('retrieves a club', (done: DoneFn) => {
    service.getClub(200).subscribe( result => {
      expect(result.id).toEqual(200);
      done();
    })
  })

  it('retrieves a player', (done: DoneFn) => {
    service.getPlayer(301).subscribe( result => {
      expect(result.id).toEqual(301);
      done();
    })
  })
});
