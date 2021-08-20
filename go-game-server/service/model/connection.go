package model

type ConnStatus int

const (
	Success ConnStatus = iota
	AuthError
)
