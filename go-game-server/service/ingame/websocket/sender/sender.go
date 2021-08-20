package sender

import (
	"log"
	"time"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamedata"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamemethod"
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
			for conn, _ := range gamedata.InGameClientData {
				if err := conn.WriteMessage(m.MessageType, m.Message); err != nil {
					log.Println(err)
					return
				}
			}
		}
	}()
}
