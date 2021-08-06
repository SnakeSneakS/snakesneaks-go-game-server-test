package game

import (
	"net/http"

	"github.com/gorilla/mux"
)

// Handler handle request to game
type Handler struct {
	r *mux.Router
}

// NewHandler instantiate
func NewHandler() *Handler {
	//log.Println("Game handler")
	//defer log.Println("Game handler end")

	h := &Handler{
		r: mux.NewRouter(),
	}

	//h.r.Handle("/login", login.NewHandler())

	return h
}

func (h *Handler) ServeHTTP(w http.ResponseWriter, r *http.Request) {
	h.r.ServeHTTP(w, r)
}
