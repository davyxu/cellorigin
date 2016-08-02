local Resources  = UnityEngine.Resources
local GameObject = UnityEngine.GameObject 


Framework = {}

function Framework.CreateUI( name, parentTransform )
	
	local prefabPath = string.format("View/%s", name )
	local presenterName = name
	
	--  资源暂时用Resources方式加载
	local prefab = Resources.Load(prefabPath)
	local go = GameObject.Instantiate(prefab)
		
	-- 避免名字中有Clone
	go.name = name
	
	-- 默认加到根的Canvas上
	if parentTransform == nil then
	
		local root = GameObject.Find( "Canvas" )
		parentTransform = root.transform
		
	end
	
	go.transform:SetParent( parentTransform, false )
	
	require(string.format("Presenter.%s", name ))
	
	-- lua的类实例创建
	local instance = Class.NewGo(presenterName, go)

	-- 组件挂接
	local classObject = go:AddComponent(typeof(CellLuaClass))
	classObject:InitAwake(presenterName)

	
	return instance
	
end


-- 执行一个Command
function Framework.ExecuteCommand( instance, name )

	if instance.Command == nil then
		return
	end
	
	local cmd = instance.Command[name]
	if cmd == nil then
		error("command not found: " .. name )
	end
	
	return cmd( instance )

end

-- 将一个值设置到view控件上
function Framework.SetViewText( instance, name, value )

	-- 一个model element里只修改了1个字段, 其他字段是空的, 表示不需要通知变化
	if value == nil then
		return
	end
	
	local view = instance.UI[name]
	if view == nil then
		error("command not found: " .. name )
	end
	
	view.text = value

end

--[[

path会将最右斜杠的部分视为name

约定: 

ui路径最右斜杠的部分=name

Command的名称=Button的name

]]
function Framework.BindUIToClass( class, path, csharpType )
	
	local name = string.gsub(path, "(.*/)(.*)", "%2")

	local trans = class.gameObject.transform
	
	-- 自动创建UI组
	local UI = class.UI
	if UI == nil then
		UI = {}
		class.UI = UI
	end
	
	local obj = trans:Find( path )
	
	if obj == nil then
		error("ui not found: " .. path )
	end
	
	local w = obj:GetComponent(csharpType)
	
	if UI[name] ~= nil then
		error("duplicate ui name of path: ".. path )
	end
	
	UI[name] = w
	
	
	-- Command总是对应同名的控件名
	if csharpType == "Button" then
	
		w.onClick:AddListener( function( )
			Framework.ExecuteCommand( class, name )
		end)
		
	end

end

--[[

将界面绑定到model, 数据增删改自动更新列表

约定: 
ScrollRect下的page只能在Viewport/Content路径下

ScrollRect的对象名称与每个元素的Prefab名称统一

]]
function Framework.BindModelToList( class, modelName, name )

	local w = class.UI[name]


	local content = w.transform:Find("Viewport/Content")
	
	if content == nil then
		error("ScrollRect content path must be 'Viewport/Content'")
	end	
	
	-- 自动创建List组
	if class.List == nil then
		class.List = {}
	end
	
	local list = class.List[name]
	
	if list ~= nil then
		error("duplicate list name of path: ".. path )
	end
	
	list = {}
	class.List[name] = list
	

	-- list 对应的model名与控件名一致
	Model.Listen( modelName, function( value, key, op )
		print( "Model :", modelName, key, op )
	
		if op == "add" then
		
			local instance = Framework.CreateUI( name, content.transform )
			instance:ApplyModel( value )
			list[key] = instance
		
		elseif op == "mod" then
			
			local instance = list[key]
			instance:ApplyModel( value )
	
		elseif op == "del" then
			
			local instance = list[key]
			GameObject.Destroy( instance.gameObject )
			list[key] = nil
		end
	
	end )
		
end



function Framework.ViewListen( class, modelName, callback )

	local w = class.UI[modelName]

	w.onValueChanged:AddListener( function( )
	
		callback( w.text )
		
	end)

end