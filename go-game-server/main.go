package main

import (
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/server"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"
)

func main() {
	server.LoadENV("./.env")
	model.StartDB()
	server.RunServer()
}
