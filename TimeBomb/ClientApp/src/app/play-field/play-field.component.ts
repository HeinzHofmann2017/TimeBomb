import {Component, Inject} from '@angular/core';
import {Router, ActivatedRoute} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {playerSpecificGameDto, PlayCardsDisplay, RoleCardsDisplay, otherPlayerDto} from "../api/api"
import {interval, Subscription} from 'rxjs';

@Component({
  selector: 'app-play-field',
  templateUrl: './play-field.component.html',
})
export class PlayFieldComponent {
  gameId: string;
  playerId: string;
  http: HttpClient;
  baseUrl: string;
  playerSpecificGameDto: playerSpecificGameDto;
  playCardsDisplay: { [index: number]: string};
  roleCardsDisplay: { [index: number]: string};
  subscription: Subscription;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router:Router, private route: ActivatedRoute){
    this.http = http;
    this.baseUrl = baseUrl;
    this.gameId = this.route.snapshot.paramMap.get('gameId');
    this.playerId = this.route.snapshot.paramMap.get('playerId');
    this.playCardsDisplay = PlayCardsDisplay;
    this.roleCardsDisplay = RoleCardsDisplay;

    const source = interval(1000);
    this.subscription = source.subscribe(val => this.loadActualState());

    this.loadActualState();
  }

  loadActualState(){
    this.http.get<playerSpecificGameDto>(this.baseUrl + 'timebomb/getactualgamestate?gameId='+this.gameId+'&playerId='+this.playerId)
      .subscribe(resultingPlayerSpecificGameDto => {
        this.playerSpecificGameDto = resultingPlayerSpecificGameDto;
      }, error => console.error(error));
  }

  nipCard(player: otherPlayerDto){
    console.log("Card of"+player.name+"is going to be nipped");
    this.http.get<playerSpecificGameDto>(this.baseUrl + 'timebomb/nipcard?gameId='+this.gameId+'&nippingPlayerId='+this.playerId+'&toBeNippedPlayerName='+player.name)
      .subscribe(resultingPlayerSpecificGameDto => {
        this.playerSpecificGameDto = resultingPlayerSpecificGameDto;
      }, error => console.error(error));
  }
}
