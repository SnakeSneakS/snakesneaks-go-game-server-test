package model

import (
	"encoding/json"
	"net/http"
)

//StatusRes is a only status response to http request
type StatusRes struct {
	Status ConnStatus `json:"status"`
}

//SessionRes is a response to user's login http request
type SessionRes struct {
	Status  ConnStatus `json:"status"`
	Session Session    `json:"session"`
}

func SendStatusResponse(w http.ResponseWriter, status ConnStatus, responseCode int) {
	if err := json.NewEncoder(w).Encode(&SessionRes{
		Status: status,
	}); err != nil {
		w.WriteHeader(responseCode)
	}
}
