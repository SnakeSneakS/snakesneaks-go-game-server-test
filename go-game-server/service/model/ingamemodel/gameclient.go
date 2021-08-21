package ingamemodel

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
	Info       GameClientInfo       `json:"info"`
	IngameInfo IngameInfo           `json:"ingame_info"`
	Conn       GameClientConnection `json:"conn"`
}

//GameClientConnection
type GameClientConnection struct {
	ConnState ConnectionState `json:"conn_state"`
	//ConnLast  time.Time `json:"conn_last,omitempty"`
}

//GameClientInfo
type GameClientInfo struct {
	UserID   uint   `json:"user_id"`
	Username string `json:"username"`
}

type IngameInfo struct {
	Transform Transform `json:"transform"`
}

func NewIngameInfo() IngameInfo {
	return IngameInfo{NewTransform(NewVector3(0, 0, 0), NewVector3(0, 0, 0))}
}

//NewGameClient creates game client already registered
func NewGameClient() GameClient {
	clientInfo := GameClientInfo{UserID: 0, Username: ""}
	clientConn := GameClientConnection{ConnState: Ready}
	clientIngameInfo := NewIngameInfo()
	return GameClient{Info: clientInfo, Conn: clientConn, IngameInfo: clientIngameInfo}
}

//ActivateClient Activate client
func ActivateClient(userID uint) (GameClient, error) {
	username, err := model.GetUsername(userID)
	if err != nil {
		return NewGameClient(), err
	}
	clientInfo := GameClientInfo{UserID: userID, Username: username}
	clientConn := GameClientConnection{ConnState: Active}
	return GameClient{Info: clientInfo, Conn: clientConn}, nil
}
