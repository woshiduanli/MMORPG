--[[
	desc: Vector3
	author: zhiwei.zou
	create: 2018-6-21
	copyright: CJ
]]

local VectorHelper = {}

--计算两个向量的距离
function VectorHelper.Distance(vecA,vecB)
	local dis = math.sqrt((vecA.x-vecB.x) * (vecA.x-vecB.x) + (vecA.y-vecB.y)*(vecA.y-vecB.y) + (vecA.z-vecB.z)*(vecA.z-vecB.z))

	return dis
end

--向量积
function VectorHelper.Cross(vecA,vecB)
	local x = (vecA.y * vecB.z) - (vecA.z * vecB.y)
	local y = (vecA.z * vecB.x) - (vecA.x * vecB.z)
	local z = (vecA.x * vecB.y) - (vecA.y * vecB.x)

	return x,y,z
end

--线性插值
function VectorHelper.Lerp(form,to,time)
	if time <= 0 then
		return form.x,form.y,form.z
	elseif time >= 1 then
		return to.x,to.y,to.z
	else
		local x = time * to.x + (1 - time) * form.x
		local y = time * to.y + (1 - time) * form.y
		local z = time * to.z + (1 - time) * form.z

		return x,y,z
	end
end

--点乘
function VectorHelper.Dot(vecA,vecB)
	return (vecA.x*vecB.x) + (vecA.y*vecB.y) + (vecA.z*vecB.z)
end

--夹角计算
function VectorHelper.Angle(vecA,vecB)
	local top = VectorHelper.Dot(vecA,vecB)

	local buttomA = math.sqrt(vecA.x*vecA.x + vecA.y * vecA.y + vecA.z * vecA.z)
	local buttomB = math.sqrt(vecB.x*vecB.x + vecB.y * vecB.y + vecB.z * vecB.z)

	return math.deg(math.acos(top / (buttomA * buttomB)))
end

--Unity向量相加，结果放在VecA中
function VectorHelper.UVec2Add(vecA, vecB)
	vecA.x = vecA.x + vecB.x
	vecA.y = vecA.y + vecB.y
end

function VectorHelper.UVec3Add(vecA, vecB)
	vecA.x = vecA.x + vecB.x
	vecA.y = vecA.y + vecB.y
	vecA.z = vecA.z + vecB.z
end

--Unity向量相减，结果放在VecA中
function VectorHelper.UVec2Sub(vecA, vecB)
	vecA.x = vecA.x - vecB.x
	vecA.y = vecA.y - vecB.y
end

function VectorHelper.UVec3Sub(vecA, vecB)
	vecA.x = vecA.x - vecB.x
	vecA.y = vecA.y - vecB.y
	vecA.z = vecA.z - vecB.z
end

--Unity向量乘以数字，结果放在VecA中
function VectorHelper.UVec2Mul(vecA, number)
	vecA.x = vecA.x * number
	vecA.y = vecA.y * number
end

function VectorHelper.UVec3Mul(vecA, number)
	vecA.x = vecA.x * number
	vecA.y = vecA.y * number
	vecA.z = vecA.z * number
end

--设置Unity向量的值
function VectorHelper.UVecSet(vec, x, y, z)
	vec.x = x
	vec.y = y

	if z ~= nil then
		vec.z = z
	end
end

function VectorHelper.Zero()
	local vecTable = {}

	vecTable.x = 0
	vecTable.y = 0
	vecTable.z = 0

	return vecTable
end

function VectorHelper.One()
	local vecTable = {}

	vecTable.x = 1
	vecTable.y = 1
	vecTable.z = 1

	return vecTable
end

function VectorHelper.Back()
	local vecTable = {}

	vecTable.x = 0
	vecTable.y = 0
	vecTable.z = -1

	return vecTable
end

function VectorHelper.Forward()
	local vecTable = {}

	vecTable.x = 0
	vecTable.y = 0
	vecTable.z = 1

	return vecTable
end

function VectorHelper.Up()
	local vecTable = {}

	vecTable.x = 0
	vecTable.y = 1
	vecTable.z = 0

	return vecTable
end

function VectorHelper.Down()
	local vecTable = {}

	vecTable.x = 0
	vecTable.y = -1
	vecTable.z = 0

	return vecTable
end

function VectorHelper.Right()
	local vecTable = {}

	vecTable.x = 1
	vecTable.y = 0
	vecTable.z = 0

	return vecTable
end

function VectorHelper.Left()
	local vecTable = {}

	vecTable.x = -1
	vecTable.y = 0
	vecTable.z = 0

	return vecTable
end

function VectorHelper.VecFormat(x, y, z)
	local vecTable = {}

	vecTable.x = x
	vecTable.y = y
	vecTable.z = z

	return vecTable
end

return VectorHelper
