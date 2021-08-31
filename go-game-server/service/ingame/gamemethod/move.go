package gamemethod

import (
	"log"

	"github.com/gorilla/websocket"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamedata"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel"
)

// HandleMoveReceivedData return
func HandleMoveReceivedData(conn *websocket.Conn, content string) {
	d, ok := gamedata.InGameClientData[conn]
	if ok == false {
		log.Printf("Failed to unmarshal chat method %s\n", content)
		return
	}

	userID := d.Info.UserID
	AddBroadcastMethod(userID, ingamemodel.GameMethod{MethodType: ingamemodel.Move, Content: content})
}
