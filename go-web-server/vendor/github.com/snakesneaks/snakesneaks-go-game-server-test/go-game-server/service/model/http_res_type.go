package model

const (
	//Failed is status "failed"
	Failed = "failed"
	//Success is status "success"
	Success = "success"
)

//StatusRes is a only status response to http request
type StatusRes struct {
	Status string `json:"status"`
}

//SessionRes is a response to user's login http request
type SessionRes struct {
	Status  string  `json:"status"`
	Session Session `json:"session"`
}
