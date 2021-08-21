package ingamemodel

const (
	//Game Request Type: ReqType
	Idle GemeMethodType = iota //Enter World
	EnterWorld
	ExitWorld
	Chat
	Move
	GetIngameClientsInfo // server -> client only
)

type GemeMethodType int

type GameMethod struct {
	MethodType GemeMethodType `json:"method"`
	Content    string         `json:"content"`
}
