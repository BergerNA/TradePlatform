/*
Библиотека связывает Quik и C#-программу, посредством именованной области памяти.
*/

#include <windows.h>

//=== Необходимые для Lua константы ============================================================================//
#define LUA_LIB
#define LUA_BUILD_AS_DLL

#define sizeCommand 500
#define sizeAnswer 3000
#define sizeDeal 4000
#define sizeQuote 1400

//=== Заголовочные файлы LUA ===================================================================================//
extern "C" {
#include "Lua\lauxlib.h"
#include "Lua\lua.h"
}

//=== Получает указатель на выделенную именованную память =====================================================//
// Имя для выделенной памяти
TCHAR Name[] = TEXT("QUIKCommand");
// Создаст, или подключится к уже созданной памяти с таким именем
HANDLE hFileMapQUIKCommand = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeCommand, Name);

// Имя для выделенной памяти
TCHAR Name1[] = TEXT("QUIKAnswer");
// Создаст, или подключится к уже созданной памяти с таким именем
HANDLE hFileMapAnswer = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeAnswer, Name1);

// Имя для выделенной памяти
TCHAR Name2[] = TEXT("Deal");
// Создаст, или подключится к уже созданной памяти с таким именем
HANDLE hFileMapDeal = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeDeal, Name2);

// Имя для выделенной памяти
TCHAR Name3[] = TEXT("Quote");
// Создаст, или подключится к уже созданной памяти с таким именем
HANDLE hFileMapQuote = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeQuote, Name3);

