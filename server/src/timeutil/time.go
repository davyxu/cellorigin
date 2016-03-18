package timeutil

import (
	"time"
)

// 1天的时间段
var Duration1Day time.Duration
var Duration0AM time.Duration
var Duration5AM time.Duration

const (
	SecondsPerDay int64 = 24 * 60 * 60
)

// 给定任意时间点, 获得这个时间点当天的跨界时间点
func makeCrossDayPoint(anyTime time.Time, offset time.Duration) time.Time {
	return time.Date(anyTime.Year(), anyTime.Month(), anyTime.Day(), 0, 0, 0, 0, anyTime.Location()).Add(offset)
}

func getTomorrowCrossDayTime(tm time.Time, offset time.Duration) time.Time {
	tmTomorrow := tm.Add(Duration1Day)
	return makeCrossDayPoint(tmTomorrow, offset)
}

// 给定两个点的UTC时间点, 计算出来跨越了几天
// maxCrossCount必须大于期望的天数,且足够小, 保证不死循环
func GetCrossDayCount(UTCSecondsPast, UTCSecondsNow int64, offset time.Duration, maxCrossCount int) int {
	tmPast := time.Unix(UTCSecondsPast, 0).Local()
	tmNow := time.Unix(UTCSecondsNow, 0).Local()

	var tm time.Time = getTomorrowCrossDayTime(tmPast, offset)
	var crossCount int

	for {

		if tm.After(tmNow) {
			break
		}

		tm = tm.Add(Duration1Day)

		crossCount++

		// 最大跨越数约束, 避免死循环
		if crossCount >= maxCrossCount {
			break
		}
	}

	return crossCount
}

// 给定过去某个UTC时间点, 根据当前时间及零点跨界偏移值, 计算出是否跨越了一天
func HasCrossOneDay(UTCSecondsPast int64, offset time.Duration) bool {
	return GetCrossDayCount(UTCSecondsPast, time.Now().UTC().Unix(), offset, 1) > 0
}

// 给定小时数[0-23]，获取当天这个小时的utc秒数
func GetTodayUTCSecondByHour(hour int32) int64 {

	now := time.Now()
	location := time.Now().Location()
	sec := time.Date(now.Year(), now.Month(), now.Day(), int(hour), 0, 0, 0, location).Unix()
	return sec
}

// 获取当前Utc Seconds
func GetUTCSecondNow() int64 {
	return time.Now().UTC().Unix()
}

func init() {
	Duration0AM, _ = time.ParseDuration("0h")
	Duration5AM, _ = time.ParseDuration("5h")

}
