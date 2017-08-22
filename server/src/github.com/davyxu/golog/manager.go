package golog

import (
	"errors"
	"io/ioutil"
	"os"
	"sync"
)

var (
	logMap      = map[string]*Logger{}
	logMapGuard sync.RWMutex
)

func add(l *Logger) {

	logMapGuard.Lock()

	if _, ok := logMap[l.name]; ok {
		panic("duplicate logger name:" + l.name)
	}

	logMap[l.name] = l

	logMapGuard.Unlock()
}

func str2loglevel(level string) Level {

	switch level {
	case "debug":
		return Level_Debug
	case "info":
		return Level_Info
	case "warn":
		return Level_Warn
	case "error":
		return Level_Error
	case "fatal":
		return Level_Fatal
	}

	return Level_Debug
}

var ErrLoggerNotFound = errors.New("logger not found")

func VisitLogger(name string, callback func(*Logger) bool) error {

	logMapGuard.RLock()

	defer logMapGuard.RUnlock()

	if name == "*" {

		for _, l := range logMap {
			if !callback(l) {
				break
			}
		}

	} else {
		l, ok := logMap[name]
		if !ok {
			return ErrLoggerNotFound
		}

		if callback(l) {
			return nil
		}

	}

	return nil
}

// 通过字符串设置某一类日志的级别
func SetLevelByString(loggerName string, level string) error {

	return VisitLogger(loggerName, func(l *Logger) bool {
		l.SetLevelByString(level)
		return true
	})
}

// 通过字符串设置某一类日志的崩溃级别
func SetPanicLevelByString(loggerName string, level string) error {

	return VisitLogger(loggerName, func(l *Logger) bool {
		l.SetPanicLevelByString(level)
		return true
	})
}

func SetColorFile(loggerName string, colorFileName string) error {

	data, err := ioutil.ReadFile(colorFileName)
	if err != nil {
		return err
	}

	return SetColorDefine(loggerName, string(data))
}

func SetColorDefine(loggerName string, jsonFormat string) error {

	cf := NewColorFile()

	if err := cf.Load(jsonFormat); err != nil {
		return err
	}

	return VisitLogger(loggerName, func(l *Logger) bool {
		l.SetColorFile(cf)
		return true
	})
}

func EnableColorLogger(loggerName string, enable bool) error {

	return VisitLogger(loggerName, func(l *Logger) bool {
		l.enableColor = enable
		return true
	})
}

func SetOutputLogger(loggerName string, filename string) error {

	mode := os.O_RDWR | os.O_CREATE | os.O_APPEND

	f, err := os.OpenFile(filename, mode, 0666)
	if err != nil {
		return err
	}

	return VisitLogger(loggerName, func(l *Logger) bool {
		l.fileOutput = f
		return true
	})
}

func ClearAll() {

	logMapGuard.Lock()
	logMap = map[string]*Logger{}
	logMapGuard.Unlock()
}
