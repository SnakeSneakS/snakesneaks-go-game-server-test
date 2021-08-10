package ingame

import (
	"context"
	"encoding/json"
	"log"
	"time"

	"github.com/gorilla/websocket"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/auth"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/gamemethod"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/gamemodel"
)

const (
	MessageSendInterval      = time.Millisecond * 200
	ConnTimeOutCheckInterval = time.Second * 5
	ConnTimeOutDuration      = time.Second * 10
)

var broadcast = make(chan MessageSet)
var StoredResMethods map[string]([]gamemodel.GameMethod) = map[string]([]gamemodel.GameMethod){}

type MessageSet struct {
	MessageType int
	Message     []byte
}

func StartConnMessageManager() {
	StartMessageSender()
}

//Sender
func StartMessageSender() {
	//to activate send, broadcast to message set : sent to all client
	go func() {
		for {
			time.Sleep(MessageSendInterval)

			if len(StoredResMethods) == 0 {
				continue
			}

			res := gamemodel.GameRes{Response: []gamemodel.GameResUnit{}}
			for user_id, methods := range StoredResMethods {
				resUnit := gamemodel.GameResUnit{UserID: user_id, Methods: methods}
				res.Response = append(res.Response, resUnit)
			}
			StoredResMethods = map[string]([]gamemodel.GameMethod){}

			message, err := json.Marshal(res)
			if err != nil {
				log.Println("Failed to Marshal GameResponse!")
			}
			broadcast <- MessageSet{MessageType: websocket.TextMessage, Message: message}

		}
	}()
	//to send
	go func() {
		for {
			m := <-broadcast
			log.Println("Send Data To All Connections")
			for conn, _ := range InGameClientData {
				if err := conn.WriteMessage(m.MessageType, m.Message); err != nil {
					log.Println(err)
					return
				}
			}
		}
	}()
}

// HandleMessage
func HandleMessage(messageType int, message []byte, conn *websocket.Conn, gcd gamemodel.GameClient) {
	log.Println("HandleWebSocket")

	if messageType == websocket.TextMessage {
		var req gamemodel.GameReq
		if err := json.Unmarshal(message, &req); err != nil {
			log.Printf(("Failed to Unmarshal message: %s\n"), message)
		}
		//Session Check
		if err := auth.CheckSession(context.TODO(), req.Session.UserID, req.Session.SessionID); err != nil {
			//session failed!
			onSessionFailed(conn)
			return
		}
		gcd.Info.UserID = req.Session.UserID

		//Handle Game Req
		for _, method := range req.GameMethods {
			log.Printf("ReceivedMethod: Method(%d), Content(%s)\n", method.MethodType, method.Content)
			var sendMethod gamemodel.GameMethod
			var send bool
			switch method.MethodType {
			case gamemodel.Chat:
				sendMethod, send = gamemethod.HandleChatReceivedData(method.Content)
				break
			default:
				break
			}
			if send == true {
				AddMethod(gcd.Info.UserID, sendMethod)
			}
		}
	}
}

//Just return the received data
/*
func PingPong(messageType int, message []byte, gcd gamemodel.GameClient) {
	m := MessageSet{MessageType: messageType, Message: message}
	broadcast <- m //ここに入れたものを送信する
}
*/

//Add Method to send
func AddMethod(userID string, gameMethod gamemodel.GameMethod) {
	methods, ok := StoredResMethods[userID]
	if ok != true {
		StoredResMethods[userID] = []gamemodel.GameMethod{gameMethod}
	}
	methods = append(methods, gameMethod)
}

func onSessionFailed(conn *websocket.Conn) {
	conn.Close()
}
