package model

//SessionReq handle request data (json)
//request data: {"session": {"user_id": "", "session_id": ""}}
type SessionReq struct {
	Session Session `json:"session"`
}

//UserReq is the request for User
type UserReq struct {
	User User `json:"user"`
}
