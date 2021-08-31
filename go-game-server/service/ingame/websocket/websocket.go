package websocket

import (
	"log"
	"net/http"
	"os"

	"github.com/gorilla/websocket"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamedata"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/websocket/receiver"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel"
)

func NewConnection(w http.ResponseWriter, r *http.Request) {
	//websocket
	var upgrader = websocket.Upgrader{
		ReadBufferSize:  model.ReadBufferSize,
		WriteBufferSize: model.WriteBufferSize,
		//EnableCompression: true,
		CheckOrigin: func(r *http.Request) bool {
			if os.Getenv("ALLOW_ORIGIN") == "*" {
				//log.Println("ALLOW ALL ORIGIN")
				return true
			} else {
				origin := r.Header.Get(("Origin"))
				return origin == os.Getenv("ALLOW_ORIGIN")
			}
		},
	}
	conn, err := upgrader.Upgrade(w, r, nil)
	if err != nil {
		log.Println(err)
		return
	}

	//SetConnectionData
	clientData := ingamemodel.NewGameClient()
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
