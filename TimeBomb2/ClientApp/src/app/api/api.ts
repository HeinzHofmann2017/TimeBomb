export enum roleCard {
  Terrorist = 0,
  Swat = 1
}

export enum playCard {
  Bomb =0,
  Success = 1,
  Safe = 2
}

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
