import {Component, Inject} from '@angular/core';
import {Router, ActivatedRoute} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {playerSpecificGameDto, PlayCardsDisplay, RoleCardsDisplay, player, revealedPlayCard, playCard, roleCard} from "../api/api"

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

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router:Router, private route: ActivatedRoute){
    this.http = http;
    this.baseUrl = baseUrl;
    this.gameId = this.route.snapshot.paramMap.get('gameId');
    this.playerId = this.route.snapshot.paramMap.get('playerId');
    this.playCardsDisplay = PlayCardsDisplay;
    this.roleCardsDisplay = RoleCardsDisplay;

    this.http.get<playerSpecificGameDto>(this.baseUrl + 'timebomb/getactualgamestate?gameId='+this.gameId+'&playerId='+this.playerId)
      .subscribe(resultingPlayerSpecificGameDto => {
        this.playerSpecificGameDto = resultingPlayerSpecificGameDto;
      }, error => console.error(error));
  }
}
