APP?=app
SERVER_1?=go-game-server #game server
SERVER_2?=go-web-server #web server

.PHONY: docker-build
## docker-build: build docker container
docker-build: 
	cp .env ./go-game-server/.env 
	cp .env ./go-web-server/.env 
	docker-compose build 

.PHONY: docker-run
## docker-run: run docker container
docker-run: 
	cp .env ./go-game-server/.env 
	cp .env ./go-web-server/.env 
	docker-compose up -d

.PHONY: docker-run-build
## docker-run-build: run&build docker container
docker-run-build: 
	cp .env ./go-game-server/.env 
	cp .env ./go-web-server/.env 
	docker-compose up -d --build 

.PHONY: docker-stop
## docker-stop: stop docker container
docker-stop: 
	docker-compose stop

check-environment: 
ifndef ENV
	$(error ENV not set, allowed values - `test` or `production`) 
endif 

.PHONY: help
## help: prints this help message
help:
	@echo "Usage: \n"
	@sed -n 's/^##//p' ${MAKEFILE_LIST} | column -t -s ':' |  sed -e 's/^/ /'
