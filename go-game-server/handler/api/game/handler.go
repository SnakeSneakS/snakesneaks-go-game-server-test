package game

import (
	"net/http"

	"github.com/gorilla/mux"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/handler/api/game/websocket"
)

// Handler handle request to game
type Handler struct {
	r *mux.Router
}

// NewHandler instantiate
func NewHandler() *Handler {
	h := &Handler{
		r: mux.NewRouter(),
	}

	h.r.Handle("/websocket{?:/?.*}", http.StripPrefix("/websocket", websocket.NewHandler()))

	return h
}

/*
WEB SOCKET CONNECTION
*/
func (h *Handler) ServeHTTP(w http.ResponseWriter, r *http.Request) {
	h.r.ServeHTTP(w, r)
}
