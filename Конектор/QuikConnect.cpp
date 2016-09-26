/*
���������� ��������� Quik � C#-���������, ����������� ����������� ������� ������.
*/

#include <windows.h>

//=== ����������� ��� Lua ��������� ============================================================================//
#define LUA_LIB
#define LUA_BUILD_AS_DLL

#define sizeCommand 500
#define sizeAnswer 3000
#define sizeDeal 4000
#define sizeQuote 1400

//=== ������������ ����� LUA ===================================================================================//
extern "C" {
#include "Lua\lauxlib.h"
#include "Lua\lua.h"
}

//=== �������� ��������� �� ���������� ����������� ������ =====================================================//
// ��� ��� ���������� ������
TCHAR Name[] = TEXT("QUIKCommand");
// �������, ��� ����������� � ��� ��������� ������ � ����� ������
HANDLE hFileMapQUIKCommand = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeCommand, Name);

// ��� ��� ���������� ������
TCHAR Name1[] = TEXT("QUIKAnswer");
// �������, ��� ����������� � ��� ��������� ������ � ����� ������
HANDLE hFileMapAnswer = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeAnswer, Name1);

// ��� ��� ���������� ������
TCHAR Name2[] = TEXT("Deal");
// �������, ��� ����������� � ��� ��������� ������ � ����� ������
HANDLE hFileMapDeal = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeDeal, Name2);

// ��� ��� ���������� ������
TCHAR Name3[] = TEXT("Quote");
// �������, ��� ����������� � ��� ��������� ������ � ����� ������
HANDLE hFileMapQuote = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeQuote, Name3);

