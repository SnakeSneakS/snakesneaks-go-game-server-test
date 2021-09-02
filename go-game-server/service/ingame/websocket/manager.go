package websocket

import (
	"github.com/gorilla/websocket"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamedata"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamemethod"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/websocket/sender"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel"
)

func StartWebsocketMessageManager() {
	sender.StartMessageSender()
}

func OnClosedConnection(conn *websocket.Conn) {
	gamemethod.AddBroadcastMethod(gamedata.InGameClientData[conn].Info.UserID, ingamemodel.GameMethod{MethodType: ingamemodel.ExitWorld})
	delete(gamedata.InGameClientData, conn)
}
