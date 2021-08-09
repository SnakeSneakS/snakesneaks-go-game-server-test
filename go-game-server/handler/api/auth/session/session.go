package session

import (
	"bytes"
	"encoding/json"
	"io"
	"log"
	"net/http"
	"time"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/auth"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"
)

//Check if Session is valid or not

// Handler handle request to game
type Handler struct {
}

// NewHandler instantiate
func NewHandler() *Handler {
	//log.Println("Game handler")
	//defer log.Println("Game handler end")

	return nil
}

/*
REQUEST: curl -X POST -H "Content-Type: application/json" -d '{"session":{"user_id":"1","session_id":"u9RQzVGhHv5qmS49hsmb2PzM5uGfEc1y"}}' localhost:8080/api/auth/logout
RESPONSE: {"Status":"success"}
*/
func (h *Handler) ServeHTTP(w http.ResponseWriter, r *http.Request) {

	t1 := time.Now()

	//io.TeeReader is used to read io.Reader "multiple times"
	buf := bytes.NewBuffer(nil)
	rt := io.TeeReader(r.Body, buf)

	//if session check failer
	if err := auth.CheckSessionHTTP(rt); err != nil {
		sessionFailedWrite(w)
		return
	}

	//success
	sessionSuccessWrite(w)

	t2 := time.Now()
	log.Print(t2.Sub(t1))
	return
}

func sessionFailedWrite(w http.ResponseWriter) {
	if err := json.NewEncoder(w).Encode(&model.StatusRes{
		Status: model.ConnFailed,
	}); err != nil {
		w.WriteHeader(http.StatusInternalServerError)
	}
}

func sessionSuccessWrite(w http.ResponseWriter) {
	if err := json.NewEncoder(w).Encode(&model.StatusRes{
		Status: model.ConnSuccess,
	}); err != nil {
		w.WriteHeader(http.StatusInternalServerError)
	}
}
