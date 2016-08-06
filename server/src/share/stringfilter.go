package share

import (
	"unicode/utf8"
)

type StringFilter struct {
	root      *tieNode // 根节点
	*ListFile          // 文件行处理模块
}

type tieNode struct {
	children map[rune]*tieNode
	end      bool // 是否是单词的结束
}

func NewStringFilter() *StringFilter {
	t := new(StringFilter)
	t.root = newTieNode()
	t.ListFile = new(ListFile)
	return t
}

func newTieNode() *tieNode {
	t := new(tieNode)
	t.end = false
	t.children = make(map[rune]*tieNode)
	return t
}

func (self *StringFilter) Insert(txt string) {

	if len(txt) < 1 {
		return
	}

	node := self.root
	key := []rune(txt)

	// 创建trie树
	for i := 0; i < len(key); i++ {
		if _, exists := node.children[key[i]]; !exists {
			node.children[key[i]] = newTieNode()
		}
		node = node.children[key[i]]
	}

	node.end = true
}

func (self *StringFilter) Replace(txt string) string {
	if len(txt) < 1 {
		return txt
	}

	node := self.root

	key := []rune(txt)
	var chars []rune = nil
	slen := len(key)

	for i := 0; i < slen; i++ {
		var match bool
		var endPos int
		if _, exists := node.children[key[i]]; exists {
			node = node.children[key[i]]
			if node.end { // 单个单词匹配
				c, _ := utf8.DecodeRuneInString("*")
				if chars == nil {
					chars = key
				}
				chars[i] = c
			}
			for j := i + 1; j < slen; j++ {
				if _, exists := node.children[key[j]]; !exists {
					break
				}

				node = node.children[key[j]]
				if !node.end {
					continue
				}

				match = true
				endPos = j

				if len(node.children) > 0 {
					continue
				}
			}

			if match {
				if chars == nil {
					chars = key
				}
				for t := i; t <= endPos; t++ { // 从敏感词开始到结束依次替换为*
					c, _ := utf8.DecodeRuneInString("*")
					chars[t] = c
				}

			}
			node = self.root
		}
	}
	if chars == nil {
		return txt
	} else {
		return string(chars)
	}
}

func (self *StringFilter) Exist(txt string) bool {
	if len(txt) < 1 {
		return false
	}

	node := self.root
	key := []rune(txt)

	for i := 0; i < len(key); i++ {
		if _, exists := node.children[key[i]]; exists {
			node = node.children[key[i]]
			if node.end { // 单个单词匹配
				return true
			}

			for j := i + 1; j < len(key); j++ {
				if _, exists := node.children[key[j]]; !exists {
					break
				}

				node = node.children[key[j]]
				if !node.end {
					continue
				}
				return true
			}
			node = self.root
		}
	}
	return false
}

func (self *StringFilter) LoadFile(fileName string) {
	self.Load(fileName, self.Insert)
}

func (self *StringFilter) Empty() bool {
	return len(self.root.children) == 0
}
