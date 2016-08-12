# 整合Lua脚本

## 理由
* 热更新
* 热修复(hot fix), 移动设备及windows环境的迅速更新代码

## 使用范围
* 除核心逻辑外的所有逻辑

## 脚本与界面的策略
* 使用DataContext设定脚本绑定
* 生成对应的lua文件, 在加载期配合界面一起加载


# 一些引用
* ToLua源码
https://github.com/topameng/tolua_runtime