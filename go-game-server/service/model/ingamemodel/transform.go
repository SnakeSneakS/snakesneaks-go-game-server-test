package ingamemodel

//Transform user location, rotation
type Transform struct {
	Position Vector3 `json:"position,string"`
	Rotation Vector3 `json:"rotation,string"`
}

func NewTransform(position Vector3, rotation Vector3) Transform {
	return Transform{Position: position, Rotation: rotation}
}

//Vevtor3 (x,y,z)
type Vector3 struct {
	X float32
	Y float32
	Z float32
}

func NewVector3(x, y, z float32) Vector3 {
	return Vector3{X: x, Y: y, Z: z}
}
