import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<WeatherForecast[]>(baseUrl + 'weatherforecast').subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

enum RoleCard {
  Terrorist = 0,
  Swat = 1
}

enum PlayCard {
  Bomb =0,
  Success = 1,
  Safe = 2
}

interface IPlayer {
  PlayerId: string;
  Name: string;
  HoldsNipper: boolean;
  RoleCard: RoleCard;
  HiddenPlayCards: PlayCard[];
}

interface IOtherPlayerDto {
  Name: string;
  HoldsNipper: boolean;
  NumberOfHiddenPlayCards: number;
}

interface IRevealedPlayCard{
  Round: number;
  NameOfPlayerWhichHadThisCard: string;
  PlayCard: PlayCard;
}

interface IPlayerSpecificGameDto {
  GameId: string;
  OtherPlayers: IOtherPlayerDto[];
  OwnPlayer: IPlayer;
  RevealedPlayCards: IRevealedPlayCard[];
  IsStarted: boolean;
  IsFinished: boolean;
  Winner: RoleCard | null;
}
