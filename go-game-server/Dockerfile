FROM golang:latest

COPY ./.env /go/we/.env
RUN mkdir /go/src/api
WORKDIR /go/src/api
ADD . /go/src/api
ENV GO111MODULE=on

