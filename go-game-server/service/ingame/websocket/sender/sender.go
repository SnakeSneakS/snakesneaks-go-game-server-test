package sender

import (
	"fmt"
	"log"
	"time"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamedata"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamemethod"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel"
)

const (
	MessageSendInterval      = time.Millisecond * 200
	ConnTimeOutCheckInterval = time.Second * 5
	ConnTimeOutDuration      = time.Second * 10
)

var broadcast = make(chan gamemethod.MessageSet)

//Sender
func StartMessageSender() {
	//set to "broadcast" channel: which is sent to all client
	go func() {
		for {
			time.Sleep(MessageSendInterval)
			gamemethod.SendBroadcastMethod(broadcast)
		}
	}()
	//to send broadcast
	go func() {
		for {
			m := <-broadcast
			log.Println("Send Data To All Connections")
			for conn, data := range gamedata.InGameClientData {
				//limit only active ingame client
				if data.Conn.ConnState != ingamemodel.Active {
					log.Println(fmt.Sprintf("User(id: %d) is not active, so ignore when broadcast", data.Info.UserID))
					return
				}
				if err := conn.WriteMessage(m.MessageType, m.Message); err != nil {
					log.Println(err)
					return
				}
			}
		}
	}()
}
