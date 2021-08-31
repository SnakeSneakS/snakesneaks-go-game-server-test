package auth

import (
	"net/http"
	"os"
)

func AllowCORS(w http.ResponseWriter) {
	if os.Getenv("ALLOW_ORIGIN") == "*" {
		return
	} else {
		w.Header().Set("Access-Control-Allow-Headers", "*")
		w.Header().Set("Access-Control-Allow-Origin", os.Getenv("ALLOW_ORIGIN"))
		w.Header().Set("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS")
	}

}
