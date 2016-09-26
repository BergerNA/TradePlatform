require("QuikConnect");

 --**********************************************************************************************************
--                                   	Модуль работы с очередями колбеков			
-- Модуль работы с очередями колбеков
local queue =
{
	-- Создать новую очередь FIFO
	new = function(self)
    self.__index = self
    return setmetatable({first = 0,last = -1},self)
	end,
 --------------------------------------------------------------------------------------------
 -- Поместить значение в очередь
 push = function(self,value)
                 local last = self.last + 1
                 self[last] = value
                 self.last = last
 end,

---------------------------------------------------------------------------------------------
 -- Изъять самое старое значение из очереди. Если очередь пуста, возвращается nil
 pop = function(self)
                local first = self.first
                if first <= self.last then
                     local value = self[first]
                     self[first] = nil
                     self.first = first + 1
                 return value
           end
 end,

---------------------------------------------------------------------------------------------
 -- Количество значений в очереди
 size = function(self)
         return self.last - self.first + 1
 end

---------------------------------------------------------------------------------------------
}

---------------------------------------------------------------------------------------------

--**********************************************End*********************************************************

--**********************************************************************************************************
--                                      Служебные переменные:
Run = true;
Connect = false;
Security = "RIZ6,SiZ6,SRZ6";
PrevNum = 0;
LastLoadDeal = 0;
path = "D:\\Story\\TradeStory.txt";

command = 
{
  ["1Connect"] = function (x) GetConnected() end,
  ["1Disconnect"] = function (x) Connect = false end,
  ["1LastDeal"] = function (CommandStr) LastLoadDeal = 5 end,
  ["1GetSecurity"] = function (x) GetSecurity(x[2]) end,
  ["1GetClassCode"] = function (x) GetClassCode() end,
  ["1Buy"] = function (x) print("s_mike") end,
  ["1Sell"] = function (x) print("s_mike") end,
}
--**********************************************End*********************************************************
 
--**********************************************************************************************************
--           Основная функция, перебор сделок по номерам и дальнейшая передача для обработки:
function main()
--	local SecurityStorFile = io.open(getScriptPath().."\\Security.txt", "r")	-- Открываем файл для чтения прошлого состояния робота
--	if tradeStorFile~=nil then
--		Security = SecurityStorFile:read("*line")
--	end
--	SecurityStorFile:close()
	deal = queue:new()
	answer = queue:new()
	--command["Security"]("sd")
	--stakan = queue:new()
   while Run do
		local CommandStr = tostring(QuikConnect.GetCommand());
		if CommandStr ~= "" then
			if CommandStr:find("1") == 1 then
				local myTable = CommandStr:split(";")
				--command["GetClassCode"](myTable)
				message(myTable[1],1)
				command[myTable[1]](myTable)
			end;
		end;
		if answer:size() > 0 then
			if QuikConnect.CheckGotAnswer() then
			    message("Send",1)
				QuikConnect.SendAnswer(answer:pop());
			end;
		end;
		
		--sleep(1000)
		if deal:size() > 0 then
			--	tradeStorFile = io.open(path, "a")
			--	tradeStorFile:write(deal:pop().."\n")
			--	tradeStorFile:close()
			if deal:size() > 51 then
				if QuikConnect.CheckGotDeal() then
				    local str = deal:pop();
					for i = 0, 48, 1 do
					str = str..deal:pop();
					end;
					--tradeStorFile = io.open(path, "a")
					--tradeStorFile:write(str)
					--tradeStorFile:close()
					str = str.."%"
					QuikConnect.SendDeal(str);
				end;
			else
			if QuikConnect.CheckGotDeal() then
			local str = deal:pop();
				local size = deal:size()
					for i = 0, size-1, 1 do
					str = str..deal:pop();
					end;
			--message("Отправка"..deal:pop(),1)
			-- Если C# получил строку параметров инструмента
				-- Отправляет строку в DLL
				--message("Отправка",1)
				QuikConnect.SendDeal(str.."%");
			end;
			end;
		end;
		
		sleep(1);
		--local num_deal = deal:pop()
	--	message("Order_num = "..deal:pop(),1)
   end;
end;
--**********************************************End*********************************************************

