package server

import (
	"fmt"
	"log"
	"net/http"
	"os"

	"github.com/fatih/color"
	"github.com/joho/godotenv"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/handler"
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
	if os.Getenv("DEBUG_MODE") == "True" {
		color.Blue("This is DEBUG MODE")
	} else {
		color.Blue("This is \"not\" DEBUG MODE")
	}

	// create handle
	h, err := handler.NewHandler()
	if err != nil {
		log.Fatalln("Failed to initialize root handler ", err)
		return
	}

	// listen
	log.Println("Server started")
	hostname := os.Getenv("HOST_NAME")
	if hostname == "localhost" {
		hostname = ""
	}
	addr := fmt.Sprintf("%s:%s", hostname, os.Getenv("GO_GAME_SERVER_PORT"))
	log.Printf("address: %s", addr)
	if err := http.ListenAndServe(addr, h); err != nil {
		panic(err)
	}
}
