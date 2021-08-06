package auth

import (
	"net/http"
	"snakesneaks-go-server-2021-8/m/handler/api/auth/login"
	"snakesneaks-go-server-2021-8/m/handler/api/auth/logout"
	"snakesneaks-go-server-2021-8/m/handler/api/auth/signup"

	"github.com/gorilla/mux"
)

// Handler handle request to game
type Handler struct {
	r *mux.Router
}

// NewHandler instantiate
func NewHandler() *Handler {
	//log.Println("Auth handler")
	//defer log.Println("Auth handler end")

	h := &Handler{
		r: mux.NewRouter(),
	}

	h.r.Handle("/login", login.NewHandler())
	h.r.Handle("/logout", logout.NewHandler())
	h.r.Handle("/signup", signup.NewHandler())

	return h
}

func (h *Handler) ServeHTTP(w http.ResponseWriter, r *http.Request) {
	h.r.ServeHTTP(w, r)
}
