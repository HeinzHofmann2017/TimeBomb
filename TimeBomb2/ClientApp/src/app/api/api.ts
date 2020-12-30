export enum roleCard {
  Terrorist = 0,
  Swat = 1
}

export let RoleCardsDisplay: { [index: number]: string} = {};
RoleCardsDisplay[roleCard.Terrorist] = "Terrorist";
RoleCardsDisplay[roleCard.Swat] = "SWAT"

export enum playCard {
  Bomb =0,
  Success = 1,
  Safe = 2
}

export let PlayCardsDisplay: { [index: number]: string} = {};
PlayCardsDisplay[playCard.Bomb] = "Bomb";
PlayCardsDisplay[playCard.Success] = "Success";
PlayCardsDisplay[playCard.Safe] = "Safe";

export class player {
  playerId: string;
  name: string;
  holdsNipper: boolean;
  roleCard: roleCard;
  hiddenPlayCards: playCard[];
}

export class otherPlayerDto {
  name: string;
  holdsNipper: boolean;
  numberOfHiddenPlayCards: number;
}

export class revealedPlayCard{
  round: number;
  nameOfPlayerWhichHadThisCard: string;
  playCard: playCard;
}

export class playerSpecificGameDto {
  gameId: string;
  otherPlayers: otherPlayerDto[];
  ownPlayer: player;
  revealedPlayCards: revealedPlayCard[];
  isStarted: boolean;
  isFinished: boolean;
  winner: roleCard | null;
}
