package mysql

import (
	"fmt"
	"log"
	"os"
	"time"

	"github.com/jinzhu/gorm"
	//mysql dialects is imported to handle mysql
	_ "github.com/jinzhu/gorm/dialects/mysql"
)

/*
var (
	dbms     = "mysql"
	USER     = os.Getenv("MYSQL_USER")
	PASS     = os.Getenv("MYSQL_PASSWORD")
	PROTOCOL = fmt.Sprintf("tcp(%s:%s)", os.Getenv("MYSQL_CONTAINER_NAME"), os.Getenv("MYSQL_PORT"))
	DBNAME   = os.Getenv("MYSQL_DATABASE")
)
*/

//Mysql struct
type Mysql struct {
	DB *gorm.DB
}

// NewMysql new
func NewMysql() *Mysql {
	mysql := &Mysql{
		DB: Connect(),
	}
	return mysql
}

//Connect connect to mysql
func Connect() *gorm.DB {
	DBMS := "mysql"
	USER := os.Getenv("MYSQL_USER")
	PASS := os.Getenv("MYSQL_PASSWORD")
	PROTOCOL := fmt.Sprintf("tcp(%s:%s)", os.Getenv("MYSQL_HOST_NAME"), os.Getenv("MYSQL_PORT"))
	DBNAME := os.Getenv("MYSQL_DATABASE")
	CHARSET := os.Getenv("MYSQL_CHARSET")
	PARSETIME := os.Getenv("MYSQL_PARSETIME")
	LOC := os.Getenv("MYSQL_LOCATION")

	CONNECT := USER + ":" + PASS + "@" + PROTOCOL + "/" + DBNAME + "?charset=" + CHARSET + "&parseTime=" + PARSETIME + "&loc=" + LOC

	log.Println("MYSQL-CONNECT: " + CONNECT)

	count := 0
	db, err := gorm.Open(DBMS, CONNECT)
	if err != nil {
		for {
			if err == nil {
				fmt.Println("")
				break
			}
			fmt.Print(".")
			time.Sleep(time.Second)
			count++
			if count > 10 {
				fmt.Println("")
				fmt.Println("DB connection failed")
				panic(err)
			}
			db, err = gorm.Open(DBMS, CONNECT)
		}
	}
	fmt.Println("mysql connection succeeded")

	return db
}

//Close close connecttion to mysql
func (m *Mysql) Close() {
	m.DB.Close()
}
