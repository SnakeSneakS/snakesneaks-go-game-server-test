package main

import (
	"github.com/snakesneaks/snakesneaks-go-game-server-test/g/server"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/g/service/model"
)

func main() {
	server.LoadENV("./.env")
	model.StartDB()
	server.RunServer()
}
