package auth

import (
	"context"
	"errors"
	"log"
	"net/http"
	"strconv"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/core"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/mysql"

	"gorm.io/gorm"
)

// NewLogin newLogin
func NewLogin(w http.ResponseWriter, r *http.Request) {
	message(w, r)
}

func message(w http.ResponseWriter, r *http.Request) {
	log.Println("login")
}

//Login user login
//post User{User.id, User.email, User.password}
//if login success return sessionID, else return ""
//use u1.id not u.id, because u1.id is a stored id, but u.id is a requested id.
func Login(u model.User) (model.Session, error) {
	log.Println("login start")
	defer log.Println("login end")

	//log.Println(u)

	//get user's email&password to check consistence below
	m := mysql.NewMysql()
	defer m.Close()
	u1 := &model.User{
		Password: "",
		Email:    "",
	}
	if err := m.DB.Where("email = ? ", u.Email).Select([]string{"id", "email", "password"}).First(&u1).Error; err != nil {
		log.Println(gorm.ErrRecordNotFound)
		return model.Session{}, err
	}

	//check if hashed password consistence with
	if err := core.CompareHashedString(u1.Password, u.Password); err != nil {
		log.Println("inconsistence: user.password ")
		return model.Session{}, errors.New("inconsistance user password")
	}

	//if password consistence, create session.
	ctx := context.TODO()
	sessionID, err := NewSession(ctx, strconv.FormatUint(u1.ID, 10))
	if err != nil {
		log.Println("failed to generate new session to ", u1.ID)
		return model.Session{}, err
	}

	//login success and return
	return model.Session{UserID: strconv.FormatUint(u1.ID, 10), SessionID: sessionID}, nil
}
