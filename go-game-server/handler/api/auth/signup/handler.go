package signup

import (
	"encoding/json"
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
	//log.Println("login handler")
	//defer log.Println("login handler end")

	return nil
}

/*
REQUEST: curl -X POST -H "Content-Type: application/json" -d '{"user":{"username":"1","email":"1","password":"1"}}' localhost:8080/api/auth/signup
RESPONSE: {"status":"success","session":{"user_id":"1","session_id":"u9RQzVGhHv5qmS49hsmb2PzM5uGfEc1y"}}
*/
func (h *Handler) ServeHTTP(w http.ResponseWriter, r *http.Request) {

	t1 := time.Now()

	//get data from request.body
	var u model.UserReq
	if err := model.UserReqHTTP(r.Body, &u); err != nil {
		log.Println("UserRequestHandle error: ", err)
		signupFailedWrite(w)
		return
	}
	log.Print(u.User.Password) ///////////ここ見るとわかるように、jsonが適切にデコードされていない

	//create new User and get session
	session, err := auth.Signup(u.User)
	if err != nil {
		log.Println("signup error: ", err)
		signupFailedWrite(w)
		return
	}

	//success
	log.Println("Signup success")
	signupSuccessWrite(w, session)

	t2 := time.Now()
	log.Print(t2.Sub(t1))
	return
}

func signupFailedWrite(w http.ResponseWriter) {
	if err := json.NewEncoder(w).Encode(&model.StatusRes{
		Status: model.Failed,
	}); err != nil {
		w.WriteHeader(http.StatusInternalServerError)
	}
}

func signupSuccessWrite(w http.ResponseWriter, session model.Session) {
	if err := json.NewEncoder(w).Encode(&model.SessionRes{
		Status:  model.Success,
		Session: session,
	}); err != nil {
		w.WriteHeader(http.StatusInternalServerError)
	}
}
