package gamemodel

import (
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"
)

const (
	//Game Request Type: ReqType
	Idle GemeMethodType = iota //Enter World
	EnterWorld
	ExitWorld
	Chat
	Move
)

type GemeMethodType int

type GameReq struct {
	Session     model.Session `json:"session"`
	GameMethods []GameMethod  `json:"methods"`
}

type GameMethod struct {
	MethodType GemeMethodType `json:"method"`
	Content    string         `json:"content"`
}
