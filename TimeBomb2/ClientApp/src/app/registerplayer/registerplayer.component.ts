import { Component } from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-registerplayer',
  templateUrl: './registerplayer.component.html',
})
export class RegisterPlayerComponent {
  gameId: string;
  constructor(private router: Router){
    this.gameId = this.router.getCurrentNavigation().extras.state.gameId;
  }
}
