package auth

import (
	"context"
	"errors"
	"log"
	"math/rand"
	"os"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/core"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/mysql"
)

//Signup user signup
//it might be better to email-configuration
//Post data User{ email, password }
func Signup(u model.User) (model.Session, error) {

	//If don't create new account
	if os.Getenv("CREATE_ACCOUNT") != "True" {
		return signUpWithNoAccount(u)
	}

	//password hash
	err := core.HashString(&u.Password)
	if err != nil {
		log.Println("failed to hash password")
		return model.Session{}, err
	}

	//save user data to mysql database
	m := mysql.NewMysql()
	defer m.Close()
	if err := m.DB.Select("Email", "Password", "Username").Create(&u).Error; err != nil {
		return model.Session{}, err
	}

	//Get the userID of created data
	if err := m.DB.Where("email = ? AND password = ?", u.Email, u.Password).First(&u).Error; err != nil {
		return model.Session{}, err //errors.New("not found created user Email:" + u.Email + " Pass:" + u.Password )
	}

	//Create session.
	ctx := context.TODO()
	sessionID, err := NewSession(ctx, u.ID)
	if err != nil {
		log.Println("failed to generate new session to ", u.ID)
		return model.Session{}, err
	}

	//login success and return
	log.Println("sessionID", sessionID)
	return model.Session{UserID: u.ID, SessionID: sessionID}, nil
}

func signUpWithNoAccount(u model.User) (model.Session, error) {
	n := 0
	u.ID = rand.Uint32()
	for _, ok := SessionData[u.ID]; ok == true; {
		u.ID = rand.Uint32()
		n += 1
		if n > 20 {
			return model.Session{}, errors.New("Failed to create new user_id. So stopped to create new session.")
		}
	}
	//Create session.
	ctx := context.TODO()
	sessionID, err := NewSession(ctx, u.ID)
	if err != nil {
		log.Println("failed to generate new session to ", u.ID)
		return model.Session{}, err
	}

	//login success and return
	log.Println("sessionID", sessionID)
	return model.Session{UserID: u.ID, SessionID: sessionID}, nil
}
