package player

// 存入DB的账号信息
type DBAccount struct {
	AccountID string // 账号标示号
}

//  内存中的账号信息
type MemAccount struct {
	*DBAccount
}
