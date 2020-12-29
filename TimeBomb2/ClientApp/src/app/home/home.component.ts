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

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }
  createNewGame() {
    console.log("Method createNewGame was called")
    this.http.get<string>(this.baseUrl + 'timebomb/creategame').subscribe(result => {
           this.gameId = result;
           console.log("New Game with Id '" + this.gameId + "' has been created")
           // route to registerplayer-component
         }, error => console.error(error));
  }

  joinExistingGame(){
    console.log("Method joinExistingGame with Id '" + this.gameId + "' has been called")
    // route to registerplayer-component
  }
}
