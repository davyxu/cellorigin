package golog

type Level int

const (
	Level_Debug Level = iota
	Level_Info
	Level_Warn
	Level_Error
	Level_Fatal
)

var levelString = [...]string{
	"[DEBUG]",
	"[INFO]",
	"[WARN]",
	"[ERROR]",
	"[FATAL]",
}
