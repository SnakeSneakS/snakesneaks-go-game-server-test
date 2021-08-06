package gamemodel

//GameUser user in game
type GameUser struct {
	Username  string    `json:"username"`
	Transform Transform `json:"transform"`
}
