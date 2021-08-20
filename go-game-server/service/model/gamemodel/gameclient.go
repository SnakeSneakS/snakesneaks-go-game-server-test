package gamemodel

import (
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"
)

const (
	Ready ConnectionState = iota
	Active
	Disconnected
)

//ConnectionState is a connection state of game user
type ConnectionState int

//GameClientInfo user client data type in game
type GameClient struct {
	Info GameClientInfo
	//IngameInfo IngameInfo
	Conn GameClientConnection
}

//GameClientConnection
type GameClientConnection struct {
	ConState ConnectionState
	//ConLast  time.Time
	//Ws       *websocket.Conn
}

//GameClientInfo
type GameClientInfo struct {
	UserID   uint
	Username string
}

type IngameInfo struct {
	Transform Transform
}

//NewGameClient creates game client already registered
func NewGameClient() GameClient {
	clientInfo := GameClientInfo{UserID: 0, Username: ""}
	clientConn := GameClientConnection{ConState: Ready}
	return GameClient{Info: clientInfo, Conn: clientConn}
}

//ActivateClient Activate client
func ActivateClient(userID uint) (GameClient, error) {
	username, err := model.GetUsername(userID)
	if err != nil {
		return NewGameClient(), err
	}
	clientInfo := GameClientInfo{UserID: userID, Username: username}
	clientConn := GameClientConnection{ConState: Active}
	return GameClient{Info: clientInfo, Conn: clientConn}, nil
}
