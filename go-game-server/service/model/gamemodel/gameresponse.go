package gamemodel

type GameRes struct {
	//Status   model.StatusRes `json:"status"`
	Response []GameResUnit `json:"response"`
}

type GameResUnit struct {
	UserID  string       `json:"user_id"`
	Methods []GameMethod `json:"methods"`
}
