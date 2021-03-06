package server

import (
	"fmt"
	"log"
	"net/http"
	"os"

	"github.com/fatih/color"
	"github.com/joho/godotenv"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/handler"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/auth"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame"
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
	color.Green(fmt.Sprintf("DEBUG_MODE: %t", os.Getenv("DEBUG_MODE") == "True"))
	color.Green(fmt.Sprintf("USE_TLS: %t", os.Getenv("USE_TLS") == "True"))
	color.Green(fmt.Sprintf("CREATE_ACCOUNT: %t", os.Getenv("CREATE_ACCOUNT") == "True"))
	color.Green(fmt.Sprintf("ALLOW_ORIGIN: %s", os.Getenv("ALLOW_ORIGIN")))

	// create handle
	h, err := handler.NewHandler()
	if err != nil {
		log.Fatalln("Failed to initialize root handler ", err)
		return
	}

	// session management
	auth.StartSessionManegement()

	//game manegement
	ingame.StartGame()

	// listen
	log.Println("Server started")
	allow_hostname := os.Getenv("ALLOW_HOST_NAME")

	if os.Getenv("USE_TLS") == "True" {
		//USE TLS
		addr := fmt.Sprintf("%s:%s", allow_hostname, os.Getenv("GO_GAME_SERVER_PORT_TLS"))
		//put certification file and key file into secret folder
		crtfile := fmt.Sprintf("./secret/%s", os.Getenv("GO_GAME_TLS_CERT_FILE"))
		keyfile := fmt.Sprintf("./secret/%s", os.Getenv("GO_GAME_TLS_KEY_FILE"))
		if err := http.ListenAndServeTLS(addr, crtfile, keyfile, h); err != nil {
			panic(err)
		}
	} else {
		//DON'T USE TLS
		addr := fmt.Sprintf("%s:%s", allow_hostname, os.Getenv("GO_GAME_SERVER_PORT"))
		log.Printf("address: %s", addr)
		if err := http.ListenAndServe(addr, h); err != nil {
			panic(err)
		}
	}

}
