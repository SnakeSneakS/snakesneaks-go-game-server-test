package websocket

import "github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/websocket/sender"

func StartWebsocketMessageManager() {
	sender.StartMessageSender()
}
