package gamemethod

import (
	"encoding/json"
	"log"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/gamemodel"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/gamemodel/gamemethod"
)

// HandleChatReceivedData return (GameMethod,bool: return true if send method)
func HandleChatReceivedData(content string) (gamemodel.GameMethod, bool) {
	var chatMethod gamemethod.ChatMethod
	if err := json.Unmarshal([]byte(content), &chatMethod); err != nil {
		log.Printf("Failed to unmarshal chat method %s\n", content)
		return gamemodel.GameMethod{}, false
	}
	log.Printf("Received Chat Text: %s\n", chatMethod.Text)

	return gamemodel.GameMethod{MethodType: gamemodel.Chat, Content: content}, true
}
