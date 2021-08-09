package gamemodel

import (
	"time"

	"github.com/gorilla/websocket"
)

const (
	None ConnectionState = iota
	Active
	Disconnected
)

//ConnectionState is a connection state of game user
type ConnectionState int

//GameClientInfo user client data type in game
type GameClient struct {
	Info GameClientInfo
	Conn GameClientConnection
}

//GameClientConnection
type GameClientConnection struct {
	ConState ConnectionState
	ConLast  time.Time
	Ws       *websocket.Conn
}

//GameClientInfo
type GameClientInfo struct {
	UserID    string
	Username  string
	Transform Transform
}
