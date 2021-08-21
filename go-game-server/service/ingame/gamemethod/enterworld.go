package gamemethod

import (
	"encoding/json"
	"errors"
	"fmt"
	"log"
	"unsafe"

	"github.com/gorilla/websocket"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamedata"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel/gamemethod"
)

// HandleEnterWorldReceivedData when a new user entered world and established WebSocket connection
func HandleEnterWorldReceivedData(UserID uint, conn *websocket.Conn) error {
	log.Printf("Received Enter World: %d\n", UserID)

	//Check connection's existance
	if _, ok := gamedata.InGameClientData[conn]; !ok {
		return errors.New("WebSocket connection not exist!")
	}
	if gamedata.InGameClientData[conn].Conn.ConnState != ingamemodel.Ready {
		if gamedata.InGameClientData[conn].Conn.ConnState == ingamemodel.Active {
			return errors.New(fmt.Sprintf("UserID(%d): this ingameClient is already active", UserID))
		} else {
			return errors.New(fmt.Sprintf("UserID(%d): this ingameClient is not ready state", UserID))
		}
	}

	//Activate User
	var err error
	gamedata.InGameClientData[conn], err = ingamemodel.ActivateClient(UserID)
	if err != nil {
		return err
	}

	//send to new entered user
	methods := []ingamemodel.GameMethod{{MethodType: ingamemodel.GetIngameClientsInfo}}
	resUnit := []ingamemodel.GameResUnit{{UserID: UserID, Methods: methods}}
	res := ingamemodel.GameRes{Response: resUnit}
	message, err := json.Marshal(res)
	if err != nil {
		log.Println("Failed to Marshal GameResponse!")
	}
	messageSet := MessageSet{MessageType: websocket.TextMessage, Message: message}
	if err := conn.WriteMessage(messageSet.MessageType, messageSet.Message); err != nil {
		log.Println(err)
		return err
	}

	//send to all users
	enterWorldMethod := gamemethod.EnterWorldMethod{IngameClientData: gamedata.InGameClientData[conn]}
	json, err := json.Marshal(&enterWorldMethod)
	if err != nil {
		log.Printf("Failed to marshal enterWorld method. UserID(%d)\n", UserID)
	}
	AddBroadcastMethod(UserID, ingamemodel.GameMethod{MethodType: ingamemodel.EnterWorld, Content: *(*string)(unsafe.Pointer(&json))})
	return nil
}
