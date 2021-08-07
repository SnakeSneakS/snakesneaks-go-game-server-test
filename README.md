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