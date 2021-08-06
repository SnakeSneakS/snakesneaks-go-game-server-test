package handler

import (
	"log"
	"net/http"
	"time"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/g/handler/api"

	"github.com/gorilla/mux"
)

// Handler
type Handler struct {
	r *mux.Router
}

// NewHandler
func NewHandler() (*Handler, error) {
	//log.Println("start")
	//defer log.Println("end")

	h := &Handler{
		r: mux.NewRouter(),
	}

	h.r.Handle("/api{?:/?.*}", http.StripPrefix("/api", api.NewHandler()))

	return h, nil
}

func (h *Handler) ServeHTTP(w http.ResponseWriter, r *http.Request) {
	log.Println("~~ access ~~")
	log.Println(r.Header)
	log.Println(r.URL)
	t1 := time.Now()

	defer log.Println("~~ access end ~~")
	defer log.Print(time.Now().Sub(t1))

	h.r.ServeHTTP(w, r)
}
