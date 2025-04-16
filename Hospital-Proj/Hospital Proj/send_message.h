#pragma once

#include <iostream>
#include <winsock2.h>
#include <string>

void sendMessage(SOCKET *client_socket, const std::string& message);