//=== ����������� ����� ����� ��� DLL ==========================================================================//
BOOL APIENTRY DllMain(HANDLE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
	return TRUE;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////COMMAND//////////////////////////////////////////////
//=== ���������� �������, ���������� �� LUA ====================================================================//
// ��������� ������ �� C# ����������
static int forLua_GetCommand(lua_State *L)
{
	//���� ��������� �� ������ �������
	if (hFileMapQUIKCommand)
	{
		//�������� ������ � ������ ������
		PBYTE pb = (PBYTE)(MapViewOfFile(hFileMapQUIKCommand, FILE_MAP_READ | FILE_MAP_WRITE, 0, 0, sizeCommand));

		//���� ������ � ������ ������ �������
		if (pb != NULL)
		{
			//���� ������ �����
			if (pb[0] == 0)
			{
				//���������� � Lua-���� ������ ������
				lua_pushstring(L, "");
			}
			else //���� � ������ ���� �������
			{
				//���������� � Lua-���� ���������� �������
				lua_pushstring(L, (char*)(pb));
				//������� ������, ����� �������� �� ��������� �������
				for (int i = 0; i < sizeCommand; i++)pb[i] = '\0';
				pb[0] = '0';
			}

			//��������� �������������
			UnmapViewOfFile(pb);
		}
		else lua_pushstring(L, "");//���� ������ � ������ ������ �� ��� �������, ���������� � Lua-���� ������ ������
	}
	else //��������� �� ������ �� ��� �������
	{
		//���������� � Lua-���� ������ ������
		lua_pushstring(L, "");
	}
	//������� ���������� ���������� �������� �� Lua-����� (������ ������, ��� ���������� �������)
	return(1);
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////END COMMAND//////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////ANSWER///////////////////////////////////////////////
static int forLua_CheckGotAnswer(lua_State *L)
{
	// ���� ��������� �� ������ �������
	if (hFileMapAnswer)
	{
		// �������� ������ (�������������) ��������������� � ������/������ ����
		PBYTE pb = (PBYTE)(MapViewOfFile(hFileMapAnswer, FILE_MAP_READ | FILE_MAP_WRITE, 0, 0, sizeAnswer));

		// ���� ������ �������
		if (pb != NULL)
		{
			// C# ������������� � ������ ���� '2', ������� ��� �����, ��� ����� ���������� � ������
			if (pb[0] == '0')
			{
				// ���� ������ ������, �� ���������� � QLua TRUE (��������� ���������� ������)
				lua_pushboolean(L, true);
			}
			else
			{
				// ���� ������ �� ������, �� ���������� � QLua FALSE (��������� ���������� ������)
				lua_pushboolean(L, false);
			}
			// ��������� �������������
			UnmapViewOfFile(pb);
		}
		else lua_pushboolean(L, false);// ���� ������ �� �������, ���������� � QLua FALSE (��������� ���������� ������)
	}
	else lua_pushboolean(L, false); // ���� ��������� �� ������ �� �������, ���������� � QLua FALSE (��������� ���������� ������)

	return(1);
}
// �������� ������ ����������� �� ������� ������� ���������� � ���������� �� � C#
static int forLua_SendAnswer(lua_State *L)
{
	// ���� ��������� �� ������ �������
	if (hFileMapAnswer)
	{
		// �������� ������ (�������������) ��������������� � ������/������ ����
		PBYTE pb = (PBYTE)(MapViewOfFile(hFileMapAnswer, FILE_MAP_READ | FILE_MAP_WRITE, 0, 0, sizeAnswer));

		// ���� ������ �������
		if (pb != NULL)
		{
			// ��������� �� Lua-����� ������ �����������
			const char *Answer = lua_tostring(L, 1);
			int Size = 0;
			// ������� ���������� �������� � ������
			for (int i = 0; i < sizeAnswer; i++)
			{
				if (Answer[i] == 0)break;
				Size++;
			}
			// ���������� ������ � ������, ������� �� 2-�� �����
			memcpy(pb + 1, Answer, Size);
			// ������������� � 1-� ���� "1", ������� ��� ����� C#, ��� ������ ��������� � ����� ������
			memcpy(pb, "1", 1);

			// ��������� �������������
			UnmapViewOfFile(pb);
		}
	}

	return(0);
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////END ANSWER///////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////DEAL/////////////////////////////////////////////////
//=== ���������� �������, ���������� �� LUA ====================================================================//
// ��������� �������-�� C# ��������� ������ ���������� �� ����������� (��� ��� �� ���� ���������� �� ����� ������)
static int forLua_CheckGotDeal(lua_State *L)
{
	// ���� ��������� �� ������ �������
	if (hFileMapDeal)
	{
		// �������� ������ (�������������) ��������������� � ������/������ ����
		PBYTE pb = (PBYTE)(MapViewOfFile(hFileMapDeal, FILE_MAP_READ | FILE_MAP_WRITE, 0, 0, sizeDeal));

		// ���� ������ �������
		if (pb != NULL)
		{
			// C# ������������� � ������ ���� '2', ������� ��� �����, ��� ����� ���������� � ������
			if (pb[0] == '0')
			{
				// ���� ������ ������, �� ���������� � QLua TRUE (��������� ���������� ������)
				lua_pushboolean(L, true);
			}
			else
			{
				// ���� ������ �� ������, �� ���������� � QLua FALSE (��������� ���������� ������)
				lua_pushboolean(L, false);
			}
			// ��������� �������������
			UnmapViewOfFile(pb);
		}
		else lua_pushboolean(L, false);// ���� ������ �� �������, ���������� � QLua FALSE (��������� ���������� ������)
	}
	else lua_pushboolean(L, false); // ���� ��������� �� ������ �� �������, ���������� � QLua FALSE (��������� ���������� ������)

	return(1);
}
// �������� ������ ����������� �� ������� ������� ���������� � ���������� �� � C#
static int forLua_SendDeal(lua_State *L)
{
	// ���� ��������� �� ������ �������
	if (hFileMapDeal)
	{
		// �������� ������ (�������������) ��������������� � ������/������ ����
		PBYTE pb = (PBYTE)(MapViewOfFile(hFileMapDeal, FILE_MAP_READ | FILE_MAP_WRITE, 0, 0, sizeDeal));

		// ���� ������ �������
		if (pb != NULL)
		{
			// ��������� �� Lua-����� ������ �����������
			const char *Params = lua_tostring(L, 1);
			int Size = 0;
			// ������� ���������� �������� � ������
			for (int i = 0; i < sizeDeal; i++)
			{
				if (Params[i] == '%')break;
				Size++;
			}
			// ���������� ������ � ������, ������� �� 2-�� �����
			memcpy(pb + 1, Params, Size);
			// ������������� � 1-� ���� "1", ������� ��� ����� C#, ��� ������ ��������� � ����� ������
			memcpy(pb, "1", 1);

			// ��������� �������������
			UnmapViewOfFile(pb);
		}
	}

	return(0);
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////END DEAL/////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////QUOTE////////////////////////////////////////////////
//��������� �������-�� ����� ��������� ������
static int forLua_CheckGotQuote(lua_State *L)
{
	//���� ��������� �� ������ �������
	if (hFileMapQuote)
	{
		//�������� ������ � ������ ������
		PBYTE pb = (PBYTE)(MapViewOfFile(hFileMapQuote, FILE_MAP_READ | FILE_MAP_WRITE, 0, 0, sizeQuote));

		//���� ������ � ������ ������ �������
		if (pb != NULL)
		{
			//��������� �� ������ ������ (������, ��� ����� ���������� ������)
			if (pb[0] == 0)
			{
				lua_pushboolean(L, true);
			}
			else
			{
				lua_pushboolean(L, false);
			}
			//��������� �������������
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

//���������� ����� ��������� �������
static int forLua_SendQuote(lua_State *L)
{
	//���� ��������� �� ������ �������
	if (hFileMapQuote)
	{
		//�������� ������ � ������ ������
		PBYTE pb = (PBYTE)(MapViewOfFile(hFileMapQuote, FILE_MAP_READ | FILE_MAP_WRITE, 0, 0, sizeQuote));

		//���� ������ � ������ ������ �������
		if (pb != NULL)
		{
			//�������� �� Lua-����� ���������� ��������
			const char *Quote = lua_tostring(L, 1);
			int Size = 0;
			//������� ���������� �������� � ������
			for (int i = 0; i < sizeQuote; i++)
			{
				if (Quote[i] == 0)break;
				Size++;
			}
			//���������� ������ � ������
			memcpy(pb, Quote, Size);
			//lua_pushstring(L, (char*)pb);//���������� ��, ��� ���������� (���� �����������������) (����� ����������� ��� �������)
			//��������� �������������
			UnmapViewOfFile(pb);
		}
		else lua_pushstring(L, "");
	}
	else lua_pushstring(L, "");

	return(1);
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////END QUOTE////////////////////////////////////////////


// ���������� ������� ����� ������ �� � �������������
static int forLua_GetMilliseconds(lua_State *L)
{
	// �������� ����� � �������������, ��������� � ������� ��
	unsigned long long _TickCount = GetTickCount64();
	char TickCount[20];
	// ������������ ����� ����������� � ��������� �������������
	sprintf_s(TickCount, "%llu", _TickCount);
	// �������� ��������� ������������� � Lua-����
	lua_pushstring(L, TickCount);

	return(1);
}

//=== ����������� ������������� � dll �������, ����� ��� ����� "������" ��� Lua ================================//
static struct luaL_reg ls_lib[] = {
	{ "CheckGotDeal", forLua_CheckGotDeal },
	{ "SendDeal", forLua_SendDeal },
	{ "GetMilliseconds", forLua_GetMilliseconds },
	{ "GetCommand", forLua_GetCommand },
	{ "CheckGotAnswer", forLua_CheckGotAnswer },
	{ "SendAnswer", forLua_SendAnswer },
	{ NULL, NULL }
};

//=== ����������� �������� ����������, �������� � ������� Lua ==================================================//
extern "C" LUALIB_API int luaopen_QuikConnect(lua_State *L) {
	luaL_openlib(L, "QuikConnect", ls_lib, 0);
	return 0;
}