function OnAllTrade(alltrade)
	if Security:find(alltrade.sec_code) ~= nil 
	and PrevNum ~= alltrade.trade_num then
		local SecStr = nil;
         SecStr = "NUM="..
		 tostring(alltrade.trade_num)..";"..
		 "SECCODE="..
		 tostring(alltrade.sec_code)..";"..
		 "PRICE="..
		 tostring(alltrade.price)..";"..
		 "QTY="..
		 tostring(alltrade.qty)..";"..
		 "DATA="..
		 tostring(alltrade.datetime.year)
	      .."-"
	      ..tostring(alltrade.datetime.month)
	      .."-"
	      ..tostring(alltrade.datetime.day)
	      .." "
	      ..tostring(alltrade.datetime.hour)
	      ..":"
	      ..tostring(alltrade.datetime.min)
	      ..":"
	      ..tostring(alltrade.datetime.sec)
	      .."."
	      ..tostring(alltrade.datetime.mcs)
	      ..";"..
		 "FLAGS="..
		 tostring(alltrade.flags).."@";
		 deal:push(SecStr)
		 PrevNum = alltrade.trade_num;
	end;
end;

--- Функция вызывается терминалом QUIK при получении изменения стакана котировок
function OnQuote(class, sec )
   -- 
   if Security:find(sec) ~= nil then
      ql2 = getQuoteLevel2(class, sec);
      -- Представляет снимок СТАКАНА в виде СТРОКИ
         QuoteStr = "";
         for i = tonumber(ql2.bid_count), 1, -1 do
            if ql2.bid[i].quantity ~= nil then   -- На некоторых ценах могут отсутствовать заявки
               QuoteStr = QuoteStr..tostring(tonumber(ql2.bid[i].quantity))..";"..tostring(tonumber(ql2.bid[i].price))..";";
            else
               QuoteStr = QuoteStr.."0;"..tostring(tonumber(ql2.bid[i].price))..";";
            end;
         end;
		 QuoteStr = QuoteStr.."###";
         for i = 1, tonumber(ql2.offer_count), 1 do
            if ql2.offer[i].quantity ~= nil then   -- На некоторых ценах могут отсутствовать заявки
               if i < tonumber(ql2.offer_count) then 
                  QuoteStr = QuoteStr..tostring(tonumber(ql2.offer[i].quantity))..";"..tostring(tonumber(ql2.offer[i].price))..";";
               else 
                  QuoteStr = QuoteStr..tostring(tonumber(ql2.offer[i].quantity))..";"..tostring(tonumber(ql2.offer[i].price));
               end;
            else 
               if i < tonumber(ql2.offer_count) then
                  QuoteStr = QuoteStr.."0;"..tostring(tonumber(ql2.offer[i].price))..";";
               else
                  QuoteStr = QuoteStr.."0;"..tostring(tonumber(ql2.offer[i].price));
               end;
            end;
         end;
		-- stakan:push(QuoteStr);
   end;
end;

function OnStop()
   Run = false;
   if Security:len() ~= 0 then
		local f = io.open(getScriptPath().."\\Security.txt", "w")
		f:write(Security)
		f:close()
	end;
end;

function GetSecurity(cl_code)
local rezult = "ID=SecurityList;Value=";
message(cl_code,1)
	if cl_code ~= "" then
		for i = 0,getNumberOf("SECURITIES") - 1 do
		if getItem("SECURITIES",i).class_name == cl_code then
			rezult = rezult..getItem("SECURITIES",i).short_name..","
			end;
		end;
		answer:push(rezult)
	end;
end;

function GetClassCode()
message("Enter",1)
local rezult = "ID=ClassNameList;Value=";
	for i = 0,getNumberOf("CLASSES") - 1 do
		rezult = rezult..getItem("CLASSES",i).name..","
	end;
	answer:push(rezult)
end;

function GetConnected()

	if isConnected() == 1 then
		answer:push("ID=Connect;Value=1")
	else answer:push("ID=Connect;Value=0")
	end;
end;

function string:split( inSplitPattern, outResults )
   if not outResults then
      outResults = { }
   end
   local theStart = 1
   local theSplitStart, theSplitEnd = string.find( self, inSplitPattern, theStart )
   while theSplitStart do
      table.insert( outResults, string.sub( self, theStart, theSplitStart-1 ) )
      theStart = theSplitEnd + 1
      theSplitStart, theSplitEnd = string.find( self, inSplitPattern, theStart )
   end
   table.insert( outResults, string.sub( self, theStart ) )
   return outResults
end