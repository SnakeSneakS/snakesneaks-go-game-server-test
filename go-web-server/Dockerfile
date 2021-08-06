FROM golang:latest
RUN mkdir /go/web
WORKDIR /go/web

#RUNを書いてからbuildしないとダメっぽい（？）
RUN go get github.com/gin-gonic/gin
RUN go get github.com/go-sql-driver/mysql
RUN go get github.com/jinzhu/gorm
RUN go get github.com/joho/godotenv