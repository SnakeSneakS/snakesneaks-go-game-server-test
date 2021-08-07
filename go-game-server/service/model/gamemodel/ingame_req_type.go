package gamemodel

import "github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"

type ChatReq struct {
	Session model.Session `json:"session,string"`
	Chat    Chat          `json:"chat,string"`
}
