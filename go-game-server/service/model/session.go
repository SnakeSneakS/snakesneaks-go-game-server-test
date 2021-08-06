package model

//Session session
type Session struct {
	UserID    string `json:"user_id"`    //redis key
	SessionID string `json:"session_id"` //redis value
}

const (
	sessionDB     = 0 //db 0 is for session management in redis
	sessionIDsize = 32
)
