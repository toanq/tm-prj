import { Injectable } from '@angular/core';
import { InMemoryDbService } from 'angular-in-memory-web-api';
import { Club, Country, Player } from './interfaces';

@Injectable({
  providedIn: 'root'
})
export class InMemoryDataService implements InMemoryDbService{
  createDb() {
    const countries: Country[] = [
      {id: 100, name: "Vietnam"},
      {id: 101, name: "England"},
      {id: 102, name: "China"},
    ]
    const clubs: Club[] = [
      {id: 200, countryId: 100, name: "Hoang Anh Gia Lai FC"},
      {id: 201, countryId: 100, name: "Viettel FC"},
      {id: 202, countryId: 101, name: "Arsenal FC"},
      {id: 203, countryId: 101, name: "Aston Villa"},
      {id: 204, countryId: 102, name: "Shandong Taisan"},
      {id: 205, countryId: 102, name: "Shanghai Port"},
    ]
    const players: Player[] = [
      {id: 301, clubId: 200, nation: 100, name: "Tuan Linh Huynh", height: 1.78, position: "Goalkeeper"},
      {id: 302, clubId: 200, nation: 100, name: "Van Truong Le", height: 1.78, position: "Goalkeeper"},
      {id: 303, clubId: 201, nation: 100, name: "Nguyen Manh Tran", height: 1.77, position: "Goalkeeper"},
      {id: 304, clubId: 201, nation: 100, name: "The Tai Quang", height: 1.78, position: "Goalkeeper"},
      {id: 305, clubId: 202, nation: 101, name: "Aaron Ramsdale", height: 1.91, position: "Goalkeeper"},
      {id: 306, clubId: 202, nation: 101, name: "Bernd Leno", height: 1.90, position: "Goalkeeper"},
      {id: 307, clubId: 203, nation: 101, name: "Emiliano Martínez", height: 1.95, position: "Goalkeeper"},
      {id: 308, clubId: 203, nation: 101, name: "Robin Olsen", height: 1.96, position: "Goalkeeper"},
      {id: 309, clubId: 204, nation: 102, name: "Dalei Wang", height: 1.85, position: "Goalkeeper"},
      {id: 310, clubId: 204, nation: 102, name: "Rongze Han", height: 1.90, position: "Goalkeeper"},
      {id: 311, clubId: 205, nation: 102, name: "Dianzou Liu", height: 1.90, position: "Goalkeeper"},
      {id: 312, clubId: 205, nation: 102, name: "Weiguo Liu", height: 1.97, position: "Goalkeeper"},
    ]
    return {countries, clubs, players};
  }

  genId <T extends Country | Club | Player>(myTable: T[]): number {
    return myTable.length > 0 ? Math.max(...myTable.map(t => t.id)) + 1 : 0;
  }
}
