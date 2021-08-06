package gamemodel

//Transform user location, rotation
type Transform struct {
	Position Vector3 `json:"position"`
	Rotation Vector3 `json:"rotation"`
}

//Vevtor3 (x,y,z)
type Vector3 struct {
	X float32
	Y float32
	Z float32
}