//=== Стандартная точка входа для DLL ==========================================================================//
BOOL APIENTRY DllMain(HANDLE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
	return TRUE;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////COMMAND//////////////////////////////////////////////
//=== Реализация функций, вызываемых из LUA ====================================================================//
// Получение команд от C# приложения
static int forLua_GetCommand(lua_State *L)
{
	//Если указатель на память получен
	if (hFileMapQUIKCommand)
	{
		//Получает доступ к байтам памяти
		PBYTE pb = (PBYTE)(MapViewOfFile(hFileMapQUIKCommand, FILE_MAP_READ | FILE_MAP_WRITE, 0, 0, sizeCommand));

		//Если доступ к байтам памяти получен
		if (pb != NULL)
		{
			//Если память чиста
			if (pb[0] == 0)
			{
				//Записывает в Lua-стек пустую строку
				lua_pushstring(L, "");
			}
			else //Если в памяти есть команда
			{
				//Записывает в Lua-стек полученную команду
				lua_pushstring(L, (char*)(pb));
				//Стирает запись, чтобы повторно не выполнить команду
				for (int i = 0; i < sizeCommand; i++)pb[i] = '\0';
				pb[0] = '0';
			}

			//Закрывает представление
			UnmapViewOfFile(pb);
		}
		else lua_pushstring(L, "");//Если доступ к байтам памяти не был получен, записывает в Lua-стек пустую строку
	}
	else //Указатель на память не был получен
	{
		//Записывает в Lua-стек пустую строку
		lua_pushstring(L, "");
	}
	//Функция возвращает записанное значение из Lua-стека (пустую строку, или полученную команду)
	return(1);
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////END COMMAND//////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////ANSWER///////////////////////////////////////////////
static int forLua_CheckGotAnswer(lua_State *L)
{
	// Если указатель на память получен
	if (hFileMapAnswer)
	{
		// Получает доступ (представление) непосредственно к чтению/записи байт
		PBYTE pb = (PBYTE)(MapViewOfFile(hFileMapAnswer, FILE_MAP_READ | FILE_MAP_WRITE, 0, 0, sizeAnswer));

		// Если доступ получен
		if (pb != NULL)
		{
			// C# устанавливает в первый байт '2', сообщая тем самым, что можно записывать в память
			if (pb[0] == '0')
			{
				// Если запись пустая, то возвращает в QLua TRUE (разрешает отправлять строку)
				lua_pushboolean(L, true);
			}
			else
			{
				// Если запись НЕ пустая, то возвращает в QLua FALSE (запрещает отправлять строку)
				lua_pushboolean(L, false);
			}
			// Закрывает представление
			UnmapViewOfFile(pb);
		}
		else lua_pushboolean(L, false);// Если доступ НЕ получен, отправляет в QLua FALSE (запрещает отправлять строку)
	}
	else lua_pushboolean(L, false); // Если указатель на память НЕ получен, отправляет в QLua FALSE (запрещает отправлять строку)

	return(1);
}
// Получает строку инструмента из Текущей Таблицы Параметров и отправляет ее в C#
static int forLua_SendAnswer(lua_State *L)
{
	// Если указатель на память получен
	if (hFileMapAnswer)
	{
		// Получает доступ (представление) непосредственно к чтению/записи байт
		PBYTE pb = (PBYTE)(MapViewOfFile(hFileMapAnswer, FILE_MAP_READ | FILE_MAP_WRITE, 0, 0, sizeAnswer));

		// Если доступ получен
		if (pb != NULL)
		{
			// Считывает из Lua-стека строку инструмента
			const char *Answer = lua_tostring(L, 1);
			int Size = 0;
			// Считает количество символов в строке
			for (int i = 0; i < sizeAnswer; i++)
			{
				if (Answer[i] == 0)break;
				Size++;
			}
			// Записывает строку в память, начиная со 2-го байта
			memcpy(pb + 1, Answer, Size);
			// Устанавливает в 1-й байт "1", сообщая тем самым C#, что запись завершена и можно читать
			memcpy(pb, "1", 1);

			// Закрывает представление
			UnmapViewOfFile(pb);
		}
	}

	return(0);
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////END ANSWER///////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////DEAL/////////////////////////////////////////////////
//=== Реализация функций, вызываемых из LUA ====================================================================//
// Проверяет получил-ли C# последнюю строку параметров по инструменту (или еще не было отправлено ни одной строки)
static int forLua_CheckGotDeal(lua_State *L)
{
	// Если указатель на память получен
	if (hFileMapDeal)
	{
		// Получает доступ (представление) непосредственно к чтению/записи байт
		PBYTE pb = (PBYTE)(MapViewOfFile(hFileMapDeal, FILE_MAP_READ | FILE_MAP_WRITE, 0, 0, sizeDeal));

		// Если доступ получен
		if (pb != NULL)
		{
			// C# устанавливает в первый байт '2', сообщая тем самым, что можно записывать в память
			if (pb[0] == '0')
			{
				// Если запись пустая, то возвращает в QLua TRUE (разрешает отправлять строку)
				lua_pushboolean(L, true);
			}
			else
			{
				// Если запись НЕ пустая, то возвращает в QLua FALSE (запрещает отправлять строку)
				lua_pushboolean(L, false);
			}
			// Закрывает представление
			UnmapViewOfFile(pb);
		}
		else lua_pushboolean(L, false);// Если доступ НЕ получен, отправляет в QLua FALSE (запрещает отправлять строку)
	}
	else lua_pushboolean(L, false); // Если указатель на память НЕ получен, отправляет в QLua FALSE (запрещает отправлять строку)

	return(1);
}
// Получает строку инструмента из Текущей Таблицы Параметров и отправляет ее в C#
static int forLua_SendDeal(lua_State *L)
{
	// Если указатель на память получен
	if (hFileMapDeal)
	{
		// Получает доступ (представление) непосредственно к чтению/записи байт
		PBYTE pb = (PBYTE)(MapViewOfFile(hFileMapDeal, FILE_MAP_READ | FILE_MAP_WRITE, 0, 0, sizeDeal));

		// Если доступ получен
		if (pb != NULL)
		{
			// Считывает из Lua-стека строку инструмента
			const char *Params = lua_tostring(L, 1);
			int Size = 0;
			// Считает количество символов в строке
			for (int i = 0; i < sizeDeal; i++)
			{
				if (Params[i] == '%')break;
				Size++;
			}
			// Записывает строку в память, начиная со 2-го байта
			memcpy(pb + 1, Params, Size);
			// Устанавливает в 1-й байт "1", сообщая тем самым C#, что запись завершена и можно читать
			memcpy(pb, "1", 1);

			// Закрывает представление
			UnmapViewOfFile(pb);
		}
	}

	return(0);
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////END DEAL/////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////QUOTE////////////////////////////////////////////////
//Проверяет получил-ли робот последний СТАКАН
static int forLua_CheckGotQuote(lua_State *L)
{
	//Если указатель на память получен
	if (hFileMapQuote)
	{
		//Получает доступ к байтам памяти
		PBYTE pb = (PBYTE)(MapViewOfFile(hFileMapQuote, FILE_MAP_READ | FILE_MAP_WRITE, 0, 0, sizeQuote));

		//Если доступ к байтам памяти получен
		if (pb != NULL)
		{
			//проверяет на пустую запись (сигнал, что можно отправлять стакан)
			if (pb[0] == 0)
			{
				lua_pushboolean(L, true);
			}
			else
			{
				lua_pushboolean(L, false);
			}
			//закрывает представление
			UnmapViewOfFile(pb);
		}
		else lua_pushboolean(L, false);
	}
	else
	{
		lua_pushboolean(L, false);
	}

	return(1);
}

//Отправляет новые изменения стакана
static int forLua_SendQuote(lua_State *L)
{
	//Если указатель на память получен
	if (hFileMapQuote)
	{
		//Получает доступ к байтам памяти
		PBYTE pb = (PBYTE)(MapViewOfFile(hFileMapQuote, FILE_MAP_READ | FILE_MAP_WRITE, 0, 0, sizeQuote));

		//Если доступ к байтам памяти получен
		if (pb != NULL)
		{
			//Получает из Lua-стека переданное значение
			const char *Quote = lua_tostring(L, 1);
			int Size = 0;
			//считает количество символов в строке
			for (int i = 0; i < sizeQuote; i++)
			{
				if (Quote[i] == 0)break;
				Size++;
			}
			//записывает стакан в память
			memcpy(pb, Quote, Size);
			//lua_pushstring(L, (char*)pb);//возвращает то, что записалось (если раскомментировать) (может пригодиться при отладке)
			//закрывает представление
			UnmapViewOfFile(pb);
		}
		else lua_pushstring(L, "");
	}
	else lua_pushstring(L, "");

	return(1);
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////END QUOTE////////////////////////////////////////////


// Возвращает текущее время работы ОС в миллисекундах
static int forLua_GetMilliseconds(lua_State *L)
{
	// Получает время в миллисекундах, прошедших с запуска ОС
	unsigned long long _TickCount = GetTickCount64();
	char TickCount[20];
	// Конвертирует число миллисекунд в строковое представление
	sprintf_s(TickCount, "%llu", _TickCount);
	// Помещает строковое представление в Lua-стек
	lua_pushstring(L, TickCount);

	return(1);
}

//=== Регистрация реализованных в dll функций, чтобы они стали "видимы" для Lua ================================//
static struct luaL_reg ls_lib[] = {
	{ "CheckGotDeal", forLua_CheckGotDeal },
	{ "SendDeal", forLua_SendDeal },
	{ "GetMilliseconds", forLua_GetMilliseconds },
	{ "GetCommand", forLua_GetCommand },
	{ "CheckGotAnswer", forLua_CheckGotAnswer },
	{ "SendAnswer", forLua_SendAnswer },
	{ NULL, NULL }
};

//=== Регистрация названия библиотеки, видимого в скрипте Lua ==================================================//
extern "C" LUALIB_API int luaopen_QuikConnect(lua_State *L) {
	luaL_openlib(L, "QuikConnect", ls_lib, 0);
	return 0;
}