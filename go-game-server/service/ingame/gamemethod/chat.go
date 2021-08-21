package gamemethod

import (
	"encoding/json"
	"log"

	"github.com/gorilla/websocket"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamedata"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel/gamemethod"
)

// HandleChatReceivedData return (GameMethod,bool: return true if send method)
func HandleChatReceivedData(conn *websocket.Conn, content string) {
	d, ok := gamedata.InGameClientData[conn]
	if ok == false {
		log.Printf("Failed to unmarshal chat method %s\n", content)
		return
	}

	userID := d.Info.UserID
	var chatMethod gamemethod.ChatMethod
	if err := json.Unmarshal([]byte(content), &chatMethod); err != nil {
		log.Printf("Failed to unmarshal chat method %s\n", content)
	}
	log.Printf("Received Chat Text: %s\n", chatMethod.Text)

	AddBroadcastMethod(userID, ingamemodel.GameMethod{MethodType: ingamemodel.Chat, Content: content})
}
