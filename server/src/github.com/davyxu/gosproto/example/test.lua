local a= require "addressbook"

for k, v in pairs( a.NameByID) do
	print(k, v)
end

for k, v in pairs( a.IDByName) do
	print(k, v)
end
