#pragma once
#include <winsock2.h>
#include <string>

// Прототип функции для получения сообщения
std::string receiveMessage(SOCKET client_socket);