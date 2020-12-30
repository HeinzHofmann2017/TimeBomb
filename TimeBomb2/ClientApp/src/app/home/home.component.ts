import {Component, Inject} from '@angular/core';
import { Router } from '@angular/router';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public gameId: string;
  private http: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private route:Router) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  createNewGame() {
    this.http.get<string>(this.baseUrl + 'timebomb/creategame')
      .subscribe(resultingGameId => {
           console.log("New Game with Id '" + resultingGameId + "' has been created")
           this.route.navigate(['/register-player'], { state: { gameId: resultingGameId}});
         }, error => console.error(error));
  }

  joinExistingGame(){
    console.log("Method joinExistingGame with Id '" + this.gameId + "' has been called")
    this.route.navigate(['/register-player'], { state: { gameId: this.gameId }});
  }
}
