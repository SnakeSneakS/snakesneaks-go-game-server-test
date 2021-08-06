package core

import (
	"log"

	"golang.org/x/crypto/bcrypt"
)

//HashString hashes string (e.g. password) and return string
func HashString(s *string) error {
	bytes := []byte(*s)

	// `hash and set password
	hashed, err := bcrypt.GenerateFromPassword(bytes, 10) //(password, cost(4~31) )
	*s = string(hashed)
	log.Printf("hashed data: %s", *s)

	return err
}

//CompareHashedString hashes string (e.g. password) and return string
func CompareHashedString(hashedPass string, rawPass string) error {
	return bcrypt.CompareHashAndPassword([]byte(hashedPass), []byte(rawPass))
}
