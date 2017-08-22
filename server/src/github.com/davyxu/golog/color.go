package golog

import (
	"encoding/json"
	"fmt"
	"strings"
)

type Color int

const (
	Color_None Color = iota
	Color_Black
	Color_Red
	Color_Green
	Color_Yellow
	Color_Blue
	Color_Purple
	Color_DarkGreen
	Color_White
)

var logColorPrefix = []string{
	"",
	"\x1b[030m",
	"\x1b[031m",
	"\x1b[032m",
	"\x1b[033m",
	"\x1b[034m",
	"\x1b[035m",
	"\x1b[036m",
	"\x1b[037m",
}

var colorByName = map[string]Color{
	"none":      Color_None,
	"black":     Color_Black,
	"red":       Color_Red,
	"green":     Color_Green,
	"yellow":    Color_Yellow,
	"blue":      Color_Blue,
	"purple":    Color_Purple,
	"darkgreen": Color_DarkGreen,
	"white":     Color_White,
}

func matchColor(name string) Color {

	lower := strings.ToLower(name)

	for cname, c := range colorByName {

		if cname == lower {
			return c
		}
	}

	return Color_None
}

func ColorFromLevel(l Level) Color {
	switch l {
	case Level_Warn:
		return Color_Yellow
	case Level_Error, Level_Fatal:
		return Color_Red
	}

	return Color_None
}

var logColorSuffix = "\x1b[0m"

type ColorMatch struct {
	Text  string
	Color string

	c Color
}

type ColorFile struct {
	Rule []*ColorMatch
}

func (self *ColorFile) ColorFromText(text string) Color {

	for _, rule := range self.Rule {
		if strings.Contains(text, rule.Text) {
			return rule.c
		}
	}

	return Color_None
}

func (self *ColorFile) Load(data string) error {

	err := json.Unmarshal([]byte(data), self)
	if err != nil {
		return err
	}

	for _, rule := range self.Rule {

		rule.c = matchColor(rule.Color)

		if rule.c == Color_None {
			return fmt.Errorf("color name not exists: %s", rule.Text)
		}

	}

	return nil
}

func NewColorFile() *ColorFile {
	return &ColorFile{}
}
