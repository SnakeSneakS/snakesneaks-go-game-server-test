package logout

import (
	"bytes"
	"encoding/json"
	"io"
	"log"
	"net/http"
	"snakesneaks-go-server-2021-8/m/service/auth"
	"snakesneaks-go-server-2021-8/m/service/model"
	"time"
)

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

	//if session check failedr
	if err := auth.CheckSessionHTTP(rt); err != nil {
		logoutFailedWrite(w)
		return
	}

	//if delete session faled
	if err := auth.DeleteSessionHTTP(buf); err != nil {
		logoutFailedWrite(w)
		return
	}

	//success
	logoutSuccessWrite(w)

	t2 := time.Now()
	log.Print(t2.Sub(t1))
	return
}

func logoutFailedWrite(w http.ResponseWriter) {
	if err := json.NewEncoder(w).Encode(&model.StatusRes{
		Status: model.Failed,
	}); err != nil {
		w.WriteHeader(http.StatusInternalServerError)
	}
}

func logoutSuccessWrite(w http.ResponseWriter) {
	if err := json.NewEncoder(w).Encode(&model.StatusRes{
		Status: model.Success,
	}); err != nil {
		w.WriteHeader(http.StatusInternalServerError)
	}
}
