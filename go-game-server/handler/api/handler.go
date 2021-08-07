package api

import (
	"net/http"
	"os"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/handler/api/auth"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/handler/api/debug"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/handler/api/game"

	"github.com/gorilla/mux"
)

//Handler handle access to ?/api/
type Handler struct {
	r *mux.Router
}

//NewHandler create Handler to ?/api/?
func NewHandler() *Handler {
	//log.Println("api handler start")
	//defer log.Println("api handler finish")

	h := &Handler{
		r: mux.NewRouter(),
	}

	h.r.Handle("/auth{?:/?.*}", http.StripPrefix("/auth", auth.NewHandler()))
	h.r.Handle("/game{?:/?.*}", http.StripPrefix("/game", game.NewHandler()))
	if os.Getenv("DEBUG_MODE") == "True" {
		h.r.Handle("/debug{?:/?.*}", http.StripPrefix("/debug", debug.NewHandler()))
	}
	return h
}

func (h *Handler) ServeHTTP(w http.ResponseWriter, r *http.Request) {
	h.r.ServeHTTP(w, r)
}
