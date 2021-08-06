package api

import (
	"net/http"
	"snakesneaks-go-server-2021-8/m/handler/api/auth"
	"snakesneaks-go-server-2021-8/m/handler/api/game"

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

	return h
}

func (h *Handler) ServeHTTP(w http.ResponseWriter, r *http.Request) {
	h.r.ServeHTTP(w, r)
}
