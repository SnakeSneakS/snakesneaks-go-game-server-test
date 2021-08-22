package main

import (
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
		color.Red("router don't run because this is not debug mode!")
		return
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

	//Web Page
	router.GET("/", func(ctx *gin.Context) {
		//User
		db := sqlConnect()
		var users []model.User
		db.Order("createdAt asc").Find(&users)
		defer db.Close()
		log.Print(users)

		//Session
		sessionData := make([]SessionData, 0)
		//run when DEBUG_MODE
		if os.Getenv("DEBUG_MODE") == "True" {
			hostname := os.Getenv("GO_GAME_SERVER_CONTAINER_NAME")
			res, err := http.Get(fmt.Sprintf("http://%s:%s/api/debug/session", hostname, os.Getenv("GO_GAME_SERVER_PORT")))
			if err != nil {
				log.Println(err)
				ctx.HTML(200, "index.html", gin.H{
					"users": users,
				})
				return
			}
			defer res.Body.Close()
			if res.StatusCode != 200 {
				fmt.Printf("StatusCode: %d\n", res.StatusCode)
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
				return
			}
			log.Print(mapData)
			for k, v := range mapData {
				s := SessionData{
					UserID:     k,
					SessionInf: v,
				}
				sessionData = append(sessionData, s)
			}
		}
		log.Print(sessionData)

		ctx.HTML(200, "index.html", gin.H{
			"users":       users,
			"sessionData": sessionData,
		})
		return
	})

	//Signup
	router.POST("/signup", func(ctx *gin.Context) {
		username := ctx.PostForm("username")
		email := ctx.PostForm("email")
		password := ctx.PostForm("password")

		//save user data to mysql database
		u := model.User{Username: username, Email: email, Password: password}
		if _, err := auth.Signup(u); err != nil {
			log.Println("Failed to create new User!")
			return
		}

		ctx.Redirect(302, "/")
		return
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

	hostname := os.Getenv("HOST_NAME")
	if hostname == "localhost" {
		hostname = ""
	}
	if os.Getenv("USE_TLS") == "True" {
		//USE TLS
		addr := fmt.Sprintf("%s:%s", hostname, os.Getenv("GO_WEB_SERVER_PORT_TLS"))
		crtfile := fmt.Sprintf("./secret/%s", os.Getenv("GO_WEB_TLS_CERT_FILE"))
		keyfile := fmt.Sprintf("./secret/%s", os.Getenv("GO_WEB_TLS_KEY_FILE"))
		router.RunTLS(addr, crtfile, keyfile)
	} else {
		//DON'T USE TLS
		addr := fmt.Sprintf("%s:%s", hostname, os.Getenv("GO_WEB_SERVER_PORT"))
		router.Run(addr)
	}

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
