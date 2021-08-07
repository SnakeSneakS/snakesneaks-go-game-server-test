package auth

import (
	"context"
	"crypto/rand"
	"encoding/json"
	"errors"
	"io"
	"log"
	"math/big"
	"time"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"
)

var SessionData = map[string]SessionInf{} //userIDm sessionID,

const (
	sessionIDsize        = 32
	sessionExpireSeconds = 604800 //604800[s]=1[week], int64
)

//SessionInf is a information of session linked by "userID"
type SessionInf struct {
	SessionID string    `json:"session_id"` //session id
	Updated   time.Time `json:"updated"`    //last updated time
}

//NewSession creates and save new session
func NewSession(ctx context.Context, userID string) (string, error) {
	//create rondom session id
	m_sessionID, err := generateRandomString(sessionIDsize)
	if err != nil {
		log.Println("failed to generate rondom string:", err)
		return "", err
	}

	setSessionData(ctx, userID, m_sessionID)
	return m_sessionID, nil
}

//check of session is expired
func checkExpire(sessionInf SessionInf) error {
	if time.Since(sessionInf.Updated).Seconds() > sessionExpireSeconds {
		return errors.New("session expire")
	}
	return nil
}

//update session time
func updateSession(s SessionInf) {
	s.Updated = time.Now()
}

// generateRandomString create random string as sessionID
func generateRandomString(size int) (string, error) {
	const letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
	r := make([]byte, size)
	for i := 0; i < size; i++ {
		num, err := rand.Int(rand.Reader, big.NewInt(int64(len(letters))))
		if err != nil {
			return "", err
		}
		r[i] = letters[num.Int64()]
	}

	return string(r), nil
}

// GetSessionData get sessionId (for CheckSession)
func GetSessionData(ctx context.Context, userID string) (string, error) {
	session, ok := SessionData[userID]
	if ok != true {
		return "", errors.New("Session Data Not Found")
	}
	updateSession(session)
	return session.SessionID, nil
}

//setSessionData set session to redis database (login)
func setSessionData(ctx context.Context, userID string, sessionID string) error {
	m_sessionInf := SessionInf{
		SessionID: sessionID,
		Updated:   time.Now(),
	}
	SessionData[userID] = m_sessionInf
	return nil
}

//DeleteSessionData delete session (logout)
func DeleteSessionData(ctx context.Context, userID string) error {
	delete(SessionData, userID)

	log.Printf("delete session: key(%s)", userID)
	return nil
}

//CheckSession checks if session is already stored in redis database (every time)
func CheckSession(ctx context.Context, userID string, sessionID string) error {
	session, ok := SessionData[userID]
	if ok != true {
		log.Printf("Session Data Not Found")
		return errors.New("Session Data Not Found")
	}
	updateSession(session)

	if session.SessionID != sessionID {
		log.Printf("mis-match session: userID(%s), sessionID(%s), storedSessionID(%s)", userID, sessionID, session.SessionID)
		return errors.New("session id mismatch")
	}
	log.Printf("right match session: userID(%s), sessionID(%s)", userID, sessionID)
	return nil
}

//DeleteAllExpiredSessions delete all sessions expired
func DeleteAllExpiredSessions() {
	for k, v := range SessionData {
		if time.Since(v.Updated).Seconds() > sessionExpireSeconds {
			delete(SessionData, k)
		}
	}
}

//HTTTP
//CONNECTION
//MOJULE
//DeleteSessionHTTP check session from http request and return if session is not there
func DeleteSessionHTTP(r io.Reader) error {
	ctx := context.TODO()

	//decode request body: JSON->struct SessionReq
	decoder := json.NewDecoder(r)
	var d model.SessionReq
	if err := decoder.Decode(&d); err != nil {
		log.Println("Failed to decode sessionReq in request/", err)
		return err
	}

	//delete fail
	if err := DeleteSessionData(ctx, d.Session.UserID); err != nil {
		return err
	}

	return nil
}

//CheckSessionHTTP check session from http request and return if session is not there
func CheckSessionHTTP(r io.Reader) error {
	ctx := context.TODO()

	//decode request body: JSON->struct SessionReq
	decoder := json.NewDecoder(r)
	var d model.SessionReq
	if err := decoder.Decode(&d); err != nil {
		log.Println("Failed to decode sessionReq in request/", err)
		return err
	}

	//if session not found
	if err := CheckSession(ctx, d.Session.UserID, d.Session.SessionID); err != nil {
		return err
	}

	//if session found
	return nil
}

/*

MEMO:
https://github.com/go-redis/redis/issues/807
can somebody tell me how to delete redis key with prefix ?

example i have redis key :
category:book
category:movie

i want to delete all redis key with prefix category*

thanks

iter := client.Scan(0, "prefix*", 0).Iterator()
for iter.Next() {
    err := client.Del(iter.Val()).Err()
    if err != nil { panic(err) }
}
if err := iter.Err(); err != nil {
    panic(err)
}
*/

/*
MEMO:
how about using jwt, not using session?
//https://qiita.com/po3rin/items/740445d21487dfcb5d9f
*/
