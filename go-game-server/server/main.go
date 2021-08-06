package server

import (
	"fmt"
	"log"
	"net/http"
	"os"

	"snakesneaks-go-server-2021-8/m/handler"

	"github.com/joho/godotenv"
)

//LoadENV load env from {path} (e.g. ./env)
func LoadENV(path string) {
	err := godotenv.Load(path)
	if err != nil {
		//if env read failed
		panic("failed to read .env file")
	}
}

//RunServer up server
func RunServer() {
	// create handle
	h, err := handler.NewHandler()
	if err != nil {
		log.Fatalln("Failed to initialize root handler ", err)
		return
	}

	// listen
	log.Println("Server started")
	addr := fmt.Sprintf("%s:%s", os.Getenv("HOST_NAME"), os.Getenv("GO_GAME_SERVER_PORT"))
	log.Printf("address: %s", addr)
	if err := http.ListenAndServe(addr, h); err != nil {
		panic(err)
	}
}
