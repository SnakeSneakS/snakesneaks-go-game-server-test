package websocket

import (
	"net/http"

	"github.com/gorilla/mux"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/ingame/websocket"
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
	websocket.NewConnection(w, r)
}
