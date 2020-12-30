import {Component, Inject} from '@angular/core';
import {Router, ActivatedRoute} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {playerSpecificGameDto} from "../api/api"

@Component({
  selector: 'app-registerplayer',
  templateUrl: './registerplayer.component.html',
})
export class RegisterPlayerComponent {
  gameId: string;
  playerName: string;
  http: HttpClient;
  baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router:Router, private route: ActivatedRoute){
    this.http = http;
    this.baseUrl = baseUrl;
    this.gameId = this.route.snapshot.paramMap.get('gameId');
  }


  registerPlayer(){
    console.log("Method RegisterPlayer with Game-Id '" + this.gameId + "' and NickName '" + this.playerName + "' has been called");
    this.http.get<playerSpecificGameDto>(this.baseUrl + 'timebomb/registernewplayer?gameId='+this.gameId+'&name='+this.playerName)
      .subscribe(resultingPlayerSpecificGameDto => {
        this.router.navigate(['/lobby', resultingPlayerSpecificGameDto.gameId, resultingPlayerSpecificGameDto.ownPlayer.playerId ]);
      }, error => console.error(error)); // Todo: Exceptionhandling of "TimeBomb2.Services.NotAllowedMoveException: Already the maximum of 6 Players joined this game. Therefore no more Players are allowed"
  }
}
