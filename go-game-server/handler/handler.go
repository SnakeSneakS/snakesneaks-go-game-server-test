package handler

import (
	"log"
	"net/http"
	"snakesneaks-go-server-2021-8/m/handler/api"
	"time"

	"github.com/gorilla/mux"
)

// Handler はサーバーのすべての通信を管理するハンドラです。
// /api でAPIをとり、それ以外はフロントエンド側のサーバーへ転送します
type Handler struct {
	r *mux.Router
}

// NewHandler は新しいHandlerを生成します。プロキシの転送先URLと、/apiで待ち受けるHandlerを指定します。
func NewHandler() (*Handler, error) {
	//log.Println("start")
	//defer log.Println("end")

	h := &Handler{
		r: mux.NewRouter(),
	}

	// /game 下の接続はgameHandlerで受け取る
	h.r.Handle("/api{?:/?.*}", http.StripPrefix("/api", api.NewHandler()))

	// /resources 下の接続はresourceHandlerで受け取る
	//h.r.Handle("/resources{?:/?.*}", http.StripPrefix("/resources", http.FileServer(http.Dir("/"))))

	// DEBUG: /log で ./log/mmorpg-backend を公開
	//r.Handle("/log{?:/?.*}", http.StripPrefix("/log", logHandler))

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
