package gamemodel

import "time"

//GameClient user client data type in game
type GameClient struct {
	UserID    string
	Username  string
	Transform Transform
	ConState  ConnectionState
	ConLast   time.Time
}
