package auth

import (
	"net/http"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/handler/api/auth/login"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/handler/api/auth/logout"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/handler/api/auth/session"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/handler/api/auth/signup"

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
	h.r.Handle("/session", session.NewHandler())

	return h
}

func (h *Handler) ServeHTTP(w http.ResponseWriter, r *http.Request) {
	h.r.ServeHTTP(w, r)
}
