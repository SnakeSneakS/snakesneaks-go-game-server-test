package game

import (
	"log"
	"net/http"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"

	"github.com/gorilla/websocket"
)

//*websocket.Conn
func HandleWebSocket(w http.ResponseWriter, r *http.Request) {
	//is it needed to store user ip address?
	var upgrader = websocket.Upgrader{
		ReadBufferSize:  model.ReadBufferSize,
		WriteBufferSize: model.WriteBufferSize,
	}
	conn, err := upgrader.Upgrade(w, r, nil)
	if err != nil {
		log.Println(err)
		return
	}

	//WRITE SPME CODE USING CONN
	for {
		messageType, p, err := conn.ReadMessage()
		if err != nil {
			log.Println(err)
			return
		}
		if err := conn.WriteMessage(messageType, p); err != nil {
			log.Println(err)
			return
		}
	}
}
