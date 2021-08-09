package gamemethod

import "github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/gamemodel"

type MoveMethod struct {
	From gamemodel.Transform `json:"from"`
	To   gamemodel.Transform `json:"to"`
}
