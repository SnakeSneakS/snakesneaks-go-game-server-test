package websocket

import (
	"log"
	"net/http"

	"github.com/gorilla/websocket"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamedata"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/websocket/receiver"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/gamemodel"
)

func NewConnection(w http.ResponseWriter, r *http.Request) {
	//websocket
	var upgrader = websocket.Upgrader{
		ReadBufferSize:  model.ReadBufferSize,
		WriteBufferSize: model.WriteBufferSize,
		//EnableCompression: true,
	}
	conn, err := upgrader.Upgrade(w, r, nil)
	if err != nil {
		log.Println(err)
		return
	}

	//SetConnectionData
	clientData := gamemodel.NewGameClient()
	gamedata.InGameClientData[conn] = clientData

	for {
		messageType, message, err := conn.ReadMessage()
		if err != nil {
			log.Println(err)
			delete(gamedata.InGameClientData, conn)
			log.Println("Delete connection!")
			return
		}

		receiver.HandleMessage(messageType, message, conn)

	}
}
