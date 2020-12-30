import {Component, Inject} from '@angular/core';
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {playerSpecificGameDto,otherPlayerDto, player, revealedPlayCard, playCard, roleCard} from "../api/api"

@Component({
  selector: 'app-registerplayer',
  templateUrl: './registerplayer.component.html',
})
export class RegisterPlayerComponent {
  gameId: string;
  playerName: string;
  http: HttpClient;
  baseUrl: string;
  playerSpecificGameDto: playerSpecificGameDto;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router:Router){
    this.http = http;
    this.baseUrl = baseUrl;
    this.gameId = this.router.getCurrentNavigation().extras.state.gameId;
  }


  registerPlayer(){
    console.log("Method RegisterPlayer with Game-Id '" + this.gameId + "' and NickName '" + this.playerName + "' has been called");
    this.http.get<playerSpecificGameDto>(this.baseUrl + 'timebomb/registernewplayer?gameId='+this.gameId+'&name='+this.playerName)
      .subscribe(resultingPlayerSpecificGameDto => {
        this.playerSpecificGameDto = resultingPlayerSpecificGameDto;
        // Todo: write result to console, and check, whether it works well.
        console.log("Whole Object: " + this.playerSpecificGameDto);
        console.log("IsStarted:"+ this.playerSpecificGameDto.isStarted);
        console.log("OwnPlayerName:"+this.playerSpecificGameDto.ownPlayer.name);
        console.log("GameId:"+this.playerSpecificGameDto.gameId);
        this.router.navigate(['/lobby'], { state: { playerSpecificGameDto: resultingPlayerSpecificGameDto }});
      }, error => console.error(error));
  }
}
