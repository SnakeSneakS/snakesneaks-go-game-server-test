package ingame

import "github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/websocket"

func StartGame() {
	websocket.StartWebsocketMessageManager()
}
