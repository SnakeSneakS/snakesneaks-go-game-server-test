package model

//StatusRes is a only status response to http request
type StatusRes struct {
	Status ConnStatus `json:"status"`
}

//SessionRes is a response to user's login http request
type SessionRes struct {
	Status  ConnStatus `json:"status"`
	Session Session    `json:"session"`
}
