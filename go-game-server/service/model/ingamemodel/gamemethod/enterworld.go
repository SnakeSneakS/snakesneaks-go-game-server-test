package gamemethod

import "github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/ingamemodel"

type EnterWorldMethod struct {
	IngameClientData ingamemodel.GameClient `json:"ingame_client_data"`
}
