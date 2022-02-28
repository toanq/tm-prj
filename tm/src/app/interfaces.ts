export interface Country {
    id: number,
    name: String
}

export interface Club {
    id: number,
    countryId: number,
    name: String
}

export interface Player {
    id: number,
    clubId: number,
    nation: number,
    name: String,
    height: number,
    position: String
}

export interface Menu {
    country: number,
    club: number,
    player: number,
    countries: Country[],
    clubs: Club[],
    players: Player[]
}