package game

import (
	"net/http"

	"github.com/gorilla/mux"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/g/service/game"
)

// Handler handle request to game
type Handler struct {
	r *mux.Router
}

// NewHandler instantiate
func NewHandler() *Handler {
	h := &Handler{}

	return h
}

/*
WEB SOCKET CONNECTION
*/
func (h *Handler) ServeHTTP(w http.ResponseWriter, r *http.Request) {
	game.HandleWebSocket(w, r)
}
