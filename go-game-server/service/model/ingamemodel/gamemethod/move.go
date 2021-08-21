package gamemethod

import "github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel"

type MoveMethod struct {
	To ingamemodel.Transform `json:"to"`
}
