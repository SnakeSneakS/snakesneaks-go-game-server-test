package main

import (
	"snakesneaks-go-server-2021-8/m/server"
	"snakesneaks-go-server-2021-8/m/service/model"
)

func main() {
	server.LoadENV("./.env")
	model.StartDB()
	server.RunServer()
}
