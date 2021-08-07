package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"os"
	"time"

	"github.com/fatih/color"
	"github.com/gin-gonic/gin"
	_ "github.com/go-sql-driver/mysql"
	"github.com/jinzhu/gorm"
	"github.com/joho/godotenv"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/auth"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"
)

//SessionData is a session data
type SessionData struct {
	UserID     string          `json:"user_id"`
	SessionInf auth.SessionInf `json:"sesson_inf"`
}

func main() {

	//env取得
	err := godotenv.Load(fmt.Sprintf("./%s.env", os.Getenv("GO_ENV")))
	if err != nil {
		//env読めなかった時の処理
		panic("failed to read .env file")
	}

	if os.Getenv("DEBUG_MODE") == "True" {
		color.Blue("This is DEBUG MODE")
	} else {
		color.Blue("This is \"not\" DEBUG MODE")
	}

	//database接続
	db := sqlConnect()
	db.AutoMigrate(&model.User{})
	defer db.Close() //実行が終わったらdb.Close
	//DELETE ALL
	//db.Delete(&User{})
	//db.Delete(&UserData{})

	router := gin.Default()
	router.Static("/statics", "./statics")
	router.LoadHTMLGlob("templates/*.html")

	router.GET("/", func(ctx *gin.Context) {
		//User
		db := sqlConnect()
		var users []model.User
		db.Order("createdAt asc").Find(&users)
		defer db.Close()

		//Session
		hostname := os.Getenv("GO_GAME_SERVER_CONTAINER_NAME")
		res, err := http.Get(fmt.Sprintf("http://%s:%s/api/debug/session", hostname, os.Getenv("GO_GAME_SERVER_PORT")))
		if err != nil {
			log.Fatal(err)
			ctx.HTML(200, "index.html", gin.H{
				"users": users,
			})
		}
		defer res.Body.Close()
		if res.StatusCode != 200 {
			fmt.Printf("StatusCode=%d\n", res.StatusCode)
			ctx.HTML(200, "index.html", gin.H{
				"users": users,
			})
			return
		}
		decoder := json.NewDecoder(res.Body)
		var mapData map[string]auth.SessionInf
		if err := decoder.Decode(&mapData); err != nil {
			log.Println("Failed to decode sessionData: ", err)
			ctx.HTML(200, "index.html", gin.H{
				"users": users,
			})
		}
		log.Print(mapData)
		sessionData := make([]SessionData, 0)
		for k, v := range mapData {
			s := SessionData{
				UserID:     k,
				SessionInf: v,
			}
			sessionData = append(sessionData, s)
		}
		log.Print(sessionData)
		ctx.HTML(200, "index.html", gin.H{
			"users":       users,
			"sessionData": sessionData,
		})
		return
	})

	router.POST("/signup", func(ctx *gin.Context) {
		username := ctx.PostForm("username")
		email := ctx.PostForm("email")
		password := ctx.PostForm("password")
		hostname := os.Getenv("GO_GAME_SERVER_CONTAINER_NAME")

		b, err := json.Marshal(model.UserReq{User: model.User{Username: username, Email: email, Password: password}})
		fmt.Printf("Send Data: \n%s", b)
		bb := bytes.NewBuffer(b)
		res, err := http.Post(fmt.Sprintf("http://%s:%s/api/auth/signup", hostname, os.Getenv("GO_GAME_SERVER_PORT")), "application/json", bb)
		if err != nil {
			log.Fatal(err)
		}
		defer res.Body.Close()
		if res.StatusCode != 200 {
			fmt.Printf("StatusCode=%d\n", res.StatusCode)
			return
		}

		ctx.Redirect(302, "/")
	})

	/*
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
	*/

	//run when DEBUG_MODE
	if os.Getenv("DEBUG_MODE") != "True" {
		log.Fatal("router don't run because this is not debug mode!")
		return
	}
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
