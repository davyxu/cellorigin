# MVVM 代码生成原理

## 工作流

* 生成UI预制件( 美术, 策划完成 )

* 对UI进行优化调整, 命名

* 对需要生成代码的对象进行脚本绑定, 包含
		
		指定View中生成控件类型
		
		指定绑定Presenter的Property,Command,List
		

* 生成View, Presenter代码

## 原理
* 需要生成代码的GameObject, 需要挂载DataContext


## 命名规则

### 作为类时
	
* Name = Prefab文件名(GameObject.Name)
	
* Presenter类名 = 类文件名 = Name + Presenter
	
* View类名 	 = 类文件名 = Name + View
	
* Model类名     = 类文件名 = Name + Model
	
	
### 作控件时

Name = GameObject.Name
	
* 在Presenter中
	属性名 = Name
	事件名 = On + Name + Changed
	ObservableCollection实例名 = Name
	ObservableCollection的ValueType (Presenter) = List Control的Item Prefab的Name + Presenter
	
* 在View中
	控件实例 = _ + Name
	BindCollectionView的 = List Control的Item Prefab的Name + Presenter以及View
	

### 流程
* 从Prefab找出DataContext