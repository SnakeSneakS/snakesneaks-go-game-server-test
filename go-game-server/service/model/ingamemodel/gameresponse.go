package ingamemodel

type GameRes struct {
	//Status   model.StatusRes `json:"status"`
	Response []GameResUnit `json:"response"`
}

type GameResUnit struct {
	UserID  uint         `json:"user_id"`
	Methods []GameMethod `json:"methods"`
}
