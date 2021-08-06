package login

import (
	"encoding/json"
	"log"
	"net/http"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/g/service/auth"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/g/service/model"
)

// Handler handle request to game
type Handler struct {
}

// NewHandler instantiate
func NewHandler() *Handler {
	//log.Println("login handler")
	//defer log.Println("login handler end")

	return nil
}

/*
REQUEST: curl -X POST -H "Content-Type: application/json" -d '{"user":{"username":"1","email":"1","password":"1"}}' localhost:8080/api/auth/login
RESPONSE: {"status":"success","session":{"user_id":"0","session_id":"zhF9lO2SQjcIMuvraDmj0vH37Apoi0f9"}}
*/
func (h *Handler) ServeHTTP(w http.ResponseWriter, r *http.Request) {
	//get data from request.body
	var u model.UserReq
	if err := model.UserReqHTTP(r.Body, &u); err != nil {
		log.Println("UserRequestHandle error: ", err)
		loginFailedWrite(w)
		return
	}

	//create new User and get session
	session, err := auth.Login(u.User)
	if err != nil {
		log.Println("Login error: ", err)
		loginFailedWrite(w)
		return
	}

	//success
	log.Println("Login success")
	log.Printf("userID: %s, sessionID: %s\n", session.UserID, session.SessionID)
	loginSuccessWrite(w, session)
	return
}

func loginFailedWrite(w http.ResponseWriter) {
	if err := json.NewEncoder(w).Encode(&model.SessionRes{
		Status: model.Failed,
	}); err != nil {
		w.WriteHeader(http.StatusInternalServerError)
	}
}

func loginSuccessWrite(w http.ResponseWriter, session model.Session) {
	if err := json.NewEncoder(w).Encode(&model.SessionRes{
		Status:  model.Success,
		Session: session,
	}); err != nil {
		w.WriteHeader(http.StatusInternalServerError)
	}
}
