module github.com/snakesneaks/snakesneaks-go-game-server-test

go 1.16

replace (
	github.com/snakesneaks/snakesneaks-go-game-server-test/g => ./go-game-server
	github.com/snakesneaks/snakesneaks-go-game-server-test/w => ./go-web-server
)

require (
	github.com/snakesneaks/snakesneaks-go-game-server-test/g v0.0.0-00010101000000-000000000000 // indirect
	github.com/snakesneaks/snakesneaks-go-game-server-test/w v0.0.0-00010101000000-000000000000 // indirect
)
