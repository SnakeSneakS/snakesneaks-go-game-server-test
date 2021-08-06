package model

import (
	"snakesneaks-go-server-2021-8/m/service/model/mysql"
)

//StartDB initialization to load env and migrate db
func StartDB() {

	//connect to database & migration
	//mysqp:
	m := mysql.NewMysql()
	defer m.Close()
	m.DB.AutoMigrate(&User{})

}
