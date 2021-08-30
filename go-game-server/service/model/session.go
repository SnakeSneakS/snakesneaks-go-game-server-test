package model

import "time"

//Session session
type Session struct {
	UserID    uint32 `json:"user_id"`    //redis key
	SessionID string `json:"session_id"` //redis value
}

//SessionInf is a information of session linked by "userID"
type SessionInf struct {
	SessionID string    `json:"session_id"` //session id
	Updated   time.Time `json:"updated"`    //last updated time
}

const (
	sessionDB     = 0 //db 0 is for session management in redis
	sessionIDsize = 32
)
