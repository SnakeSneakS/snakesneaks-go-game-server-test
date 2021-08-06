package migrate

import (
	"database/sql"

	_ "github.com/go-sql-driver/mysql" //mysql
	"github.com/golang-migrate/migrate"
	"github.com/golang-migrate/migrate/database/mysql"
	_ "github.com/golang-migrate/migrate/source/file" //migration file
)

func runMigrate() {
	db, _ := sql.Open("mysql", "user:password@tcp(host:port)/dbname?multiStatements=true")
	driver, _ := mysql.WithInstance(db, &mysql.Config{})
	m, _ := migrate.NewWithDatabaseInstance(
		"file:///migrations",
		"mysql",
		driver,
	)

	m.Steps(2)
}
