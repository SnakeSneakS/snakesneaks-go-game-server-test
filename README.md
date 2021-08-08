# NOW Developing... 
Every part has possibility to change... 

# Usage 
- clone this repository ```git clone https://github.com/SnakeSneakS/snakesneaks-go-game-server-test.git```
- cd this repository ```cd snakesneaks-go-game-server-test```
- set up .env file ```cp ./sample.env ./.env```
- modify .env file as you want 
- open unity-game-client and set up information for hostname and port (or others) **THIS IS NOT SUITABLE, SO I WILL IMPROVE YOU CAN SETUP JUST EDIT ".env" FILE** 
- build and run server ```make docker-run-build``` 
- check working by accessing ```http://${hostname}:${port}```
- check working by open unity-game-client/XXXXXXXXX and test **I WILL ADD EXPLANATON LATER** 
- after check finished, stop server ```make docker-stop```

# USE TLS 
- in env file, set ```USE_TLS=True``` 
- in ```go-game-server/secret```, create certification files and set .env file ```GO_GAME_TLS_CERT_FILE``` and ```GO_GAME_TLS_CERT_FILE```. If you debug in local, openssl might be good. (```openssl genrsa -out debug.key 2048``` ```openssl req -new -x509 -sha256 -key debug.key -out debug.crt -days 365```)
- in ```go-game-server/secret```, create certification files in the same way
- in order to access https web page with your self-signed certificate, you need to make your browser to accept all certification 

# Env File
- ```DEBUG_MODE```: ```True``` or other, if not ```True```, you can't access to web-server and go-server-sessionData. 