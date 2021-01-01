import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { RegisterPlayerComponent } from './registerplayer/registerplayer.component';
import { CounterComponent } from './manual/manual.component';
import { LobbyComponent } from './lobby/lobby.component';
import { PlayFieldComponent } from './play-field/play-field.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    RegisterPlayerComponent,
    CounterComponent,
    LobbyComponent,
    PlayFieldComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'register-player/:gameId', component: RegisterPlayerComponent },
      { path: 'lobby/:gameId/:playerId', component: LobbyComponent },
      { path: 'play-field/:gameId/:playerId', component: PlayFieldComponent},
      { path: 'manual', component: CounterComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
