package ingamemodel

import (
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"
)

type GameReq struct {
	Session     model.Session `json:"session"`
	GameMethods []GameMethod  `json:"methods"`
}
