package gamemethod

import (
	"encoding/json"
	"log"

	"github.com/gorilla/websocket"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/gamemodel"
)

var StoredResMethods map[uint]([]gamemodel.GameMethod) = map[uint]([]gamemodel.GameMethod){}

type MessageSet struct {
	MessageType int
	Message     []byte
}

//Add Method to send
func AddBroadcastMethod(userID uint, gameMethod gamemodel.GameMethod) {
	methods, ok := StoredResMethods[userID]
	if ok != true {
		StoredResMethods[userID] = []gamemodel.GameMethod{gameMethod}
	}
	methods = append(methods, gameMethod)
}

func StartGameMethodManager() {

}

//broadcast
func SendBroadcastMethod(broadcast chan MessageSet) {
	if len(StoredResMethods) == 0 {
		return
	}

	res := gamemodel.GameRes{Response: []gamemodel.GameResUnit{}}
	for user_id, methods := range StoredResMethods {
		resUnit := gamemodel.GameResUnit{UserID: user_id, Methods: methods}
		res.Response = append(res.Response, resUnit)
	}
	StoredResMethods = map[uint]([]gamemodel.GameMethod){}

	message, err := json.Marshal(res)
	if err != nil {
		log.Println("Failed to Marshal GameResponse!")
	}
	broadcast <- MessageSet{MessageType: websocket.TextMessage, Message: message}

}
