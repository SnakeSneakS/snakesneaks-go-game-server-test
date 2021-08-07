package session

import (
	"encoding/json"
	"log"
	"net/http"
	"os"
	"time"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/auth"
)

// Handler handle request to game
type Handler struct {
}

// NewHandler instantiate
func NewHandler() *Handler {
	return nil
}

/*
REQUEST: curl -X GET -H "Content-Type: application/json" -d localhost:8080/api/debug/session
RESPONSE: [{"user_id":"0","session_inf":{"session_id":"XXXX","updated":time.time}}]
*/
func (h *Handler) ServeHTTP(w http.ResponseWriter, r *http.Request) {

	t1 := time.Now()

	//success
	sessionDataWrite(w)

	t2 := time.Now()
	log.Print(t2.Sub(t1))
	return
}

func sessionDataWrite(w http.ResponseWriter) {
	log.Print(auth.SessionData)
	if os.Getenv("DEBUG_MODE") == "True" {
		d, err := json.Marshal(auth.SessionData)
		if err != nil {
			log.Print("SessionData Marshal Error: " + err.Error())
		}
		w.Write([]byte(d))
	}
}
