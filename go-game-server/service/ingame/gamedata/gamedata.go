package gamedata

import (
	"github.com/gorilla/websocket"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel"
)

var InGameClientData = map[*websocket.Conn]ingamemodel.GameClient{}
