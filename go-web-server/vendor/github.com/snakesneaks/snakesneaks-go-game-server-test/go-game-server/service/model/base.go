package model

import (
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/mysql"
)

//StartDB initialization to load env and migrate db
func StartDB() {

	//connect to database & migration
	//mysqp:
	m := mysql.NewMysql()
	defer m.Close()
	m.DB.AutoMigrate(&User{})

}
