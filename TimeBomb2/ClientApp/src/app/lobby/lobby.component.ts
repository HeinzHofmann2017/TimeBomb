import {Component, Inject} from '@angular/core';
import {Router, ActivatedRoute} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {playerSpecificGameDto} from "../api/api"

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
})
export class LobbyComponent {
  gameId: string;
  playerId: string;
  http: HttpClient;
  baseUrl: string;
  playerSpecificGameDto: playerSpecificGameDto;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router:Router, private route: ActivatedRoute){
    this.http = http;
    this.baseUrl = baseUrl;
    this.gameId = this.route.snapshot.paramMap.get('gameId');
    this.playerId = this.route.snapshot.paramMap.get('playerId');

    this.http.get<playerSpecificGameDto>(this.baseUrl + 'timebomb/getactualgamestate?gameId='+this.gameId+'&playerId='+this.playerId)
      .subscribe(resultingPlayerSpecificGameDto => {
        this.playerSpecificGameDto = resultingPlayerSpecificGameDto;
        console.log("Successfully received dto after Reload. IsGameStarted:" + resultingPlayerSpecificGameDto.isStarted);
        if(resultingPlayerSpecificGameDto.isStarted){
          console.log("If clause is entered")
          this.router.navigate(['/play-field', resultingPlayerSpecificGameDto.gameId, resultingPlayerSpecificGameDto.ownPlayer.playerId ]);
        }
      }, error => console.error(error));
  }


  startGame(){
    this.http.get<playerSpecificGameDto>(this.baseUrl + 'timebomb/startgame?gameId='+this.gameId+'&playerId='+this.playerId)
      .subscribe(resultingPlayerSpecificGameDto => {
        console.debug("Button was pressed and game is started:" + resultingPlayerSpecificGameDto.isStarted)
        this.router.navigate(['/play-field', resultingPlayerSpecificGameDto.gameId, resultingPlayerSpecificGameDto.ownPlayer.playerId ]);
      }, error => console.error(error));
  }
}
