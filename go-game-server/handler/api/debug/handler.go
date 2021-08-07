package debug

import (
	"net/http"

	"github.com/gorilla/mux"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/handler/api/debug/session"
)

// Handler handle request to game
type Handler struct {
	r *mux.Router
}

// NewHandler instantiate
func NewHandler() *Handler {
	//log.Println("Debug handler")
	//defer log.Println("Debug handler end")

	h := &Handler{
		r: mux.NewRouter(),
	}

	h.r.Handle("/session", session.NewHandler())

	return h
}

func (h *Handler) ServeHTTP(w http.ResponseWriter, r *http.Request) {
	h.r.ServeHTTP(w, r)
}
