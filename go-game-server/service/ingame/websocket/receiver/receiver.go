package receiver

import (
	"context"
	"encoding/json"
	"log"

	"github.com/gorilla/websocket"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/auth"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamedata"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamemethod"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/gamemodel"
)

// HandleMessage
func HandleMessage(messageType int, message []byte, conn *websocket.Conn) {
	log.Println("HandleWebSocket")

	if messageType == websocket.TextMessage {
		var req gamemodel.GameReq
		if err := json.Unmarshal(message, &req); err != nil {
			log.Printf(("Failed to Unmarshal message: %s\n"), message)
		}

		//Session Check
		if err := auth.CheckSession(context.TODO(), req.Session.UserID, req.Session.SessionID); err != nil {
			//session failed!
			onSessionFailed(conn)
			return
		}

		//Set username if status is Active
		if _, ok := gamedata.InGameClientData[conn]; !ok {
			onSessionFailed(conn)
			return
		}
		switch gamedata.InGameClientData[conn].Conn.ConState {
		case gamemodel.Ready:
			var err error
			gamedata.InGameClientData[conn], err = gamemodel.ActivateClient(req.Session.UserID)
			if err != nil {
				onSessionFailed(conn)
			}
			break
		case gamemodel.Active:
			break
		case gamemodel.Disconnected:
			return
		}

		//Handle Game Req
		for _, method := range req.GameMethods {
			log.Printf("ReceivedMethod: Method(%d), Content(%s)\n", method.MethodType, method.Content)

			switch method.MethodType {
			case gamemodel.Chat:
				gamemethod.HandleChatReceivedData(conn, method.Content)
				break
			default:
				break
			}
		}
	}
}

func onSessionFailed(conn *websocket.Conn) {
	conn.Close()
}