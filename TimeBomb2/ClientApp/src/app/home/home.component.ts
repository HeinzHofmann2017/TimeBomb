import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public gameId: string;

  createNewGame() {
    console.log("Method createNewGame was called")
    // create new game
    console.log("New Game with Id '" + this.gameId + "' has been created")
    // route to registerplayer-component
  }

  joinExistingGame(){
    console.log("Method joinExistingGame with Id '" + this.gameId + "' has been called")
    // route to registerplayer-component
  }
}
