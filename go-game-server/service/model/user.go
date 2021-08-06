package model

import (
	"encoding/json"
	"io"
	"time"
)

//User user
type User struct {
	ID        uint64    `gorm:"column:id;primaryKey;" json:"id"`
	Username  string    `gorm:"column:username;type:varchar(255);" json:"username"`
	Email     string    `gorm:"column:email;type:varchar(255);unique;" json:"email"`
	Password  string    `gorm:"column:password;type:varchar(255);" json:"password"`
	CreatedAt time.Time `gorm:"column:createdAt;type:datetime;default:current_timestamp;"`
	UpdatedAt time.Time `gorm:"column:updatedAt;type:datetime;"`
}

//NewUser User
func NewUser() *User {
	u := &User{
		ID:       1,
		Username: "default",
	}
	return u
}

//UserReqHTTP get user data from http request(json)
func UserReqHTTP(r io.Reader, src interface{}) error {
	decoder := json.NewDecoder(r)

	if err := decoder.Decode(&src); err != nil {
		return err
	}
	return nil
}
