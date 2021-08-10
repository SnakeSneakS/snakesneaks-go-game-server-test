package ingame

import (
	"log"
	"net/http"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/gamemodel"

	"github.com/gorilla/websocket"
)

//*websocket.Conn
func HandleWebSocket(w http.ResponseWriter, r *http.Request) {
	//is it needed to store user ip address?
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

	InGameClientData[conn] = gamemodel.GameClient{}

	for {
		messageType, message, err := conn.ReadMessage()
		if err != nil {
			log.Println(err)
			delete(InGameClientData, conn)
			log.Println("Delete connection!")
			return
		}

		if gcd, ok := InGameClientData[conn]; ok {
			HandleMessage(messageType, message, conn, gcd)
		} else {
			break
		}

	}

}
