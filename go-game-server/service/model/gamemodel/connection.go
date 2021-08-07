package gamemodel

const (
	None ConnectionState = iota
	Active
	Disconected
)

type ConnectionState int
