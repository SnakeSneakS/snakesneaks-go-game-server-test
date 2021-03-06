package auth

import (
	"context"
	"log"
	"strconv"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/core"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/mysql"
)

//Signup user signup
//it might be better to email-configuration
//Post data User{ email, password }
func Signup(u model.User) (model.Session, error) {
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
	sessionID, err := NewSession(ctx, strconv.FormatUint(u.ID, 10))
	if err != nil {
		log.Println("failed to generate new session to ", u.ID)
		return model.Session{}, err
	}

	//login success and return
	log.Println("sessionID", sessionID)
	return model.Session{UserID: strconv.FormatUint(u.ID, 10), SessionID: sessionID}, nil
}
