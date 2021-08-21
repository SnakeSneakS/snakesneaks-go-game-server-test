package gamemethod

import "github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel"

type GetIngameClientsData struct {
	Clients []ingamemodel.GameClient `json:"clients"`
}
