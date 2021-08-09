package ingame

import (
	"github.com/gorilla/websocket"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/gamemodel"
)

var InGameClientData = map[*websocket.Conn]gamemodel.GameClient{}

func StartGame() {
	StartConnMessageManager()
}

func SendDiffMessageToAll() {

}

func SendAllDataToAll() {

}
