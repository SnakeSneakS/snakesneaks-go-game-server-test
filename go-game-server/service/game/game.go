package game

import "github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/gamemodel"

var GameClientData []gamemodel.GameClient

func StartGame() {
	GameClientData = make([]gamemodel.GameClient, 0)
}
