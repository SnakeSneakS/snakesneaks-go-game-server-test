package main

import (
	"fmt"
	"log"
	"os"
	"strconv"
	"time"

	"github.com/gin-gonic/gin"
	_ "github.com/go-sql-driver/mysql"
	"github.com/jinzhu/gorm"
	"github.com/joho/godotenv"
)

//User is a user
type User struct {
	gorm.Model
	Name  string
	Email string
}

//UserData is a Account Data
type UserData struct { //dbの内容変えた時には、db作り直し。(dashbordでゴミ箱マークで消す→docker volume ls→表示されたdbを消す docker volume rm {名前})
	gorm.Model
	Name     string `form:"name" binding:"required" gorm:"unique;not null"`
	Email    string `form:"email" binding:"required" gorm:"unique;not null"`
	Password string `form:"password" binding:"required"`
}

func main() {

	//env取得
	err := godotenv.Load(fmt.Sprintf("./%s.env", os.Getenv("GO_ENV")))
	if err != nil {
		//env読めなかった時の処理
		panic("failed to read .env file")
	}

	//database接続
	db := sqlConnect()
	db.AutoMigrate(&User{})
	db.AutoMigrate(&UserData{})
	defer db.Close() //実行が終わったらdb.Close
	//DELETE ALL
	//db.Delete(&User{})
	//db.Delete(&UserData{})

	router := gin.Default()
	router.Static("/statics", "./statics")
	router.LoadHTMLGlob("templates/*.html")

	router.GET("/", func(ctx *gin.Context) {
		db := sqlConnect()
		var users []User
		var userDatas []UserData
		db.Order("created_at asc").Find(&users)
		db.Order("created_at asc").Find(&userDatas)
		defer db.Close()

		ctx.HTML(200, "index.html", gin.H{
			"users":     users,
			"userDatas": userDatas,
		})
	})

	//sign-in
	router.GET("/sign-in.html", func(ctx *gin.Context) {
		ctx.HTML(200, "sign-in.html", gin.H{
			"error": "",
		})
	})

	router.POST("/sign-in", func(ctx *gin.Context) {
		db := sqlConnect()
		name := ctx.PostForm("name")
		email := ctx.PostForm("email")
		password := ctx.PostForm("password")

		userData := &UserData{Name: name, Password: password, Email: email}

		/*
			if check := db.Where("Name = ?", name).First(&userData); check.Error != nil {
				fmt.Println("already exist user " + name + "with email " + email)
				ctx.HTML(200, "sign-in.html", gin.H{
					"error": "already existing email",
				})
			} else {
				db.Create(&userData)
				fmt.Println("create user " + name + "with email " + email)
				ctx.Redirect(302, "/")
			}*/

		//UNIQUEのやり方が分からない！！！errに必ずなる！？
		result := db.Create(&userData)
		defer db.Close()
		if result.Error != nil {
			fmt.Println("already exist user " + name + "with email " + email)
			ctx.HTML(200, "sign-in.html", gin.H{
				"error": "this account already exsist(name and email is unique)",
			})
		} else {
			fmt.Println("create user " + name + "with email " + email)
			ctx.Redirect(302, "/")
		}

	})

	router.POST("/delete-userData", func(ctx *gin.Context) {
		db := sqlConnect()

		var userData UserData
		db.Debug().Delete(&userData)
		defer db.Close()

		ctx.Redirect(302, "/")
	})

	router.POST("/new", func(ctx *gin.Context) {
		db := sqlConnect()
		name := ctx.PostForm("name")
		email := ctx.PostForm("email")
		fmt.Println("create user " + name + "with email " + email)
		db.Create(&User{Name: name, Email: email})
		defer db.Close()
		ctx.Redirect(302, "/")
	})

	router.POST("/delete/:id", func(ctx *gin.Context) {
		db := sqlConnect()
		n := ctx.Param("id")
		id, err := strconv.Atoi(n)
		if err != nil {
			panic("id is not a number")
		}
		var user User
		db.First(&user, id)
		db.Delete(&user)
		defer db.Close()

		ctx.Redirect(302, "/")
	})

	router.Run(fmt.Sprintf(":%s", os.Getenv("GO_WEB_SERVER_PORT")))
}

func sqlConnect() (database *gorm.DB) {
	DBMS := "mysql"
	USER := os.Getenv("MYSQL_USER")
	PASS := os.Getenv("MYSQL_PASSWORD")
	PROTOCOL := fmt.Sprintf("tcp(%s:%s)", os.Getenv("MYSQL_CONTAINER_NAME"), os.Getenv("MYSQL_PORT"))
	DBNAME := os.Getenv("MYSQL_DATABASE")

	CONNECT := USER + ":" + PASS + "@" + PROTOCOL + "/" + DBNAME + "?charset=utf8mb4&parseTime=true&loc=Asia%2FTokyo"

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
			if count > 180 {
				fmt.Println("")
				fmt.Println("DB connection failed")
				panic(err)
			}
			db, err = gorm.Open(DBMS, CONNECT)
		}
	}
	fmt.Println("DB connection succeeded")

	return db
}
