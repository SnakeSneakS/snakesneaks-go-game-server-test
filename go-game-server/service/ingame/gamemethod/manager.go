package gamemethod

import (
	"encoding/json"
	"log"

	"github.com/gorilla/websocket"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel"
)

var StoredBroadcastMethods map[uint]([]ingamemodel.GameMethod) = map[uint]([]ingamemodel.GameMethod){}

type MessageSet struct {
	MessageType int
	Message     []byte
}

//Add Method to send
func AddBroadcastMethod(userID uint, gameMethod ingamemodel.GameMethod) {
	methods, ok := StoredBroadcastMethods[userID]
	if ok != true {
		StoredBroadcastMethods[userID] = []ingamemodel.GameMethod{gameMethod}
	}
	methods = append(methods, gameMethod)
}

func StartGameMethodManager() {

}

//broadcast
func SendBroadcastMethod(broadcast chan MessageSet) {
	if len(StoredBroadcastMethods) == 0 {
		return
	}

	res := ingamemodel.GameRes{Response: []ingamemodel.GameResUnit{}}
	for user_id, methods := range StoredBroadcastMethods {
		resUnit := ingamemodel.GameResUnit{UserID: user_id, Methods: methods}
		res.Response = append(res.Response, resUnit)
	}
	StoredBroadcastMethods = map[uint]([]ingamemodel.GameMethod){}

	message, err := json.Marshal(res)
	if err != nil {
		log.Println("Failed to Marshal GameResponse!")
	}
	broadcast <- MessageSet{MessageType: websocket.TextMessage, Message: message}

}
