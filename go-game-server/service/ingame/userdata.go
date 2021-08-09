package ingame

import (
	"log"

	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model"
	"github.com/snakesneaks/snakesneaks-go-game-server-test/go-game-server/service/model/mysql"
	"gorm.io/gorm"
)

func getUser(userID uint) (model.User, error) {
	m := mysql.NewMysql()
	defer m.Close()
	u := model.User{
		Username: "",
	}
	if err := m.DB.Where("user_id = ? ", userID).Select([]string{"username"}).First(&u).Error; err != nil {
		log.Println(gorm.ErrRecordNotFound)
		return model.User{}, err
	}
	return u, nil
}
