package gamemethod

import (
	"encoding/json"
	"errors"
	"fmt"
	"log"
	"os"
	"unsafe"

	"github.com/gorilla/websocket"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamedata"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel/gamemethod"
)

// HandleEnterWorldReceivedData when a new user entered world and established WebSocket connection
func HandleEnterWorldReceivedData(UserID uint32, Content string, conn *websocket.Conn) error {
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
	if os.Getenv("CREATE_ACCOUNT") != "True" {
		err := EnterWorldFromRequest(UserID, Content, conn)
		if err != nil {
			return err
		}
	} else {
		err := EnterWorldFromStoredData(UserID, conn)
		if err != nil {
			return err
		}
	}

	//send to new entered user
	var err error
	data := gamemethod.GetIngameClientsData{
		Clients: []ingamemodel.GameClient{},
	}
	for _, v := range gamedata.InGameClientData {
		data.Clients = append(data.Clients, v)
	}

	var content []byte
	content, err = json.Marshal(&data)
	if err != nil {
		log.Println(err)
		return err
	}
	methods := []ingamemodel.GameMethod{{MethodType: ingamemodel.GetIngameClientsInfo, Content: string(content)}}
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

func EnterWorldFromRequest(UserID uint32, Content string, conn *websocket.Conn) error {

	var enterWorldMethod gamemethod.EnterWorldMethod
	if err := json.Unmarshal([]byte(Content), &enterWorldMethod); err != nil {
		log.Printf("Failed to unmarshal chat method %s\n", Content)
	}
	log.Printf("Received EnterWorld Method, UserID: %d\n", enterWorldMethod.IngameClientData.Info.UserID)

	clientInfo := ingamemodel.GameClientInfo{UserID: UserID, Username: enterWorldMethod.IngameClientData.Info.Username}
	clientConn := ingamemodel.GameClientConnection{ConnState: ingamemodel.Active}
	gamedata.InGameClientData[conn] = ingamemodel.GameClient{Info: clientInfo, Conn: clientConn}
	return nil
}

func EnterWorldFromStoredData(UserID uint32, conn *websocket.Conn) error {
	var err error
	gamedata.InGameClientData[conn], err = ingamemodel.ActivateClient(UserID)
	if err != nil {
		return err
	}
	return nil
}
